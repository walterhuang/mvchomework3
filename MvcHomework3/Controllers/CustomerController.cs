using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHomework3.Models;
using System.IO;
using System.Text;

namespace MvcHomework3.Controllers
{
    //[CheckIdFilter]
    public class CustomerController : BaseController
    {
        //private CustomerEntities db = new CustomerEntities();
        private CustomerRepository repo = RepositoryHelper.GetCustomerRepository();

        public ActionResult Download()
        {
            string csv = CreateCSVTextFile(repo.All().ToList());
            return File(ASCIIEncoding.GetEncoding("big5").GetBytes(csv), "text/csv",
                string.Format("{0}_客戶資料匯出.csv", DateTime.Now.ToString("yyyyMMdd")));
        }

        // http://stackoverflow.com/a/17698392/823247
        private string CreateCSVTextFile<T>(List<T> data)
        {
            var properties = typeof(T).GetProperties();
            var result = new StringBuilder();

            foreach (var row in data)
            {
                var values = properties.Select(p => p.GetValue(row, null))
                                       .Select(v => StringToCSVCell(Convert.ToString(v)));
                var line = string.Join(",", values);
                result.AppendLine(line);
            }

            return result.ToString();
        }

        private string StringToCSVCell(string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }

        // GET: /Customer/
        public ActionResult Index()
        {
            return View(repo.All());
        }

        // GET: /Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = repo.All().SingleOrDefault(i => i.Id == id); //db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,EIN,Phone,Fax,Address,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //db.Customers.Add(customer);
                //db.SaveChanges();
                repo.Add(customer);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: /Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = repo.All().SingleOrDefault(i => i.Id == id); //db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,EIN,Phone,Fax,Address,Email")] CustomerUpdateVM customer)
        {
            if (TryUpdateModel(customer)) //ModelState.IsValid)
            {
                //db.Entry(customer).State = EntityState.Modified;
                //db.SaveChanges();
                var updated = repo.All().SingleOrDefault(i => i.Id == customer.Id);
                //updated.Name = customer.Name;
                //updated.EIN = customer.EIN;
                //updated.Phone = customer.Phone;
                //updated.Fax = customer.Fax;
                //updated.Address = customer.Address;
                //updated.Email = customer.Email;
                AutoMapper.Mapper.DynamicMap<CustomerUpdateVM, Customer>(customer, updated);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = repo.All().SingleOrDefault(i => i.Id == id); //db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = repo.All().SingleOrDefault(i => i.Id == id); //db.Customers.Find(id);
            repo.Delete(customer); //db.Customers.Remove(customer);
            repo.UnitOfWork.Commit(); //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose(); //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
