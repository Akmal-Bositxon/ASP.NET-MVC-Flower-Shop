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
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index(int? page)
        {
            IList<Wishlist> clList = new List<Wishlist>();
            WishlistRepository clRep = new WishlistRepository();
            //manual paging (paging on SQL server side - more efficient especially for large tables)
            var pageNumber = page ?? 1; // default to 1st page if no page specified

            int totalItemsCount;
            clList = clRep.GetAllWishlist(pageNumber, 4, out totalItemsCount,Convert.ToInt32( Request.Cookies["userIdCook"].Value));
            var pagedClientList = new StaticPagedList<Wishlist>(clList, pageNumber, 2, totalItemsCount);
            return View("Index", pagedClientList);
  
        }

        // GET: Wishlist/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Wishlist/Create
        public ActionResult Create(int id)
        {
            var clientId = Request.Cookies["userIdCook"].Value;
            var flower = new FlowerRepository().GetFlowerByID(id);
            var client = new ClientRepository().GetClientByID(Convert.ToInt32(clientId));

            HttpCookie floweridcook = new HttpCookie("floweridcook");
            var flowerid = flower.FlowerId;
            floweridcook.Value = flowerid.ToString();
            floweridcook.Expires = DateTime.Now.AddDays(7);
            Response.SetCookie(floweridcook);

            HttpCookie flowernamecook = new HttpCookie("flowernamecook");
            var flowername = flower.FlowerName;
            flowernamecook.Value = flowername.ToString();
            flowernamecook.Expires = DateTime.Now.AddDays(7);
            Response.SetCookie(flowernamecook);

            HttpCookie flowercolorcook = new HttpCookie("flowercolorcook");
            var flowercolor = flower.Color;
            flowercolorcook.Value = flowercolor.ToString();
            flowercolorcook.Expires = DateTime.Now.AddDays(7);
            Response.SetCookie(flowercolorcook);

            HttpCookie flowerpricecook = new HttpCookie("flowerpricecook");
            var flowerprice = flower.Price;
            flowerpricecook.Value = flowerprice.ToString();
            flowerpricecook.Expires = DateTime.Now.AddDays(7);
            Response.SetCookie(flowerpricecook);

            return View();
        }

        // POST: Wishlist/Create
        [HttpPost]
        public ActionResult Create(Wishlist wish, int FlowerIdC, int UserID)
        {
            try
            {
                // TODO: Add insert logic here
                WishlistRepository clRepository = new WishlistRepository();

                clRepository.CreateWishList(wish, FlowerIdC, UserID);

                
                return RedirectToAction("Index");
            }
            catch(Exception except)
            {
                ModelState.AddModelError("", "Impossible to create Client!  Error: " + except.Message);
                return View();
            }
        }

        // GET: Wishlist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Wishlist/Edit/5
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

        // GET: Wishlist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Wishlist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var repo = new WishlistRepository();
                repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Report()
        {
            IList<Report> list = new List<Report>();
            var repo = new WishlistRepository();
            list = repo.GenereateReport();
            return View(list);
        }
    }
}
