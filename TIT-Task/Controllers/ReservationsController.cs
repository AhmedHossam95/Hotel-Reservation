using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIT_Task.Models;

namespace TIT_Task.Controllers
{
    public class ReservationsController : Controller
    {

        HotelEntities _context = new HotelEntities();
        // GET: Reservations
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Create(int id =0)
        {

            ViewBag.RoomTypeId = new SelectList(_context.RoomTypes.ToList(), "Id", "Type");
            var res = new Reservation();
            ViewBag.EditMode = false;

            if (id != 0)
            {
                ViewBag.EditMode = true;
                ViewBag.RoomId = new SelectList(_context.Rooms.ToList(), "Id", "Number");
                res = _context.Reservations.Find(id);
                 ViewBag.Price = _context.RoomTypes.Find(res.Room.RoomTypeId).Price;

            }
            
            return View(res);
        }


        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,RoomId,StartDate,EndDate,Duration,Price")]Reservation newReservation)
        {
            if (newReservation.Id == 0)
            {
                ViewBag.RoomTypeId = new SelectList(_context.RoomTypes.ToList(), "Id", "Type");



                var Room = _context.Rooms.Find(newReservation.RoomId);
                var RoomType = _context.RoomTypes.Find(Room.RoomTypeId);
                newReservation.Price = newReservation.Duration * RoomType.Price;

                _context.Reservations.Add(newReservation);
            }
            else
            {
                var resInDb = _context.Reservations.Find(newReservation.Id);
                _context.Entry(resInDb).State = System.Data.Entity.EntityState.Modified;

            }
            _context.SaveChanges();

            ViewBag.EditMode = false;

            return View();
           
         
            
        }

      






  







        public ActionResult _List()
        {
            var reservationList = _context.Reservations.ToList();
            
            return PartialView("_List",reservationList);
        }
  




        [HttpPost]
        public JsonResult Delete(int id)
        {

            var reservation = _context.Reservations.Find(id);
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return Json(true , JsonRequestBehavior.AllowGet);


        }


        public JsonResult GetRoomsList(int RoomTypeId)
        {
            _context.Configuration.ProxyCreationEnabled = false;
            var data = _context.Rooms.Where(r => r.RoomTypeId == RoomTypeId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        





    }
}