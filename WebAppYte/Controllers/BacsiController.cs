using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppYte.Models;
using PagedList;

namespace WebAppYte.Controllers
{
    public class BacsiController : Controller
    {
        private modelWeb db = new modelWeb();

        // GET: Bacsi
        public ActionResult Index()
        {
            var quanTris = db.QuanTris.Include(q => q.Khoa).Where(x => x.VaiTro == 2);
            return View(quanTris.ToList());
        }

        // GET: Bacsi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }
        // GET: Bacsi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // POST: Bacsi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDQuanTri,TaiKhoan,MatKhau,VaiTro,ThongTinBacSi,TrinhDo,IDKhoa,HoTen,AnhBia")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
            return View(quanTri);
        }

        // GET: Admin/QuanTris/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: Admin/QuanTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuanTri quanTri = db.QuanTris.Find(id);
            db.QuanTris.Remove(quanTri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Quanlyhoidap(int? page)
        {
            if (page == null) page = 1;
            var hoiDaps = db.HoiDaps.Include(h => h.NguoiDung).Include(h => h.QuanTri).Where(n => n.TrangThai == 0).OrderByDescending(a => a.NgayGui).ThenBy(a => a.IDHoidap).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Traloicauhoi(int? id)
        {
            if (id == null)

            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoiDap hoiDap = db.HoiDaps.Find(id);
            if (hoiDap == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        // POST: Hoidap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Traloicauhoi([Bind(Include = "IDHoidap,CauHoi,TraLoi,IDNguoiDung,IDQuanTri,NgayGui,GhiChu,TrangThai")] HoiDap hoiDap)
        {
            if (ModelState.IsValid)
            {
                hoiDap.TrangThai = 1;
                db.Entry(hoiDap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Quanlyhoidap");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
            return View(hoiDap);
        }

        public ActionResult Kiemtralichhen(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Lichdangcho(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 0).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Lichdaxacnhan(int? page)
        {
            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 1).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(lich.ToPagedList(pageNumber, pageSize));
        }

        // GET: Lichkham/Edit/5
        public ActionResult Xacnhanlichhen(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LichKham lichKham = db.LichKhams.Find(id);
            if (lichKham == null)
            {
                return HttpNotFound();
            }
            // NguoiDung n = new NguoiDung();
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs.Where(x => x.IDNguoiDung == lichKham.IDNguoiDung), "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(x => x.IDQuanTri == lichKham.IDQuanTri), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
            return View(lichKham);
        }

        // POST: Lichkham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanlichhen([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,ZoomInfo,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lichKham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Kiemtralichhen", "Bacsi");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", lichKham.IDQuanTri);
            return View(lichKham);
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using WebAppYte.Models;
//using PagedList;
//using WebAppYte.Services;
//using Microsoft.Extensions.Configuration;

//namespace WebAppYte.Controllers
//{
//    public class BacsiController : Controller
//    {
//        private readonly modelWeb _db;
//        private readonly ZoomService _zoomService;
//        private readonly IConfiguration _configuration;

//        public BacsiController(modelWeb db, IConfiguration configuration)
//        {
//            _db = db;
//            _configuration = configuration;
//            var apiKey = _configuration["Zoom:ApiKey"];
//            var apiSecret = _configuration["Zoom:ApiSecret"];
//            _zoomService = new ZoomService(apiKey, apiSecret);
//        }

//        // GET: Bacsi
//        public ActionResult Index()
//        {
//            var quanTris = db.QuanTris.Include(q => q.Khoa).Where(x => x.VaiTro==2);
//            return View(quanTris.ToList());
//        }

//        // GET: Bacsi/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            QuanTri quanTri = db.QuanTris.Find(id);
//            if (quanTri == null)
//            {
//                return HttpNotFound();
//            }
//            return View(quanTri);
//        }
//        // GET: Bacsi/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            QuanTri quanTri = db.QuanTris.Find(id);
//            if (quanTri == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
//            return View(quanTri);
//        }

//        // POST: Bacsi/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "IDQuanTri,TaiKhoan,MatKhau,VaiTro,ThongTinBacSi,TrinhDo,IDKhoa,HoTen,AnhBia")] QuanTri quanTri)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(quanTri).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.IDKhoa = new SelectList(db.Khoas, "IDKhoa", "TenKhoa", quanTri.IDKhoa);
//            return View(quanTri);
//        }

//        // GET: Admin/QuanTris/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            QuanTri quanTri = db.QuanTris.Find(id);
//            if (quanTri == null)
//            {
//                return HttpNotFound();
//            }
//            return View(quanTri);
//        }

//        // POST: Admin/QuanTris/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            QuanTri quanTri = db.QuanTris.Find(id);
//            db.QuanTris.Remove(quanTri);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }


//        public ActionResult Quanlyhoidap(int? page)
//        {
//            if (page == null) page = 1;
//            var hoiDaps = db.HoiDaps.Include(h => h.NguoiDung).Include(h => h.QuanTri).Where(n => n.TrangThai == 0).OrderByDescending(a=>a.NgayGui).ThenBy(a=>a.IDHoidap).ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(hoiDaps.ToPagedList(pageNumber, pageSize));
//        }

//        public ActionResult Traloicauhoi(int? id)
//        {
//            if (id == null)

//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            HoiDap hoiDap = db.HoiDaps.Find(id);
//            if (hoiDap == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
//            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
//            return View(hoiDap);
//        }

//        // POST: Hoidap/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Traloicauhoi([Bind(Include = "IDHoidap,CauHoi,TraLoi,IDNguoiDung,IDQuanTri,NgayGui,GhiChu,TrangThai")] HoiDap hoiDap)
//        {
//            if (ModelState.IsValid)
//            {
//                hoiDap.TrangThai = 1;
//                db.Entry(hoiDap).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Quanlyhoidap");
//            }
//            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", hoiDap.IDNguoiDung);
//            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", hoiDap.IDQuanTri);
//            return View(hoiDap);
//        }

//        public ActionResult Kiemtralichhen(int ? page)
//        {
//            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(lich.ToPagedList(pageNumber, pageSize));
//        }
//        public ActionResult Lichdangcho(int? page)
//        {
//            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x=>x.TrangThai==0).ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(lich.ToPagedList(pageNumber, pageSize));
//        }
//        public ActionResult Lichdaxacnhan(int? page)
//        {
//            var lich = db.LichKhams.OrderByDescending(x => x.BatDau).ThenBy(y => y.IDLichKham).Where(x => x.TrangThai == 1).ToList();
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            return View(lich.ToPagedList(pageNumber, pageSize));
//        }

//        // GET: Lichkham/Edit/5
//        public ActionResult Xacnhanlichhen(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            LichKham lichKham = db.LichKhams.Find(id);
//            if (lichKham == null)
//            {
//                return HttpNotFound();
//            }
//           // NguoiDung n = new NguoiDung();
//            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs.Where(x => x.IDNguoiDung == lichKham.IDNguoiDung), "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
//            ViewBag.IDQuanTri = new SelectList(db.QuanTris.Where(x => x.IDQuanTri == lichKham.IDQuanTri), "IDQuanTri", "HoTen", lichKham.IDQuanTri);
//            return View(lichKham);
//        }

//        // POST: Lichkham/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Xacnhanlichhen([Bind(Include = "IDLichKham,ChuDe,MoTa,BatDau,KetThuc,TrangThai,ZoomInfo,KetQuaKham,IDNguoiDung,IDQuanTri")] LichKham lichKham)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(lichKham).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Kiemtralichhen","Bacsi");
//            }
//            ViewBag.IDNguoiDung = new SelectList(db.NguoiDungs, "IDNguoiDung", "HoTen", lichKham.IDNguoiDung);
//            ViewBag.IDQuanTri = new SelectList(db.QuanTris, "IDQuanTri", "TaiKhoan", lichKham.IDQuanTri);
//            return View(lichKham);
//        }






//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}


