using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Models;
using System.Web.Security;

namespace Authentication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Membership model)
        {
            using(var context = new OfficeEntities())
            {
                bool isValid = context.User.Any(x=>x.UserName == model.UserName && x.Password == model.Password);
                if(isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,false);
                    return RedirectToAction("Index", "Employees");
                }
                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }  
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            using (var context = new OfficeEntities())
            {
               
                var existingUser = context.User.FirstOrDefault(u => u.UserName == model.UserName);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Name already exists. Please choose a different name.";
                    return View(model); 
                }
                context.User.Add(model);
                context.SaveChanges();
            }
            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}