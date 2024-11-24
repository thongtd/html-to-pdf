using html_to_pdf.Models;
using Microsoft.AspNetCore.Mvc;
using RazorLight;
using SelectPdf;

namespace html_to_pdf.Controllers
{
    [ApiController]
    [Route("html-to-pdf")]
    public class DemoHtmlToPdfController : ControllerBase
    {
        [HttpGet, Route("export-pdf")]
        public async Task<IActionResult> ConvertHtmlToPdf()
        {
            var orders = BuildDummyModel();
            var htmlContent = await RenderCshtmlToHtml(orders);

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(htmlContent);

            using (MemoryStream stream = new MemoryStream())
            {
                doc.Save(stream);
                doc.Close();
                return File(stream.ToArray(), "application/pdf", "example.pdf");
            }
        }

        private IEnumerable<OrderModel> BuildDummyModel()
        {
            var rd = new Random();
            var orders = new List<OrderModel>();
            for (int i = 0; i < 100; i++)
            {
                orders.Add(new OrderModel
                {
                    OrderId = i + 1,
                    CustomerName = $"Alex {i + 1}",
                    TotalPrice = rd.NextDouble(),
                    OrderDate = DateTime.Now.AddDays(i * -1)
                });
            }

            return orders;
        }

        private async Task<string> RenderCshtmlToHtml(IEnumerable<OrderModel> model)
        {
            var templateFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templateFolderPath)
                .UseMemoryCachingProvider()
                .SetOperatingAssembly(typeof(Program).Assembly)
                .Build();

            string htmlContent = await engine.CompileRenderAsync("Example.cshtml", model);
            return htmlContent;
        }
    }
}