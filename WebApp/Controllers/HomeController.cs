using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using WebApp.Models;
using System.IO;
using Aspose.Words.Saving;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var pathToWordFile = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.doc");
            var pathToPptFile = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.ppt");

            var wordDoc = new Aspose.Words.Document(pathToWordFile);
            var pptSlide = new Aspose.Slides.Presentation(pathToPptFile);
            

            var slidePath = Path.Combine(_hostingEnvironment.WebRootPath, "Slide.jpg");
            if (System.IO.File.Exists(slidePath))
            {
                System.IO.File.Delete(slidePath);
            }

            using (var bmp = pptSlide.Slides[0].GetThumbnail(Convert.ToSingle(200) / 100f, Convert.ToSingle(200) / 100f))
            {
                if (null != bmp)
                {
                    bmp.Save(slidePath);
                }
            }

            var wordPath = Path.Combine(_hostingEnvironment.WebRootPath, "Word.jpg");
            if (System.IO.File.Exists(wordPath))
            {
                System.IO.File.Delete(wordPath);
            }

            var options = new ImageSaveOptions(Aspose.Words.SaveFormat.Jpeg)
            {
                PageIndex = 0,
                JpegQuality = 100,
                Resolution = 200
            };

            wordDoc.Save(wordPath, options);

          
            var bitmapPath = Path.Combine(_hostingEnvironment.WebRootPath, "Bitmap.jpg");
            if (System.IO.File.Exists(bitmapPath))
            {
                System.IO.File.Delete(bitmapPath);
            }

            var bitmap = new Bitmap(500,500);

            using(Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.FillRectangle(new SolidBrush(Color.White), 0, 0, 500, 500);
                g.DrawString("Hello World", new Font("Verdana", 32), new SolidBrush(Color.Red), new PointF(50, 50));
            }

            bitmap.Save(bitmapPath, ImageFormat.Jpeg);
            bitmap.Dispose();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
