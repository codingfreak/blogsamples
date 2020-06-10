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
        static async Task Main(string[] args)
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
                pdf.SaveAs("Result.pdf");
            }
            // open the preview (only Windows)
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Result.pdf");
            var previewProcess = new Process
            {
                StartInfo =
                {
                    FileName = "explorer",
                    Arguments = $@"""{path}"""
                }
            };
            previewProcess.Start();
            Console.WriteLine("PDF generated.");
        }
    }
}
