using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Entiteti;
using NBPMuzika.Models.Baze;
namespace NBPMuzika.Controllers.ApiControllers
{
    public class SearchController : Controller
    {
        Neo4j neo;
        Redis red;
        public SearchController()
        {
            neo = new Neo4j();
            red = new Redis();
        }
        // GET: Search
        public ActionResult Index()
        {

            // List<Pretraga> lista = neo.vratiPretraga(new Strana {Limit=7 });
            // return View(lista);
            return View();
        }

        public ActionResult Detail(Strana str)
        {
            ViewBag.page = str.Page;
            ViewBag.naziv = str.Pretraga;
            str.Limit = 7;
            if (str.Page > 0)
                str.Page -= 1;
            str.Offset = str.Limit * str.Page;

            Res lista = red.KesiraneVrednosti(str.Pretraga, str.Page);
            if (lista == null)
            {
                lista = neo.vratiPretraga(str);
                red.KesirajRezultat(str.Pretraga, lista, str.Page);
            }

            // Res lista = neo.vratiPretraga(str);
            ViewBag.count = lista.count;
            return View(lista.p);
        }
    }
}