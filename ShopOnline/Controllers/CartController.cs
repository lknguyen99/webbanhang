using PayPal.Api;
using ShopOnline.Helper;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Controllers
{
    public class CartController : Controller
    {
        private PayPal.Api.Payment payment;
        WebBanhangEntities5 db = new WebBanhangEntities5();
        // GET: Cart
        private const string Cartsession = "Cartsession";
        private const string TKcollection = "Taikhoan";
        public ActionResult Index()
        {
           
            return View();
        }
        // lay gio hang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGioHang = Session[Cartsession] as List<Giohang>;
            if(lstGioHang == null)
            {
                // neu gio hang chua ton tai
                lstGioHang = new List<Giohang>();
                Session[Cartsession] = lstGioHang;
            }
            return lstGioHang;
        }

        // Them vao gio hang
        public ActionResult ThemGioHang(int idproduct, string strURL)
        {
            // lay ra session gio hang

            List<Giohang> lstGiohang = Laygiohang();

            // kiem tra sach nay da ton tai trong session[Cartsession] chua ?

            Giohang sanpham = lstGiohang.Find(n => n.idProduct == idproduct);
            if (sanpham == null)
            {
                sanpham = new Giohang(idproduct);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.SLProduct++;
                return Redirect(strURL);
            }
        }


        // phuong thuc tinh tong so luong hang
        private int TongSoLuong()
        {
            int tongsoluong = 0;
            List<Giohang> lstGiohang = Session[Cartsession] as List<Giohang>;

            // kiem tra xem hang co chua neu co thi thuc hien
            if(lstGiohang != null)
            {
                tongsoluong = lstGiohang.Sum(n => n.SLProduct);
            }
            return tongsoluong;
        }

        // phuong thuc tinh tong tien
        
        private decimal TongTien()
        {
            decimal tongtien = 0;
            List<Giohang> lstGiohang = Session[Cartsession] as List<Giohang>;
            if (lstGiohang != null)
            {
                tongtien = lstGiohang.Sum(n => n.TotalPrice);
            }
            return tongtien;
        }

        // phuong thuc xu ly hien thi gio hang

        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGiohang);
        }

        // hien thi dat hang de cap nhap cac thong tin cho don hang
        // phuong thuc dat hang
        // HTTP GET
        [HttpGet]
        public ActionResult DatHang()
        {
            KhachHang kh = (KhachHang) Session[TKcollection];
            List<Giohang> lstGiohang = Session[Cartsession] as List<Giohang>;
            if (kh == null)
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if(lstGiohang == null)
            {
                return RedirectToAction("Index", "Product");
            }

            // lay gio hang tu session
            List<Giohang> lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstgiohang);
        }

        //HTTP POST
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session[TKcollection];
            List<Giohang> lstgiohang = Laygiohang();
            dh.khID = kh.khID;
            dh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            dh.Ngaygiao = DateTime.Parse(ngaygiao);
            dh.Tinhtranggiaohang = false;
            dh.Tinhtrangthanhtoan = false;
            db.DonHangs.Add(dh);
            db.SaveChanges();
            //them chi tiet don hang
            foreach(var item in lstgiohang)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.OrderID = dh.OrderID;
                ctdh.ProductID = item.idProduct;
                ctdh.SL = item.SLProduct;
                ctdh.Price = item.priceProduct;
                db.ChiTietDonHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session[Cartsession] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }

        //phuong thuc xac nhan don hang

        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        //Paypal
        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {

                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority
                    +
                     "/Cart/PaymentWithPayPal?";
                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }
            return View("SuccessView");
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList() { items = new List<Item>() };
            //Các giá trị bao gồm danh sách sản phẩm, thông tin đơn hàng
            //Sẽ được thay đổi bằng hành vi thao tác mua hàng trên website
            List<Giohang> lstgiohang = (List<Giohang>)Session[Cartsession];
            //List<Giohang> lstGiohang = Session[Cartsession] as List<Giohang>;
            decimal a = 20000;
            decimal tongtien = TongTien();
            foreach (var giohang in lstgiohang)
            {
                itemList.items.Add(new Item()
                {
                    //Thông tin đơn hàng
                    name = giohang.nameProduct,
                    currency = "USD",
                    //price = (Math.Round(((giohang.priceProduct) / a),1)).ToString(),
                    price = (giohang.priceProduct/a).ToString(),
                    quantity = giohang.SLProduct.ToString(),
                    sku = "sku"
                });

                //Hình thức thanh toán qua paypal
                var payer = new Payer() { payment_method = "paypal" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl,
                    return_url = redirectUrl
                };
                //các thông tin trong đơn hàng
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = (tongtien/a).ToString(),
                    //subtotal = (Math.Round(((tongtien) / a),1)).ToString(),
                };

                //Đơn vị tiền tệ và tổng đơn hàng cần thanh toán

                var amount = new Amount()
                {
                    currency = "USD",

                    //tinh tong tien co ca tax ship va thanh tien
                    total = (Convert.ToInt32(details.tax) + Convert.ToInt32(details.shipping) + Convert.ToInt32(details.subtotal)).ToString(),
                    //details.subtotal,
                    details = details
                };
                var transactionList = new List<Transaction>();
                //Tất cả thông tin thanh toán cần đưa vào transaction
                transactionList.Add(new Transaction()
                {
                    description = "Transaction description.",
                    invoice_number = Guid.NewGuid().ToString(),
                    amount = amount,
                    item_list = itemList
                });
                payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }
            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }



        //public ActionResult AddItem(int productID, int quantity, product product)
        //{
        //    //var product = new product().ViewDetail();
        //    var cart = Session[Cartsession];
        //    // da co list san roi
        //    if (cart != null)
        //    {
        //        var list = (List<ChiTietDonHang>)cart;
        //        if (list.Exists(x => x.product.id == productID))
        //        {
        //            foreach (var item in list)
        //            {
        //                if (item.product.id == productID)
        //                {
        //                    item.SL += quantity;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // tao moi doi tuong trong gio hang
        //            var item = new ChiTietDonHang();
        //            item.product.id = productID;
        //            item.SL = quantity;
        //            list.Add(item);
        //        }
        //        Session[Cartsession] = list;

        //    }
        //    // neu khong co thi tao moi list
        //    else
        //    {
        //        // tao moi doi tuong trong gio hang

        //        var item = new ChiTietDonHang();
        //        item.product.id = productID;
        //        item.SL = quantity;

        //        // tao moi danh sach gio hang
        //        var list = new List<ChiTietDonHang>();
        //        list.Add(item);
        //        // gan vao session
        //        Session[Cartsession] = list;
        //    }
        //    return RedirectToAction("Index");
        //    }
    }
}
