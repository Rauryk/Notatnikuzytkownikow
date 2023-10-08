using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Notatnikuzytkownikow.Models;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace Notatnikuzytkownikow.Controllers
{
    public class Save : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> EditProject([FromForm] Dictionary<string, string> entry)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XML_Table._path);
            XmlNode node = xmlDoc.SelectSingleNode("/catalog");
            XmlElement _catalog_node = xmlDoc.CreateElement("cd");

            int i = 0;
            foreach (var item in entry)
            { i++;
                if(i !=1 && entry.Count()-2 > i)
                {
                    var a = item.Value;
                    if (xmlDoc.SelectSingleNode("/catalog/cd[" + entry["id"] +"]/"+item.Key) != null)
                        xmlDoc.SelectSingleNode("/catalog/cd[" + entry["id"] +"]/"+item.Key).InnerText =  item.Value;
                    else
                    {
                        if (item.Value != null)
                        {
                            XmlNode nonFuel = xmlDoc.SelectSingleNode("/catalog/cd[" + entry["id"] + "]");
                            if (nonFuel == null) 
                            {
                                New(entry);
                                return RedirectToAction("Index", "Home");
                            } 
                            XmlNode dispute = xmlDoc.SelectSingleNode("cd");
                            
                            
                            XmlNode xmlRecordNo = xmlDoc.CreateNode(XmlNodeType.Element, item.Key, null);
                            xmlRecordNo.InnerText = item.Value;
                            nonFuel.InsertBefore(xmlRecordNo, dispute);
                        }
                    }
                }

            }
            if (!string.IsNullOrEmpty( entry["NewDictiKey"]) && !string.IsNullOrEmpty(entry["NewDictiValue"]))
            {
                XmlNode nonFuel = xmlDoc.SelectSingleNode("/catalog/cd[" + entry["id"] + "]");
                XmlNode dispute = xmlDoc.SelectSingleNode("cd");

                XmlNode xmlRecordNo = xmlDoc.CreateNode(XmlNodeType.Element, entry["NewDictiKey"], null);
                xmlRecordNo.InnerText = entry["NewDictiValue"];
                nonFuel.InsertBefore(xmlRecordNo, dispute);
            }
            xmlDoc.Save(XML_Table._path);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> New([FromForm] Dictionary<string, string> entry)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(XML_Table._path);
            XmlNode node = xmlDoc.SelectSingleNode("/catalog");
            XmlElement _catalog_node = xmlDoc.CreateElement("cd");

            int i =0;
            foreach (var item in entry)
            {
                i++;

                if (i != 1 && entry.Count() - 2 > i)
                {
                    XmlElement _new_node = xmlDoc.CreateElement(item.Key);
                    _catalog_node.AppendChild(_new_node);
                    _new_node.InnerText = item.Value;
                    xmlDoc.DocumentElement.AppendChild(_catalog_node);
                }
                

            }
            if (!string.IsNullOrEmpty(entry["NewDictiKey"]) && !string.IsNullOrEmpty(entry["NewDictiValue"]))
            {
                XmlNode nonFuel = xmlDoc.SelectSingleNode("/catalog/cd[" + entry["id"] + "]");
                XmlNode dispute = xmlDoc.SelectSingleNode("cd");


                XmlNode xmlRecordNo = xmlDoc.CreateNode(XmlNodeType.Element, entry["NewDictiKey"], null);
                xmlRecordNo.InnerText = entry["NewDictiValue"];
                nonFuel.InsertBefore(xmlRecordNo, dispute);
            }

            xmlDoc.Save(XML_Table._path) ;

            return RedirectToAction("Index", "Home");
        }
    }
}
