using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBPMuzika.Models.Entiteti
{
    public class AlbumModel
    {
        public List<Album> Album { get; set; }
        public List<Pesma> Pesme { get; set; }
        public List<Bend> Bend { get; set; }
        public List<Producent> Producent { get; set; }
    }
}