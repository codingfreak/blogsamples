using System;
using System.Linq;

namespace codingfreaks.blogsamples.IronPdf
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using global::IronPdf;

    using HandlebarsDotNet;

    using Helpers;

    using Models;

    class Program
    {

        private static string _targetFilePath;

        static async Task Main(string[] args)
        {
            _targetFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Result.pdf");
            await SimpleTestAsync();
            //await TestInvoiceAsync();
            // open the preview (only Windows)
            var previewProcess = new Process
            {
                StartInfo =
                {
                    FileName = "explorer",
                    Arguments = $@"""{_targetFilePath}"""
                }
            };
            previewProcess.Start();
            Console.WriteLine("PDF generated.");
        }

        /**
         * Performs a test by generating an invoice PDF using IronPDf.
        **/
        private static async Task TestInvoiceAsync()
        {
            // generate invoice data
            var invoice = InvoiceHelper.GenerateRandomInvoice(10);
            // load templates
            var pageTemplate = await File.ReadAllTextAsync("Templates/Invoice.html");
            var rowTemplate = await File.ReadAllTextAsync("Templates/InvoiceRow.html");
            Handlebars.RegisterTemplate("positionRow", rowTemplate);
            // compile templates
            var template = Handlebars.Compile(pageTemplate);
            var result = template(invoice);
            // render to PDF
            using (var renderer = new HtmlToPdf
            {
                PrintOptions =
                {
                    FirstPageNumber = 1,
                    Footer = new SimpleHeaderFooter
                    {
                        DrawDividerLine = true,
                        RightText = "Page {page} of {total-pages}"
                    }
                }
            })
            {
                var baseUri = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"));
                var pdf = await renderer.RenderHtmlAsPdfAsync(result, baseUri);
                pdf.SaveAs(_targetFilePath);
            }
        }
    }
}
