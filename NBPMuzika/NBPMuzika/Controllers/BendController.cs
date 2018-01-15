using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Baze;
using NBPMuzika.Models.Entiteti;

namespace NBPMuzika.Controllers
{
    public class BendController : Controller
    {
        Neo4j neo;
        Redis redis;

        public BendController()
        {
            neo = new Neo4j();
            redis = new Redis();
        }


        // GET: Bend
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bend/Details/5
        public ActionResult Details(int id)
        {
            BendModel bm = new BendModel();
            bm.Bendovi = neo.vratiBendPoID(id);
            bm.Albumi = neo.sviAlbumiGrupe(id);
            bm.Izvodjaci = neo.sviClanoviGrupe(id);

            string zaKesiranje = bm.Bendovi[0].id + "." + bm.Bendovi[0].name + "/Bend";

            
            redis.IncrementTopPages(zaKesiranje);
            redis.UpdateRecentPages(zaKesiranje);

            return View(bm);
        }

        
    }
}
