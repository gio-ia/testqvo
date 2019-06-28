using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Customer
    {
        ServiceBase Service = null;

        public Customer()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string customerId = "")
        {
            switch(method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/customers/{customerId}";
                    Service.MethodStr = "GET";
                    break;
                case Methods.List:
                    Service.Url = $"{Service.BasePath}/customers";
                    Service.MethodStr = "GET";
                    break;
                case Methods.Create:
                    Service.Url = $"{Service.BasePath}/customers";
                    Service.MethodStr = "POST";
                    break;
                case Methods.Update:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/customers/{customerId}";
                    Service.MethodStr = "PUT";
                    break;
                case Methods.Delete:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/customers/{customerId}";
                    Service.MethodStr = "DELETE";
                    break;
            }
        }

        public string Find(string customerId)
        {
            SetUrl(Methods.Find, customerId);
            return Service.Get();
        }

        public string FindByEmail(string email)
        {
            SetUrl(Methods.Find);
            Service.SetFilter("email", "=", email);
            return Service.Get();
        }

        public string List()
        {
            SetUrl(Methods.List);
            Service.SetPaging();
            return Service.Get();
        }

        public void Delete(string customerId)
        {
            SetUrl(Methods.Delete, customerId);
            Service.Delete();
        }

        public string Create(string email, string name, string phone)
        {
            SetUrl(Methods.Create);
            string json = JsonConvert.SerializeObject(new { email, name, phone });
            return Service.PostPut(json);
        }

        public string Update(string customerId, string email, string name, string phone, string defaultPaymentMethodId)
        {
            SetUrl(Methods.Update, customerId);
            string json = string.Empty;
            if (string.IsNullOrEmpty(defaultPaymentMethodId))
            { json = JsonConvert.SerializeObject(new { customer_id = customerId, email, name, phone }); }
            else
            { json = JsonConvert.SerializeObject(new { customer_id = customerId, email, name, phone, default_payment_method_id = defaultPaymentMethodId }); }

            return Service.PostPut(json);
        }
    }
}
