using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.TextFormatting;

namespace Przepisoinator
{
    public class Recepy
    {
        public static Recepy BasicRecepy = new()
        {
            Name = "Przepis",
            ID = -1,
            Ingredients = new List<RecepyIngredient>() { new RecepyIngredient("", MeasurementUnit.BasicUnit, 1)},

        };
        public string Name { get; set; } = "";

        [JsonIgnore]
        public FlowDocument DescriptionFlow { get; set; }

        public JsonFlowDocument FlowJsonDocument 
        {
            get
            {
                return JsonFlowDocument.FromDocument(DescriptionFlow);
            }
            set 
            {
                DescriptionFlow = value.ToFlowDocument();
            } 
        }
        public long ID { get; protected set; } = 0;
        public List<RecepyIngredient> Ingredients { get; set; }
        public int ServingCount { get; set; }
        public List<string> Tags { get; set; }
        public double Rating { get; set; }
        public bool Tried { get; set; }

        public Recepy() 
        {
            DescriptionFlow = new FlowDocument();
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

        internal void RemoveTag(int index)
        {
            if(index > 0 && index < Ingredients.Count)
                Tags.RemoveAt(index);
        }

        internal void EditTag(int index, string text)
        {
            if (index > 0 && index < Ingredients.Count)
                Tags[index] = text.Trim();
        }

        public static Recepy GetEmptyRecepy()
        {
            return new Recepy();
        }
    }
}
