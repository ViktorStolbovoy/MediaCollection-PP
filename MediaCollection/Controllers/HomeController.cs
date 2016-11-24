using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MediaCollection
{
    public class HomeController : MCControllerBase
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
			var titles = TitlePersistence.ListTitles(null, TitleKind.Title);
			return View();									    
        }

		public ActionResult AddTitle()
		{
			return null;
		}

		public ActionResult UpdateTitleName()
		{
			return null;
		}

		public ActionResult ReparentTitle()
		{
			return null;
		}

		public ActionResult SetTitleOrder()
		{
			return null;
		}

		public ActionResult SetTitleLocation()
		{
			return null;
		}




		public ActionResult Search()
		{
			return View();
		}
		
		public ActionResult RescanLocation(int locationId)
		{

			return View();
		}

		public ActionResult Previos(int id)
		{
			return View();
		}

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

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

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

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

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

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
