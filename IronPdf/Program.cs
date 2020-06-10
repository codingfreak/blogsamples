using System;
using System.Linq;

namespace codingfreaks.blogsamples.IronPdf
{
    using System;
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
            var renderer = new HtmlToPdf();
            var pdf = await renderer.RenderHtmlAsPdfAsync(result);
            pdf.SaveAs("Result.pdf");
            Console.WriteLine("Hello World!");
        }
    }
}
