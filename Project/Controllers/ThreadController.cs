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
    public class ThreadController : Controller
    {
        public static ArrayList Thread_list, Comment_list;
        public static Thread current_thread;
        static string message;

        //Ctor
        public ThreadController()
        {
            CommentDal cdal = new CommentDal();
            List<Comment> c = cdal.Comments.ToList<Comment>();
            c.OrderBy(x => x.time.TimeOfDay).ToList();
            c.Reverse();
            Comment_list = new ArrayList(c);
        }

        //Action result for the threads page
        public ActionResult Threads()
        {
            ThreadDal threads = new ThreadDal();
            Thread_list = new ArrayList(threads.Threads.ToList<Thread>());
            ViewBag.list = Thread_list;
            ViewBag.message = getMessage();
            return View();
        }

        //Action result for single thread with all it's comments
        public ActionResult ThreadPage()
        {

            CommentDal cdal = new CommentDal();

            List<Comment> f = cdal.Comments.ToList<Comment>();
            f.OrderBy(x => x.time.TimeOfDay).ToList();
            Comment_list = new ArrayList(f);
            ViewBag.Comments = Comment_list;

            string id = Request.Params
                     .Cast<string>()
                     .Where(p => p.StartsWith("button"))
                     .Select(p => p.Substring("button".Length))
                     .First();


            int i = Int32.Parse(id);
            current_thread = (Thread)Thread_list[i];
            current_thread.Body.Replace("\r\n", "<br />");
            return View(current_thread);


        }

        //Action result for writing post
        public ActionResult WritePost()
        {
            return View();
        }

        //Action result for submmiting the new post
        public ActionResult new_thread()
        {
            current_thread = new Thread()
            {

                Author = ((SuperUser)Session["user"]).Username,
                Title = Request.Form["t_title"].ToString(),
                Body = Request.Form["t_body"].ToString(),
                time = DateTime.Now
            };
            //current_thread.Body = "<pre>" + Request.Form["t_body"].ToString() + "</pre>";

            //current_thread.Body.Replace("\r\n", Environment.NewLine);

            //post comment on sql
            ThreadDal tdal = new ThreadDal();
            tdal.Threads.Add(current_thread);
            tdal.SaveChanges();

            //order the comment list
            List<Thread> f = tdal.Threads.ToList<Thread>();
            f.OrderBy(x => x.time.TimeOfDay).ToList();
            f.Reverse();
            Thread_list = new ArrayList(f);

            //set the bag again
            ViewBag.list = Thread_list;
            ViewBag.Comments = Comment_list;
            //resend the view with the model
            return View("ThreadPage", current_thread);
        }

        //Action result for posting new comment
        public ActionResult post_comment()
        {
            //make comment
            Comment c = new Comment()
            {
                ThreadID = Int32.Parse(Request.Form["t_id"].ToString()),
                Author = ((SuperUser)Session["user"]).Username,
                Body = Request.Form["body_cmnt"].ToString(),
                time = DateTime.Now
            };

            //post comment on sql
            CommentDal cdal = new CommentDal();
            cdal.Comments.Add(c);
            cdal.SaveChanges();

            //order the comment list
            List<Comment> f = cdal.Comments.ToList<Comment>();
            f.OrderBy(x => x.time.TimeOfDay).ToList();
            Comment_list = new ArrayList(f);

            //set the bag again
            ViewBag.Comments = Comment_list;

            //resend the view with the model
            return View("ThreadPage", current_thread);
        }

        //Action result for deleting a thread
        public ActionResult DeleteThread()
        {
            ThreadDal td = new ThreadDal();
            CommentDal cd = new CommentDal();
            Thread t1,t2;

            string id = Request.Params
                        .Cast<string>()
                        .Where(p => p.StartsWith("button"))
                        .Select(p => p.Substring("button".Length))
                        .First();
            int i = Int32.Parse(id);
            t1 = (Thread)Thread_list[i];
            t2 = (from x in td.Threads where t1.ID == x.ID select x).FirstOrDefault();
            td.Threads.Remove(t2);
            td.SaveChanges();

            List<Comment> comments = (from x in cd.Comments where x.ThreadID == t1.ID select x).ToList();
            if(comments != null)
            {
                foreach (Comment comment in comments)
                {
                    cd.Comments.Remove(comment);
                }
                cd.SaveChanges();
                message = "Thread successfully deleted";
            }
            return RedirectToAction("Threads");
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