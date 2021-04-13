using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class EventController : Controller
    {
        WebBanhangEntities5 _db = new WebBanhangEntities5();
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult getEvent()
        {
            var v = from t in _db.news
                    where t.hide == false
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
    }
}