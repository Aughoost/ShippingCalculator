using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.Services.Description;
using TSBLiving.Models;

namespace TSBLiving.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login objUser)
        {
            if (ModelState.IsValid)
            {
                using (TSBLivingEntities db = new TSBLivingEntities())
                {
                    var obj = db.Logins.Where(a => a.Username.Equals(objUser.Username) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.id.ToString();
                        Session["UserName"] = obj.Username.ToString();
                        return RedirectToAction("UserDashBoard");
                    }

                }
            }
            return View(objUser);
        }
        public ActionResult GetAutoCompleteData(string term)
        {
            //var Countries = (from c in db.Calculators
            //                 where c.Name.StartsWith(term)
            //                 select new { c.Name, c.Code });
            //return Json(Countries, JsonRequestBehavior.AllowGet);
            var emp = (from x in db.Calculators where x.Code.StartsWith(term) select new { label = x.Code }).Take(5);

            if (emp.Any())
            {
                return Json(emp);

            }
            else
            {
                var emp1 = (from x in db.Calculators where x.Name.StartsWith(term) select new { label = x.Name }).Take(5);
                return Json(emp1);
            }

        
        }
        public string connectionString = "Data Source=LAPTOP-81PDU62G\\SQLEXPRESS;Initial Catalog=TSBLiving;Integrated Security=True;Application Name=EntityFramework";
        public static DataTable dtbMyDataTable = new DataTable();

        [HttpPost]
        public ActionResult GetWeightData(string code)
        {
            //var Countries = (from c in db.Calculators
            //                 where c.Name.StartsWith(term)
            //                 select new { c.Name, c.Code });
            //return Json(Countries, JsonRequestBehavior.AllowGet);
            //var emp = (from x in db.Calculators where x.Code.StartsWith(value.Weight) select new { label = x.Weight }).ToList();
            //var emp = (from x in db.Calculators where x.Code.Contains(value.Weight) select new { label = x.Weight }).ToList();
            //if (emp.Count == 0)
            //{
            //    var emp1 = (from x in db.Calculators where x.Name.StartsWith(value.Weight) select new { label = x.Weight }).ToList();
            //    return Json(emp1);
            //}

            //return Json(emp);

            //var items = new Models.Items();

            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    string query = "SELECT Weight FROM Calculator WHERE Code= @PRCode;";
            //    using (SqlCommand cmd = new SqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@PRCode", value.Code);
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //       DataSet ds = new DataSet();
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(ds);
            //        items.Code = value.Code;
            //        return Json(ds);
            //    }
            //}
            return Json(new { w = db.Calculators.Where(w => w.Code == code || w.Name == code).Select(s => s.Weight).FirstOrDefault() ?? "" });
        }

        public ActionResult GetLocation(string code)
        {
           
            //var Countries = (from c in db.Calculators
            //                 where c.Name.StartsWith(term)
            //                 select new { c.Name, c.Code });
            //return Json(Countries, JsonRequestBehavior.AllowGet);
            //var emp = (from x in db.Calculators where x.Code.StartsWith(value.Weight) select new { label = x.Weight }).ToList();
            //var emp = (from x in db.Calculators where x.Code.Contains(value.Weight) select new { label = x.Weight }).ToList();
            //if (emp.Count == 0)
            //{
            //    var emp1 = (from x in db.Calculators where x.Name.StartsWith(value.Weight) select new { label = x.Weight }).ToList();
            //    return Json(emp1);
            //}

            //return Json(emp);

            //var items = new Models.Items();

            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    string query = "SELECT Weight FROM Calculator WHERE Code= @PRCode;";
            //    using (SqlCommand cmd = new SqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@PRCode", value.Code);
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //       DataSet ds = new DataSet();
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(ds);
            //        items.Code = value.Code;
            //        return Json(ds);
            //    }
            //}
            return Json(new { w = db.Cities.Where(w => w.PostCode == code || w.Region == code).Select(s => s.Island).FirstOrDefault() ?? "" });
        }
        public ActionResult GetCity(string code)
        {

            var x =  db.Cities.Where(w => w.PostCode == code).Select(s => s.Region).FirstOrDefault();
            var y = db.Cities.Where(w => w.PostCode == code).Select(s => s.Island).FirstOrDefault();

            var response = Json(new { param1 = x, param2 = y });

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        //List<Models.Items> allsearch = db.Calculators.Where(x => x.Code.Contains(term)).Select(x => new Models.Items
        //{
        //    Code = x.Code,
        //    Name = x.Name

        //}).ToList();
        //return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        TSBLivingEntities db = new TSBLivingEntities();


        public ActionResult UserDashBoard()
        {
            //if (Session["UserID"] != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
            var model = new Items();
            //1 = akl
            //2 = wlg
            //3 = Chch

            return View(model);
        }
        [HttpPost]
        public ActionResult UserDashBoard(Items x, string fom)
        {
            //if (Session["UserID"] != null)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
            var model = new Items();

            double TotalWeight = double.Parse(x.SubTotalWeight);
            //x.WeightRange = "0";
            //x.FinalShippingCost = "0";

            //akl to christchurch
            if (x.DispatchID == 1 && x.Location == "South Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "52.00";
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "88.00";
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "35.00";
                    ViewBag.WeightRange = "26kg–29.99kg";
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "82.00";
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "75kg–84.99kg";
                }
                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "195.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "400.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "425.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //wlg  to North island
            if (x.DispatchID == 1 && x.Location == "North Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "24.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "40.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "50.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "30.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "65.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "120.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "150.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "245.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "85.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //AKL  to AKL
            if (x.DispatchID == 1 && x.CityofCust == "Auckland" && x.Location == "North Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "6.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "20.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "25.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "33.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "42.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }

            //wlg to South Island //done
            if (x.DispatchID == 2 && x.Location == "South Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "52.00";
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "88.00";
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "35.00";
                    ViewBag.WeightRange = "26kg–29.99kg";
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "82.00";
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "75kg–84.99kg";
                }
                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "195.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "400.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "425.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //wlg  to North island
            if (x.DispatchID == 2 && x.Location == "North Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "24.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "40.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "50.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "30.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "65.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "120.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "150.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "245.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "85.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //Wlg  to WLG
            if (x.DispatchID == 2 && x.CityofCust == "Wellington" && x.Location == "North Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "6.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "20.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "25.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "33.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "42.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }

            //Christchurch to  South Island
            if (x.DispatchID == 3 && x.Location == "South Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "52.00";
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "88.00";
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "35.00";
                    ViewBag.WeightRange = "26kg–29.99kg";
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "82.00";
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "75kg–84.99kg";
                }
                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "195.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "400.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "425.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //Christchurch  to North island
            if (x.DispatchID == 3 && x.Location == "North Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "24.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "40.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "50.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "30.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "65.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "110.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "120.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "150.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "245.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "85.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }
            //Christchurch  to Christchurch
            if (x.DispatchID == 3 && x.CityofCust == "Christchurch City" && x.Location == "South Island")
            {
                if (TotalWeight <= 1 && TotalWeight < 9.99)
                {
                    ViewBag.FinalShippingCost = "6.00"; //ok
                    ViewBag.WeightRange = "1kg–9.9kg";
                }
                if (TotalWeight >= 10 && TotalWeight < 15.99)
                {
                    ViewBag.FinalShippingCost = "12.00"; //ok
                    ViewBag.WeightRange = "10kg–15.99kg";
                }
                if (TotalWeight >= 16 && TotalWeight < 21.99)
                {
                    ViewBag.FinalShippingCost = "20.00"; //ok
                    ViewBag.WeightRange = "16kg–21.99kg";
                }
                if (TotalWeight >= 22 && TotalWeight < 25.99)
                {
                    ViewBag.FinalShippingCost = "25.00"; //ok
                    ViewBag.WeightRange = "22kg–25.99kg";
                }
                if (TotalWeight >= 26 && TotalWeight < 29.99)
                {
                    ViewBag.FinalShippingCost = "25.00";
                    ViewBag.WeightRange = "26kg–29.99kg"; //ok
                }

                if (TotalWeight >= 30 && TotalWeight < 49.99)
                {
                    ViewBag.FinalShippingCost = "33.00"; //ok
                    ViewBag.WeightRange = "30kg–49.99kg";
                }
                if (TotalWeight >= 75 && TotalWeight < 84.99)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "75kg–84.99kg"; //ok
                }

                if (TotalWeight >= 85 && TotalWeight < 94)
                {
                    ViewBag.FinalShippingCost = "80.00";
                    ViewBag.WeightRange = "85kg–94.99kg";
                }
                if (TotalWeight >= 95 && TotalWeight < 149.99)
                {
                    ViewBag.FinalShippingCost = "100.00";
                    ViewBag.WeightRange = "95kg–149.99kg";
                }
                if (TotalWeight >= 150 && TotalWeight < 800)
                {
                    ViewBag.FinalShippingCost = "175.00";
                    ViewBag.WeightRange = "150kg–800kg";
                }
                if (TotalWeight >= 50 && TotalWeight < 74.99)
                {
                    ViewBag.FinalShippingCost = "42.00";
                    ViewBag.WeightRange = "50kg–74.99kg";
                }
                if (TotalWeight >= 2000 && TotalWeight > 2000)
                {
                    ViewBag.FinalShippingCost = "200.00";
                    ViewBag.WeightRange = "2000kg–2000kg";
                }
            }


            return View(model);
        }


        
    }
}