using Newtonsoft.Json;
using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Card
    {
        ServiceBase Service = null;

        public Card()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string customerId, string cardId = null, string inscriptionId = null)
        {
            switch (method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");
                    else if (!string.IsNullOrEmpty(cardId))
                        Service.Url = $"{Service.BasePath}/customers/{customerId}/cards/{cardId}";
                    else if (!string.IsNullOrEmpty(inscriptionId))
                        Service.Url = $"{Service.BasePath}/customers/{customerId}/cards/inscriptions/{inscriptionId}";
                    else
                        throw new Exception("CardId or InscriptionID can't be null or empty");

                    Service.MethodStr = "GET";
                    break;

                case Methods.List:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/customers/{customerId}/cards";
                    Service.MethodStr = "GET";
                    break;

                case Methods.Delete:
                    if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(cardId))
                        throw new Exception("Customer can't be null or empty");

                    Service.Url = $"{Service.BasePath}/customers/{customerId}/cards/{cardId}";
                    Service.MethodStr = "DELETE";
                    break;

                case Methods.Create:
                    if (string.IsNullOrEmpty(customerId))
                        throw new Exception("Customer can't be null or empty");
                    else if (!string.IsNullOrEmpty(cardId))
                        Service.Url = $"{Service.BasePath}/customers/{customerId}/cards/{cardId}/charge";
                    else
                        Service.Url = $"{Service.BasePath}/customers/{customerId}/cards/inscriptions";

                    Service.MethodStr = "POST";
                    break;

                case Methods.Update:
                    throw new NotImplementedException();
            }
        }

        public string Find(string customerId, string cardId)
        {
            SetUrl(Methods.Find, customerId, cardId);
            return Service.Get();
        }

        public string List(string customerId)
        {
            SetUrl(Methods.List, customerId);
            Service.SetPaging();
            return Service.Get();
        }

        public void Delete(string customerId)
        {
            SetUrl(Methods.Delete, customerId);
            Service.Delete();
        }

        public string CreateInscription(string customerId)
        {
            SetUrl(Methods.Create, customerId);
            string json = JsonConvert.SerializeObject(new { return_url = Service.ReturnFromQvo });
            return Service.PostPut(json);
        }

        public string FindInscription(string customerId, string InscriptionId)
        {
            SetUrl(Methods.Find, customerId, null, InscriptionId);
            return Service.Get();
        }

        public string ChargeToCard(string customerId, string cardId, int amount, string description)
        {
            SetUrl(Methods.Create, customerId, cardId);
            string json = JsonConvert.SerializeObject(new { customer_id = customerId, card_id = cardId, amount, description });
            return Service.PostPut(json);
        }


    }
}
