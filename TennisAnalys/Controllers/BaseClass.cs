using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace TennisAnalys.Controllers
{
    static class BaseClass
    {
        private static string IP;
        private static int PORT;

        #region Set Proxy
        public static void InitialProxy()
        {
            try
            {
                string kod = MethodGET("http://xseo.in/freeproxy");
                List<string> allProxy = GetListProxyForSite(kod);

                foreach (var item in allProxy)
                {
                    string[] proxy = item.Split(':');
                    int p = Convert.ToInt32(proxy[1]);

                    if (TestProxy(proxy[0], p))
                    {
                        IP = proxy[0];
                        PORT = p;

                        break;
                    }
                }
            }
            catch { }
        }

        private static List<string> GetListProxyForSite(string kodPage)
        {
            List<string> lRez = new List<string>();
            HtmlDocument html = new HtmlDocument();

            html.LoadHtml(kodPage);
            var list = html.DocumentNode.SelectNodes("//font[@class='cls1']");

            foreach (var item in list)
            {
                if (!item.InnerText.Contains(":"))
                    continue;

                lRez.Add(item.InnerText);
            }

            return lRez;
        }

        private static bool TestProxy(string ip, int port)
        {
            try
            {
                string str = MethodGET("https://www.marathonbet.com/su/live", ip, port);

                if (str.Contains("<title>Ставки live"))
                    return true;

                return false;
            }
            catch { return false; }
        }

        public static string GetIpProxy()
        {
            return IP;
        }

        public static int GetPortProxy()
        {
            return PORT;
        }

        #endregion

        public static string MethodGET(string url, string proxyIP = "", int proxyPORT = 0)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[12192];
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (proxyPORT != 0)
                    request.Proxy = new WebProxy(proxyIP, proxyPORT);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                }
                while (count > 0);
            }
            catch (Exception)
            {
                return "";
            }
            return sb.ToString();
        }

        public static string MethodPOST(string url, string postData, string proxyIP = "", int proxyPORT = 0)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = null;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            if (proxyPORT != 0)
                request.Proxy = new WebProxy(proxyIP, proxyPORT);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                response = (HttpWebResponse)request.GetResponse();
            }
            catch { }


            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}