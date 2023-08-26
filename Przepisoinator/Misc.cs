using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Przepisoinator
{
    internal class Misc
    {
        public static JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };
        static Misc()
        {

        }
    }
}
