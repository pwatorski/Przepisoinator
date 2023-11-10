using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przepisoinator
{
    class RecepyListViewItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TagString { get => string.Join(", ", Tags); }
        public List<string> Tags { get; set; }
        public Recepy Recepy { get; protected set; }

        public RecepyListViewItem(Recepy recepy)
        {
            Name = recepy.Name;
            Description = recepy.DescriptionText;
            Tags = recepy.Tags;
            Recepy = recepy;
        }

        public static RecepyListViewItem FromRecepy(Recepy recepy)
        {
            return new RecepyListViewItem(recepy);
        }
    }
}
