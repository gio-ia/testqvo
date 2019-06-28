using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Subscription
    {
        ServiceBase Service = null;

        public Subscription()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string subscriptionId = "", bool cancelAtPeriodEnd = true)
        {
            switch(method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(subscriptionId))
                        throw new Exception("Subscription can't be null or empty");

                    Service.Url = $"{Service.BasePath}/subscriptions/{subscriptionId}";
                    Service.MethodStr = "GET";
                    break;

                case Methods.List:
                    Service.Url = $"{Service.BasePath}/subscriptions";
                    Service.MethodStr = "GET";
                    break;

                case Methods.Create:
                    Service.Url = $"{Service.BasePath}/subscriptions";
                    Service.MethodStr = "POST";
                    break;

                case Methods.Update:
                    if (string.IsNullOrEmpty(subscriptionId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/subscriptions/{subscriptionId}";
                    Service.MethodStr = "PUT";
                    break;

                case Methods.Delete:
                    if (string.IsNullOrEmpty(subscriptionId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/subscriptions/{subscriptionId}&{cancelAtPeriodEnd}";
                    Service.MethodStr = "DELETE";
                    break;
            }
        }

        public string Find(string subscriptionId)
        {
            SetUrl(Methods.Find, subscriptionId);
            return Service.Get();
        }

        public string FindByStatus(string status)
        {
            SetUrl(Methods.Find);
            Service.SetFilter("status", "=", status);
            return Service.Get();
        }

        public string List()
        {
            SetUrl(Methods.List);
            Service.SetPaging();
            return Service.Get();
        }

        public void Cancel(string subscriptionId, bool cancelAtPeriodEnd)
        {
            SetUrl(Methods.Delete, subscriptionId, cancelAtPeriodEnd);
            Service.Delete();
        }

        public string Create(string customerId, string planId, DateTime? start = null, bool prorate = true, int? cycleCount = null)
        {
            var taxName = "IVA";
            decimal taxPercent = 19m;
            start = start ?? DateTime.Now.AddHours(1);

            string json = string.Empty;
            if (!prorate && cycleCount != null)
            {
                json = JsonConvert.SerializeObject(new { customer_id = customerId, plan_id = planId, start, prorate, cycle_count = cycleCount, tax_name = taxName, tax_percent = taxPercent });
            }
            else if(cycleCount != null)
            {
                json = JsonConvert.SerializeObject(new { customer_id = customerId, plan_id = planId, start, cycle_count = cycleCount, tax_name = taxName, tax_percent = taxPercent });
            }
            else //(cycleCount == null)
            {
                json = JsonConvert.SerializeObject(new { customer_id = customerId, plan_id = planId, start, tax_name = taxName, tax_percent = taxPercent });
            }

            SetUrl(Methods.Create);
            return Service.PostPut(json);
        }

        public string Update(string subscriptionId, string customerId, string planId, bool prorate = true, int? cycleCount = null)
        {
            string json = string.Empty;
            if (!prorate && cycleCount != null)
            {
                json = JsonConvert.SerializeObject(new { customer_id = customerId, plan_id = planId, prorate, cycle_count = cycleCount });
            }
            else if (cycleCount != null)
            {
                json = JsonConvert.SerializeObject(new { customer_id = customerId, plan_id = planId, cycle_count = cycleCount });
            }


            SetUrl(Methods.Update, subscriptionId);
            return Service.PostPut(json);
        }

    }
}
