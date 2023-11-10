using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Przepisoinator
{
    static class RTBEncoderDecoder
    {
        public static string Encode(FlowDocument flowDocument)
        {

            return "";
        }

        public static FlowDocument Decode(string input)
        {
            FlowDocument flowDocument = new FlowDocument();
            return flowDocument;
        }
    }

    public class JsonFlowDocument
    {
        public List<JsonFlowParagraph> Paragraphs { get; set; }

        public JsonFlowDocument(List<JsonFlowParagraph> paragraphs)
        {
            Paragraphs = paragraphs;
        }
        public static JsonFlowDocument FromDocument(FlowDocument? flowDocument)
        {
            if (flowDocument == null)
            {
                return new JsonFlowDocument(new List<JsonFlowParagraph>());
            }

            return new JsonFlowDocument(flowDocument.Blocks.OfType<Paragraph>().Select(JsonFlowParagraph.FromParagraph).ToList());
        }
        public static JsonFlowDocument FromJson(string json)
        {
            var flow = JsonSerializer.Deserialize<JsonFlowDocument>(json) ??
                new JsonFlowDocument(
                    new List<JsonFlowParagraph>()
                    {
                        new JsonFlowParagraph(
                            new List<JsonFlowInline>())
                    });
            return flow;
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, Misc.JsonOptions);
        }

        public FlowDocument ToFlowDocument()
        {
            FlowDocument flowDocument = new FlowDocument();
            foreach (var p in Paragraphs)
            {
                flowDocument.Blocks.Add(p.ToParagraph());
            }
            return flowDocument;
        }
    }

    public class JsonFlowParagraph
    {
        public List<JsonFlowInline> Inlines { get; set; }

        public JsonFlowParagraph(List<JsonFlowInline> inlines)
        {
            Inlines = inlines;
        }
        public static JsonFlowParagraph FromParagraph(Paragraph paragraph)
        {
            return new JsonFlowParagraph(paragraph.Inlines.Select(JsonFlowInline.FromInline).ToList());
        }

        internal Block ToParagraph()
        {
            Paragraph paragraph = new Paragraph();
            foreach (var i in Inlines)
            {
                if (i.IsBreakline)
                {
                    paragraph.Inlines.Add(new LineBreak());
                }
                else
                {
                    paragraph.Inlines.Add(i.Text);
                    paragraph.Inlines.LastInline.FontStyle = i.FontStyle;
                    paragraph.Inlines.LastInline.FontWeight = i.FontWeight;
                }
            }
            return paragraph;
        }
    }

    public class JsonFlowInline
    {

        public string Text { get; set; }
        public bool IsItallic { get; set; }
        [JsonPropertyName("weight")]
        public int FontWeightID { get; set; }
        public bool IsBreakline { get; set; }

        [JsonIgnore]
        public FontStyle FontStyle { get => IsItallic ? FontStyles.Italic : FontStyles.Normal; }
        [JsonIgnore]
        public FontWeight FontWeight { get => FontWeight.FromOpenTypeWeight(FontWeightID); }

        public JsonFlowInline(string text = "", bool isItallic = false, int fontWeightID = 400, bool isBreakline = false)
        {
            Text = text;
            IsItallic = isItallic;
            FontWeightID = fontWeightID;
            IsBreakline = isBreakline;
        }


        public static JsonFlowInline FromInline(Inline inline)
        {
            if (inline.GetType() == typeof(LineBreak))
            {
                return new JsonFlowInline(isBreakline: true);
            }
            return new JsonFlowInline(
                inline.ContentStart.GetTextInRun(LogicalDirection.Forward),
                inline.FontStyle == FontStyles.Italic,
                inline.FontWeight.ToOpenTypeWeight(),
                false);
        }
    }
}
