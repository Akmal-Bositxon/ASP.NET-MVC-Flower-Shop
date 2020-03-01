using _00005529_DBSD_CW2.DAL;
using _00005529_DBSD_CW2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace _00005529_DBSD_CW2.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Auth
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            ClientRepository clRep = new ClientRepository();
            bool authenticated = clRep.Login(user.UserName, user.Password);
            if (authenticated)
            {
                HttpCookie userIdCook = new HttpCookie("userIdCook");
                var userid = clRep.GetClientByUsername(user.UserName);
                userIdCook.Value = userid.ToString();
                userIdCook.Expires = DateTime.Now.AddDays(7);
                Response.SetCookie(userIdCook);
               
                var ident = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, user.UserName),
                      
                    },

                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                    new AuthenticationProperties { IsPersistent = false }, ident);

                return RedirectToAction("Index", "Home");
   
            }

            ModelState.AddModelError("", "Invalid username or password! Try Again");
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }

    // POST: Author/Create
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

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Author/Edit/5
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

        // GET: Author/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Author/Delete/5
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
