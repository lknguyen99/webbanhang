using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Models
{
    public class Giohang
    {
        //model chua du lieu tu database ShopOnline
        WebBanhangEntities5 db = new WebBanhangEntities5();
        public int idProduct { get; set; }
        public string nameProduct { get; set; }
        public string imgProduct { get; set; }
        public decimal priceProduct { get; set; }
        public int SLProduct { get; set; }
        public decimal TotalPrice
        {
            get { return priceProduct * SLProduct; }
        }
        public Giohang(int idproduct)
        {
            idProduct = idproduct;
            product products = db.products.Single(n => n.id == idProduct);
            nameProduct = products.name;
            imgProduct = products.img;
            priceProduct = decimal.Parse(products.price.ToString());
            SLProduct = 1;
            
        }
    }
}