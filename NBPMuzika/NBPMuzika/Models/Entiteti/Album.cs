using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class Album
    {
        public int id { get; set; }
        public String name { get; set; }
        public String zanr { get; set; }
        public int godina { get; set; }
        public int brojPesama { get; set; }
    }
}