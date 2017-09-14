using IISLogAnalyzer.Parser;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace IISLogAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
              
        [HttpPost]
        public ActionResult Upload()
        {            
            string filename = System.Guid.NewGuid().ToString();
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {                    
                    var path = Path.Combine(Server.MapPath("~/Docs/"), filename);
                    
                    file.SaveAs(path);
                }
            }

            return RedirectToAction("Analyze", "Parse", new { pageNum = 1, pageSize = 5, filename = filename });
        }
    }
}