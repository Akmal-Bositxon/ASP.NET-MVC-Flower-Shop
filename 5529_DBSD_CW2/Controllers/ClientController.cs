using _00005529_DBSD_CW2.DAL;
using _00005529_DBSD_CW2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace _00005529_DBSD_CW2.Controllers
{
    
    public class ClientController : Controller
        {
        // GET: Client
        [Authorize]
        public ActionResult Index(string LastNameFilter, DateTime? DateOfBitrhFilter, string CityFilter, string sortField, int? page)
            {
                IList<Client> clList = new List<Client>();
                ClientRepository clRep = new ClientRepository();
            //manual paging (paging on SQL server side - more efficient especially for large tables)
            var pageNumber = page ?? 1; // default to 1st page if no page specified
          
            int totalItemsCount;
            clList = clRep.FilterClients(LastNameFilter, DateOfBitrhFilter, CityFilter, sortField, pageNumber, 2, out totalItemsCount);
            var pagedClientList = new StaticPagedList<Client>(clList, pageNumber, 2, totalItemsCount);

            ViewBag.sortFieldLastName = sortField == "LastName" ? "LastName_desc" : "LastName"; //saving sort order
            ViewBag.sortFieldFirstName = sortField == "FirstName" ? "FirstName_desc" : "FirstName"; //saving sort order


            //remember search settings
            ViewBag.LastnameFilter = LastNameFilter;
            ViewBag.DateOfBitrhFilter = DateOfBitrhFilter;
            ViewBag.CityFilter = CityFilter;
            
            ViewBag.rep = clRep;
        
            //remember sort
            ViewBag.CurrentSort = sortField;
            //remember page
            ViewBag.CurrentPage = page;



            return View("Index", pagedClientList);
            }

        // GET: Client/Details/5
        [Authorize]
        public ActionResult Details(int id)
            {
            ClientRepository clRepo = new ClientRepository();
            Client cl = clRepo.GetClientByID(id);
            return View(cl);

        }

        // GET: Client/Create
        [AllowAnonymous]
        public ActionResult Create()
            {
            return View();
            }

        // POST: Client/Create
        [AllowAnonymous]
        [HttpPost]
            public ActionResult Create(Client cl, HttpPostedFileBase imageFile, string comPassword)
            {
            //try
            //{
                if (imageFile?.ContentLength > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        imageFile.InputStream.CopyTo(stream);
                        cl.Photo = stream.ToArray();
                    }
                }
                ClientRepository clRepository = new ClientRepository();
                if (cl.Password.Equals(comPassword))
                {
                    if (cl.Password.Length >= 3 & cl.Password.Length <= 8 & cl.Password.Contains("_"))
                    {
                        if(cl.UserName.StartsWith("0")|| cl.UserName.StartsWith("1")|| cl.UserName.StartsWith("2")|| cl.UserName.StartsWith("3")
                            || cl.UserName.StartsWith("4") || cl.UserName.StartsWith("5") || cl.UserName.StartsWith("6") || cl.UserName.StartsWith("7")
                            || cl.UserName.StartsWith("8") || cl.UserName.StartsWith("9"))
                        {
                            ModelState.AddModelError("", "UserName should not start with a digit");
                            return View();
                     
                        }
                        else
                        {
                        if (clRepository.isUniqueName(cl.UserName))
                        {
                            clRepository.CreateClient(cl, comPassword);
                                return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username is not unique. Please enter unique username.");
                            return View();
                        }


                    }

                    
                    }
                    else
                    {

                        ModelState.AddModelError("", "The Length of Password should be between 3 and 8 characters. And One of the characters should be '_'.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Password Comfirmation Failed. Please Try Again.");
                    return View();
                }




            //}
            //catch (Exception except)
            //{
            //    ModelState.AddModelError("", "Impossible to create Client!  Error: " + except.Message);
            //    return View();
            //}
        }

        // GET: Client/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
            {
            ClientRepository clRep = new ClientRepository();
            return View(clRep.GetClientByID(id));
            }

        // POST: Client/Edit/5
        [Authorize]
        [HttpPost]
            public ActionResult Edit(Client cl)
            {
                ClientRepository clRep = new ClientRepository();
                try
                {
                clRep.UpdateClient(cl);
                //clRep.UpdateClientStoredProc(cl);
                return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Impossible to update Client Info! Error: " + ex.Message);
                    return View();
                }
            }

        // GET: Client/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
            {
            ClientRepository clRepo = new ClientRepository();
            Client cl = clRepo.GetClientByID(id);
            return View(cl);
        }

        // POST: Client/Delete/5
        [Authorize]
        [HttpPost]
            public ActionResult Delete(int id, Client cl)
            {
            ClientRepository clRepo = new ClientRepository();
            try
                {
                clRepo.DeleteById(id);
                return RedirectToAction("Index");
                }
                catch (Exception ex)
            {
                ModelState.AddModelError("", "Impossible to Delete Client Info! Error: " + ex.Message);
                ViewBag.CityList = clRepo.GetAllClients();
                return View();
            }
            }
        public FileResult ShowPhoto(int id)
        {
            var repo = new ClientRepository();
            var cl = repo.GetClientByID(id);
            if (cl != null && cl.Photo?.Length > 0)
                return File(cl.Photo, "image/jpeg", cl.LastName + ".jpg");
            return null;
        }
    }
    }
