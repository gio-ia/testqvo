using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
//using System.Threading.Tasks;
//using static ClassLibrary1.qvo.ServiceBase;

namespace ClassLibrary1.qvo
{
    public class ServiceBase
    {
        public enum Methods
        {
            Find,
            List,
            Create,
            Update,
            Delete,
        }

        public string BasePath { get { return BASEPATH; } }
        public string ReturnFromQvo { get { return returnFromQvo; } }
        public string Credentials { get { return credentials; } }
        public string MethodStr { get; set; }
        public string Url { get; set; }
        //public string Filtering { get { return filtering; } }
        //public string Ordering { get { return ordering; } }
        //public string Paging { get { return paging; } }

        private const string BASEPATH = "https://palyground.qvo.cl";
        private const string FAPBASE = "?";
        private const string PAGING_STR = "page={0}&per_page={1}";
        private const string FILTERING_STR = "where={\"{0}\"{\"{1}\":{2}}}";
        private const string ORDERING_STR = "order_by={0} {1}";
        private string returnFromQvo;
        private string credentials;
        private string filtering;
        private string ordering;
        private string paging;

        public ServiceBase()
        {
            returnFromQvo = ConfigurationManager.AppSettings["returnUrlFromQvo"];
            credentials = ConfigurationManager.AppSettings["qvoKey"];
            filtering = null;
            ordering = null;
            paging = null;
        }

        private HttpWebRequest SetRequest()
        {
            StringBuilder fap = new StringBuilder();
            if (!string.IsNullOrEmpty(paging))
                fap.Append((string.IsNullOrEmpty(fap.ToString()) ? "" : "&") + paging);
            if (!string.IsNullOrEmpty(filtering))
                fap.Append((string.IsNullOrEmpty(fap.ToString()) ? "" : "&") + filtering);
            if (!string.IsNullOrEmpty(ordering))
                fap.Append((string.IsNullOrEmpty(fap.ToString()) ? "" : "&") + ordering);
            
            HttpWebRequest httpWebRequest = null;
            if(string.IsNullOrEmpty(fap.ToString()))
                httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            else
                httpWebRequest = (HttpWebRequest)WebRequest.Create($"{Url}?{fap}");

            httpWebRequest.Headers.Add("Authorization", $"Bearer {Credentials}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = MethodStr;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            return httpWebRequest;
        }

        public void SetFilter(string atribute, string operat, string strValue = null, int? intValue = null, bool? blnValue = null)
        {
            if (string.IsNullOrEmpty(atribute))
                throw new System.Exception("The atribute can't be null or empty");
            if (string.IsNullOrEmpty(operat))
                throw new System.Exception("The operator can't be null or empty");
            if (string.IsNullOrEmpty(strValue) && intValue == null && blnValue == null)
                throw new System.Exception("At least one of the value params must be not null");

            if (!string.IsNullOrEmpty(strValue))
                filtering = string.Format(FILTERING_STR, atribute, operat, $"\"{strValue}\"");
            else if (intValue != null)
                filtering = string.Format(FILTERING_STR, atribute, operat, intValue);
            else if (blnValue != null)
                filtering = string.Format(FILTERING_STR, atribute, operat, blnValue);
            else
                filtering = "";
        }

        public void SetPaging(int numberPage = 1, int qPages = 20)
        {
            numberPage = numberPage < 1 ? 1 : numberPage;
            qPages = qPages < 1 ? 1 : qPages;
            paging = string.Format(PAGING_STR, numberPage, qPages);        // "page={number_page}&per_page={pages}";
        }

        public void SetOrderig(string atribute, string order = "ASC")
        {
            paging = string.Format(ORDERING_STR, atribute, order);        // "page={number_page}&per_page={pages}";
        }

        public string Execute(string json)
        {
            var obj = string.Empty;
            var request = SetRequest();
            if(MethodStr == "POST" || MethodStr == "PUT")
            {
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(json);
                    sw.Flush();
                    sw.Close();
                }
            }
            var response = (HttpWebResponse)request.GetResponse();

            return obj;
        }
        public string Get()
        {
            var response = (HttpWebResponse)SetRequest().GetResponse();
            var customer = string.Empty;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                customer = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd()).ToString();
            }
            return customer;
        }

        public void Delete()
        {
            SetRequest().GetResponse();
        }

        public string PostPut(string json)
        {
            var request = SetRequest();
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(json);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();
            var customer = string.Empty;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                customer = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd()).ToString();
            }
            return customer;
        }

    }

    //public static class ExtServiceBase
    //{
    //    public static string ToMethodString(this Methods mtd)
    //    {
    //        switch (mtd)
    //        {
    //            case Methods.Find:
    //            case Methods.List:
    //                return "GET";
    //            case Methods.Create:
    //                return "POST";
    //            case Methods.Update:
    //                return "PUT";
    //            case Methods.Delete:
    //                return "DELETE";
    //        }
    //        return "";
    //    }
    //}
}
