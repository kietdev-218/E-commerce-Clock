using MyProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers {
    public class ThemXoaSuaController : Controller {
        //
        // GET: /ThemXoaSua/
        public ActionResult ThemXoaSua() {
            return View();
        }
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        public ActionResult ThemSanPham() {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemSanPham(SanPham sp, HttpPostedFileBase fUpload)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            if (fUpload != null)
            {
                if (fUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        TempData["UploadFail"] = "Hinh anh da ton tai";
                        return View();
                    }
                    else
                    {
                        fUpload.SaveAs(path);
                        sp.Anh = fUpload.FileName;
                    }
                }
            }
            else {
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh";
                return View();
            }
            db.SanPhams.InsertOnSubmit(sp);
            db.SubmitChanges();
            TempData["Added"] = "Thêm sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult XoaSanPham(int maSP) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null) {
                return HttpNotFound();
            }
            db.SanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult ChiTietSanPham(int maSP) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            SanPham sanPham = db.SanPhams.Single(ma => ma.MaSP == maSP);
            if (sanPham == null) {
                return HttpNotFound();
            }
            else {
                return View(sanPham);
            }
        }
        
        [HttpGet]
        public ActionResult SuaSanPham(int id)
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(id));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [ValidateInput(false)] // Cho phép nhập đoạn mã html vào csdl. Nhập đoạn mã html ở thẻ input nào thì khi binding ra giao diện nhớ ghi @Html.Raw(data hiển thị). VD: Xem ở view chi tiết chỗ @Html.Raw(Model.MoTa).
                               // Do mô tả chứa đoạn mã html ( <br />) nên phải sử dụng cú pháp razor mvc @Html.Raw(). Đọc đoạn mã html
        [HttpPost]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase fUpload)
        {
            SanPham product = db.SanPhams.SingleOrDefault(n => n.MaSP.Equals(sp.MaSP));
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            if (product == null)
            {
                return HttpNotFound();
            }
            product.TenSP = sp.TenSP;
            product.MoTa = sp.MoTa;
            product.GioiTinh = sp.GioiTinh;
            product.GiaBan = sp.GiaBan;
            product.GiaNhap = sp.GiaNhap;
            if (fUpload != null)
            {
                if (fUpload.ContentLength > 0)
                {
                    product.Anh = fUpload.FileName; // k cần phải thêm trùng, sửa sản phẩm k cần giống như thêm
                }
            }
            else
            {
                TempData["UploadFail"] = "Vui lòng chọn hình ảnh !";
                return View();
            }
            product.MaLoaiSP = sp.MaLoaiSP;
            product.MaNCC = sp.MaNCC;
            if (sp.SoLuongTon <= 0)
            {
                product.SoLuongTon = 1;
            }
            else
            {
                product.SoLuongTon = sp.SoLuongTon;
            }
            db.SubmitChanges();
            TempData["Edited"] = "Sửa thông tin sản phẩm thành công";
            return RedirectToAction("DanhMucCacSanPham", "Admin");
        }

        public ActionResult timKiemSanPham(string tenSP) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            if (!string.IsNullOrEmpty(tenSP)) {
                var query = from sp in db.SanPhams where sp.TenSP.Contains(tenSP) || sp.LoaiSanPham.TenLoaiSP.Contains(tenSP) select sp;
                if (query.Count() == 0) {
                    return RedirectToAction("thongBaoRong", "ThemXoaSua");
                }
                return View(query);
            }
            return View();
        }

        public ActionResult thongBaoRong() {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            ViewBag.stringEmpty = "Không tìm thấy sản phẩm";
            return View();
        }

        public ActionResult QuanLiDonHang() {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            if (Session["Admin"] == null) {
                return RedirectToAction("DangNhap", "User");
            }
            var loadData = db.ChiTietHoaDons;
            return View(loadData);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(int maHD) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            hd.TinhTrang = true;
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        [HttpPost]
        public ActionResult HuyDH(int maHD) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            HoaDon hd = db.HoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            ChiTietHoaDon cthd = db.ChiTietHoaDons.SingleOrDefault(n => n.MaHD.Equals(maHD));
            SanPham sp1 = db.SanPhams.SingleOrDefault(i => i.MaSP == cthd.MaSP);
            // Cập nhật số lượng tồn trong database
            sp1.SoLuongTon = sp1.SoLuongTon + cthd.SoLuong;
            db.HoaDons.DeleteOnSubmit(hd);
            db.SubmitChanges();
            return RedirectToAction("QuanLiDonHang", "ThemXoaSua");
        }

        public ActionResult QuanLiKhachHang() {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            ViewBag.GetList = from a in db.KhachHangs
                              //join b in db.KhachHangs
                              //on a.MaKH equals b.MaKH
                              select new HDKhachHangModel {
                                  MaKH = a.MaKH,
                                  TenKH = a.TenKH,
                                  TaiKhoan = a.TaiKhoan,
                                  MatKhau = a.MatKhau,
                                  SoDienThoai = a.SDT,
                                  DiaChi = a.DiaChi,
                                  Email = a.Email,
                              };
            return View(ViewBag.GetList);
        }
        [HttpPost]
        public ActionResult XoaTaiKhoan(int maKH) {
            ViewBag.loaiSP = db.LoaiSanPhams.OrderBy(sp => sp.MaLoaiSP);
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH.Equals(maKH));
            db.KhachHangs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("QuanLiKhachHang", "ThemXoaSua");
        }
    }
}
