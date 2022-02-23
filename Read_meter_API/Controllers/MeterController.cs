using Read_meter_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Read_meter_API.Controllers
{

    public class MeterController : ApiController
    {
        public string Get()
        {
            return "hi";
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Meter/Hell")]
        public HttpResponseMessage Post([FromBody] List<Readers> id)
        {
            try
            {
                string re = "";
                int num = 1;
                var newL = id.Distinct().GroupBy(e => e.id).ToList();
                if (id.Count > 99999)
                {
                    var con = new System.Data.SqlClient.SqlConnection(WebConfigurationManager.ConnectionStrings["Name"].ConnectionString);

                    var cmd1 = new System.Data.SqlClient.SqlCommand("select * from accounts  select * from [InsertMeterRec]", con);
                    cmd1.CommandType = System.Data.CommandType.Text;
                    con.Open();
                    int i = cmd1.ExecuteNonQuery();

                    System.Data.SqlClient.SqlDataAdapter Adaptor = new System.Data.SqlClient.SqlDataAdapter(cmd1);

                    DataSet datas = new DataSet();

                    Adaptor.Fill(datas);

                    List<Account> Lacc = new List<Account>();

                    foreach (DataRow r in datas.Tables[0].Rows)
                    {
                        Account ac = new Account();
                        ac.Accountid = r["account"].ToString();
                        ac.name = r["name"].ToString();
                        ac.Lname = r["lname"].ToString();
                        Lacc.Add(ac);
                    }

                    List<Readers> chk = new List<Readers>();
                    foreach (DataRow r in datas.Tables[1].Rows)
                    {
                        Readers rechk = new Readers();
                        rechk.id = r["id"].ToString();
                        rechk.date = r["date"].ToString();
                        rechk.meter_value = r["value"].ToString();
                        chk.Add(rechk);
                    }
                    con.Close();
                    int err = 0;

                    for (int i1 = 1; i1 < newL.Count; i1++)
                    {
                        if (Lacc.Count(e => e.Accountid == newL[i1].Key) > 0 && chk.Count(r => r.id == newL[i1].Key) == 0)
                        {
                            object s = newL[i1].Key;
                            Readers u = id.Where(e => e.id == s).FirstOrDefault();
                            Regex r = new Regex(@"^[0-9]");
                            if (r.IsMatch(u.meter_value) && u.meter_value.Length < 100000)
                            {
                                var cmd = new System.Data.SqlClient.SqlCommand("insert into InsertMeterRec values('" + u.id + "','" + u.date + "','" + u.meter_value + "')", con);
                                cmd.CommandType = System.Data.CommandType.Text;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //var i = u

                                if (cmd.ExecuteNonQuery() == 1)
                                {
                                    num++;
                                }
                            }
                            con.Close();
                        }
                        else
                        {
                            err++;
                        }

                    }
                    if (num > 1)
                    {
                        int newNum = id.Count - num;
                        re = "inserted " + num.ToString() + ", total error" + newNum.ToString() + "  error:" + err;
                    }
                    else
                    {
                        re = "all data are inserted";
                    }
                }
                else
                {
                     re = "Data Is Too Large";
                }



                HttpResponseMessage response = Request.CreateResponse(re);
                return response;
            
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }

    }
    
}
