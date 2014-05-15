namespace MvcHomework3.Models
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}		
		
		public static BankRepository GetBankRepository()
		{
			var repository = new BankRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static BankRepository GetBankRepository(IUnitOfWork unitOfWork)
		{
			var repository = new BankRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ContactRepository GetContactRepository()
		{
			var repository = new ContactRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ContactRepository GetContactRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ContactRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static CustomerRepository GetCustomerRepository()
		{
			var repository = new CustomerRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static CustomerRepository GetCustomerRepository(IUnitOfWork unitOfWork)
		{
			var repository = new CustomerRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}