using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using openpaySDKDemo.Models;
using System.Data.SqlClient;
using System.Data;
using openpaySDKDemo.Helper;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Web.Configuration;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using openpaySDK;


namespace openpaySDKDemo.Controllers
{
    public class openpayController : Controller
    {
        //
        // GET: /openpay/
        public ActionResult Index()
        {
            Cart_Val();
            //int AUTH_USER_ID = Convert.ToInt32(Request.ServerVariables["AUTH_USER"]);

            //HttpCookie mycookie = new HttpCookie("mycookie");
            //mycookie.Value = "value1";
            //mycookie.Expires = DateTime.Now.AddDays(100);
            //Response.Cookies.Add(mycookie);
            ////Response.SetCookie(mycookie);
            //HttpCookie mycookie1 = Request.Cookies["mycookie"];        
            if (Session["TempDataForCookies"] == null)
            {
                Session["TempDataForCookies"] = null;
            }            

            ViewBag.TabActive = "Index";
            return View();
        }
        [Authorize]
        public ActionResult Profile()
        {
            Cart_Val();
            ViewBag.ContentShow = "No";
            ViewBag.TabActive = "Index";
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Orders] where [UserId] = " + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ViewBag.ContentShow = "Yes";
            }
            else
            {
                ViewBag.ContentShow = "No";
            }
            string query1 = "select * from [Users] where [id] = " + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]);
            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            if (dt1.Rows.Count > 0)
            {
                ViewBag.Name = dt1.Rows[0]["Name"].ToString();
                ViewBag.Email = dt1.Rows[0]["Email"].ToString();
            }
            else
            {

            }
            con.Close();
            return View();
        }
        public ActionResult Products()
        {
            Cart_Val();
            ViewBag.TabActive = "Products";           
            return View();
        }
        public ActionResult ProductDetails(int id)
        {
            Cart_Val();
            ViewBag.TabActive = "ProductDetails";            
            ViewBag.Id = "";
            ViewBag.PName = "";
            ViewBag.PDescription = "";
            ViewBag.Price = "";
            ViewBag.ImagePath = "";
            if(id == 1)
            {
                ViewBag.Id = "1";
                ViewBag.PName = "Sony Headphone";
                ViewBag.PDescription = "Sony 310AP Wired Headset with Mic";
                ViewBag.Price = "99";
                ViewBag.ImagePath = "sony.jpeg";
            }
            else if (id == 2)
            {
                ViewBag.Id = "2";
                ViewBag.PName = "JBL Headset";
                ViewBag.PDescription = "JBL T450BT Wired Headset";
                ViewBag.Price = "69";
                ViewBag.ImagePath = "jbl.jpeg";
            }
            else if (id == 3)
            {
                ViewBag.Id = "3";
                ViewBag.PName = "Boat Earphone";
                ViewBag.PDescription = "boAt Rockerz 400 Wireless";
                ViewBag.Price = "35";
                ViewBag.ImagePath = "boat.jpeg";
            }
            else if (id == 4)
            {
                ViewBag.Id = "4";
                ViewBag.PName = "Sony Headphone";
                ViewBag.PDescription = "Sony 310AP Wired Headset with Mic";
                ViewBag.Price = "99";
                ViewBag.ImagePath = "sony.jpeg";
            }
            else if (id == 5)
            {
                ViewBag.Id = "5";
                ViewBag.PName = "JBL Headset";
                ViewBag.PDescription = "JBL T450BT Wired Headset";
                ViewBag.Price = "69";
                ViewBag.ImagePath = "jbl.jpeg";
            }
            else if (id == 6)
            {
                ViewBag.Id = "6";
                ViewBag.PName = "Boat Earphone";
                ViewBag.PDescription = "boAt Rockerz 400 Wireless";
                ViewBag.Price = "35";
                ViewBag.ImagePath = "boat.jpeg";
            }           
            return View();
        }
        [Authorize]
        public ActionResult Orders()
        {
            Cart_Val();
            ViewBag.TabActive = "Orders";
            ViewBag.IsAdmin = "No";
            List<ModelopenpayAPI.Orders> list_tempDataForOrders = new List<ModelopenpayAPI.Orders>();
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string queryAdmin = "select * from [Users] where [id] = " + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]);
            DataTable dtAdmin = new DataTable();
            SqlCommand cmdAdmin = new SqlCommand(queryAdmin, con);
            SqlDataAdapter daAdmin = new SqlDataAdapter(cmdAdmin);
            daAdmin.Fill(dtAdmin);
            if (dtAdmin.Rows.Count > 0)
            {
                string query = "select * from [Orders] where [UserId] = '" + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + "' order by id desc ";
                if (dtAdmin.Rows[0]["Email"].ToString().ToLower().Trim() == "admin@gmail.com")
                {
                    query = "select * from [Orders] order by id desc ";
                    ViewBag.IsAdmin = "Yes";
                }
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int sn = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sn = sn + 1;
                    ModelopenpayAPI.Orders tempDataForOrders = new ModelopenpayAPI.Orders
                    {
                        sn = sn,
                        id = int.Parse(dt.Rows[i]["id"].ToString()),
                        Date = Convert.ToDateTime(dt.Rows[i]["Date"].ToString()),
                        PlanId = dt.Rows[i]["PlanId"].ToString(),
                        OrderId = dt.Rows[i]["OrderId"].ToString(),
                        Amount = Math.Floor(Convert.ToDouble(dt.Rows[i]["Amount"].ToString()) * 100) / 100,
                        NewPurchasePrice = Math.Floor(Convert.ToDouble(dt.Rows[i]["NewPurchasePrice"].ToString()) * 100) / 100,
                        ReducePriceBy = Math.Floor(Convert.ToDouble(dt.Rows[i]["ReducePriceBy"].ToString()) * 100) / 100,
                        FullRefund = Convert.ToBoolean(dt.Rows[i]["FullRefund"].ToString()),
                        IsDispatch = Convert.ToBoolean(dt.Rows[i]["IsDispatch"].ToString()),
                        Dispatch = dt.Rows[i]["Dispatch"].ToString(),
                        Action = dt.Rows[i]["Action"].ToString(),
                        Status = dt.Rows[i]["Status"].ToString(),
                        IsShow = Convert.ToBoolean(dt.Rows[i]["IsShow"].ToString()),
                        UserId = int.Parse(dt.Rows[i]["UserId"].ToString())
                    };
                    list_tempDataForOrders.Add(tempDataForOrders);
                }
            }

            con.Close();
            ViewBag.Orders = list_tempDataForOrders;
            return View();
        }
        public ActionResult SignIn()
        {
            Cart_Val();
            ViewBag.ReturnUrl = Request.QueryString["ReturnUrl"];
            ViewBag.TabActive = "Index";
            ViewBag.SignInPage = "SignInPage";
            ViewBag.ValidationMsg = "";
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Modelopenpay.SignInSuccessModel _modelSI, string ReturnUrl)
        {
            Cart_Val();
            //try
            //{
                
            //}
            //catch(Exception ex)
            //{
            //    Response.Write(ex.Message.ToString());
            //}

            ViewBag.ValidationMsg = "";
            ViewBag.ContentShow = "No";
            string Email = Request.Form["email"];
            string Password = Request.Form["password"];
            ReturnUrl = Request.Form["ReturnUrl"];
            //if (_modelSI != null)
            //{
            string EmpPassword = _modelSI.Password.Trim();
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Users] where [email] = '" + Email.Trim() + "' and [password] ='" + Password.Trim() + "' ";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                HttpCookie AuthCookie;
                System.Web.Security.FormsAuthentication.SetAuthCookie(dt.Rows[0]["id"].ToString(), false);
                AuthCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(dt.Rows[0]["id"].ToString(), true);
                AuthCookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(AuthCookie);
                if (ReturnUrl.Contains("/openpay/"))
                {
                    Response.Redirect("~" + ReturnUrl);
                }
                else
                {
                    Response.Redirect("~/openpay/Profile");
                }
                //Response.Redirect("~/openpay/SignUp",false);    
            }
            else
            {
                ViewBag.ValidationMsg = "Please enter correct email address or password";
            }
            con.Close();
            //}

            return View();

        }
        public ActionResult SignUp()
        {
            Cart_Val();
            ViewBag.TabActive = "Index";
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string id)
        {
            Cart_Val();
            ViewBag.ExistEmail = "";
            ViewBag.TabActive = "Index";
            string Name = Request.Form["name"];
            string Email = Request.Form["email"];
            string Password = Request.Form["password"];
            string Token = Request.Form["_token"];


            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Users] where [email] = '" + Email.Trim() + "' ";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ViewBag.ExistEmail = "ExistEmail";
            }
            else
            {
                ViewBag.ExistEmail = "";
                string query1 = " insert into [Users] values('" + DateTime.Now.ToString("MM/dd/yyy HH:mm:ss") + "','" + Name.Trim() + "','" + Email.Trim() + "','" + Password.Trim() + "','" + Token.Trim() + "') ";
                SqlCommand cmd1 = new SqlCommand(query1, con);
                cmd1.ExecuteNonQuery();

                string query2 = "select * from [Users] where [email] = '" + Email.Trim() + "' and [password] ='" + Password.Trim() + "' ";
                DataTable dt2 = new DataTable();
                SqlCommand cmd2 = new SqlCommand(query2, con);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    HttpCookie AuthCookie;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(dt2.Rows[0]["id"].ToString(), false);
                    AuthCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(dt2.Rows[0]["id"].ToString(), true);
                    AuthCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(AuthCookie);
                    Response.Redirect("~/openpay/Profile");
                    //Response.Redirect("~/openpay/SignUp",false);                   
                }
            }

            con.Close();
            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl);
            return View();
        }
        public ActionResult Cart()
        {            
            ViewBag.TabActive = "Index";
            List<Logic.TempDataForCookies> list_tempDataForCookies = (List<Logic.TempDataForCookies>)Session["TempDataForCookies"];
            List<Logic.TempDataForCookies> list_tempDataForCookies1 = new List<Logic.TempDataForCookies>();
            Logic.TempDataForCookies tempDataForCookies1 = new Logic.TempDataForCookies();
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "Boat Earphone",
                Description = "boAt Rockerz 400 Wireless",
                Price = 35,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "JBL Headset",
                Description = "JBL T450BT Wired Headset",
                Price = 69,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "Sony Headphone",
                Description = "Sony 310AP Wired Headset with Mic",
                Price = 99,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);


            int TotalPriceOP = 0;
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "Boat Earphone")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "Boat Earphone")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "JBL Headset")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "JBL Headset")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "Sony Headphone")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "Sony Headphone")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                if (list_tempDataForCookies1[i].TPrice != 0)
                {
                    TotalPriceOP = TotalPriceOP + list_tempDataForCookies1[i].TPrice;
                }
            }
            if (TotalPriceOP == 0)
            {
                ViewBag.CartBuild = new List<Logic.TempDataForCookies>();
            }
            else
            {
                ViewBag.CartBuild = list_tempDataForCookies1;
            }
            ViewBag.TotalPriceOP = TotalPriceOP;
            return View();
        }
        public List<Logic.TempDataForCookies> Cart_Val()
        {
            List<Logic.TempDataForCookies> return_val = new List<Logic.TempDataForCookies>();
            List<Logic.TempDataForCookies> list_tempDataForCookies = (List<Logic.TempDataForCookies>)Session["TempDataForCookies"];
            List<Logic.TempDataForCookies> list_tempDataForCookies1 = new List<Logic.TempDataForCookies>();
            Logic.TempDataForCookies tempDataForCookies1 = new Logic.TempDataForCookies();
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "Boat Earphone",
                Description = "boAt Rockerz 400 Wireless",
                Price = 35,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "JBL Headset",
                Description = "JBL T450BT Wired Headset",
                Price = 69,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);
            tempDataForCookies1 = new Logic.TempDataForCookies
            {
                Id = 0,
                Name = "Sony Headphone",
                Description = "Sony 310AP Wired Headset with Mic",
                Price = 99,
                Quantity = 0
            };
            list_tempDataForCookies1.Add(tempDataForCookies1);


            int TotalPriceOP = 0;
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "Boat Earphone")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "Boat Earphone")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "JBL Headset")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "JBL Headset")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                int PriceOP = 0;
                int QuantityOP = 0;
                int IndiTotalPriceOP = 0;
                if (list_tempDataForCookies1[i].Name == "Sony Headphone")
                {
                    if (list_tempDataForCookies != null)
                    {
                        for (int j = 0; j < list_tempDataForCookies.Count(); j++)
                        {
                            if (list_tempDataForCookies[j].Name == "Sony Headphone")
                            {
                                list_tempDataForCookies1[i].Id = list_tempDataForCookies[j].Id;
                                PriceOP = list_tempDataForCookies[j].Price;
                                IndiTotalPriceOP = IndiTotalPriceOP + list_tempDataForCookies[j].Price;
                                QuantityOP = QuantityOP + list_tempDataForCookies[j].Quantity;
                                list_tempDataForCookies1[i].Price = PriceOP;
                                list_tempDataForCookies1[i].Quantity = QuantityOP;
                                list_tempDataForCookies1[i].TPrice = IndiTotalPriceOP;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < list_tempDataForCookies1.Count(); i++)
            {
                if (list_tempDataForCookies1[i].TPrice != 0)
                {
                    TotalPriceOP = TotalPriceOP + list_tempDataForCookies1[i].TPrice;
                }
            }
            if (TotalPriceOP == 0)
            {
                ViewBag.CartBuild = new List<Logic.TempDataForCookies>();
            }
            else
            {
                ViewBag.CartBuild = list_tempDataForCookies1;
            }
            ViewBag.TotalPriceOP = TotalPriceOP;
            return return_val;
        }
        [Authorize]
        public ActionResult CheckOut(string p)
        {
            Cart_Val();
            ViewBag.TabActive = "Index";
            ViewBag.TotalChcekOutVal = p;
            ViewBag.ViewMsg = "";
            ViewBag.ValidationMsg = "";


            ViewBag.Date = "";
            ViewBag.FirstName = "";
            ViewBag.LastName = "";
            ViewBag.Address = "";
            ViewBag.openpayEmail = "";
            ViewBag.DOB = "";
            ViewBag.Subrub = "";
            ViewBag.State = "";
            ViewBag.PostCode = "";
          
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select top 1* from [Users_Details] where [UserId] = '" + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + "' order by id desc ";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ViewBag.Date = dt.Rows[0]["Date"].ToString();
                ViewBag.FirstName = dt.Rows[0]["FirstName"].ToString();
                ViewBag.LastName = dt.Rows[0]["LastName"].ToString();
                ViewBag.Address = dt.Rows[0]["Address"].ToString();
                ViewBag.openpayEmail = dt.Rows[0]["openpayEmail"].ToString();
                ViewBag.DOB = dt.Rows[0]["DOB"].ToString();
                ViewBag.Subrub = dt.Rows[0]["Subrub"].ToString();
                ViewBag.State = dt.Rows[0]["State"].ToString();
                ViewBag.PostCode = dt.Rows[0]["PostCode"].ToString();                             
            }
            con.Close();


            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CheckOut(Modelopenpay.CheckOutModel _modelCO, string ReturnURL)
        {
            Cart_Val();
            decimal _totalprice = Convert.ToDecimal(Request.Form["_totalprice"]);
            ViewBag.TotalChcekOutVal = _totalprice;
            ViewBag.TabActive = "Index";
            ViewBag.ValidationMsg = "";
            _modelCO.DOB = _modelCO.DOB.Trim();
            try
            {
                DateTime _dob = DateTime.ParseExact(_modelCO.DOB, "dd MMM yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    DateTime _dob = DateTime.ParseExact(_modelCO.DOB, "d MMM yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    _modelCO = null;
                    ViewBag.ValidationMsg = "Please enter correct date format(dd MMM yyyy eg. " + DateTime.Now.ToString("dd MMM yyyy") + ")";
                }
            }
            if (_modelCO != null)
            {
                string _orderID = "OP";

                SqlConnection con = new SqlConnection(ConnectionString.Connection);
                con.Open();

                string query_select_UserDetails = "select top 1* from [Users_Details] where [UserId] = '" + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + "' order by id desc ";
                DataTable dt_select_UserDetails = new DataTable();
                SqlCommand cmd_select_UserDetails = new SqlCommand(query_select_UserDetails, con);
                SqlDataAdapter da_select_UserDetails = new SqlDataAdapter(cmd_select_UserDetails);
                da_select_UserDetails.Fill(dt_select_UserDetails);
                if (dt_select_UserDetails.Rows.Count == 0)
                {
                    string query_insert_UserDetails = " insert into [Users_Details] values('" + DateTime.Now.ToString("MM/dd/yyy HH:mm:ss") + "','" + _modelCO.FirstName + "','" + _modelCO.LastName + "','" + _modelCO.Address + "','" + _modelCO.Email + "','" + _modelCO.DOB + "','" + _modelCO.Subrub + "','" + _modelCO.State + "','" + _modelCO.PostCode + "','',1," + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + ") ";
                    SqlCommand cmd_insert_UserDetails = new SqlCommand(query_insert_UserDetails, con);
                    cmd_insert_UserDetails.ExecuteNonQuery();
                }
                else
                {
                    string query_update_UserDetails = " update [Users_Details] set FirstName = '" + _modelCO.FirstName + "', LastName = '" + _modelCO.LastName + "', Address= '" + _modelCO.Address + "', openpayEmail = '" + _modelCO.Email + "', [DOB] = '" + _modelCO.DOB + "', Subrub = '" + _modelCO.Subrub + "', State = '" + _modelCO.State + "', PostCode = '" + _modelCO.PostCode + "' where [UserId] = '" + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + "' ";
                    SqlCommand cmd_update_UserDetails = new SqlCommand(query_update_UserDetails, con);
                    cmd_update_UserDetails.ExecuteNonQuery();
                }

                string query = "select top 1* from [Orders] order by id desc ";
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int orderID = int.Parse(dt.Rows[0]["OrderId"].ToString().Replace("OP", "").Trim());
                    orderID = orderID + 1;
                    for (int i = 0; i < 8 - orderID.ToString().Length; i++)
                    {
                        _orderID = _orderID + "0";
                    }
                    _orderID = _orderID + orderID;
                }
                else
                {
                    _orderID = "OP00000001";
                }

                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestNewOnlineOrder _RequestNewOnlineOrder = new openpayModels.RequestNewOnlineOrder();

                _RequestNewOnlineOrder.PurchasePrice = _totalprice;
                _RequestNewOnlineOrder.PlanCreationType = "Pending";
                _RequestNewOnlineOrder.RetailerOrderNo = _orderID;
                _RequestNewOnlineOrder.ChargeBackCount = 0;
                _RequestNewOnlineOrder.CustomerQuality = 1;

                _RequestNewOnlineOrder.FirstName = _modelCO.FirstName;
                _RequestNewOnlineOrder.OtherNames = "";
                _RequestNewOnlineOrder.FamilyName = _modelCO.LastName;
                _RequestNewOnlineOrder.Email = _modelCO.Email;
                _RequestNewOnlineOrder.DateOfBirth = _modelCO.DOB;
                _RequestNewOnlineOrder.Gender = "M";
                _RequestNewOnlineOrder.PhoneNumber = "";
                _RequestNewOnlineOrder.ResAddress1 = _modelCO.Address;
                _RequestNewOnlineOrder.ResAddress2 = "";
                _RequestNewOnlineOrder.ResSuburb = _modelCO.Subrub;
                _RequestNewOnlineOrder.ResState = _modelCO.State;
                _RequestNewOnlineOrder.ResPostCode = _modelCO.PostCode;
                _RequestNewOnlineOrder.DeliveryDate = DateTime.Now.ToString("dd MMM yyyy");
                _RequestNewOnlineOrder.DelAddress1 = _modelCO.Address;
                _RequestNewOnlineOrder.DelAddress2 = "";
                _RequestNewOnlineOrder.DelSuburb = _modelCO.Subrub;
                _RequestNewOnlineOrder.DelState = _modelCO.State;
                _RequestNewOnlineOrder.DelPostCode = _modelCO.PostCode;

                _req_call.NewOnlineOrder = _RequestNewOnlineOrder;

                openpayModels.Response_Call _res_call = _openpayMethods.openpayNewOnlineOrder(_req_call);

                // entry in database
                string query_insert = " insert into [Orders] values('" + DateTime.Now.ToString("MM/dd/yyy HH:mm:ss") + "','" + _res_call.NewOnlineOrder.PlanID.Trim() + "','" + _RequestNewOnlineOrder.RetailerOrderNo.Trim() + "','" + _RequestNewOnlineOrder.PurchasePrice + "','" + _RequestNewOnlineOrder.PurchasePrice + "','0.00',0,0,'Not Yet Dispatch','','',1," + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + ") ";
                SqlCommand cmd_insert = new SqlCommand(query_insert, con);
                cmd_insert.ExecuteNonQuery();
                con.Close();
            }
            return View();
        }
        [Authorize]
        public ActionResult CallBack(string status, string planid, string orderid)
        {
            Cart_Val();
            string _statusDatabase = "";
            ViewBag.TabActive = "Index";
            if (status.ToUpper() == "LODGED" || status.ToUpper() == "SUCCESS")
            {
                ViewBag.CalBackMsg = "Successfully purchased products!";
                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderCapturePayment _RequestOnlineOrderCapturePayment = new openpayModels.RequestOnlineOrderCapturePayment();
                _RequestOnlineOrderCapturePayment.PlanID = planid;
                _req_call.OnlineOrderCapturePayment = _RequestOnlineOrderCapturePayment;

                try
                {
                    openpayModels.Response_Call _res_call = _openpayMethods.openpayOnlineOrderCapturePayment(_req_call);
                    if (_res_call.OnlineOrderCapturePayment.status == "0")
                    {
                        ViewBag.CalBackMsg = "You have purchased the products price $" + _res_call.OnlineOrderCapturePayment.PurchasePrice + " successfully!";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Order is placed successfully</font></li>";
                    }
                    else
                    {
                        ViewBag.CalBackMsg = _res_call.OnlineOrderCapturePayment.reason;
                        _statusDatabase = _res_call.OnlineOrderCapturePayment.reason;
                    }                    
                }
                catch (Exception ex)
                {

                }
            }
            else if (status.ToUpper() == "CANCELLED")
            {
                ViewBag.CalBackMsg = "Your order has been cancelled";
                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Your order has been cancelled</font></li>";
            }
            else if (status.ToUpper() == "FAILURE")
            {
                ViewBag.CalBackMsg = "Product is not purchased successfully!";
                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Product is not purchased successfully!</font></li>";
            }
            else if (status != null && status != "")
            {
                ViewBag.CalBackMsg = status;
                _statusDatabase = status;
            }
            clearsession();
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = " update [Orders] set [Status] = '" + _statusDatabase + "', [OrderId] = '" + orderid + "' where PlanId = '" + planid + "' ";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            return View();
        }
        [Authorize]
        public ActionResult Success()
        {
            Cart_Val();
            ViewBag.TabActive = "Index";
            clearsession();
            return View();
        }
        [Authorize]
        public ActionResult Failure()
        {
            Cart_Val();
            ViewBag.TabActive = "Index";
            return View();
        }
        public string Refund(string id, decimal refundValue)
        {
            string returnval = "";
            string _statusDatabase = "";
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Orders] where id = " + int.Parse(id);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                decimal NewPurchasePrice = Convert.ToDecimal(dt.Rows[0]["NewPurchasePrice"].ToString()) - refundValue;
                decimal ReducePriceBy = refundValue;
                decimal TotalRefund = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString()) - NewPurchasePrice;
                bool FullRefund = false;
                if (NewPurchasePrice == Convert.ToDecimal("0"))
                {
                    FullRefund = true;
                }

                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderReduction _RequestOnlineOrderReduction = new openpayModels.RequestOnlineOrderReduction();
                _RequestOnlineOrderReduction.PlanID = dt.Rows[0]["PlanId"].ToString();
                _RequestOnlineOrderReduction.NewPurchasePrice = NewPurchasePrice;
                _RequestOnlineOrderReduction.ReducePriceBy = ReducePriceBy;
                _RequestOnlineOrderReduction.FullRefund = FullRefund;
                _req_call.OnlineOrderReduction = _RequestOnlineOrderReduction;

                _statusDatabase = dt.Rows[0]["Status"].ToString();

                try
                {
                    openpayModels.Response_Call _res_call = _openpayMethods.openpayOnlineOrderReduction(_req_call);
                    string final_message = "";
                    if (_res_call.OnlineOrderReduction.status == "0")
                    {
                        if (FullRefund == true)
                        {
                            _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Full Amount Refunded</font></li>" + _statusDatabase;
                            returnval = "<strong>Full Amount Refunded</strong>  ";
                        }
                        else
                        {
                            _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">$" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + " is refunded out of $" + Math.Floor(Convert.ToDouble(dt.Rows[0]["Amount"].ToString()) * 100) / 100 + "</font></li>" + _statusDatabase;
                            returnval = "<strong>$" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + " refunded</strong>  ";
                        }
                        string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "', NewPurchasePrice = '" + Math.Floor(Convert.ToDouble(NewPurchasePrice) * 100) / 100 + "', ReducePriceBy = '" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + "', FullRefund = '" + FullRefund + "' where id = " + int.Parse(id);
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderReduction.reason + "</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderReduction.reason + "</font></li>" + _statusDatabase;
                        returnval = final_message;
                        if (_statusDatabase != "New Purchase Price / Reduce Price By is less than or equal to zero" && _statusDatabase != "")
                        {
                            string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
                            SqlCommand cmd1 = new SqlCommand(query1, con);
                            cmd1.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            con.Close();
            return returnval;
        }
        public string OrderStatus(string id)
        {
            string returnval = "";
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Orders] where id = " + int.Parse(id);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderStatus _RequestOnlineOrderStatus = new openpayModels.RequestOnlineOrderStatus();
                _RequestOnlineOrderStatus.PlanID = dt.Rows[0]["PlanId"].ToString();
                _req_call.OnlineOrderStatus = _RequestOnlineOrderStatus;

                openpayModels.Response_Call _res_call = _openpayMethods.openpayOnlineOrderStatus(_req_call);
                returnval = "<div><p>Order Status: " + _res_call.OnlineOrderStatus.OrderStatus + "</p><p>Plan Status: " + _res_call.OnlineOrderStatus.PlanStatus + "</p><p>Purchased Price: " + _res_call.OnlineOrderStatus.PurchasePrice + "</p></div>";

                returnval = returnval + "<ul>" + dt.Rows[0]["Status"].ToString() + "</ul>";
            }
            con.Close();
            return returnval;
        }
        public string Dispatch(string id)
        {
            string returnval = "";
            string _statusDatabase = "";
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Orders] where id = " + int.Parse(id);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderDispatchPlan _RequestOnlineOrderDispatchPlan = new openpayModels.RequestOnlineOrderDispatchPlan();
                _RequestOnlineOrderDispatchPlan.PlanID = dt.Rows[0]["PlanId"].ToString();
                _req_call.OnlineOrderDispatchPlan = _RequestOnlineOrderDispatchPlan;

                _statusDatabase = dt.Rows[0]["Status"].ToString();
                try
                {
                    openpayModels.Response_Call _res_call = _openpayMethods.openpayOnlineOrderDispatchPlan(_req_call);

                    string final_message = "";
                    if (_res_call.OnlineOrderDispatchPlan.status == "0")
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>" + _statusDatabase;
                        string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "', [IsDispatch] = 'True' ,[Dispatch] = 'Dispatched' where id = " + int.Parse(id);
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderDispatchPlan.reason + "</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderDispatchPlan.reason + "</font></li>" + _statusDatabase;
                        string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.ExecuteNonQuery();
                    }
                    returnval = final_message;
                }
                catch (Exception ex)
                {

                }
            }
            con.Close();
            return returnval;
        }
        public string FraudAnalysis(string id, string FAValue)
        {
            if (FAValue == null)
            {
                FAValue = "";
            }
            FAValue = FAValue.Trim();
            string returnval = "";
            string _statusDatabase = "";
            SqlConnection con = new SqlConnection(ConnectionString.Connection);
            con.Open();
            string query = "select * from [Orders] where id = " + int.Parse(id);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderFraudAlert _RequestOnlineOrderFraudAlert = new openpayModels.RequestOnlineOrderFraudAlert();
                _RequestOnlineOrderFraudAlert.PlanID = dt.Rows[0]["PlanId"].ToString();
                _RequestOnlineOrderFraudAlert.Details = FAValue;
                _req_call.OnlineOrderFraudAlert = _RequestOnlineOrderFraudAlert;
                try
                {
                    openpayModels.Response_Call _res_call = _openpayMethods.openpayOnlineOrderFraudAlert(_req_call);
                    _statusDatabase = dt.Rows[0]["Status"].ToString();
                    string final_message = "";
                    if (_res_call.OnlineOrderFraudAlert.status == "0")
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderFraudAlert.reason + " </font> (" + FAValue + ")</li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderFraudAlert.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;
                        string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.ExecuteNonQuery();
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderFraudAlert.reason + " </font> (" + FAValue + ")</li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + _res_call.OnlineOrderFraudAlert.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;
                        string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.ExecuteNonQuery();
                    }
                    returnval = final_message;
                }
                catch (Exception ex)
                {

                }
            }
            con.Close();
            return returnval;
        }
        public openpayModels.Request_Call RequestVal()
        {
            openpayModels.Request_Call _req_call = new openpayModels.Request_Call();
            openpayModels.Settings _req_Settings = new openpayModels.Settings();
            if (WebConfigurationManager.AppSettings["_Location"] == "AU")
            {
                _req_Settings.JamToken = WebConfigurationManager.AppSettings["_JamTokenAU"];
                _req_Settings.AuthToken = WebConfigurationManager.AppSettings["_AuthTokenAU"];
            }
            else if (WebConfigurationManager.AppSettings["_Location"] == "UK")
            {
                _req_Settings.JamToken = WebConfigurationManager.AppSettings["_JamTokenUK"];
                _req_Settings.AuthToken = WebConfigurationManager.AppSettings["_AuthTokenUK"];
            }
            openpayModels.Location _req_Location = new openpayModels.Location();
            _req_Location.Code = WebConfigurationManager.AppSettings["_Location"];

            openpayModels.URL _req_URL = new openpayModels.URL();
            _req_URL.IsLiveURL = Convert.ToBoolean(WebConfigurationManager.AppSettings["_LiveURL"]);
            _req_URL.CallbackURL = "http://localhost:8495/openpay/CallBack";
            _req_URL.CancelURL = "http://localhost:8495/openpay/CallBack";
            _req_URL.FailURL = "http://localhost:8495/openpay/CallBack";

            _req_Settings.Location = _req_Location;
            _req_Settings.URL = _req_URL;
            _req_call.Settings = _req_Settings;
            return _req_call;
        }
        public void AddToCart(string id, string page)
        {
            List<Logic.TempDataForCookies> list_tempDataForCookies = new List<Logic.TempDataForCookies>();
            if (Session["TempDataForCookies"] != null)
            {
                list_tempDataForCookies = (List<Logic.TempDataForCookies>)Session["TempDataForCookies"];
            }
            if (id == "1")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 1,
                    Name = "Sony Headphone",
                    Description = "Sony 310AP Wired Headset with Mic",
                    Price = 99,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Index");
                }
            }
            else if (id == "2")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 2,
                    Name = "JBL Headset",
                    Description = "JBL T450BT Wired Headset",
                    Price = 69,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Index");
                }
            }
            else if (id == "3")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 3,
                    Name = "Boat Earphone",
                    Description = "boAt Rockerz 400 Wireless",
                    Price = 35,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Index");
                }
            }
            else if (id == "4")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 4,
                    Name = "Sony Headphone",
                    Description = "Sony 310AP Wired Headset with Mic",
                    Price = 99,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Products");
                }
            }
            else if (id == "5")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 5,
                    Name = "JBL Headset",
                    Description = "JBL T450BT Wired Headset",
                    Price = 69,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Products");
                }
            }
            else if (id == "6")
            {
                Logic.TempDataForCookies tempDataForCookies = new Logic.TempDataForCookies
                {
                    Id = 6,
                    Name = "Boat Earphone",
                    Description = "boAt Rockerz 400 Wireless",
                    Price = 35,
                    Quantity = 1
                };
                list_tempDataForCookies.Add(tempDataForCookies);
                Session["TempDataForCookies"] = list_tempDataForCookies;
                if (page == "productdetails")
                {
                    Response.Redirect("~/openpay/ProductDetails/" + id);
                }
                else
                {
                    Response.Redirect("~/openpay/Products");
                }
            }
        }
        public void DeductToCart(string id)
        {
            List<Logic.TempDataForCookies> list_tempDataForCookies = new List<Logic.TempDataForCookies>();
            if (Session["TempDataForCookies"] != null)
            {
                list_tempDataForCookies = (List<Logic.TempDataForCookies>)Session["TempDataForCookies"];
            }
            for (int i = 0; i < list_tempDataForCookies.Count(); i++)
            {
                if (list_tempDataForCookies[i].Id == int.Parse(id))
                {
                    list_tempDataForCookies.RemoveAt(i);
                    break;
                }
            }
            Session["TempDataForCookies"] = list_tempDataForCookies;
            Response.Redirect("~/openpay/Cart");
        }
        public void DeleteToCart(string id)
        {
            List<Logic.TempDataForCookies> list_tempDataForCookies = new List<Logic.TempDataForCookies>();
            if (Session["TempDataForCookies"] != null)
            {
                list_tempDataForCookies = (List<Logic.TempDataForCookies>)Session["TempDataForCookies"];
            }

            if (id == "1")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 1);
                list_tempDataForCookies.RemoveAll(r => r.Id == 4);
            }
            else if (id == "2")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 2);
                list_tempDataForCookies.RemoveAll(r => r.Id == 5);
            }
            else if (id == "3")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 3);
                list_tempDataForCookies.RemoveAll(r => r.Id == 6);
            }
            else if (id == "4")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 1);
                list_tempDataForCookies.RemoveAll(r => r.Id == 4);
            }
            else if (id == "5")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 2);
                list_tempDataForCookies.RemoveAll(r => r.Id == 5);
            }
            else if (id == "6")
            {
                list_tempDataForCookies.RemoveAll(r => r.Id == 3);
                list_tempDataForCookies.RemoveAll(r => r.Id == 6);
            }
            Session["TempDataForCookies"] = list_tempDataForCookies;
            Response.Redirect("~/openpay/Cart");
        }
        public void clearsession()
        {
            Session["TempDataForCookies"] = null;
        }
    }
}
