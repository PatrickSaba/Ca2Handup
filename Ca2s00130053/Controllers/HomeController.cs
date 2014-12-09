using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ca2s00130053.Models;

namespace Ca2s00130053.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        MovieDb db = new MovieDb();


        public ActionResult Index()
        {          

            return View(db.Movies.ToList());
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var q = db.Movies.Find(id); 
            if (q == null)
            {
                ViewBag.PageTitle = String.Format("Sorry, record {0} not found.", id);
                
            }
            else
            {
                ViewBag.PageTitle = "Details of " + q.Title + " (" + ((q.Actors.Count == 0) ? "None" : q.Actors.Count.ToString()) + ')';
                ViewBag.Genre = q.Genre;
                
            }

            return View(q);
            
        }

        //
        // GET: /Home/Create
        [HttpGet]
        public ActionResult Create()
        {
           ViewBag.DirectorList = db.Directors.ToList();
            return View();
        }

        //
        // POST: /Home/Create


        [HttpPost]
        public ActionResult Create(Movie collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new MovieDb())
                    {
                        db.Movies.Add(collection);
                        db.SaveChanges();
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public PartialViewResult CreateActor(Actor incomingActor)
        {
            if (ModelState.IsValid)
            {
                db.Actors.Add(incomingActor);
                db.SaveChanges();
                return ActorsById(incomingActor.MovieID);
            }
            return null;
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.directorsList = db.Directors.ToList();
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(Movie editMovie)
        {
            try
            {
                // TODO: Add update logic here
                db.Entry(editMovie).State = EntityState.Modified;
                db.SaveChanges();
               return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public PartialViewResult DirectorsById(int id)
        {
            return PartialView("_Director", db.Directors.Find(id));
        }

        public PartialViewResult ActorsById(int id)
        {
            var movie = db.Movies.Find(id);
            @ViewBag.movieId = id;
            @ViewBag.movieName = movie.Title;
            return PartialView("_ActorsInMovie", movie.Actors);
        }
        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View(db.Movies.Find(id));
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Movies.Remove(db.Movies.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public HttpStatusCodeResult UpdateActor(Actor actor)
        {
            try
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost, ActionName("DeleteActor")]
        public PartialViewResult DeleteActor(int id, int movieId)
        {
            db.Actors.Remove(db.Actors.Find(id));
            db.SaveChanges();
            return PartialView("_ActorsInMovie", db.Actors.Find(movieId).Actors);
        }
    }
}
