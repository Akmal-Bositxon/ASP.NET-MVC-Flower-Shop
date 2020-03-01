using _00005529_DBSD_CW2.DAL;
using _00005529_DBSD_CW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace _00005529_DBSD_CW2.Controllers
{
    public class FlowerController : Controller
    {
        // GET: Flower
        [AllowAnonymous]
        public ActionResult Index(DateTime? DeliveredDateFilter, string sortField, int? page, string ColorFilter, string FlowerNameFilter,
            string sortFieldDate, string sortFieldPrice )
        {
            IList<Flower> clList = new List<Flower>();
            FlowerRepository clRep = new FlowerRepository();
            //manual paging (paging on SQL server side - more efficient especially for large tables)
            var pageNumber = page ?? 1; // default to 1st page if no page specified

            int totalItemsCount;
            clList = clRep.GetAllFlowersPaged(DeliveredDateFilter,   sortField, pageNumber, 4, out totalItemsCount, ColorFilter,FlowerNameFilter, sortFieldDate, sortFieldPrice);
            var pagedClientList = new StaticPagedList<Flower>(clList, pageNumber, 2, totalItemsCount);

            ViewBag.sortFieldFlower = sortField == "FlowerId" ? "FlowerId_desc" : "FlowerId"; //saving sort order
            ViewBag.sortFieldFlowerDate = sortFieldDate == "DeliveredDate" ? "Delivered_desc" : "DeliveredDate";
            ViewBag.sortFieldFlowerPrice = sortFieldPrice == "Price" ? "Price_desc" : "Price";

            //remember search settings
            ViewBag.DeliveredDateFilter = DeliveredDateFilter;
   
            //remember sort
            ViewBag.CurrentSort = sortField;
            //remember page
            ViewBag.CurrentPage = page;

            return View("Index", pagedClientList);
        }

        // GET: Flower/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            FlowerRepository clRepo = new FlowerRepository();
            Flower cl = clRepo.GetFlowerByID(id);
            return View(cl);
        }

        // GET: Flower/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Flower/Create
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

        // GET: Flower/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Flower/Edit/5
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

        // GET: Flower/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Flower/Delete/5
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
