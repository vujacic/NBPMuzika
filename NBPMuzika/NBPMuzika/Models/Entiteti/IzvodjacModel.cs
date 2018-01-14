using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class IzvodjacModel
    {
        public List<Izvodjac> Izvodjac { get; set; }
        public List<Bend> Bendovi { get; set; }
        public List<Album> Albumi { get; set; }
    }
}