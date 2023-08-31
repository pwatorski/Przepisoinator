using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class MeasurementUnit
    {
        public static MeasurementUnit BasicUnit { get; private set; }
        public static Dictionary<long, MeasurementUnit> AllUnits { get; private set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string LowerName { get; set; }
        public string Symbol { get; set; }
        [JsonIgnore]
        public string LowerSymbol { get; set; }
        public long ID { get; set; } = -1;
        public Dictionary<long, double> Conversions { get; set; }

        static MeasurementUnit()
        {
            AllUnits = new Dictionary<long, MeasurementUnit>();
            BasicUnit = new MeasurementUnit("Jednostka", "Ø", 0);
            var _ = new MeasurementUnit("Kilogram", "Kg", 1);
            _ = new MeasurementUnit("Kilogram", "g", 2);
            _ = new MeasurementUnit("Kilogram", "l", 3);
            _ = new MeasurementUnit("Kilogram", "ml", 4);
            _ = new MeasurementUnit("Sztuki", "szt.", 5);
            _ = new MeasurementUnit("Ząbki", "ząbki", 6);
        }

        public MeasurementUnit(string name, string symbol, long id)
        {
            Name = name;
            LowerName = name.ToLower();
            Symbol = symbol;
            LowerSymbol = symbol.ToLower();
            ID = id;
            AllUnits.Add(id, this);
            Conversions = new Dictionary<long, double>()
            {
                {ID, 1}
            };
        }

        public double GetAmountIn(double amount, MeasurementUnit otherUnit)
        {
            if(!Conversions.ContainsKey(otherUnit.ID))
            {
                return -1;
            }
            return Conversions[otherUnit.ID] * amount;
        }

        public double GetSimilarity(string lowName, bool includeSymbol=true)
        {
            if (includeSymbol)
                return Math.Max(
                    StringSimilarity.Get(LowerName, lowName),
                    StringSimilarity.Get(LowerSymbol, lowName));
            return StringSimilarity.Get(LowerName, lowName);
        }

        public static MeasurementUnit FindMostSimilar(string name, bool includeSymbol = true)
        {
            var mostSImilar = BasicUnit;
            double similarity = 0;
            name = name.ToLower();
            foreach(var u in AllUnits.Values)
            {
                var curSim = u.GetSimilarity(name, includeSymbol);
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

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Misc.JsonOptions);
        }

        public void SaveInDirectory(string dirPath)
        {
            using var sw = new StreamWriter(Path.Combine(dirPath, $"{ID}.json"), false, Encoding.UTF8);
            sw.Write(ToJson());
        }

        public static void SaveAllToDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            AllUnits.Values.ToList().ForEach(x=>x.SaveInDirectory(dirPath));
            
        }

        public static MeasurementUnit FromJson(string json)
        {
            return JsonSerializer.Deserialize<MeasurementUnit>(json)??BasicUnit;
        }

        public static MeasurementUnit FromFile(string filePath)
        {
            using var sr = new StreamReader(filePath, Encoding.UTF8);
            return FromJson(sr.ReadToEnd());
        }

        public static int LoadAllFromDir(string dirPath)
        {
            foreach( var path in Directory.EnumerateFiles(dirPath))
            {
                _ = FromFile(path);
            }
            return AllUnits.Values.Count;
        }
    }
}
