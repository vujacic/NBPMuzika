using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Entiteti;
using NBPMuzika.Models.Baze;

namespace NBPMuzika.Controllers
{
    public class AlbumController : Controller
    {
        Neo4j neo;
        Redis red;
        public AlbumController()
        {
            neo = new Neo4j();
            red = new Redis();
        }
        // GET: Album
        public ActionResult Index()
        {
            return View();
        }

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            AlbumModel am = new AlbumModel();
            am.Album = neo.vratiAlbumPoID(id);
            am.Pesme = neo.svePesmeAlbuma(id);
            am.Bend = neo.bendKojiJeIzdao(id);
            am.Producent = neo.sviProducentiAlbuma(id);

            string zaKesiranje = am.Album[0].id + "." + am.Album[0].name+"/Album";

            red.IncrementTopPages(zaKesiranje);
            red.UpdateRecentPages(zaKesiranje);

            return View(am);
        }
    }
}
