using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class Album
    {
        public String name { get; set; }
        public String Zanr { get; set; }
        public int GodinaOsivanja { get; set; }
        public int BrojPesama { get; set; }
    }
}