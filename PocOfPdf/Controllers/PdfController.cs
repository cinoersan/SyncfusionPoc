using Microsoft.AspNetCore.Mvc;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf.Graphics;
using System.IO;

namespace PocOfPdf.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PdfController : ControllerBase
	{
		[HttpGet("get")]
		public IActionResult Get()
		{
			// Burası için bir rapor url oluşturulmaması gerekiyor.
			var stream = ReportPdf("https://venngage.com/blog/business-report-templates/");
			return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output.pdf");
		}

		[HttpGet("gethtml")]
		public IActionResult GetHtml()
		{
			// Burası için bir rapor url oluşturulmaması gerekiyor.
			var stream = ReportPdfHtml("<div>test<div>");
			return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output.pdf");
		}

		private MemoryStream ReportPdf(string url)
		{
			var htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

			var settings = new BlinkConverterSettings
			{
				BlinkPath = Path.Combine(Directory.GetCurrentDirectory(), "BlinkBinariesWindows")
			};

			//Assign Blink settings to HTML converter
			htmlConverter.ConverterSettings = settings;

			//Convert URL to PDF
			var document = htmlConverter.Convert(url);

			//Saving the PDF to the MemoryStream
			var stream = new MemoryStream();

			document.Save(stream);

			//Download the PDF document in the browser
			return stream;
		}

		private MemoryStream ReportPdfHtml(string html)
		{
			var htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

			var margins = new PdfMargins { All = 50 };

			var settings = new BlinkConverterSettings
			{
				BlinkPath = Path.Combine(Directory.GetCurrentDirectory(), "BlinkBinariesWindows"),
				Margin = margins,

			};

			//Assign Blink settings to HTML converter
			htmlConverter.ConverterSettings = settings;


			//Convert URL to PDF
			var document = htmlConverter.Convert(html, "/");

			//Saving the PDF to the MemoryStream
			var stream = new MemoryStream();

			document.Save(stream);

			//Download the PDF document in the browser
			return stream;
		}
	}
}
