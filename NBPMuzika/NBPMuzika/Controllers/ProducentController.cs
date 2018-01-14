using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Baze;
using NBPMuzika.Models.Entiteti;

namespace NBPMuzika.Controllers
{
    public class ProducentController : Controller
    {
        Neo4j neo;
        Redis red;
        public ProducentController()
        {
            neo = new Neo4j();
            red = new Redis();
        }

        // GET: Producent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            ProducentModel pm = new ProducentModel();
            pm.Producent = neo.vratiProducentaPoID(id);
            pm.Albumi = neo.sviAlbumiProducenta(id);

            string zaKesiranje = pm.Producent[0].id + "." + pm.Producent[0].name+"/Producent";
            red.UpdateRecentPages(zaKesiranje);
            red.IncrementTopPages(zaKesiranje);

            return View(pm);
        }
    }
}