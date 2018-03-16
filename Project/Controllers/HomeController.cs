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
    //This controller will be used for the home page and home panels for users and admins.
    public class HomeController : Controller
    {
        //For using the session object
        SuperUser su;
        //Messege to pass to the views
        static string message;

        //Action result for the home page
        public ActionResult Home()
        {
            return View();
        }

        //Action result for user or admin panel according to the session
        public ActionResult userHomePage()
        {
            su = Session["user"] as SuperUser;
            if (message != null)
                ViewBag.message = getMessage();
            ThreadDal t_dal = new ThreadDal();
            ViewBag.threads = t_dal.Threads.ToList<Thread>();
            if (su.getType() == "RICK")
            {
                List<User> users = new List<User>();
                UserDAL dal = new UserDAL();
                
                users = dal.Users.ToList<User>();
                ViewBag.mortys = users;
                
                return View("RickPage");
               
            }
            return View("MortyPage");
        }

        //Action result for changing password
        public ActionResult changePassowrd()
        {
            su = Session["user"] as SuperUser;
            if(su.getType() == "RICK")
            {
                AdminDAL dal = new AdminDAL();
                Admin admin = (from p in dal.Admins where p.Username == su.Username select p).SingleOrDefault();
                admin.Password = Request.Form["password"].ToString();
                dal.SaveChanges();  
            }
            else
            {
                UserDAL dal = new UserDAL();
                User user = (from p in dal.Users where p.Username == su.Username select p).SingleOrDefault();
                user.Password = Request.Form["password"].ToString();
                dal.SaveChanges();
                
            }
            message = "Password changed successfully!";
            return RedirectToAction("userHomePage");
        }

        //Action result for delete a user (morty)
        public ActionResult KillMorty()
        {
            string mortyTokill = Request.Form["mortyToKill"].ToString();
            UserDAL dal = new UserDAL();
            User user = (from p in dal.Users where p.Username == mortyTokill select p).SingleOrDefault();
            if (user!=null)
            {
                dal.Users.Remove(user);
                dal.SaveChanges();
                message = mortyTokill + " deleted successfully!";
            }
            else
                message = "No such morty with the name " + mortyTokill;
            return RedirectToAction("userHomePage");
        }

        //Get all thread in json format
        public ActionResult GetThreadJson()
        {
            ThreadDal dal = new ThreadDal();
            List<Thread> list = dal.Threads.ToList<Thread>();
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //Funtion to use the message only once
        public string getMessage()
        {
            string tmp = message;
            message = null;
            return tmp;
        }
    }
}