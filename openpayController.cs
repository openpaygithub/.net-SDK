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


namespace openpaySDKDemo.Controllers
{
    public class openpayController : Controller
    {
        //
        // GET: /openpay/
        [Authorize]
        [HttpPost]
        public ActionResult CheckOut(Modelopenpay.CheckOutModel _modelCO, string ReturnURL)
        {            
            string _totalprice = Request.Form["_totalprice"];
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

                string strPathAndQuery = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
                string strUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");

          
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive()];
                string _Call1_NewOnlineOrder = WebConfigurationManager.AppSettings["_Call1_NewOnlineOrder"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "<NewOnlineOrder>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PurchasePrice>" + _totalprice + ".00</PurchasePrice>"
                                + "<PlanCreationType>Pending</PlanCreationType>"
                                + "</NewOnlineOrder>"; // - request

                try
                {
                    string URL = _ServiceBaseURL + _Call1_NewOnlineOrder;
                    string innerXML = openpay_POST(URL, inputXML);

                   

                    XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseNewOnlineOrder));
                    StringReader rdr = new StringReader(innerXML);
                    ModelopenpayAPI.ResponseNewOnlineOrder resultingMessage = (ModelopenpayAPI.ResponseNewOnlineOrder)serializer.Deserialize(rdr);
                    ViewBag.ViewMsg = resultingMessage.reason;
                    if (resultingMessage.PlanID != "" && resultingMessage.reason == "")
                    {
                        string PurchasePrice = _totalprice + ".00";//Format : 100.00(Not more than $1 million)
                        string JamCallbackURL = strUrl + "openpay/CallBack";//Not more than 250 characters
                        string JamCancelURL = strUrl + "openpay/CallBack";//Not more than 250 characters
                        string JamFailURL = strUrl + "openpay/CallBack";//Not more than 250 characters
                        string form_url = WebConfigurationManager.AppSettings["_GateWayURL" + IsLive()];
                        string JamRetailerOrderNo = _orderID;//Consumer site order number
                        string JamEmail = _modelCO.Email;//Not more than 150 characters
                        string JamFirstName = _modelCO.FirstName;//First name(Not more than 50 characters)
                        string JamOtherNames = "";//Middle name(Not more than 50 characters)
                        string JamFamilyName = _modelCO.LastName;//Last name(Not more than 50 characters)
                        string JamDateOfBirth = _modelCO.DOB;//dd mmm yyyy
                        string JamResAddress1 = _modelCO.Address;//Not more than 100 characters
                        string JamResAddress2 = "";//Not more than 100 characters
                        string JamResSubrub = _modelCO.Subrub;//Not more than 100 characters
                        string JamResState = _modelCO.State;//Not more than 3 characters
                        string JamResPostCode = _modelCO.PostCode;//Not more than 4 characters
                        string JamDelAddress1 = _modelCO.Address;//Not more than 100 characters
                        string JamDelAddress2 = "";//Not more than 100 characters
                        string JamDelSubrub = _modelCO.Subrub;//Not more than 100 characters
                        string JamDelState = _modelCO.State;//Not more than 3 characters
                        string JamDelPostCode = _modelCO.PostCode;//Not more than 4 characters
                        string JamDeliveryDate = DateTime.Now.ToString("dd mmm yyyy");//dd mmm yyyy                                              
                        string JamPlanID = resultingMessage.PlanID;  //Plan ID

                        string pagegurl = form_url + "?JamCallbackURL=" + JamCallbackURL + "&JamCancelURL=" + JamCancelURL 
                            + "&JamFailURL=" + JamFailURL + "&JamAuthToken=" + urlencode(_JamToken) + "&JamPlanID=" + urlencode((string)JamPlanID) 
                            + "&JamRetailerOrderNo=" + urlencode(JamRetailerOrderNo) + "&JamPrice=" + urlencode(PurchasePrice) + "&JamEmail=" 
                            + urlencode(JamEmail) + "&JamFirstName=" + urlencode(JamFirstName) + "&JamOtherNames=" + urlencode(JamOtherNames) 
                            + "&JamFamilyName=" + urlencode(JamFamilyName) + "&JamDateOfBirth=" + urlencode(JamDateOfBirth) + "&JamResAddress1=" 
                            + urlencode(JamResAddress1) + "&JamResAddress2=" + urlencode(JamResAddress2) + "&JamResSubrub=" + urlencode(JamResSubrub) 
                            + "&JamResState=" + urlencode(JamResState) + "&JamResPostCode=" + urlencode(JamResPostCode) + "&JamDelAddress1=" 
                            + urlencode(JamDelAddress1) + "&JamDelAddress2=" + urlencode(JamDelAddress2) + "&JamDelSubrub=" + urlencode(JamDelSubrub) 
                            + "&JamDelState=" + urlencode(JamDelState) + "&JamDelPostCode=" + urlencode(JamDelPostCode) + "&JamDeliveryDate=" 
                            + urlencode(JamDeliveryDate);

                        // entry in database

                        string query_insert = " insert into [Orders] values('" + DateTime.Now.ToString("MM/dd/yyy HH:mm:ss") + "','" + JamPlanID.Trim() + "','" + JamRetailerOrderNo.Trim() + "','" + PurchasePrice.Trim() + "','" + PurchasePrice.Trim() + "','0.00',0,0,'Not Yet Dispatch','','',1," + Convert.ToInt32(Request.ServerVariables["AUTH_USER"]) + ") ";
                        SqlCommand cmd_insert = new SqlCommand(query_insert, con);
                        cmd_insert.ExecuteNonQuery();

                        Response.Redirect(pagegurl);
                    }
                    if (resultingMessage.reason == "Invalid Purchase Price (less than 0, Not Numeric or outside of Min/Max purchase Price range)")
                    {
                        string _MinMax_MinMaxPurchasePrice = WebConfigurationManager.AppSettings["_MinMax_MinMaxPurchasePrice"];
                        inputXML = "<MinMaxPurchasePrice>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "</MinMaxPurchasePrice>"; // - request

                        URL = _ServiceBaseURL + _MinMax_MinMaxPurchasePrice;
                        innerXML = openpay_POST(URL, inputXML);

                        serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseMinMaxPurchasePrice));
                        rdr = new StringReader(innerXML);
                        ModelopenpayAPI.ResponseMinMaxPurchasePrice resultingMessage1 = (ModelopenpayAPI.ResponseMinMaxPurchasePrice)serializer.Deserialize(rdr);
                        if (resultingMessage1.status == "0" && resultingMessage1.reason == "")
                        {
                            ViewBag.ViewMsg = "Ordered price should be > " + resultingMessage1.MinPrice + "(Min Price) and < " + resultingMessage1.MaxPrice + "(Max Price)";
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    string pagegurl = strUrl + "openpay/CallBack?status=" + ex.Message.ToString() + "";
                    Response.Redirect(pagegurl);
                    // - exception handling code should go here }
                }
                con.Close();
            }
            return View();
        }
        public string urlencode(string encode)
        {
            return HttpUtility.UrlEncode(encode);
        }
        public string Refund(string id, double refundValue)
        {
            string returnval = "";
            string _statusDatabase = "";
            
                double NewPurchasePrice = Convert.ToDouble(dt.Rows[0]["NewPurchasePrice"].ToString()) - refundValue;
                double ReducePriceBy = refundValue;
                double TotalRefund = Convert.ToDouble(dt.Rows[0]["Amount"].ToString()) - NewPurchasePrice;
                bool FullRefund = false;
                if (NewPurchasePrice == Convert.ToDouble("0"))
                {
                    FullRefund = true;
                }
                // - service base url and method name
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive()];
                string _Call4_OnlineOrderReduction = WebConfigurationManager.AppSettings["_Call4_OnlineOrderReduction"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "<OnlineOrderReduction>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
                                + "<NewPurchasePrice>" + NewPurchasePrice.ToString() + "</NewPurchasePrice>"
                                + "<ReducePriceBy>" + ReducePriceBy.ToString() + "</ReducePriceBy>"
                                + "<FullRefund>" + FullRefund.ToString() + "</FullRefund>"
                                + "</OnlineOrderReduction>"; // - request

                try
                {
                    string URL = _ServiceBaseURL + _Call4_OnlineOrderReduction;
                    string innerXML = openpay_POST(URL, inputXML);
    
                    XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderReduction));
                    StringReader rdr = new StringReader(innerXML);
                    ModelopenpayAPI.ResponseOnlineOrderReduction resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderReduction)serializer.Deserialize(rdr);
                    _statusDatabase = dt.Rows[0]["Status"].ToString();
                    string final_message = "";
                    if (resultingMessage.status == "0")
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
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>" + _statusDatabase;
                        returnval = final_message;
                    }
                }
                catch (Exception ex)
                {

                }
           
            return returnval;
        }       
        public string Dispatch(string id)
        {
            string returnval = "";
            string _statusDatabase = "";
                
                // - service base url and method name
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive()];
                string _Call5_OnlineOrderDispatchPlan = WebConfigurationManager.AppSettings["_Call5_OnlineOrderDispatchPlan"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "<OnlineOrderDispatchPlan>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
                                + "</OnlineOrderDispatchPlan>"; // - request

                try
                {
                    string URL = _ServiceBaseURL + _Call5_OnlineOrderDispatchPlan;
                    string innerXML = openpay_POST(URL, inputXML);

                    XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderDispatchPlan));
                    StringReader rdr = new StringReader(innerXML);
                    ModelopenpayAPI.ResponseOnlineOrderDispatchPlan resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderDispatchPlan)serializer.Deserialize(rdr);
                    _statusDatabase = dt.Rows[0]["Status"].ToString();
                    string final_message = "";
                    if (resultingMessage.status == "0")
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>" + _statusDatabase;                       
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>" + _statusDatabase;                      
                    }
                    returnval = final_message;
                }
                catch (Exception ex)
                {

                }
            
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
               
                // - service base url and method name
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive()];
                string _FraudAnalysis_OnlineOrderFraudAlert = WebConfigurationManager.AppSettings["_FraudAnalysis_OnlineOrderFraudAlert"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];              

                string inputXML = "<OnlineOrderFraudAlert>"
                            + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                            + "<AuthToken>" + _AuthToken + "</AuthToken>"
                            + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
                            + "<Details>" + FAValue + "</Details>"
                            + "</OnlineOrderFraudAlert>"; // - request                

                try
                {
                    string URL = _ServiceBaseURL + _FraudAnalysis_OnlineOrderFraudAlert;
                    string innerXML = openpay_POST(URL, inputXML);

                    XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderFraudAlert));
                    StringReader rdr = new StringReader(innerXML);
                    ModelopenpayAPI.ResponseOnlineOrderFraudAlert resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderFraudAlert)serializer.Deserialize(rdr);
                    _statusDatabase = dt.Rows[0]["Status"].ToString();
                    string final_message = "";
                    if (resultingMessage.status == "0")
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;                       
                    }
                    else
                    {
                        final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;                      
                    }
                    returnval = final_message;                   
                }
                catch (Exception ex)
                {

                }
          
            return returnval;
        }
        public string IsLive()
        {
            string isLive = "";
            if (Convert.ToBoolean(WebConfigurationManager.AppSettings["_LiveURL"]))
            {
                isLive = "Live";
            }
            return isLive;
        }       
        [Authorize]
        public ActionResult CallBack(string status, string planid, string orderid)
        {           
            string _statusDatabase = "";
            ViewBag.TabActive = "Index";
            if (status.ToUpper() == "LODGED" || status.ToUpper() == "SUCCESS")
            {
                ViewBag.CalBackMsg = "Successfully purchased products!";

                // - service base url and method name
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive()];
                string _Call3_OnlineOrderCapturePayment = WebConfigurationManager.AppSettings["_Call3_OnlineOrderCapturePayment"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "<OnlineOrderCapturePayment>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PlanID>" + planid + "</PlanID>"
                                + "</OnlineOrderCapturePayment>"; // - request

                try
                {
                    string URL = _ServiceBaseURL + _Call3_OnlineOrderCapturePayment;
                    string innerXML = openpay_POST(URL, inputXML);
                    
                    XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderCapturePayment));
                    StringReader rdr = new StringReader(innerXML);
                    ModelopenpayAPI.ResponseOnlineOrderCapturePayment resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderCapturePayment)serializer.Deserialize(rdr);
                    if (resultingMessage.status == "0")
                    {
                        ViewBag.CalBackMsg = "You have purchased the products price $" + resultingMessage.PurchasePrice + " successfully!";
                        _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Order is placed successfully</font></li>";
                    }
                    else
                    {
                        ViewBag.CalBackMsg = resultingMessage.reason;
                        _statusDatabase = resultingMessage.reason;
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
            return View();
        }
        [Authorize]
        public ActionResult Success()
        {          
            ViewBag.TabActive = "Index";
            clearsession();
            return View();
        }
        [Authorize]
        public ActionResult Failure()
        {            
            ViewBag.TabActive = "Index";
            return View();
        }
        public string openpay_POST(string URL, string inputXML)
        {
            string innerXML = "";
        
            HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;   
            http.Timeout = 40000;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AlwaysGoodCertificate);

            http.AllowAutoRedirect = true;
            http.Method = "POST";
            http.ContentType = "application/xml; charset=utf-8";
            http.UserAgent = "WWW-Mechanize/1.73";
            byte[] dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(inputXML);
            http.ContentLength = dataBytes.Length;
            HttpWebResponse resp;
            using (Stream postStream = http.GetRequestStream())
            {
                postStream.Write(dataBytes, 0, dataBytes.Length);
            }
            resp = (HttpWebResponse)http.GetResponse();
            WebResponse getRes = http.GetResponse();

            using (StreamReader sr = new StreamReader(getRes.GetResponseStream()))
            {
                innerXML = sr.ReadToEnd().Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            }

            return innerXML;
        }        
    }
}
