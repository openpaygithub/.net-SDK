# openpay Sdk .NET Documentation

### SCOPE : 
The objective of this system is to provide a solution to make payment of the Bills using a multiple window system. In this system, the Customers should able to pay their prement through openpay SDK system. The Administrator will update the different calls provided by the openpay API which can be done through online. If the customer is facing issues in Online payment or other transactions he/she should able to see their order status.

# There is an instruction how to implement .NET SDK code for openpay application:

### •	To declare some variable/key for openpay

* * You can declare “JamToken” and “AuthToken” key’s value in `<appSettings>` tag in web.config page or it can be declared globally in .cs page.
     `<add key="_JamToken" value="30000000000000889|155f5b95-a40a-4ae5-8273-41ae83fec8c9" />`

     `<add key="_AuthToken" value="155f5b95-a40a-4ae5-8273-41ae83fec8c9" />`
     * And you can also declare below mentioned key’s value in <appSettings> tag in web.config page.
* In the file the basic urls are define like this and use those constant.. 

* These are for LIVE URL
     `<add key="_GateWayURL" value="https://retailer.myopenpay.com.au/WebSalesTraining/" />`

     `<add key="_GateWayURLLive" value="https://retailer.myopenpay.com.au/WebSalesLive/" />`
* And these are for TRAINING URL
     `<add key="_ServiceBaseURL" value="https://retailer.myopenpay.com.au/ServiceTraining/JAMServiceImpl.svc/" />`

     `<add key="_ServiceBaseURLLive" value="https://retailer.myopenpay.com.au/ServiceLive/JAMServiceImpl.svc/" />`

* These are some method/call name

     `<add key="_Call1_NewOnlineOrder" value="NewOnlineOrder" />`

     `<add key="_Call3_OnlineOrderCapturePayment" value="OnlineOrderCapturePayment" />`

     `<add key="_Call4_OnlineOrderReduction" value="OnlineOrderReduction" />`

     `<add key="_Call5_OnlineOrderDispatchPlan" value="OnlineOrderDispatchPlan" />`

     `<add key="_MinMax_OnlineOrderDispatchPlan" value="MinMaxPurchasePrice" />`
      
     `<add key="_FraudAnalysis_OnlineOrderFraudAlert" value="OnlineOrderFraudAlert" />`
     
     `<add key="_LiveURL" value="False" />`
     
     `<add key="_Location" value="AU" />`
     * For UK location you can use(This is up to you).
     `<add key="_Location" value="UK" />`
     
       
### User Parameters from site
<pre>string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL"];
                                         //For Live URL please use _ServiceBaseURLLive                        
string form_url = WebConfigurationManager.AppSettings["_GateWayURL"];
                                           //For Live URL please use _GateWayURLLive
string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                                          //JamToken
string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                                          //AuthToken
                                          </pre>




### Now you have to run the Call-1 method through “NEW ONLINE ORDER” API Which is -
string inputXML = "`<NewOnlineOrder>`"
                  + "`<JamAuthToken>`" + _JamToken + "`</JamAuthToken>`"
                  + "`<AuthToken>`" + _AuthToken + "`</AuthToken>`"
                  + "`<PurchasePrice>`" + _totalprice + ".00`</PurchasePrice>`"
                  + "`<PlanCreationType>`Pending`</PlanCreationType>`"
                  + "`</NewOnlineOrder>`"; // - request

string _Call1_NewOnlineOrder = WebConfigurationManager.AppSettings["_Call1_NewOnlineOrder"];
string URL = _ServiceBaseURL + _Call1_NewOnlineOrder;
HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                                                // - service base url and method name

            http.Timeout = 40000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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
            string innerXML = ""; 
            using (StreamReader sr = new StreamReader(getRes.GetResponseStream()))
            {
                innerXML = sr.ReadToEnd;
            }


### You can use the code to convert string to XML for Call-2 Process
XmlSerializer serializer = new    XmlSerializer(typeof(Model_openpay.ResponseNewOnlineOrder));
                StringReader rdr = new StringReader(innerXML);

Model_openpay.ResponseNewOnlineOrder responseXML = (Model_openpay.ResponseNewOnlineOrder)serializer.Deserialize(rdr);
### Now we have got the Plan Id and going to ready for payment so we have to run Call-2 Process
string PurchasePrice = _totalprice + ".00";  
                                           //Format : 100.00(Not more than $1 million)                        string JamCallbackURL = strUrl + "openpay/CallBack";
                                           //Not more than 250 characters                        string JamCancelURL = strUrl + "openpay/CallBack";
                                           //Not more than 250 characters                       string JamFailURL = strUrl + "openpay/CallBack";
                                           //Not more than 250 characters                        
string JamRetailerOrderNo = _orderID;
                                           //Consumer site order number                       string JamEmail = _modelCO.Email;
                                           //Not more than 150 characters                        string JamFirstName = _modelCO.FirstName;
                                           //First name(Not more than 50 characters)                        string JamOtherNames = "";
                                           //Middle name(Not more than 50 characters)                        string JamFamilyName = _modelCO.LastName;
                                           //Last name(Not more than 50 characters)                        string JamDateOfBirth = _modelCO.DOB;
                                           //dd mmm yyyy                        
string JamAddress1 = _modelCO.Address; 
                                           //Not more than 100 characters                        string JamAddress2 = "";                   
                                           //Not more than 100 characters                        
string JamSubrub = _modelCO.Subrub;
                                           //Not more than 100 characters                        string JamState = _modelCO.State;
                                           //Not more than 3 characters                        string JamPostCode = _modelCO.PostCode;
                                           //Not more than 4 characters                        string JamDeliveryDate = DateTime.Now.ToString("dd mmm yyyy");
                                           //dd mmm yyyy                        
