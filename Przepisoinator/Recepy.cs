using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class Recepy
    {
        public string Name { get; set; } = "???";
        public string Description { get; set; } = "???";
        public long ID { get; protected set; } = 0;
        public List<RecepyIngredient> Ingredients { get; set; }
        public int ServingCount { get; set; }
        public List<string> Tags { get; set; }
        public double Rating { get; set; }
        public bool Tried { get; set; }

        public Recepy() 
        {
            Ingredients = new List<RecepyIngredient>();
            Tags = new List<string>();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Misc.JsonOptions);
        }

        public static Recepy? FromJson(string json)
        {
            return JsonSerializer.Deserialize<Recepy>(json);
        }
    }
}
