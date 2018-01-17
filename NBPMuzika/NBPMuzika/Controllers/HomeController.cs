using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBPMuzika.Models.Entiteti;
using NBPMuzika.Models.Baze;

namespace NBPMuzika.Controllers
{
    public class HomeController : Controller
    {
        Neo4j neo;
        Redis red;
        public HomeController()
        {
            neo = new Neo4j();
            red = new Redis();
        }
        public ActionResult Index()
        {
            Sve sve = new Sve
            {
                current = cur(),
                novo = novo(),
                trending = top()
            };
            //List<Pretraga> popular=top();
            //List<Pretraga> trenutno = cur();
            //List<Pretraga> recent = novo();
            //List<Pretraga> top=neo.vratiPretraga(new Strana { })
            return View(sve);
        }

        List<Pretraga> top()
        {
            var rez = red.vratiTopID(7);
            if(rez.Count==0)
            {
                return null;
            }
            else
            {
                return parse2(rez);
            }
        }

        List<Pretraga> cur()
        {
            var rez = red.vratiRecentID();
            if (rez.Count == 0)
                return null;
            else
            {
                return parse2(rez);
            }
        }

        List<Pretraga> novo()
        {
            return neo.vratiPoc(new Strana { Page = 1, Limit = 7 });
        }

        Pretraga parse(int id,string p)
        {
            string[] temp = p.Split('/');
            return new Pretraga
            {
                id = id,
                name = temp[0],
                type = temp[1]
            };
        }

        List<Pretraga> parse2(Dictionary<int,string> rez)
        {
            List<Pretraga> top = new List<Pretraga>();
            foreach (KeyValuePair<int, string> p in rez)
                top.Add(parse(p.Key, p.Value));
            return top;
        }
    }
}
