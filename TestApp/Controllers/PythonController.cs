using CsQuery;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TestApp.Controllers
{
    public class PythonController : Controller
    {
        // GET: Python
        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<ActionResult> GetDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://movie.douban.com/top250";
                string responseBody = "";
                Config.HtmlEncoder = HtmlEncoders.None;
                Config.OutputFormatter = OutputFormatters.HtmlEncodingNone;
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    CQ doc = new CQ(responseBody);
                    var rows = doc.Select(".grid_view li");
                    var urlarr = new List<string>();
                    var titlearr = new List<string>();
                    var authorarr = new List<string>();
                    var typearr = new List<string>();
                    var comarr = new List<string>();
                    var languagearr = new List<string>();
                    foreach (var row in rows)
                    {
                        urlarr.Add(row.Cq().Find("a").First().Attr("href"));
                        titlearr.Add(row.Cq().Find(".title").First().Text());
                    }

                    foreach(var urls in urlarr)
                    {
                        response = await client.GetAsync(urls);
                        if (response.IsSuccessStatusCode)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                            doc = new CQ(responseBody);
                            rows = doc.Select("#info span .pl");
                            //导演
                            authorarr.Add(rows.First(e => e.InnerText == "导演").NextElementSibling.FirstElementChild.InnerText);
                            rows = doc.Select("#info .pl");
                            //类型
                            typearr.Add(rows.First(e => e.InnerText == "类型:").NextElementSibling.InnerText);
                            //制片国家/地区
                            comarr.Add(rows.First(e => e.InnerText == "制片国家/地区:").NextSibling.NodeValue.Trim());
                            //语言
                            languagearr.Add(rows.First(e => e.InnerText == "语言:").NextSibling.NodeValue.Trim());
                        }
                            
                    }


                    // 创建工作簿
                    IWorkbook workbook = new XSSFWorkbook();
                    // 创建工作表
                    ISheet sheet = workbook.CreateSheet("Report");
                    // 创建表头
                    var headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("电影名称");
                    headerRow.CreateCell(1).SetCellValue("Url");
                    headerRow.CreateCell(2).SetCellValue("导演");
                    headerRow.CreateCell(3).SetCellValue("类型");
                    headerRow.CreateCell(4).SetCellValue("制片国家/地区");
                    headerRow.CreateCell(5).SetCellValue("语言");

                }
                else
                {
                    Console.WriteLine($"Failed to request {url}. Status code: {response.StatusCode}");
                }

                return Json(new { state = responseBody });
            }
        }
    }
}