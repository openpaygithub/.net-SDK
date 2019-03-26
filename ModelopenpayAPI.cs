using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace openpaySDKDemo.Models
{
    public class ModelopenpayAPI
    {
        public class NewOnlineOrder
        {
            public string JamAuthToken { get; set; }
            public string AuthToken { get; set; }
            public string PurchasePrice { get; set; }
            public string PlanCreationType { get; set; }
        }
        public class ResponseNewOnlineOrder
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
            public string EncryptedPlanID { get; set; }
        }
        public class Orders
        {
            public int sn { get; set; }
            public int id { get; set; }
            public DateTime Date { get; set; }
            public string PlanId { get; set; }
            public string OrderId { get; set; }
            public double Amount { get; set; }
            public double NewPurchasePrice { get; set; }
            public double ReducePriceBy { get; set; }
            public bool FullRefund { get; set; }
            public bool IsDispatch { get; set; }
            public string Dispatch { get; set; }
            public string Action { get; set; }
            public string Status { get; set; }
            public bool IsShow { get; set; }
            public int UserId { get; set; }
        }
        public class ResponseOnlineOrderCapturePayment
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
            public string PurchasePrice { get; set; }
        }
        public class ResponseOnlineOrderReduction
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
        public class ResponseOnlineOrderDispatchPlan
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
        public class ResponseMinMaxPurchasePrice
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string MinPrice { get; set; }
            public string MaxPrice { get; set; }
        }
        public class ResponseOnlineOrderFraudAlert
        {
            public string status { get; set; }
            public string reason { get; set; }
            public string PlanID { get; set; }
        }
    }
}