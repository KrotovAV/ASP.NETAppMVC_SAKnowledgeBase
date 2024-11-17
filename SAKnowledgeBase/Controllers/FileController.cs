using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using OfficeOpenXml;
using SAKnowledgeBase.DataBase;
using SAKnowledgeBase.DataBase.Entities;
using SAKnowledgeBase.Models;
using SAKnowledgeBase.Models.ViewModel;
using SAKnowledgeBase.Repositories.Interfaces;


namespace SAKnowledgeBase.Controllers
{
    public class FileController : Controller
    {

        private IRepository<Theme> _themeRepo;
        private IRepository<Question> _questionRepo;
        private IRepository<Info> _infoRepo;

        public FileController(IRepository<Theme> themeRepo, IRepository<Question> questionRepo, IRepository<Info> infoRepo)
        {
            _themeRepo = themeRepo;
            _questionRepo = questionRepo;
            _infoRepo = infoRepo;
        }

        public IActionResult UploadThemeExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadThemeExcel(IFormFile file)
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

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                Theme theme = new Theme();
                                theme.ThemeName = reader.GetValue(1).ToString();
                                theme.SequenceNum = Convert.ToInt32(reader.GetValue(2).ToString());

                                //await _themeRepo.AddAsync(theme);
                                _themeRepo.Add(theme);

                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
        public IActionResult UploadQuestionExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadQuestionExcel(IFormFile file)
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

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                Question question = new Question();
                                question.QuestionName = reader.GetValue(1).ToString();
                                question.SequenceNum = Convert.ToInt32(reader.GetValue(2).ToString());
                                question.ThemeId = Convert.ToInt32(reader.GetValue(3).ToString());

                                //await _questionRepo.AddAsync(question);
                                _questionRepo.Add(question);

                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
        public IActionResult UploadInfoExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadInfoExcel(IFormFile file)
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

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                Info info = new Info();
                                info.Text = reader.GetValue(1).ToString();
                                info.QuestionId = Convert.ToInt32(reader.GetValue(2).ToString());
                                info.SequenceNum = Convert.ToInt32(reader.GetValue(3).ToString());
                                info.FormatId = Convert.ToInt32(reader.GetValue(4).ToString());

                                //info.PhotoPath = reader.GetValue(5).ToString();
                                if (reader.GetValue(5) != null) 
                                {
                                    info.PhotoPath = reader.GetValue(5).ToString();
                                }
                                else info.PhotoPath = null;

                                info.Level = Convert.ToInt32(reader.GetValue(6).ToString());

                                //info.Link = reader.GetValue(7).ToString();
                                if (reader.GetValue(7) != null)
                                {
                                    info.Link = reader.GetValue(7).ToString();
                                }
                                else info.Link = null;

                                //await _infoRepo.AddAsync(info);
                                 _infoRepo.Add(info);

                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ExportThemeExcel()
        {
            var themes = await _themeRepo.Items.OrderBy(x => x.SequenceNum).ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Themes");
                worksheet.Cells["A1"].Value = "Id";
                worksheet.Cells["B1"].Value = "ThemeName";
                worksheet.Cells["C1"].Value = "SequenceNum";

                for (int i = 0; i < themes.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = themes[i].Id;
                    worksheet.Cells[i + 2, 2].Value = themes[i].ThemeName;
                    worksheet.Cells[i + 2, 3].Value = themes[i].SequenceNum;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "aapplication/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Themes.xlsx");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportQuestionExcel()
        {
            var questions = await _questionRepo.Items
                .OrderBy(x => x.SequenceNum)
                .OrderBy(x => x.Theme.SequenceNum)
                .ToListAsync();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Themes");
                worksheet.Cells["A1"].Value = "Id";
                worksheet.Cells["B1"].Value = "QuestionName";
                worksheet.Cells["C1"].Value = "SequenceNum";
                worksheet.Cells["D1"].Value = "ThemeId";

                for (int i = 0; i < questions.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = questions[i].Id;
                    worksheet.Cells[i + 2, 2].Value = questions[i].QuestionName;
                    worksheet.Cells[i + 2, 3].Value = questions[i].SequenceNum;
                    worksheet.Cells[i + 2, 4].Value = questions[i].ThemeId;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "aapplication/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Questions.xlsx");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportInfoExcel()
        {
            var infos = await _infoRepo.Items
                .OrderBy(x => x.SequenceNum)
                .OrderBy(x => x.Question.SequenceNum)
                .OrderBy(x => x.Question.Theme.SequenceNum)
                .ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Themes");
                worksheet.Cells["A1"].Value = "Id";
                worksheet.Cells["B1"].Value = "Text";
                worksheet.Cells["C1"].Value = "QuestionId";
                worksheet.Cells["D1"].Value = "SequenceNum";
                worksheet.Cells["E1"].Value = "FormatId";
                worksheet.Cells["F1"].Value = "PhotoPath";
                worksheet.Cells["G1"].Value = "Level";
                worksheet.Cells["H1"].Value = "Link";

                for (int i = 0; i < infos.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = infos[i].Id;
                    worksheet.Cells[i + 2, 2].Value = infos[i].Text;
                    worksheet.Cells[i + 2, 3].Value = infos[i].QuestionId;
                    worksheet.Cells[i + 2, 4].Value = infos[i].SequenceNum;
                    worksheet.Cells[i + 2, 5].Value = infos[i].FormatId;
                    worksheet.Cells[i + 2, 6].Value = infos[i].PhotoPath;
                    worksheet.Cells[i + 2, 7].Value = infos[i].Level;
                    worksheet.Cells[i + 2, 8].Value = infos[i].Link;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "aapplication/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Infos.xlsx");
            }
        }

    }
       
}
