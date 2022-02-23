using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Read_meter_API.Models
{
    public class Readers
    {
        public string id { get; set; }
        public string date { get; set; }

        public string meter_value { get; set; }
    }
    public class Account
    {
        public string Accountid { get; set; }
        public string name { get; set; }

        public string Lname { get; set; }
    }
}