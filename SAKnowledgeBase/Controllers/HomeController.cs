using Microsoft.AspNetCore.Mvc;
using SAKnowledgeBase.Models;
using System.Diagnostics;
using System.Text;

namespace SAKnowledgeBase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadExcel()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                //    using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                //    {
                //        using (var reader = ExcelReaderFactory.CreateReader(stream))
                //        {
                //            do
                //            {
                //                bool isHeaderSkipped = false;

                //                while (reader.Read())
                //                {
                //                    if (!isHeaderSkipped)
                //                    {
                //                        isHeaderSkipped = true;
                //                        continue;
                //                    }

                //                    Theme theme = new Theme();
                //                    theme.ThemeName = reader.GetValue(1).ToString();
                //                    theme.SequenceNum = Convert.ToInt32(reader.GetValue(2).ToString());

                //                    await _themeRepo.AddAsync(theme);

                //                    //Student s = new Student();
                //                    //s.Name = reader.GetValue(1).ToString();
                //                    //s.Marks = Convert.ToInt32(reader.GetValue(2).ToString());

                //                    //_context.Add(s);
                //                    //await _context.SaveChangesAsync();


                //                }
                //            } while (reader.NextResult());

                //            ViewBag.Message = "success";
                //        }
                //    }
            }
            //else
            //    ViewBag.Message = "empty";
            return View();
        }

    }
}
