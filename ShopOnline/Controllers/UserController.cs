using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShopOnline.Controllers
{
    public class UserController : Controller
    {
        WebBanhangEntities5 db = new WebBanhangEntities5();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //phuong thuc dang ky
        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        // http post
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["TenKH"];
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["SDT"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            // thong bao chua nhap trong form dang ky
            if(String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Ho ten khach hang khong duoc de trong";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi2"] = "So dien thoai khong duoc de trong";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi3"] = "Dia chi khong duoc de trong";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi4"] = "Ten tai khoan khong duoc de trong";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi5"] = "Mat khau khong duoc de trong";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi6"] = "Mat khau nhap lai khong duoc de trong";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi7"] = "Email khong duoc de trong";
            }
            else
            {
                kh.TenKH = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.DiaChi = diachi;
                kh.Email = email;
                kh.SDT = Convert.ToInt32(dienthoai);
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");

            }

            return this.DangKy();
        }

        // Dang nhap
        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }

        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["Taikhoan"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phai nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau))
                {
                    ViewData["Loi2"] = "Phai nhap mat khau";
                }
                else
                {
                    // gan gia tri cho doi tuong tao moi
                    KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                    if(kh != null)
                    {
                    
                    ViewBag.Thongbao = "Đăng nhập thành công!!!";
                        Session["Taikhoan"] = kh;
                    }
                    else
                    {
                        ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng!!!";
                    }
                }
            return this.DangNhap();
        }
    }
}