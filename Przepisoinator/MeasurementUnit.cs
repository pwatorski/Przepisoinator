using Microsoft.VisualBasic;
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
        public bool OnlyName { get; set; }
        [JsonIgnore]
        public string LowerSymbol { get; set; }
        public string ShortName { get => OnlyName ? Name : Symbol; }
        public long ID { get; set; } = -1;
        public Dictionary<long, double> Conversions { get; set; }
        public static Dictionary<long, Dictionary<long, double>> AllConversions { get; set; }

        static MeasurementUnit()
        {
            AllUnits = new Dictionary<long, MeasurementUnit>();
            AllConversions = new Dictionary<long, Dictionary<long, double>>();
            BasicUnit = new MeasurementUnit("Jednostka", "Ø", 0);
            var _ = new MeasurementUnit("Kilogram", "kg", 1);
            _ = new MeasurementUnit("Gram", "g", 2);
            _.AddConversion(1, 0.001);
            _ = new MeasurementUnit("Litr", "l", 3);
            _ = new MeasurementUnit("Mililitr", "ml", 4);
            _ = new MeasurementUnit("Sztuki", "szt.", 5);
            _ = new MeasurementUnit("Ząbki", "ząbki", 6);
            _ = new MeasurementUnit("Łyżeczki", "łyzeczki", 7);
            _ = new MeasurementUnit("Opakowania", "pak.", 8);
            _ = new MeasurementUnit("Szklanka", "szkl.", 9);
            _ = new MeasurementUnit("Szczypta", "szczypta", 10);
            _.AddConversion(2, 5);
            _.AddConversion(4, 5);


        }

        public MeasurementUnit(string name, string symbol, long id)
        {
            Name = name;
            LowerName = name.ToLower();
            Symbol = symbol;
            LowerSymbol = symbol.ToLower();
            ID = id;
            AllUnits.Add(id, this);
            
            AddConversion(ID, 1, true);
            Conversions = AllConversions[ID];
        }

        public void AddConversion(long targetId, double conversion, bool ignoreTarget=false)
        {
            InsertConversion(ID, targetId, conversion);
            if (!ignoreTarget)
            {
                AllUnits[targetId].AddConversion(ID, 1.0 / conversion, ignoreTarget = true);
            }
        }

        public void RemoveConversion(long targetID, bool ignoreTarget = false)
        {
            DeleteConversion(ID);
            if (!ignoreTarget)
            {
                AllUnits[targetID].DeleteConversion(ID);
            }
        }

        protected void DeleteConversion(long targetID)
        {
            if (AllConversions.ContainsKey(ID))
            {
                if (AllConversions[ID] != null && AllConversions[ID].ContainsKey(targetID))
                {
                    AllConversions[ID].Remove(targetID);
                }
            }
        }

        protected void InsertConversion(long source, long target, double conversion)
        {
            Dictionary<long, double> sourceConvos;
            if (!AllConversions.ContainsKey(source))
            {
                sourceConvos = new Dictionary<long, double>();
                AllConversions.Add(source, sourceConvos);
            }
            else
            {
                sourceConvos = AllConversions[source];
            }
            if(sourceConvos.ContainsKey(target))
            {
                sourceConvos[target] =  conversion;
            }
            else
            {
                sourceConvos.Add(target, conversion);
            }

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
            if (includeSymbol && !OnlyName)
                return Math.Max(
                    StringSimilarity.Get(LowerName, lowName),
                    StringSimilarity.Get(LowerSymbol, lowName));
            return StringSimilarity.Get(LowerName, lowName);
        }

        public static MeasurementUnit FindMostSimilar(string name, out double similarity, bool includeSymbol = true, double treshold = 0)
        {
            var mostSImilar = BasicUnit;
            similarity = 0;
            name = name.ToLower();
            foreach (var u in AllUnits.Values)
            {
                var curSim = u.GetSimilarity(name, includeSymbol);
                if (curSim == 1)
                {
                    similarity = curSim;
                    return u;
                }
                if (curSim >= treshold && curSim > similarity)
                {
                    similarity = curSim;
                    mostSImilar = u;
                }
            }

            return mostSImilar;
        }

        public static MeasurementUnit FindMostSimilar(string name, bool includeSymbol = true)
        {
            return FindMostSimilar(name, out _, includeSymbol);
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
