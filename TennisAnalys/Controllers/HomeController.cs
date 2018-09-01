using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using TennisAnalys.Models;

namespace TennisAnalys.Controllers
{
    public class HomeController : Controller
    {
        private List<Data> datas = new List<Data>();
        public ActionResult Index()
        {
            ViewBag.Result = Parse();

            return View();
        }

        [HttpPost]
        public void SetKoef(string value, int number, string name)
        {
            var x = 2;
        }

        #region PARSE
        private List<Data> Parse()
        {
            int index = 1;
            string url = "https://1xvxe.host/en/live/Tennis/";
            string url1 = "https://ua1xbet.com/LiveFeed/GetGameZip?id=";
            string url2 = "&lng=ru&cfview=0&isSubGames=true&GroupEvents=true&countevents=250";

            string kodPage = BaseClass.MethodGET(url);
            List<string> listId = GetIdMatches(kodPage);

            foreach (var item in listId)
            {
                string str = BaseClass.MethodGET(url1 + item + url2);
                var d = FillingData(str);
                d.Id = index++;
                datas.Add(d);
            }

            return datas;
        }

        private List<string> GetIdMatches(string kod)
        {
            if (kod.Equals(""))
                return new List<string>();

            Regex reg = new Regex(@"Tennis/.*?/(?<val>.*?)-");

            //string urlBase = "https://1xvxe.host/en/"; 
            List<string> list = new List<string>();
            HtmlDocument html = new HtmlDocument();

            html.LoadHtml(kod);
            var l = html.DocumentNode.SelectNodes("//div[@class='c-events-scoreboard__item']/a");

            foreach (var item in l)
            {
                string id = reg.Match(item.Attributes["href"].Value).Groups["val"].Value;
                list.Add(id);
            }

            return list;
        }

        private Data FillingData(string kod)
        {
            Data data = new Data();
            var o = JObject.Parse(kod);

            data.Score = "3-2";
            data.Liga = (string)o["Value"]["LE"];
            data.Match = (string)o["Value"]["O1E"] + " - " + (string)o["Value"]["O2E"];

            var matches = (JArray)o["Value"]["GE"];

            foreach (var item in matches)
            {
                if ((int)item["E"][0][0]["T"] == 1)
                {
                    data.P1M = (float)item["E"][0][0]["C"];
                    data.P2M = (float)item["E"][1][0]["C"];

                    break;
                }
            }

            var e = o["Value"]["SC"];

            if (kod.Contains("3-й  Сет"))
            {
                var period = o["Value"]["SG"];

                if (period == null)
                    return data;

                int count = period.Count();
                period = o["Value"]["SG"][count - 1]["GE"];

                if (period == null)
                    period = o["Value"]["SG"][count - 2]["GE"][0]["E"];
                else
                    period = o["Value"]["SG"][count - 1]["GE"][0]["E"];

                foreach (var item in period)
                {
                    if ((int)item[0]["T"] == 1)
                        data.P1S = (float)item[0]["C"];
                    else if ((int)item[0]["T"] == 3)
                        data.P2S = (float)item[0]["C"];
                }
            }

            if(data.P1S > 0 || data.P2S >0)
            {
                data.ColorRow = "orange";
                data.ColorP1M = "orange";
                data.ColorP2M = "orange";
                data.ColorP1S = "orange";
                data.ColorP2S = "orange";
            }

            return data;
        }
        #endregion

    }
}