using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TennisAnalys.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Liga { get; set; }
        public string Match { get; set; }
        public string Score { get; set; }
        public float P1M { get; set; }
        public float P2M { get; set; }
        public float P1S { get; set; }
        public float P2S { get; set; }
        public float CurrentP1M { get; set; }
        public float CurrentP2M { get; set; }
        public float CurrentP1S { get; set; }
        public float CurrentP2S { get; set; }
        public string ColorRow { get; set; }
        public string ColorP1M { get; set; }
        public string ColorP2M { get; set; }
        public string ColorP1S { get; set; }
        public string ColorP2S { get; set; }

        public Data()
        {
            ColorRow = "white";
            ColorP1M = "white";
            ColorP2M = "white";
            ColorP1S = "white";
            ColorP2S = "white";
        }
    }
}