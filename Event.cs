using System;
using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class Event
    {
        ServiceBase Service = null;

        public Event()
        {
            Service = new ServiceBase();
        }

        public void SetUrl(Methods method, string eventId = "")
        {
            switch(method)
            {
                case Methods.Find:
                    if (string.IsNullOrEmpty(eventId))
                        throw new Exception("Event can't be null or empty");

                    Service.Url = $"{Service.BasePath}/events/{eventId}";
                    Service.MethodStr = "GET";
                    break;

                case Methods.List:
                    Service.Url = $"{Service.BasePath}/events";
                    Service.MethodStr = "GET";
                    break;

                case Methods.Create:
                    throw new NotImplementedException();

                case Methods.Update:
                    throw new NotImplementedException();

                case Methods.Delete:
                    throw new NotImplementedException();
            }
        }

        public string Find(string eventId)
        {
            SetUrl(Methods.Find, eventId);
            return Service.Get();
        }

        public string FindByType(string type)
        {
            SetUrl(Methods.Find);
            Service.SetFilter("type", "=", type);
            return Service.Get();
        }

        public string List()
        {
            SetUrl(Methods.List);
            Service.SetPaging();
            return Service.Get();
        }


    }
}
