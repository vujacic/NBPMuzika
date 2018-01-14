using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Baze;
using NBPMuzika.Models.Entiteti;

namespace NBPMuzika.Controllers
{
    public class IzvodjacController : Controller
    {
        Neo4j neo;
        Redis red;
        public IzvodjacController()
        {
            neo = new Neo4j();
            red = new Redis();
        }

        // GET: Izvodjac
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            IzvodjacModel im = new IzvodjacModel();
            im.Izvodjac = neo.vratiIzvodjacaPoID(id);
            im.Bendovi = neo.bendoviIzvodjaca(id);
            im.Albumi = neo.sviAlbumiIzvodjaca(id);

            string zaKesiranje = im.Izvodjac[0].id + "." + im.Izvodjac[0].name + "/Izvodjac";
            red.IncrementTopPages(zaKesiranje);
            red.UpdateRecentPages(zaKesiranje);

            return View(im);
        }
    }
}