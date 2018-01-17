using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class Pretraga
    {
        public long id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int count { get; set; }
    }

    public class Strana
    {
        public string Pretraga { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        //public string Type { get; set; }

    }

    public class Res
    {
        public List<Pretraga> p;
        public int count;
    }

   
}