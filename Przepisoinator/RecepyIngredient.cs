using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

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

        protected static RecepyIngredient TryParseUnitFirst(List<string> words, double value)
        {
            bool success = false;
            string unitName = "";
            string ingredientName = "";
            int wordPointer = 0;
            int besWordCount = 0;
            double bestSim = 0;
            MeasurementUnit unit;
            MeasurementUnit bestUnit = MeasurementUnit.BasicUnit;

            //go to end looking for units
            do
            {
                if (wordPointer == 0)
                    unitName = words[0];
                else
                    unitName += $" {words[wordPointer]}";
                unit = MeasurementUnit.FindMostSimilar(unitName, out double curSim, true, 0.75);
                if (curSim > bestSim)
                {
                    besWordCount = wordPointer + 1;
                    bestUnit = unit;
                    bestSim = curSim;
                }
                wordPointer++;
            } while (wordPointer < words.Count && bestSim < 1);

            //if unit is similar to a known one
            if (bestSim > 0.75 && besWordCount < words.Count)
            {

                ingredientName = string.Join(" ", words.Skip(besWordCount));

            }
            else
            {
                bestUnit = MeasurementUnit.BasicUnit;
                ingredientName = string.Join(" ", words);
            }
            return new RecepyIngredient(ingredientName, bestUnit, value);
        }

        public static List<RecepyIngredient> TryParseFromText(string text)
        {
            var words = text.Split(' ').Where(x=>x.Length > 0).ToList();


            if (words.Count == 0) return new List<RecepyIngredient>();

            double value = 1;
            int wordCount = words.Count;

            if (text.Count(x => x == ',')>1)
            {
                return text.Split(',').Select(x => TryParseFromText(x.Trim())).SelectMany(x=>x).ToList();
            }

            // Look from the start
            if (double.TryParse(words[0], out value))
            {
                return new List<RecepyIngredient>() { TryParseUnitFirst(words.Skip(1).ToList(), value) };
            }

            var ingreientTest = TryParseUnitFirst(words, 1);
            if(ingreientTest.Unit.ID != MeasurementUnit.BasicUnit.ID)
            {
                return new List<RecepyIngredient>() { ingreientTest };
            }

            // Look for number anywhere in text
            int wordPointer = words.Count;
            bool foundVal = false;
            do
            {
                wordPointer -= 1;
                if (double.TryParse(words[wordPointer], out value))
                {
                    foundVal = true;
                }
            } while (wordPointer > 1 && !foundVal);
            if (foundVal)
            {
                MeasurementUnit unit = MeasurementUnit.BasicUnit;
                if (wordPointer < words.Count - 1)
                {
                    string unitName = words[wordPointer + 1];
                    for (int i = wordPointer + 2; i < words.Count; i++)
                    {
                        unitName += $" {words[i]}";
                    }
                    double curSim;
                    unit = MeasurementUnit.FindMostSimilar(unitName, out curSim, true, 0.75);
                    string ingredientName = words[0];
                    for (int i = 1; i < wordPointer; i++)
                    {
                        ingredientName += $" {words[i]}";
                    }
                    return new List<RecepyIngredient>() { new RecepyIngredient(ingredientName, unit, value) };
                }
            }



            return new List<RecepyIngredient>() { new RecepyIngredient(text, MeasurementUnit.BasicUnit, 1) };
        }
    }
}
