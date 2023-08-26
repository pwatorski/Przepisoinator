using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Przepisoinator
{
    public class UnitConversion
    {
        public static Dictionary<long, Dictionary<long, double>> Conversions;

        public MeasurementUnit Source;
        public MeasurementUnit Target;
        public double Conversion { get; set; }
        static UnitConversion()
        {
            Conversions = new Dictionary<long, Dictionary<long, double>>() { 
                { 
                    0,
                    new Dictionary<long, double>() { 
                        { 0, 1 } 
                    }
                } 
            };
        }
        public UnitConversion(MeasurementUnit source, MeasurementUnit target, double conversion)
        {
            Source = source;
            Target = target;
            Conversion = conversion;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Misc.JsonOptions);
        }
    }
}
