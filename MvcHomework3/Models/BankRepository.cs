using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MvcHomework3.Models
{   
	public  class BankRepository : EFRepository<Bank>, IBankRepository
	{

	}

	public  interface IBankRepository : IRepository<Bank>
	{

	}
}