using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class Ingredient
    {
        public static Ingredient BasicIngredient { get; private set; }
        public static Dictionary<long, Ingredient> Ingredients { get; private set; }
        public string Name { get; set; } = "???";
        [JsonIgnore]
        public string LowerName { get; set; }

        static Ingredient()
        {
            Ingredients = new Dictionary<long, Ingredient>();
            BasicIngredient = new Ingredient("Nic");
        }

        public Ingredient(string name)
        {
            Name = name;
            LowerName = name.ToLower();
        }

        public double GetSimilarity(string lowName)
        {
            return StringSimilarity.Get(LowerName, lowName);
        }

        public static Ingredient FindMostSimilar(string name)
        {
            var mostSImilar = BasicIngredient;
            double similarity = 0;
            name = name.ToLower();
            foreach (var u in Ingredients.Values)
            {
                var curSim = u.GetSimilarity(name);
                if (curSim == 1)
                {
                    return u;
                }
                if (curSim > similarity)
                {
                    curSim = similarity;
                    mostSImilar = u;
                }
            }

            return mostSImilar;
        }
    }
}
