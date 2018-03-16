using Project.DataAccessLayer;
using Project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class Login_LogoutController : Controller
    {
        //Action result for sign in
        public ActionResult SignIn(User dummy)
        {
            UserDAL userDal = new UserDAL();
            AdminDAL adminDal = new AdminDAL();
            if (userDal.Users.Any(u => u.Username == dummy.Username && u.Password == dummy.Password))
            {
                Session["user"] = dummy;
                return RedirectToAction("userHomePage", "Home");
            }
            else if (adminDal.Admins.Any(u => u.Username == dummy.Username && u.Password == dummy.Password))
            {
                Admin admin = new Admin
                {
                    Username = dummy.Username,
                    Password = dummy.Password
                };
                Session["user"] = admin;
                return RedirectToAction("userHomePage", "Home");
            }
            else
            {
                ViewBag.error = "Username or Password is incorrect!";
                return View("login");
            }
                
        }

        //Action result for sign up
        public ActionResult SignUp()
        {
            User dummy = new User()
            {
                Username = Request.Form["Username"],
                Password = Request.Form["Password"],
                regTime = DateTime.Now
            };


            UserDAL dal = new UserDAL();
            AdminDAL adminDal = new AdminDAL();
            if (dal.Users.Any(u => u.Username == dummy.Username) || adminDal.Admins.Any(u => u.Username == dummy.Username))
            {
                ViewBag.error = "Username already exists!";
                return View("login");
            }
            dal.Users.Add(dummy);
            dal.SaveChanges();
            Session["user"] = dummy;
            return RedirectToAction("userHomePage", "Home");

        }

        //Action result to sign ouy
        public ActionResult SignOut()
        {
            Session["user"] = null;
            return RedirectToAction("Home", "Home");
        }

        //ACtion result for the login page
        public ActionResult login()
        {
            return View();
        }
    }
}