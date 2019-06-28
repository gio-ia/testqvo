using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Plan
    {
        ServiceBase Service = null;

        public Plan()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string planId = "")
        {
            switch (method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(planId))
                        throw new Exception("Plan can't be null or empty");

                    Service.Url = $"{Service.BasePath}/plans/{planId}";
                    Service.MethodStr = "GET";
                    break;
                case Methods.List:
                    Service.Url = $"{Service.BasePath}/plans";
                    Service.MethodStr = "GET";
                    break;
                case Methods.Create:
                    Service.Url = $"{Service.BasePath}/plans";
                    Service.MethodStr = "POST";
                    break;
                case Methods.Update:
                    if (string.IsNullOrEmpty(planId))
                        throw new Exception("Plan can't be null or empty");

                    Service.Url = $"{Service.BasePath}/plans/{planId}";
                    Service.MethodStr = "PUT";
                    break;
                case Methods.Delete:
                    if (string.IsNullOrEmpty(planId))
                        throw new Exception("Plan can't be null or empty");

                    Service.Url = $"{Service.BasePath}/plans/{planId}";
                    Service.MethodStr = "DELETE";
                    break;
            }
        }

        public string Find(string planId)
        {
            SetUrl(Methods.Find, planId);
            return Service.Get();
        }

        public string FindByName(string name)
        {
            SetUrl(Methods.Find);
            Service.SetFilter("name", "=", name);
            return Service.Get();
        }

        public string List()
        {
            SetUrl(Methods.List);
            Service.SetPaging();
            return Service.Get();
        }

        public void Delete(string planId)
        {
            SetUrl(Methods.Delete, planId);
            Service.Delete();
        }

        public string Create(string planId, string name, int price, int intervalCount, int? trialPeriodDays = null, int? defaultCycleCount = null)
        {
            var currency = "CLP";
            var interval = "month";
            intervalCount = intervalCount < 1 ? 1 : intervalCount;
            string json = string.Empty;

            if(trialPeriodDays!=null || defaultCycleCount != null)
                json = JsonConvert.SerializeObject(new { id = planId, name, price, currency, interval, interval_count = intervalCount, trial_period_days = trialPeriodDays, default_cycle_count = defaultCycleCount });
            else if (trialPeriodDays != null)
                json = JsonConvert.SerializeObject(new { id = planId, name, price, currency, interval, interval_count = intervalCount, trial_period_days = trialPeriodDays });
            else if (defaultCycleCount != null)
                json = JsonConvert.SerializeObject(new { id = planId, name, price, currency, interval, interval_count = intervalCount, default_cycle_count = defaultCycleCount });
            else
                json = JsonConvert.SerializeObject(new { id = planId, name, price, currency, interval, interval_count = intervalCount });


            SetUrl(Methods.Create);
            return Service.PostPut(json);
        }

        public string Update(string planId, string name)
        {
            SetUrl(Methods.Update, planId);
            string json = JsonConvert.SerializeObject(new { id = planId, name });
            return Service.PostPut(json);
        }
    }
}
