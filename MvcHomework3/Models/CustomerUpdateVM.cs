using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHomework3.Models
{
    public class CustomerUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EIN { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}