using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MyProject.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult LayoutAdmin() {
            return View();
        }

        public ActionResult DongHoNam() {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var listDongHoNam = db.SanPhams.OrderBy(sp => sp.MaSP).ToList();
            return View(listDongHoNam);
        }

        public ActionResult DongHoNu(int page = 1, int pageSize = 12) {
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            var DongHoNu = new Product();
            var model = DongHoNu.ListAll(page, pageSize);
            return View(model);
        }

        public ActionResult DanhMucCacSanPham(int page = 1, int pageSize = 12) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            //var listSP = db.SanPhams.OrderBy(sp => sp.MaSP).ToList();
            //return View(listSP);
            var sanPham = new Product();
            var model = sanPham.ListAll(page, pageSize);
            return View(model);
        }

        public ActionResult SanPhamTheoLoai(int maLoaiSP)
        {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            var dsSPTheoLoai = db.SanPhams.Where(sp => sp.MaLoaiSP == maLoaiSP).OrderBy(sp => sp.GiaBan).ToList();
            if (dsSPTheoLoai.Count == 0)
            {
                ViewBag.thongBao = "Sản phẩm đã hết. Xin quý khách thông cảm";
            }
            return View(dsSPTheoLoai);
        }

        public ActionResult DangXuat() {
            Session["Admin"] = null;
            return RedirectToAction("TrangChu", "Home");
        }

    }
}
