using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class DefaultController : Controller
    {

        WebBanhangEntities5 _db = new WebBanhangEntities5();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getNew()
        {
            var v = from t in _db.news
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getBanner()
        {
            var v = from t in _db.Banners
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getCategory()
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.categories
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }


        [ChildActionOnly]
        public PartialViewResult getProduct(long id, string metatitle)
        {
            ViewBag.title = "san-pham";
            ViewBag.meta = metatitle;
            var v = from t in _db.products
                    where t.categoryid == id && t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

    }
}