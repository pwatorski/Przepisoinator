using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przepisoinator
{
    public class RecepySection
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecepyIngredient> Ingredients { get; set; }
    }
}
