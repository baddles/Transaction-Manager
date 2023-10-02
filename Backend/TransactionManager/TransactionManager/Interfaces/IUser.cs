using System;
namespace TransactionManager.Interfaces
{
	public interface IUser
	{
		public byte[]? GetUserPasswordFromDB(string username);
		/*
		 * Following function have no plans to implement in the near future:
		 * public void AddUser(User newUser); // register function
		 * public User DeleteUser(string username); // delete user function.
		 * public void UpdateUser(User user); // change password
		*/
	}
}

