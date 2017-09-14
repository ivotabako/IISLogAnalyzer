using IISLogAnalyzer.Parser;
using IISLogAnalyzer.Parser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace IISLogAnalyzer.Controllers
{
    public class ParseController : Controller
    {        
        [HttpGet]
        public ActionResult Analyze(int pageNum, int pageSize, string filename)
        {
            if (pageNum <= 0)
                pageNum = 1;

            if (pageSize <= 0)
                pageSize = 5;

            var path = Path.Combine(Server.MapPath("~/Docs/"), filename);

            ParseService parser = new ParseService();
            var data = parser.ParseLogFileType(path);

            var page = data.Skip((pageNum - 1) * pageSize).Take(pageSize);

            ViewBag.pageNum = pageNum;
            ViewBag.pageSize = pageSize;
            ViewBag.filename = filename;

            return View(page);
        }        
    }
}
