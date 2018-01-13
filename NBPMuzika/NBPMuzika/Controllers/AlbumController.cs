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
        public AlbumController()
        {
            neo = new Neo4j();
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

            return View(am);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
