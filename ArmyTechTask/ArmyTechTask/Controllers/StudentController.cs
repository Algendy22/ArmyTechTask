using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArmyTechTask.Models;

namespace ArmyTechTask.Controllers
{
    public class StudentController : Controller
    {
        private SchoolEntities db = new SchoolEntities();

        // GET: Student
        public ActionResult Index(string searchString)
        {


          
     
            var movies = from m in db.People

                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
                return View(movies.OrderBy(s => s.FirstName));
            }
            var people = db.People.Include(p => p.OfficeAssignment);
            return View(people.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,LastName,FirstName,HireDate,EnrollmentDate,Discriminator")] Person person, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/StudentImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }


            person.Student_img = pic;
            person.Discriminator = "Student";

            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", person.PersonID);
            return View(person);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", person.PersonID);
            return View(person);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,LastName,FirstName,HireDate,EnrollmentDate,Discriminator")] Person person, HttpPostedFileBase file)
        {

            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/StudentImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }


            person.Student_img = pic;
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", person.PersonID);
            return View(person);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
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
