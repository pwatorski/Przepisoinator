using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class RecepyIngredient
    {
        
        public long UnitId { get=>Unit.ID; set=>Unit=MeasurementUnit.AllUnits[value]; }
        [JsonIgnore]
        public string ActiveName { get => Name; }
        public string Name { get; set; }
        [JsonIgnore]
        public MeasurementUnit Unit;
        public double Value { get; set; }
        public int Indent { get; set; }

        [JsonConstructor]
        public RecepyIngredient(string name, long unitId, double value, int indent=0)
        {
            Name = name;
            Unit = MeasurementUnit.AllUnits[unitId];
            Value = value;
            Indent = indent;
        }

        public static RecepyIngredient GetEmptyIngredient()
        {
            return new RecepyIngredient("", MeasurementUnit.BasicUnit, 1);
        }

        public RecepyIngredient(string name, MeasurementUnit unit, double value, int indent = 0)
        {
            Name = name;
            Unit = unit;
            Value = value;
            Indent = indent;
        }

        public bool ConvertInPlace(MeasurementUnit? newUnit)
        {
            if(newUnit == null)
            {
                return false;
            }
            var newAmount = Unit.GetAmountIn(Value, newUnit);
            if (newAmount < 0) return false;
            Value = newAmount;
            return true;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Misc.JsonOptions);
        }

        public static RecepyIngredient FromJson(string json)
        {
            return JsonSerializer.Deserialize<RecepyIngredient>(json) ?? new RecepyIngredient("", MeasurementUnit.BasicUnit, 1);
        }

        internal void SetIndet(int indent)
        {
            Indent = indent;
        }
    }
}
