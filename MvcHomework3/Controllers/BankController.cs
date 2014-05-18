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
    public class BankController : Controller
    {
        //private CustomerEntities db = new CustomerEntities();
        private BankRepository repo = RepositoryHelper.GetBankRepository();
        private CustomerRepository customerRepo;
        public BankController()
        {
            customerRepo = RepositoryHelper.GetCustomerRepository(repo.UnitOfWork);
        }

        // GET: /Bank/
        public ActionResult Index()
        {
            var banks = //db.Banks.Include(b => b.Customer);
                repo.All().Include(b => b.Customer);
            return View(banks.ToList());
        }

        // GET: /Bank/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = //db.Banks.Find(id);
                repo.All().SingleOrDefault(b => b.Id == id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // GET: /Bank/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All() , "Id", "Name");
            return View();
        }

        // POST: /Bank/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CustomerId,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                //db.Banks.Add(bank);
                //db.SaveChanges();
                repo.Add(bank);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", bank.CustomerId);
            return View(bank);
        }

        // GET: /Bank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = //db.Banks.Find(id);
                repo.All().SingleOrDefault(b => b.Id == id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", bank.CustomerId);
            return View(bank);
        }

        // POST: /Bank/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CustomerId,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] BankUpdateVM bank)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(bank).State = EntityState.Modified;
                //db.SaveChanges();
                var updated = repo.All().FirstOrDefault(b => b.Id == bank.Id);
                AutoMapper.Mapper.DynamicMap<BankUpdateVM, Bank>(bank, updated);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(/*db.Customers*/ customerRepo.All(), "Id", "Name", bank.CustomerId);
            return View(bank);
        }

        // GET: /Bank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = //db.Banks.Find(id);
                repo.All().SingleOrDefault(b => b.Id == id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // POST: /Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bank bank = //db.Banks.Find(id);
                repo.All().SingleOrDefault(b => b.Id == id);
            //db.Banks.Remove(bank);
            //db.SaveChanges();
            repo.Delete(bank);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
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
