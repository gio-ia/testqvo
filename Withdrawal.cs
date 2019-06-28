using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Withdrawal
    {
        ServiceBase Service = null;

        public Withdrawal()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string withdrawalId = "")
        {
            switch(method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(withdrawalId))
                        throw new Exception("Withdrawal can't be null or empty");

                    Service.Url = $"{Service.BasePath}/withdrawalals/{withdrawalId}";
                    Service.MethodStr = "GET";
                    break;

                case Methods.List:
                    Service.Url = $"{Service.BasePath}/withdrawalals";
                    Service.MethodStr = "GET";
                    break;

                case Methods.Create:
                    Service.Url = $"{Service.BasePath}/withdrawalals";
                    Service.MethodStr = "POST";
                    break;

                case Methods.Update:
                    throw new NotImplementedException();

                case Methods.Delete:
                    throw new NotImplementedException();
            }
        }

        public string Find(string withdrawalId)
        {
            SetUrl(Methods.Find, withdrawalId);
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

        public void Create(int amount)
        {
            SetUrl(Methods.Create);
            string json = JsonConvert.SerializeObject(new { amount });
            Service.PostPut(json);
        }


    }
}
