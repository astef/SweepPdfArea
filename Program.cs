using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.PdfCleanup.Autosweep;

namespace SweepPdfArea
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                PdfDocument pdfDoc;
                if (o.File == null)
                {
                    pdfDoc = new PdfDocument(
                        new PdfReader(o.InputFile),
                        new PdfWriter(o.OutputFile));
                }
                else
                {
                    pdfDoc = new PdfDocument(
                        new PdfReader(new MemoryStream(File.ReadAllBytes(o.File))),
                        new PdfWriter(o.File));
                }

                ICleanupStrategy cleanupStrategy = new AreaCleanupStrategy(o);

                PdfAutoSweep autoSweep = new PdfAutoSweep(cleanupStrategy);
                autoSweep.CleanUp(pdfDoc.GetPage(o.Page));

                pdfDoc.Close();

            });
        }

        private static Color GetColor(string hex)
        {
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);
            return new DeviceRgb(r, g, b);
        }

        public sealed class Options
        {
            [Option("file", Required = true, SetName = "singleFile", HelpText = "Input and output PDF file to read from and write to.")]
            public string File { get; set; }

            [Option("input", Required = true, SetName = "twoFiles", HelpText = "Input PDF file to read from.")]
            public string InputFile { get; set; }
            
            [Option("output", Required = true, SetName = "twoFiles", HelpText = "Output PDF file to write to.")]
            public string OutputFile { get; set; }

            [Option("fill-color", Default = "000000", HelpText = "Background color of the fill area.")]
            public string FillColor { get; set; }
            
            [Option("page", Default = 1, HelpText = "Page, where the fill should be done.")]
            public int Page { get; set; }
            
            [Option('x', Required = true, HelpText = "X coordinate of the fill area.")]
            public float X { get; set; }

            [Option('y', Required = true, HelpText = "Y coordinate of the fill area.")]
            public float Y { get; set; }
            
            [Option('w', Required = true, HelpText = "Width of the fill area.")]
            public float W { get; set; }

            [Option('h', Required = true, HelpText = "Height of the fill area.")]
            public float H { get; set; }
        }

        private class AreaCleanupStrategy : ICleanupStrategy
        {
            private readonly Options _options;

            public AreaCleanupStrategy(Options options)
            {
                _options = options;
            }

            public void EventOccurred(IEventData data, EventType type)
            {
            }

            public ICollection<EventType> GetSupportedEvents()
            {
                return null;
            }

            public ICollection<IPdfTextLocation> GetResultantLocations()
            {
                var result = new List<IPdfTextLocation>
                {
                    new DefaultPdfTextLocation(1, new Rectangle(_options.X, _options.Y, _options.W, _options.H), string.Empty)
                };
                return result;
            }

            public Color GetRedactionColor(IPdfTextLocation location)
            {
                return GetColor(_options.FillColor);
            }

            public ICleanupStrategy Reset()
            {
                return this;
            }
        }
    }
}