string JamPlanID = responseXML.PlanID;    
                                         //Please declare Plan ID   

string pagegurl = form_url + "?JamCallbackURL=" + JamCallbackURL + "&JamCancelURL=" + JamCancelURL + "&JamFailURL=" + JamFailURL + "&JamAuthToken=" + urlencode(_JamToken) + "&JamPlanID=" + urlencode((string)JamPlanID) + "&JamRetailerOrderNo=" + urlencode(JamRetailerOrderNo) + "&JamPrice=" + urlencode(PurchasePrice) + "&JamEmail=" + urlencode(JamEmail) + "&JamFirstName=" + urlencode(JamFirstName) + "&JamOtherNames=" + urlencode(JamOtherNames) + "&JamFamilyName=" + urlencode(JamFamilyName) + "&JamDateOfBirth=" + urlencode(JamDateOfBirth) + "&JamAddress1=" + urlencode(JamAddress1) + "&JamAddress2=" + urlencode(JamAddress2) + "&JamSubrub=" + urlencode(JamSubrub) + "&JamState=" + urlencode(JamState) + "&JamPostCode=" + urlencode(JamPostCode) + "&JamDeliveryDate=" + urlencode(JamDeliveryDate); // These is callbackURL

### After the process is completed, the Jam system will redirect to the URL “JamCallbackURL” supplied along with a response value for the transaction
 [JamCallbackURL]?status=SUCCESS&planid=3000000022284&orderid=OP0000001
 [JamCallbackURL]?status=CANCELLED&planid=3000000022284&orderid= OP0000001
[JamCallbackURL]?status=FAILURE&planid=3000000022284&orderid= OP0000001

### Add the Call-3 method “PAYMENT CAPTURE” API is like below through CallBack function
public CallBack(string status, string planid, string orderid)
{
            if (status.ToUpper() == "LODGED" || status.ToUpper() == "SUCCESS")
            {
                
                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL];
                string _Call3_OnlineOrderCapturePayment = WebConfigurationManager.AppSettings["_Call3_OnlineOrderCapturePayment"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "`<OnlineOrderCapturePayment>`"
                                + "`<JamAuthToken>`" + _JamToken + "`</JamAuthToken>`"
                                + "`<AuthToken>`" + _AuthToken + "`</AuthToken>`"
                                + "`<PlanID>`" + planid + "`</PlanID>`"
                                + "`</OnlineOrderCapturePayment>`"; // - request

                
string URL = _ServiceBaseURL + _Call3_OnlineOrderCapturePayment;
                    HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                                                // - service base url and method name

            http.Timeout = 40000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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
                string innerXML = sr.ReadToEnd;
            }
}


### For Min-Max Calculation we can use the below code
string _MinMax_OnlineOrderDispatchPlan = WebConfigurationManager.AppSettings["_MinMax_OnlineOrderDispatchPlan"];
                        inputXML = "`<MinMaxPurchasePrice>`"
                                + "`<JamAuthToken>`" + _JamToken + "`</JamAuthToken>`"
                                + "`<AuthToken>`" + _AuthToken + "`</AuthToken>`"
                                + "`</MinMaxPurchasePrice>`"; // - request

                        URL = _ServiceBaseURL + _MinMax_OnlineOrderDispatchPlan;

                        HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                                                // - service base url and method name

            http.Timeout = 40000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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
                string innerXML = sr.ReadToEnd;
            }


### For Refund Process 
string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL"];
                string _Call4_OnlineOrderReduction = WebConfigurationManager.AppSettings["_Call4_OnlineOrderReduction"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "`<OnlineOrderReduction>`"
                                + "`<JamAuthToken>`" + _JamToken + "`</JamAuthToken>`"
                                + "`<AuthToken>`" + _AuthToken + "`</AuthToken>`"
                                + "`<PlanID>`" + dt.Rows[0]["PlanId"].ToString() + "`</PlanID>`"
                                + "`<NewPurchasePrice>`" + NewPurchasePrice.ToString() + "`</NewPurchasePrice>`"
                                + "`<ReducePriceBy>`" + ReducePriceBy.ToString() + "`</ReducePriceBy>`"
                                + "`<FullRefund>`" + FullRefund.ToString() + "`</FullRefund>`"
                                + "`</OnlineOrderReduction>`"; // - request

                    string URL = _ServiceBaseURL + _Call4_OnlineOrderReduction;
            
            HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                                                // - service base url and method name

            http.Timeout = 40000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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
                string innerXML = sr.ReadToEnd;
            }

### For Plan Dispatch

                string _ServiceBaseURL = WebConfigurationManager.AppSettings["_ServiceBaseURL"];
                string _Call5_OnlineOrderDispatchPlan = WebConfigurationManager.AppSettings["_Call5_OnlineOrderDispatchPlan"];
                // - assign request XML here 
                string _JamToken = WebConfigurationManager.AppSettings["_JamToken"];
                string _AuthToken = WebConfigurationManager.AppSettings["_AuthToken"];
                string inputXML = "`<OnlineOrderDispatchPlan>`"
                                + "`<JamAuthToken>`" + _JamToken + "`</JamAuthToken>`"
                                + "`<AuthToken>`" + _AuthToken + "`</AuthToken>`"
                                + "`<PlanID>`" + dt.Rows[0]["PlanId"].ToString() + "`</PlanID>`"
                                + "`</OnlineOrderDispatchPlan>`"; // - request



            string URL = _ServiceBaseURL + _Call5_OnlineOrderDispatchPlan;
            HttpWebRequest http = WebRequest.Create(URL) as HttpWebRequest;
                                                // - service base url and method name

            http.Timeout = 40000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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
                string innerXML = sr.ReadToEnd;
            }
                    
