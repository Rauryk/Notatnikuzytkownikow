
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notatnikuzytkownikow.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;

namespace Notatnikuzytkownikow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {XML_Table.table.Clear();
            if (!System.IO.File.Exists(XML_Table._path)) 
            {
                using (XmlWriter writer = XmlWriter.Create(XML_Table._path))
                {
                    writer.WriteStartElement("catalog");
                    writer.WriteStartElement("cd");
                    writer.WriteElementString("id", "1");
                    writer.WriteElementString("imie", "Adam");
                    writer.WriteElementString("nazwisko", "Nowak");
                    writer.WriteElementString("DataUrodzenia", "2000-01-01");
                    writer.WriteElementString("plec", "M");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            XDocument doc = XDocument.Load(XML_Table._path);

            foreach (XElement row in doc.Descendants("cd"))
            {
                var cell = new Dictionary<string, string>();

                foreach (XElement element in row.Descendants().Where(p => p.HasElements == false))
                {

                    cell.Add(element.Name.LocalName, element.Value);

                }

                XML_Table.table.Add(cell);

            }

            var _Columns = XML_Table.table.SelectMany(x => x.Keys).Distinct().ToList().ToDictionary(x => x);

            foreach (var item in XML_Table.table)
            {
                foreach (var column in _Columns)
                {

                    item.TryAdd(column.Key, null);
                }
            }


            XML_Table.entity.properties = XML_Table.table;
            return View(XML_Table.entity);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public ActionResult AddMod(int item)
        {
            XML_Table._index =item;
            return View("AddMod") ;
        }

        public ActionResult Save(int item)
        {
            XML_Table._index = item;
            return View("AddMod");
        }

        public ActionResult back()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Export()
        {
        String csv = null;

        foreach (var row in XML_Table.table[0])
        {
            csv += row.Key + ",";

        }

        csv += Environment.NewLine;

        foreach (var item in XML_Table.table)
        {
            foreach(var column in item)
            {
                csv += column.Value + ",";
            }

            csv += Environment.NewLine;
        
        }
        System.IO.File.WriteAllText(XML_Table._pathEXP, csv);

        return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
    public class XML_Table: Controller
    {
        public static DynamicTable entity = new DynamicTable();
        public static int _index;
        public static List<Dictionary<string, string>> table = new List<Dictionary<string, string>>();
        public static string _path = @".\template.xml";
        public static string _pathEXP = @".\rapoer.csv";
    }
}