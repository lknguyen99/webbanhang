using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class ProductController : Controller
    {

        WebBanhangEntities5 _db = new WebBanhangEntities5();
        // GET: Product
        public ActionResult Index(string meta)
        {
            var v = from t in _db.categories
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault());
        }

        public ActionResult Detail(long id)
        {
            var v = from t in _db.products
                    where t.id == id
                    select t;
            return View(v.FirstOrDefault());
        }
    }
}