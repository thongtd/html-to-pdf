
# Export HTML to PDF

Exporting to PDF from a .cshtml file is a common requirement in real-world projects, especially for generating reports, invoices, or other dynamic documents. Here's a streamlined approach tailored for practical use:

# Steps to Export to PDF from a .cshtml File
 1. Render the .cshtml to an HTML string.
 2. Convert the HTML string to PDF using a library.

### **Complete Implementation**

#### **1. Render `.cshtml` File to HTML**

Use **RazorLight** to render a `.cshtml` file to an HTML string.

-   **Install RazorLight:**
        
    `dotnet add package RazorLight` 
    
-   **Rendering Function:**
```cs
private async Task<string> RenderCshtmlToHtml(string cshtmlFileName, object model)
{
    var templateFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates");

    var engine = new RazorLight.RazorLightEngineBuilder()
        .UseFileSystemProject(templateFolderPath) // Path to your .cshtml folder
        .UseMemoryCachingProvider()
        .Build();

    string htmlContent = await engine.CompileRenderAsync(cshtmlFileName, model); // Render the .cshtml with the model
    return htmlContent;
}
```
#### **2. Convert HTML to PDF**

Use **SelectPdf** for HTML-to-PDF conversion.

-   **Install SelectPdf:**
        
    `dotnet add package Select.HtmlToPdf` 
    
-   **Conversion Function:**
```cs
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
```
