using System.Collections.Generic;
using System.Linq;
using JWT_Authentication_Microservices.Models;

namespace JWT_Authentication_Microservices.Repositories
{
	public class UserRepository
	{
		public List<User> TestUsers;

		public UserRepository()
		{
			TestUsers = new List<User>();
			TestUsers.Add(new User() {Username = "Patrick", Password = "Fuel"});
			TestUsers.Add(new User() { Username = "Henry", Password = "Ford" });
		}

		public User GetUser(string username)
		{
			try
			{
				return TestUsers.First(user => user.Username.Equals(username));
			}
			catch
			{
				return null;
			}
		}
	}
}