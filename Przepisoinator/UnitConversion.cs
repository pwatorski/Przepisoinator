using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Przepisoinator
{
    public class UnitConverter
    {
        public static Dictionary<int, Dictionary<int, double>> Conversions;
        static UnitConverter()
        {
            Conversions = new Dictionary<int, Dictionary<int, double>>() { 
                { 
                    0,
                    new Dictionary<int, double>() { 
                        { 0, 1 } 
                    }
                } 
            };
        }
    }
}
