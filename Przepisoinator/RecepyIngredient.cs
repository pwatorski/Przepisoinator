using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class RecepyIngredient
    {
        public long IngredientId { get; set; }
        public long UnitId { get; set; }
        public string ActiveName { get => Ingredient.NumberName(Value); }
        public Ingredient Ingredient;
        public MeasurementUnit Unit;
        public double Value { get; set; }

        public static RecepyIngredient GetEmptyIngredient()
        {
            return new RecepyIngredient(new Ingredient(""), MeasurementUnit.BasicUnit, 1);
        }

        public RecepyIngredient(Ingredient ingredient, MeasurementUnit unit, float value)
        {
            Ingredient = ingredient;
            Unit = unit;
            Value = value;
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
            return JsonSerializer.Deserialize<RecepyIngredient>(json) ?? new RecepyIngredient(Ingredient.BasicIngredient, MeasurementUnit.BasicUnit, 1);
        }
    }
}
