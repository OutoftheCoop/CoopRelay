using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CoopRelay.Tools
{
    public class HttpRequest
    {
        public String Url { get; set; }
        public NameValueCollection Data { get; set; }
        public HttpMethod Method { get; set; }
        public enum HttpMethod
        {
            GET,
            POST
        }

        public HttpRequest(string url)
        {
            Url = url;
            Data = new NameValueCollection();
            Method = HttpMethod.GET;
        }

        public HttpRequest(string url, HttpMethod method)
        {
            Url = url;
            Data = new NameValueCollection();
            Method = method;
        }

        public void Add(String name, String value)
        {
            Data.Add(name, value);
        }

        public String Send()
        {
            var req = WebRequest.Create(Url);
            var data = ConstructQueryString(Data);
            byte[] send = Encoding.Default.GetBytes(data);
            if (Method == HttpMethod.GET)
            {
                req.Method = "GET";
            }
            else
            {
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = send.Length;
                Stream sout = req.GetRequestStream();
                sout.Write(send, 0, send.Length);
                sout.Flush();
                sout.Close();
            }
            
            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }

        private String ConstructQueryString(NameValueCollection parameters)
        {
            List<string> items = new List<string>();
            foreach (String name in parameters)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name])));
            return String.Join("&", items.ToArray());
        }
    }
}
