using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openpaySDK
{
    public class openpayModels
    {
        public class Request_Call
        {
            public RequestNewOnlineOrder NewOnlineOrder { get; set; }
            public RequestOnlineOrderCapturePayment OnlineOrderCapturePayment { get; set; }
            public RequestOnlineOrderStatus OnlineOrderStatus { get; set; }
            public RequestOnlineOrderReduction OnlineOrderReduction { get; set; }
            public RequestOnlineOrderDispatchPlan OnlineOrderDispatchPlan { get; set; }
            public RequestOnlineOrderFraudAlert OnlineOrderFraudAlert { get; set; }
            public Settings Settings { get; set; }
        }
        public class Response_Call
        {
            public ResponseNewOnlineOrder NewOnlineOrder { get; set; }
            public ResponseMinMaxPurchasePrice MinMaxPurchasePrice { get; set; }
            public ResponseOnlineOrderCapturePayment OnlineOrderCapturePayment { get; set; }
            public ResponseOnlineOrderStatus OnlineOrderStatus { get; set; }
            public ResponseOnlineOrderReduction OnlineOrderReduction { get; set; }
            public ResponseOnlineOrderDispatchPlan OnlineOrderDispatchPlan { get; set; }
            public ResponseOnlineOrderFraudAlert OnlineOrderFraudAlert { get; set; }
            public ProductDetails ProductDetails { get; set; }
            public Error Error { get; set; }
        }
        public class RequestNewOnlineOrder
        {
            public decimal PurchasePrice { get; set; }
            public string PlanCreationType { get; set; }
            public string RetailerOrderNo { get; set; }
            public int ChargeBackCount { get; set; }
            public int CustomerQuality { get; set; }
            public string FirstName { get; set; }
            public string OtherNames { get; set; }
            public string FamilyName { get; set; }
            public string Email { get; set; }
            public string DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string PhoneNumber { get; set; }
            public string ResAddress1 { get; set; }
            public string ResAddress2 { get; set; }
            public string ResSuburb { get; set; }
            public string ResState { get; set; }
            public string ResPostCode { get; set; }
            public string DeliveryDate { get; set; }
            public string DelAddress1 { get; set; }
            public string DelAddress2 { get; set; }
            public string DelSuburb { get; set; }
            public string DelState { get; set; }
            public string DelPostCode { get; set; }
        }
        public class ResponseNewOnlineOrder
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
            public string EncryptedPlanID { get; set; }
        }
        public class ResponseMinMaxPurchasePrice
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string MinPrice { get; set; }
            public string MaxPrice { get; set; }
        }
        public class RequestOnlineOrderCapturePayment
        {
            public string PlanID { get; set; }
        }
        public class ResponseOnlineOrderCapturePayment
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
            public string PurchasePrice { get; set; }
        }
        public class RequestOnlineOrderStatus
        {
            public string PlanID { get; set; }
        }
        public class ResponseOnlineOrderStatus
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
            public string OrderStatus { get; set; }
            public string PlanStatus { get; set; }
            public decimal PurchasePrice { get; set; }
        }
        public class RequestOnlineOrderReduction
        {
            public string PlanID { get; set; }
            public decimal NewPurchasePrice { get; set; }
            public decimal ReducePriceBy { get; set; }
            public bool FullRefund { get; set; }
        }
        public class ResponseOnlineOrderReduction
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
        public class RequestOnlineOrderDispatchPlan
        {
            public string PlanID { get; set; }
        }
        public class ResponseOnlineOrderDispatchPlan
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
        public class RequestOnlineOrderFraudAlert
        {
            public string PlanID { get; set; }
            public string Details { get; set; }
        }
        public class ResponseOnlineOrderFraudAlert
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
        public class Settings
        {
            public string JamToken { get; set; }
            public string AuthToken { get; set; }
            public Location Location { get; set; }
            public URL URL { get; set; }
        }
        public class Location
        {
            public string Code { get; set; }
        }
        public class URL
        {
            public string CallbackURL { get; set; }
            public string CancelURL { get; set; }
            public string FailURL { get; set; }
            public bool IsLiveURL { get; set; }
        }
        public class Error
        {
            public string reason { get; set; }
        }
        public class Static_Request
        {
            public string ServiceBaseURL { get; set; }
            public string GateWayURL { get; set; }
        }
        public class ProductDetails
        {
            public decimal PurchasePrice { get; set; }
        }
    }
}
