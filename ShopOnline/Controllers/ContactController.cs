using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class ContactController : Controller
    {
        WebBanhangEntities5 _db = new WebBanhangEntities5();
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getContact()
        {
            var v = from t in _db.contacts
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
    }
}