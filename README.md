# openpay Sdk .NET Documentation

### SCOPE : 
The objective of this system is to provide a solution to make payment of the Bills using a multiple window system. In this system, the Customers should able to pay their prement through openpay SDK system. The Administrator will update the different calls provided by the openpay API which can be done through online. If the customer is facing issues in Online payment or other transactions he/she should able to see their order status.

# There is an instruction how to implement .NET SDK code for openpay application:

### •	To declare some variable/key for openpay

* * You can declare “JamToken” and “AuthToken” key’s value in `<appSettings>` tag in web.config page or it can be declared in .cs page.
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
* These values are dependent on location if you set UK location then you will have to change these above values and set location wise.
   
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
<pre> public openpayModels.Request_Call RequestVal()
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
        } </pre>

### How to set URL Tranning/Live

<pre>public openpayModels.Static_Request StaticRequestVal(string _Lcode, bool IsLive)
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
        }</pre>

### Now you have to run the Call-1 method through “NEW ONLINE ORDER” API Which is -
<pre>openpayMethods _openpayMethods = new openpayMethods();
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

                openpayModels.Response_Call _res_call = _openpayMethods.openpayNewOnlineOrder(_req_call);</pre>
                
<pre>public openpayModels.Response_Call openpayNewOnlineOrder(openpayModels.Request_Call _request)
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
        }</pre>

### You can use the code to convert string to XML for Call-2 Process
<pre> XmlSerializer serializer = new XmlSerializer(typeof(openpayModels.ResponseNewOnlineOrder));
                StringReader rdr = new StringReader(innerXML);
                openpayModels.ResponseNewOnlineOrder resultingMessage = (openpayModels.ResponseNewOnlineOrder)serializer.Deserialize(rdr);
                _response.NewOnlineOrder = resultingMessage;</pre>
### Now we have got the Plan Id and going to ready for payment so we have to run Call-2 Process
<pre>decimal PurchasePrice = _request.NewOnlineOrder.PurchasePrice;//Format : 100.00(Not more than $1 million)
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
                    HttpContext.Current.Response.Redirect(pagegurl);</pre>

### After the process is completed, the Jam system will redirect to the URL “JamCallbackURL” supplied along with a response value for the transaction
 [JamCallbackURL]?status=SUCCESS&planid=3000000022284&orderid=OP0000001
 [JamCallbackURL]?status=CANCELLED&planid=3000000022284&orderid= OP0000001
[JamCallbackURL]?status=FAILURE&planid=3000000022284&orderid= OP0000001

### Add the Call-3 method “PAYMENT CAPTURE” API is like below through CallBack function
 <pre>public ActionResult CallBack(string status, string planid, string orderid)
        {
            if (status.ToUpper() == "LODGED" || status.ToUpper() == "SUCCESS")
            {
                ViewBag.CalBackMsg = "Successfully purchased products!";
                openpayMethods _openpayMethods = new openpayMethods();
                openpayModels.Request_Call _req_call = RequestVal();
                openpayModels.RequestOnlineOrderCapturePayment _RequestOnlineOrderCapturePayment = new openpayModels.RequestOnlineOrderCapturePayment();
                _RequestOnlineOrderCapturePayment.PlanID = planid;
                _req_call.OnlineOrderCapturePayment = _RequestOnlineOrderCapturePayment;
            }
            else if (status.ToUpper() == "CANCELLED")
            {
              
            }
            else if (status.ToUpper() == "FAILURE")
            {
               
            }           
        }</pre>
        
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
                    
