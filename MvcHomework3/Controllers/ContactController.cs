using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHomework3.Models;

namespace MvcHomework3.Controllers
{
    public class ContactController : Controller
    {
        //private CustomerEntities db = new CustomerEntities();
        private ContactRepository repo = RepositoryHelper.GetContactRepository();
        private CustomerRepository customerRepo;

        public ContactController()
        {
            customerRepo = RepositoryHelper.GetCustomerRepository(repo.UnitOfWork);
        }

        // GET: /Contact/
        public ActionResult Index()
        {
            var contacts = //db.Contacts.Include(c => c.Customer);
                repo.All().Include(c => c.Customer);
            return View(contacts.ToList());
        }

        // GET: /Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = //db.Contacts.Find(id);
                repo.All().SingleOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: /Contact/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All() , "Id", "Name");
            return View();
        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CustomerId,Title,Name,Email,Mobile,Phone")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                //db.Contacts.Add(contact);
                //db.SaveChanges();
                repo.Add(contact);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", contact.CustomerId);
            return View(contact);
        }

        // GET: /Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = //db.Contacts.Find(id);
                repo.All().SingleOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ContactUpdateVM vm = new ContactUpdateVM();
            AutoMapper.Mapper.DynamicMap<Contact, ContactUpdateVM>(contact, vm);
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", contact.CustomerId);

            var titles = //repo.All().ToList();
                repo.All().Select(i => i.Title).Distinct().ToList();
            var selectList = new List<SelectListItem>();
            foreach (var item in titles)
                selectList.Add(new SelectListItem { Text = item, Value = item });
            
            ViewBag.TitleList = new SelectList(selectList, "Value", "Text", vm.Title);
            
            return View(vm);
        }

        // POST: /Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CustomerId,Title,Name,Email,Mobile,Phone")] ContactUpdateVM contact)
        {
            //if (ModelState.IsValid)
            if(TryUpdateModel(contact) && ModelState.IsValid)
            {
                //db.Entry(contact).State = EntityState.Modified;
                //db.SaveChanges();
                var updated = repo.All().SingleOrDefault(c => c.Id == contact.Id);
                AutoMapper.Mapper.DynamicMap<ContactUpdateVM, Contact>(contact, updated);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", contact.CustomerId);
            return View(contact);
        }

        // GET: /Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = //db.Contacts.Find(id);
                repo.All().SingleOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(contact);
        }

        // POST: /Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Contact contact = repo.All().SingleOrDefault(c => c.Id == id); //db.Contacts.Find(id);
            //db.Contacts.Remove(contact);
            //db.SaveChanges();
            repo.Delete(contact);
            repo.UnitOfWork.Commit();
            //return RedirectToAction("Index");
            return Redirect(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
