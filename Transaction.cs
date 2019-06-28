using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Transaction
    {
        ServiceBase Service = null;

        public Transaction()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string transactionId = "")
        {
            switch(method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(transactionId))
                        throw new Exception("Transaction can't be null or empty");

                    Service.Url = $"{Service.BasePath}/transactions/{transactionId}";
                    Service.MethodStr = "GET";
                    break;

                case Methods.List:
                    Service.Url = $"{Service.BasePath}/transactions";
                    Service.MethodStr = "GET";
                    break;

                case Methods.Create:
                    Service.Url = $"{Service.BasePath}/transactions/{transactionId}/refund";
                    Service.MethodStr = "POST";
                    break;

                case Methods.Update:
                    throw new NotImplementedException();

                case Methods.Delete:
                    throw new NotImplementedException();
            }
        }

        public string Find(string transactionId)
        {
            SetUrl(Methods.Find, transactionId);
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

        public void Refund(string transactionId, int amount)
        {
            SetUrl(Methods.Create, transactionId);
            //Confirmar con QVO si el parametro es obligatorio - documentacion no clara
            string json = JsonConvert.SerializeObject(new { amount });
            Service.PostPut(json);
        }


    }
}
