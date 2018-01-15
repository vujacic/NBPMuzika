using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class BendModel
    {
        public List<Bend> Bendovi { get; set; }
        public List<Izvodjac> Izvodjaci { get; set; }
        public List<Album> Albumi { get; set; }
    }
}