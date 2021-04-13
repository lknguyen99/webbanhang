using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //link đến trang sản phẩm
            routes.MapRoute("Product", "{type}/{meta}",
                new { controller = "Product", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "san-pham"  }
                },
                new[] { "ShopOnline.Controllers" });


            //link đến trang chi tiết sản phẩm
            routes.MapRoute("Detail", "{type}/{meta}/{id}",
                new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "san-pham"  }
                },
                new[] { "ShopOnline.Controllers" });

            //link đến chi tiết sản phẩm quần áo nam
            routes.MapRoute("Chi tiết nam", "{type}/{meta}/{id}",
                new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "dien-thoai"  }
                },
                new[] { "ShopOnline.Controllers" });

            //link đến chi tiết sản phẩm quần áo nữ
            routes.MapRoute("Chi tiết nữ", "{type}/{meta}/{id}",
                new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "lap-top"  }
                },
                new[] { "ShopOnline.Controllers" });


            //link đến trang chủ
            routes.MapRoute("Trang chu", "{meta}",
                new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"meta", "trang-chu"  }
                },
                new[] { "ShopOnline.Controllers" });

            //link đến trang liên hệ
            routes.MapRoute("Liên hệ", "{meta}",
                new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"meta", "lien-he"  }
                },
                new[] { "ShopOnline.Controllers" });

            //link đến trang tin tức sự kiện
            routes.MapRoute("Tin tức - Sự kiện", "{meta}",
                new { controller = "Event", action = "Index", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"meta", "tin-tuc-su-kien"  }
                },
                new[] { "ShopOnline.Controllers" });

           

            //link đến gio hang
            routes.MapRoute("Giỏ hàng", "{meta}",
                new { controller = "Cart", action = "GioHang", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"meta", "gio-hang"  }
                },
                new[] { "ShopOnline.Controllers" });

            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"ShopOnline.Controllers"}
            );
        }
    }
}
