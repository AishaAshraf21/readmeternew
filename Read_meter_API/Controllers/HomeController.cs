using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Read_meter_API.Models;


namespace Read_meter_API.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View(new List<CustomerModel>());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                //Execute a loop over the rows.
                var lines = System.IO.File.ReadAllLines(filePath);
                List<Reader> r = new List<Reader>();
                foreach (string line in lines)
                {
                    var a = line.Split(',');
                    var r1 = new Reader();

                    r1.id = a[0];
                    r1.date = a[1];
                    r1.meter_value = a[2];

                    r.Add(r1);


                }

                using (var client = new System.Net.WebClient())
                {

                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("Accept:application/json");
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                    var result = client.UploadString("http://localhost:59949/api/Meter/Hell", json);

                    ViewBag.re = result;
                }

            }

            return View();
        }
        public class Readers
        {
            public string id { get; set; }
            public string date { get; set; }

            public string meter_value { get; set; }
        }
    }
    public class CustomerModel
    {
        ///<summary>
        /// Gets or sets CustomerId.
        ///</summary>
        public int CustomerId { get; set; }

        ///<summary>
        /// Gets or sets Name.
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// Gets or sets Country.
        ///</summary>
        public string Country { get; set; }
    
}
        public class Reader
    {
        public string id { get; set; }
        public string date { get; set; }

        public string meter_value { get; set; }
    }
}
