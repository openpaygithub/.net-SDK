using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace openpaySDK
{
    public class openpayMethods
    {
        public openpayModels.Response_Call openpayNewOnlineOrder(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _Call1_NewOnlineOrder = "NewOnlineOrder";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;

                string inputXML = "";
                if (_request.Settings.Location.Code.ToUpper().Trim() == "UK")
                {
                    inputXML = "<NewOnlineOrder>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PurchasePrice>" + _request.NewOnlineOrder.PurchasePrice + "</PurchasePrice>"
                                + "<PlanCreationType>" + _request.NewOnlineOrder.PlanCreationType + "</PlanCreationType>"
                                + "<RetailerOrderNo>" + _request.NewOnlineOrder.RetailerOrderNo + "</RetailerOrderNo>"
                                + "<ChargeBackCount>" + _request.NewOnlineOrder.ChargeBackCount + "</ChargeBackCount>"
                                + "<CustomerQuality>" + _request.NewOnlineOrder.CustomerQuality + "</CustomerQuality>"
                                + "<FirstName>" + _request.NewOnlineOrder.FirstName + "</FirstName>"
                                + "<OtherNames>" + _request.NewOnlineOrder.OtherNames + "</OtherNames>"
                                + "<FamilyName>" + _request.NewOnlineOrder.FamilyName + "</FamilyName>"
                                + "<Email>" + _request.NewOnlineOrder.Email + "</Email>"
                                + "<DateOfBirth>" + _request.NewOnlineOrder.DateOfBirth + "</DateOfBirth>"
                                + "<Gender>" + _request.NewOnlineOrder.Gender + "</Gender>"
                                + "<PhoneNumber>" + _request.NewOnlineOrder.PhoneNumber + "</PhoneNumber>"
                                + "<ResAddress1>" + _request.NewOnlineOrder.ResAddress1 + "</ResAddress1>"
                                + "<ResAddress2>" + _request.NewOnlineOrder.ResAddress2 + "</ResAddress2>"
                                + "<ResSuburb>" + _request.NewOnlineOrder.ResSuburb + "</ResSuburb>"
                                + "<ResState>" + _request.NewOnlineOrder.ResState + "</ResState>"
                                + "<ResPostCode>" + _request.NewOnlineOrder.ResPostCode + "</ResPostCode>"
                                + "<DeliveryDate>" + _request.NewOnlineOrder.DeliveryDate + "</DeliveryDate>"
                                + "<DelAddress1>" + _request.NewOnlineOrder.DelAddress1 + "</DelAddress1>"
                                + "<DelAddress2>" + _request.NewOnlineOrder.DelAddress2 + "</DelAddress2>"
                                + "<DelSuburb>" + _request.NewOnlineOrder.DelSuburb + "</DelSuburb>"
                                + "<DelState>" + _request.NewOnlineOrder.DelState + "</DelState>"
                                + "<DelPostCode>" + _request.NewOnlineOrder.DelPostCode + "</DelPostCode>"
                                + "</NewOnlineOrder>"; // - request
                }
                else if (_request.Settings.Location.Code.ToUpper().Trim() == "AU")
                {
                    inputXML = "<NewOnlineOrder>"
                                + "<JamAuthToken>" + _request.Settings.JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _request.Settings.AuthToken + "</AuthToken>"
                                + "<PurchasePrice>" + _request.NewOnlineOrder.PurchasePrice + "</PurchasePrice>"
                                + "<PlanCreationType>" + _request.NewOnlineOrder.PlanCreationType + "</PlanCreationType>"
                                + "</NewOnlineOrder>"; // - request
                }

                string URL = _ServiceBaseURL + _Call1_NewOnlineOrder;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseNewOnlineOrder));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseNewOnlineOrder resultingMessage = (openpayModels.ResponseNewOnlineOrder)serializer.Deserialize(rdr);
                _response.NewOnlineOrder = resultingMessage;

                openpayModels.ProductDetails _productdetails = new openpayModels.ProductDetails();
                _productdetails.PurchasePrice = _request.NewOnlineOrder.PurchasePrice;
                _response.ProductDetails = _productdetails;

                if (resultingMessage.PlanID != "" && resultingMessage.reason == "")
                {
                    decimal PurchasePrice = _request.NewOnlineOrder.PurchasePrice;//Format : 100.00(Not more than $1 million)
                    string JamCallbackURL = _request.Settings.URL.CallbackURL;//Not more than 250 characters
                    string JamCancelURL = _request.Settings.URL.CancelURL;//Not more than 250 characters
                    string JamFailURL = _request.Settings.URL.FailURL;//Not more than 250 characters
                    string form_url = _staticRequest.GateWayURL;
                    string RetailerOrderNo = _request.NewOnlineOrder.RetailerOrderNo;//Consumer site order number
                    string Email = _request.NewOnlineOrder.Email;//Not more than 150 characters
                    string FirstName = _request.NewOnlineOrder.FirstName;//First name(Not more than 50 characters)
                    string OtherNames = _request.NewOnlineOrder.OtherNames;//Middle name(Not more than 50 characters)
                    string FamilyName = _request.NewOnlineOrder.FamilyName;//Last name(Not more than 50 characters)
                    string DateOfBirth = _request.NewOnlineOrder.DateOfBirth;//dd mmm yyyy
                    string ResAddress1 = _request.NewOnlineOrder.ResAddress1;//Not more than 100 characters
                    string ResAddress2 = _request.NewOnlineOrder.ResAddress2;//Not more than 100 characters
                    string ResSubrub = _request.NewOnlineOrder.ResSuburb;//Not more than 100 characters
                    string ResState = _request.NewOnlineOrder.ResState;//Not more than 3 characters
                    string ResPostCode = _request.NewOnlineOrder.ResPostCode;//Not more than 4 characters
                    string DelAddress1 = _request.NewOnlineOrder.DelAddress1;//Not more than 100 characters
                    string DelAddress2 = _request.NewOnlineOrder.DelAddress2;//Not more than 100 characters
                    string DelSubrub = _request.NewOnlineOrder.ResSuburb;//Not more than 100 characters
                    string DelState = _request.NewOnlineOrder.DelState;//Not more than 3 characters
                    string DelPostCode = _request.NewOnlineOrder.DelPostCode;//Not more than 4 characters
                    string DeliveryDate = _request.NewOnlineOrder.DeliveryDate;//dd mmm yyyy                                              
                    string JamPlanID = resultingMessage.PlanID;  //Plan ID

                    string pagegurl = "";
                    if (_request.Settings.Location.Code.ToUpper().Trim() == "UK")
                    {
                        //pagegurl = form_url + "?JamCallbackURL=" + JamCallbackURL + "&JamCancelURL=" + JamCancelURL
                        //    + "&JamFailURL=" + JamFailURL + "&JamAuthToken=" + _url_Encode(_JamToken) + "&JamPlanID=" + _url_Encode((string)JamPlanID)
                        //    + "&JamRetailerOrderNo=" + _url_Encode(RetailerOrderNo) + "&JamEmail=" + _url_Encode(Email) + "&JamDateOfBirth=" + _url_Encode(DateOfBirth);

                        pagegurl = form_url + "?JamCallbackURL=" + JamCallbackURL + "&JamCancelURL=" + JamCancelURL
                            + "&JamFailURL=" + JamFailURL + "&JamAuthToken=" + _url_Encode(_JamToken) + "&JamPlanID=" + _url_Encode((string)JamPlanID)
                            + "&JamRetailerOrderNo=" + _url_Encode(RetailerOrderNo);
                    }
                    else
                    {
                        pagegurl = form_url + "?JamCallbackURL=" + JamCallbackURL + "&JamCancelURL=" + JamCancelURL
                            + "&JamFailURL=" + JamFailURL + "&JamAuthToken=" + _url_Encode(_JamToken) + "&JamPlanID=" + _url_Encode((string)JamPlanID)
                            + "&JamRetailerOrderNo=" + _url_Encode(RetailerOrderNo) + "&JamPrice=" + PurchasePrice + "&JamEmail="
                            + _url_Encode(Email) + "&JamFirstName=" + _url_Encode(FirstName) + "&JamOtherNames=" + _url_Encode(OtherNames)
                            + "&JamFamilyName=" + _url_Encode(FamilyName) + "&JamDateOfBirth=" + _url_Encode(DateOfBirth) + "&JamResAddress1="
                            + _url_Encode(ResAddress1) + "&JamResAddress2=" + _url_Encode(ResAddress2) + "&JamResSubrub=" + _url_Encode(ResSubrub)
                            + "&JamResState=" + _url_Encode(ResState) + "&JamResPostCode=" + _url_Encode(ResPostCode) + "&JamDelAddress1="
                            + _url_Encode(DelAddress1) + "&JamDelAddress2=" + _url_Encode(DelAddress2) + "&JamDelSubrub=" + _url_Encode(DelSubrub)
                            + "&JamDelState=" + _url_Encode(DelState) + "&JamDelPostCode=" + _url_Encode(DelPostCode) + "&JamDeliveryDate="
                            + _url_Encode(DeliveryDate);
                    }
                    HttpContext.Current.Response.Redirect(pagegurl);
                }
                if (resultingMessage.reason == "Invalid Purchase Price (less than 0, Not Numeric or outside of Min/Max purchase Price range)")
                {
                    string _MinMax_MinMaxPurchasePrice = "MinMaxPurchasePrice";
                    inputXML = "<MinMaxPurchasePrice>"
                            + "<JamAuthToken>" + _request.Settings.JamToken + "</JamAuthToken>"
                            + "<AuthToken>" + _request.Settings.AuthToken + "</AuthToken>"
                            + "</MinMaxPurchasePrice>"; // - request

                    URL = _ServiceBaseURL + _MinMax_MinMaxPurchasePrice;
                    innerXML = openpayPOST(URL, inputXML);

                    serializer = new XmlSerializer(typeof(openpayModels.ResponseMinMaxPurchasePrice));
                    rdr = new StringReader(innerXML);
                    openpayModels.ResponseMinMaxPurchasePrice resultingMessage1 = (openpayModels.ResponseMinMaxPurchasePrice)serializer.Deserialize(rdr);
                    _response.MinMaxPurchasePrice = resultingMessage1;

                    if (resultingMessage1.status == "0" && resultingMessage1.reason == "")
                    {
                        openpayModels.Error _res_error = new openpayModels.Error();
                        _res_error.reason = "Ordered price should be > " + resultingMessage1.MinPrice + "(Min Price) and < " + resultingMessage1.MaxPrice + "(Max Price)";
                        _response.Error = _res_error;
                    }
                }
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                string pagegurl = _request.Settings.URL.CallbackURL + "?status=" + ex.Message.ToString() + "";
                HttpContext.Current.Response.Redirect(pagegurl);
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Response_Call openpayOnlineOrderCapturePayment(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _Call3_OnlineOrderCapturePayment = "OnlineOrderCapturePayment";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;
                
                // - assign request XML here 
                string inputXML = "<OnlineOrderCapturePayment>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PlanID>" + _request.OnlineOrderCapturePayment.PlanID + "</PlanID>"
                                + "</OnlineOrderCapturePayment>"; // - request

                string URL = _ServiceBaseURL + _Call3_OnlineOrderCapturePayment;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseOnlineOrderCapturePayment));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseOnlineOrderCapturePayment resultingMessage = (openpayModels.ResponseOnlineOrderCapturePayment)serializer.Deserialize(rdr);

                _response.OnlineOrderCapturePayment = resultingMessage;
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Response_Call openpayOnlineOrderReduction(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _Call4_OnlineOrderCapturePayment = "OnlineOrderReduction";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;

                // - assign request XML here 
                string inputXML = "<OnlineOrderReduction>"
                                + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                + "<PlanID>" + _request.OnlineOrderReduction.PlanID + "</PlanID>"
                                + "<NewPurchasePrice>" + _request.OnlineOrderReduction.NewPurchasePrice + "</NewPurchasePrice>"
                                + "<ReducePriceBy>" + _request.OnlineOrderReduction.ReducePriceBy + "</ReducePriceBy>"
                                + "<FullRefund>" + _request.OnlineOrderReduction.FullRefund + "</FullRefund>"
                                + "</OnlineOrderReduction>"; // - request

                string URL = _ServiceBaseURL + _Call4_OnlineOrderCapturePayment;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseOnlineOrderReduction));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseOnlineOrderReduction resultingMessage = (openpayModels.ResponseOnlineOrderReduction)serializer.Deserialize(rdr);

                _response.OnlineOrderReduction = resultingMessage;
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Response_Call openpayOnlineOrderDispatchPlan(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _Call5_OnlineOrderDispatchPlan = "OnlineOrderDispatchPlan";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;

                // - assign request XML here 
                string inputXML = "<OnlineOrderDispatchPlan>"
                                 + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                                 + "<AuthToken>" + _AuthToken + "</AuthToken>"
                                 + "<PlanID>" + _request.OnlineOrderDispatchPlan.PlanID + "</PlanID>"
                                 + "</OnlineOrderDispatchPlan>"; // - request

                string URL = _ServiceBaseURL + _Call5_OnlineOrderDispatchPlan;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseOnlineOrderDispatchPlan));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseOnlineOrderDispatchPlan resultingMessage = (openpayModels.ResponseOnlineOrderDispatchPlan)serializer.Deserialize(rdr);

                _response.OnlineOrderDispatchPlan = resultingMessage;
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Response_Call openpayOnlineOrderFraudAlert(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _FraudAnalysis_OnlineOrderFraudAlert = "OnlineOrderFraudAlert";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;

                // - assign request XML here 
                string inputXML = "<OnlineOrderFraudAlert>"
                             + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                             + "<AuthToken>" + _AuthToken + "</AuthToken>"
                             + "<PlanID>" + _request.OnlineOrderFraudAlert.PlanID + "</PlanID>"
                             + "<Details>" + _request.OnlineOrderFraudAlert.Details + "</Details>"
                             + "</OnlineOrderFraudAlert>"; // - request         

                string URL = _ServiceBaseURL + _FraudAnalysis_OnlineOrderFraudAlert;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseOnlineOrderFraudAlert));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseOnlineOrderFraudAlert resultingMessage = (openpayModels.ResponseOnlineOrderFraudAlert)serializer.Deserialize(rdr);

                _response.OnlineOrderFraudAlert = resultingMessage;
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Response_Call openpayOnlineOrderStatus(openpayModels.Request_Call _request)
        {
            openpayModels.Response_Call _response = new openpayModels.Response_Call();
            try
            {
                openpayModels.Static_Request _staticRequest = StaticRequestVal(_request.Settings.Location.Code.ToUpper().Trim(), _request.Settings.URL.IsLiveURL);

                // - service base url and method name
                string _ServiceBaseURL = _staticRequest.ServiceBaseURL;
                string _Check_OnlineOrderStatus = "OnlineOrderStatus";
                // - assign request XML here 
                string _JamToken = _request.Settings.JamToken;
                string _AuthToken = _request.Settings.AuthToken;

                // - assign request XML here 
                string inputXML = "<OnlineOrderStatus>"
                             + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
                             + "<AuthToken>" + _AuthToken + "</AuthToken>"
                             + "<PlanID>" + _request.OnlineOrderStatus.PlanID + "</PlanID>"
                             + "</OnlineOrderStatus>"; // - request         

                string URL = _ServiceBaseURL + _Check_OnlineOrderStatus;
                string innerXML = openpayPOST(URL, inputXML);

                XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseOnlineOrderStatus));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseOnlineOrderStatus resultingMessage = (openpayModels.ResponseOnlineOrderStatus)serializer.Deserialize(rdr);

                _response.OnlineOrderStatus = resultingMessage;
            }
            catch (Exception ex)
            {
                openpayModels.Error _res_error = new openpayModels.Error();
                _res_error.reason = ex.Message.ToString();
                _response.Error = _res_error;
                // - exception handling code should go here }
            }
            return _response;
        }
        public openpayModels.Static_Request StaticRequestVal(string _Lcode, bool IsLive)
        {
            openpayModels.Static_Request _sReq = new openpayModels.Static_Request();
            if (_Lcode.ToUpper().Trim() == "UK")
            {
                if (IsLive)
                {
                    _sReq.ServiceBaseURL = "https://integration.training.myopenpay.co.uk/JamServiceImpl.svc/";
                    _sReq.GateWayURL = "https://websales.training.myopenpay.co.uk/";
                }
                else
                {
                    _sReq.ServiceBaseURL = "https://integration.training.myopenpay.co.uk/JamServiceImpl.svc/";
                    _sReq.GateWayURL = "https://websales.training.myopenpay.co.uk/";
                }
            }
            else
            {
                if (IsLive)
                {
                    _sReq.ServiceBaseURL = "https://retailer.myopenpay.com.au/ServiceLive/JAMServiceImpl.svc/";
                    _sReq.GateWayURL = "https://retailer.myopenpay.com.au/WebSalesLive/";
                }
                else
                {
                    _sReq.ServiceBaseURL = "https://retailer.myopenpay.com.au/ServiceTraining/JAMServiceImpl.svc/";
                    _sReq.GateWayURL = "https://retailer.myopenpay.com.au/WebSalesTraining/";
                }
            }
            return _sReq;
        }
        public string _url_Encode(string encode)
        {
            return HttpUtility.UrlEncode(encode);
        }
        //public string Refund(string id, double refundValue)
        //{
        //    string returnval = "";
        //    string _statusDatabase = "";
        //    SqlConnection con = new SqlConnection(ConnectionString.Connection);
        //    con.Open();
        //    string query = "select * from [Orders] where id = " + int.Parse(id);
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        double NewPurchasePrice = Convert.ToDouble(dt.Rows[0]["NewPurchasePrice"].ToString()) - refundValue;
        //        double ReducePriceBy = refundValue;
        //        double TotalRefund = Convert.ToDouble(dt.Rows[0]["Amount"].ToString()) - NewPurchasePrice;
        //        bool FullRefund = false;
        //        if (NewPurchasePrice == Convert.ToDouble("0"))
        //        {
        //            FullRefund = true;
        //        }
        //        System.Net.HttpWebRequest req = null;
        //        System.Net.HttpWebResponse res = null;
        //        // - service base url and method name
        //        string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive() + IsUK()];
        //        string _Call4_OnlineOrderReduction = WebConfigurationManager.AppSettings["_Call4_OnlineOrderReduction"];
        //        // - assign request XML here 
        //        string _JamToken = WebConfigurationManager.AppSettings["_JamToken" + IsUK()];
        //        string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken" + IsUK()];
        //        string inputXML = "<OnlineOrderReduction>"
        //                        + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
        //                        + "<AuthToken>" + _AuthToken + "</AuthToken>"
        //                        + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
        //                        + "<NewPurchasePrice>" + NewPurchasePrice.ToString() + "</NewPurchasePrice>"
        //                        + "<ReducePriceBy>" + ReducePriceBy.ToString() + "</ReducePriceBy>"
        //                        + "<FullRefund>" + FullRefund.ToString() + "</FullRefund>"
        //                        + "</OnlineOrderReduction>"; // - request

        //        try
        //        {
        //            string URL = _ServiceBaseURL + _Call4_OnlineOrderReduction;
        //            string innerXML = openpayPOST(URL, inputXML);


        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
        //            //http.Timeout = 40000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

        //            //http.AllowAutoRedirect = true;
        //            //http.Method = "POST";
        //            //http.ContentType = "application/xml; charset=utf-8";
        //            //http.UserAgent = "WWW-Mechanize/1.73";                  
        //            //byte[] dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(inputXML);
        //            //http.ContentLength = dataBytes.Length;                    
        //            //HttpWebResponse resp;                   
        //            //using (Stream postStream = http.GetRequestStream())
        //            //{
        //            //    postStream.Write(dataBytes, 0, dataBytes.Length);
        //            //}                 
        //            //resp = (HttpWebResponse)http.GetResponse();
        //            //WebResponse getRes = http.GetResponse();
        //            //string innerXML = "";
        //            //using (StreamReader sr = new StreamReader(getRes.GetResponseStream()))
        //            //{
        //            //    innerXML = sr.ReadToEnd().Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
        //            //}


        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //req = (HttpWebRequest)WebRequest.Create(URL);
        //            //req.Method = "POST"; req.ContentType = "application/xml; charset=utf-8";
        //            //// - web service call request timeout is 2 minutes 
        //            //req.Timeout = 120000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //            //var xmlDoc = new XmlDocument { XmlResolver = null };
        //            //xmlDoc.LoadXml(inputXML); req.ContentLength = xmlDoc.InnerXml.Length;
        //            //var sw = new StreamWriter(req.GetRequestStream());
        //            //sw.Write(xmlDoc.InnerXml); sw.Close();
        //            //// - make the call and capture response 
        //            //res = (HttpWebResponse)req.GetResponse();
        //            //Stream responseStream = res.GetResponseStream();
        //            //var streamReader = new StreamReader(responseStream);
        //            //// - read the response into an xml document 
        //            //var ResonseXmlDocument = new XmlDocument();
        //            //ResonseXmlDocument.LoadXml(streamReader.ReadToEnd());
        //            //string innerXML = ResonseXmlDocument.InnerXml.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");

        //            XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderReduction));
        //            StringReader rdr = new StringReader(innerXML);
        //            ModelopenpayAPI.ResponseOnlineOrderReduction resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderReduction)serializer.Deserialize(rdr);
        //            _statusDatabase = dt.Rows[0]["Status"].ToString();
        //            string final_message = "";
        //            if (resultingMessage.status == "0")
        //            {
        //                if (FullRefund == true)
        //                {
        //                    _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">Full Amount Refunded</font></li>" + _statusDatabase;
        //                    returnval = "<strong>Full Amount Refunded</strong>  ";
        //                }
        //                else
        //                {
        //                    _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">$" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + " is refunded out of $" + Math.Floor(Convert.ToDouble(dt.Rows[0]["Amount"].ToString()) * 100) / 100 + "</font></li>" + _statusDatabase;
        //                    returnval = "<strong>$" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + " refunded</strong>  ";
        //                }
        //                string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "', NewPurchasePrice = '" + Math.Floor(Convert.ToDouble(NewPurchasePrice) * 100) / 100 + "', ReducePriceBy = '" + Math.Floor(Convert.ToDouble(TotalRefund) * 100) / 100 + "', FullRefund = '" + FullRefund + "' where id = " + int.Parse(id);
        //                SqlCommand cmd1 = new SqlCommand(query1, con);
        //                cmd1.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>";
        //                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>" + _statusDatabase;
        //                returnval = final_message;
        //                if (_statusDatabase != "New Purchase Price / Reduce Price By is less than or equal to zero" && _statusDatabase != "")
        //                {
        //                    string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
        //                    SqlCommand cmd1 = new SqlCommand(query1, con);
        //                    cmd1.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    con.Close();
        //    return returnval;
        //}
        //public string OrderStatus(string id)
        //{
        //    string returnval = "";
        //    SqlConnection con = new SqlConnection(ConnectionString.Connection);
        //    con.Open();
        //    string query = "select * from [Orders] where id = " + int.Parse(id);
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        returnval = "<ul>" + dt.Rows[0]["Status"].ToString() + "</ul>";
        //    }
        //    con.Close();
        //    return returnval;
        //}
        //public string Dispatch(string id)
        //{
        //    string returnval = "";
        //    string _statusDatabase = "";
        //    SqlConnection con = new SqlConnection(ConnectionString.Connection);
        //    con.Open();
        //    string query = "select * from [Orders] where id = " + int.Parse(id);
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        System.Net.HttpWebRequest req = null;
        //        System.Net.HttpWebResponse res = null;
        //        // - service base url and method name
        //        string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive() + IsUK()];
        //        string _Call5_OnlineOrderDispatchPlan = WebConfigurationManager.AppSettings["_Call5_OnlineOrderDispatchPlan"];
        //        // - assign request XML here 
        //        string _JamToken = WebConfigurationManager.AppSettings["_JamToken" + IsUK()];
        //        string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken" + IsUK()];
        //        string inputXML = "<OnlineOrderDispatchPlan>"
        //                        + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
        //                        + "<AuthToken>" + _AuthToken + "</AuthToken>"
        //                        + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
        //                        + "</OnlineOrderDispatchPlan>"; // - request

        //        try
        //        {
        //            string URL = _ServiceBaseURL + _Call5_OnlineOrderDispatchPlan;
        //            string innerXML = openpayPOST(URL, inputXML);

        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
        //            //http.Timeout = 40000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

        //            //http.AllowAutoRedirect = true;
        //            //http.Method = "POST";
        //            //http.ContentType = "application/xml; charset=utf-8";
        //            //http.UserAgent = "WWW-Mechanize/1.73";
        //            //byte[] dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(inputXML);
        //            //http.ContentLength = dataBytes.Length;
        //            //HttpWebResponse resp;
        //            //using (Stream postStream = http.GetRequestStream())
        //            //{
        //            //    postStream.Write(dataBytes, 0, dataBytes.Length);
        //            //}
        //            //resp = (HttpWebResponse)http.GetResponse();
        //            //WebResponse getRes = http.GetResponse();
        //            //string innerXML = "";
        //            //using (StreamReader sr = new StreamReader(getRes.GetResponseStream()))
        //            //{
        //            //    innerXML = sr.ReadToEnd().Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
        //            //}


        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //req = (HttpWebRequest)WebRequest.Create(URL);
        //            //req.Method = "POST"; req.ContentType = "application/xml; charset=utf-8";
        //            //// - web service call request timeout is 2 minutes 
        //            //req.Timeout = 120000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //            //var xmlDoc = new XmlDocument { XmlResolver = null };
        //            //xmlDoc.LoadXml(inputXML); req.ContentLength = xmlDoc.InnerXml.Length;
        //            //var sw = new StreamWriter(req.GetRequestStream());
        //            //sw.Write(xmlDoc.InnerXml); sw.Close();
        //            //// - make the call and capture response 
        //            //res = (HttpWebResponse)req.GetResponse();
        //            //Stream responseStream = res.GetResponseStream();
        //            //var streamReader = new StreamReader(responseStream);
        //            //// - read the response into an xml document 
        //            //var ResonseXmlDocument = new XmlDocument();
        //            //ResonseXmlDocument.LoadXml(streamReader.ReadToEnd());
        //            //string innerXML = ResonseXmlDocument.InnerXml.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");

        //            XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderDispatchPlan));
        //            StringReader rdr = new StringReader(innerXML);
        //            ModelopenpayAPI.ResponseOnlineOrderDispatchPlan resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderDispatchPlan)serializer.Deserialize(rdr);
        //            _statusDatabase = dt.Rows[0]["Status"].ToString();
        //            string final_message = "";
        //            if (resultingMessage.status == "0")
        //            {
        //                final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>";
        //                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">It has been dispatched</font></li>" + _statusDatabase;
        //                string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "', [IsDispatch] = 'True' ,[Dispatch] = 'Dispatched' where id = " + int.Parse(id);
        //                SqlCommand cmd1 = new SqlCommand(query1, con);
        //                cmd1.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>";
        //                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + "</font></li>" + _statusDatabase;
        //                string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
        //                SqlCommand cmd1 = new SqlCommand(query1, con);
        //                cmd1.ExecuteNonQuery();
        //            }
        //            returnval = final_message;
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    con.Close();
        //    return returnval;
        //}
        //public string FraudAnalysis(string id, string FAValue)
        //{
        //    if (FAValue == null)
        //    {
        //        FAValue = "";
        //    }
        //    FAValue = FAValue.Trim();
        //    string returnval = "";
        //    string _statusDatabase = "";
        //    SqlConnection con = new SqlConnection(ConnectionString.Connection);
        //    con.Open();
        //    string query = "select * from [Orders] where id = " + int.Parse(id);
        //    DataTable dt = new DataTable();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    if (dt.Rows.Count > 0)
        //    {
        //        System.Net.HttpWebRequest req = null;
        //        System.Net.HttpWebResponse res = null;
        //        // - service base url and method name
        //        string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL" + IsLive() + IsUK()];
        //        string _FraudAnalysis_OnlineOrderFraudAlert = WebConfigurationManager.AppSettings["_FraudAnalysis_OnlineOrderFraudAlert"];
        //        // - assign request XML here 
        //        string _JamToken = WebConfigurationManager.AppSettings["_JamToken" + IsUK()];
        //        string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken" + IsUK()];

        //        string inputXML = "<OnlineOrderFraudAlert>"
        //                    + "<JamAuthToken>" + _JamToken + "</JamAuthToken>"
        //                    + "<AuthToken>" + _AuthToken + "</AuthToken>"
        //                    + "<PlanID>" + dt.Rows[0]["PlanId"].ToString() + "</PlanID>"
        //                    + "<Details>" + FAValue + "</Details>"
        //                    + "</OnlineOrderFraudAlert>"; // - request                

        //        try
        //        {
        //            string URL = _ServiceBaseURL + _FraudAnalysis_OnlineOrderFraudAlert;
        //            string innerXML = openpayPOST(URL, inputXML);

        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
        //            //http.Timeout = 40000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

        //            //http.AllowAutoRedirect = true;
        //            //http.Method = "POST";
        //            //http.ContentType = "application/xml; charset=utf-8";
        //            //http.UserAgent = "WWW-Mechanize/1.73";
        //            //byte[] dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(inputXML);
        //            //http.ContentLength = dataBytes.Length;
        //            //HttpWebResponse resp;
        //            //using (Stream postStream = http.GetRequestStream())
        //            //{
        //            //    postStream.Write(dataBytes, 0, dataBytes.Length);
        //            //}
        //            //resp = (HttpWebResponse)http.GetResponse();
        //            //WebResponse getRes = http.GetResponse();
        //            //string innerXML = "";
        //            //using (StreamReader sr = new StreamReader(getRes.GetResponseStream()))
        //            //{
        //            //    innerXML = sr.ReadToEnd().Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
        //            //}


        //            //////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            //req = (HttpWebRequest)WebRequest.Create(URL);
        //            //req.Method = "POST"; req.ContentType = "application/xml; charset=utf-8";
        //            //// - web service call request timeout is 2 minutes 
        //            //req.Timeout = 120000;
        //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //            //var xmlDoc = new XmlDocument { XmlResolver = null };
        //            //xmlDoc.LoadXml(inputXML); req.ContentLength = xmlDoc.InnerXml.Length;
        //            //var sw = new StreamWriter(req.GetRequestStream());
        //            //sw.Write(xmlDoc.InnerXml); sw.Close();
        //            //// - make the call and capture response 
        //            //res = (HttpWebResponse)req.GetResponse();
        //            //Stream responseStream = res.GetResponseStream();
        //            //var streamReader = new StreamReader(responseStream);
        //            //// - read the response into an xml document 
        //            //var ResonseXmlDocument = new XmlDocument();
        //            //ResonseXmlDocument.LoadXml(streamReader.ReadToEnd());
        //            //string innerXML = ResonseXmlDocument.InnerXml.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");



        //            XmlSerializer serializer = new XmlSerializer(typeof(ModelopenpayAPI.ResponseOnlineOrderFraudAlert));
        //            StringReader rdr = new StringReader(innerXML);
        //            ModelopenpayAPI.ResponseOnlineOrderFraudAlert resultingMessage = (ModelopenpayAPI.ResponseOnlineOrderFraudAlert)serializer.Deserialize(rdr);
        //            _statusDatabase = dt.Rows[0]["Status"].ToString();
        //            string final_message = "";
        //            if (resultingMessage.status == "0")
        //            {
        //                final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>";
        //                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;
        //                string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
        //                SqlCommand cmd1 = new SqlCommand(query1, con);
        //                cmd1.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //                final_message = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>";
        //                _statusDatabase = "<li><font color=\"#00aeec\">" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt") + "</font>  --  <font color=\"#a52a2a\">" + resultingMessage.reason + " </font> (" + FAValue + ")</li>" + _statusDatabase;
        //                string query1 = " update [Orders] set [Status] = '" + _statusDatabase + "' where id = " + int.Parse(id);
        //                SqlCommand cmd1 = new SqlCommand(query1, con);
        //                cmd1.ExecuteNonQuery();
        //            }
        //            returnval = final_message;
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    con.Close();
        //    return returnval;
        //}
        //public string IsLive()
        //{
        //    string isLive = "";
        //    if (Convert.ToBoolean(WebConfigurationManager.AppSettings["_LiveURL"]))
        //    {
        //        isLive = "Live";
        //    }
        //    return isLive;
        //}
        //public string IsUK()
        //{
        //    string isLive = "";
        //    if (WebConfigurationManager.AppSettings["_Location"].ToString().ToUpper() == "UK")
        //    {
        //        isLive = "UK";
        //    }
        //    return isLive;
        //}
        public string openpayPOST(string URL, string inputXML)
        {
            string innerXML = "";
            try
            {
                HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                http.Timeout = 40000;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(_AlwaysGoodCertificate);

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
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        private static bool _AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
