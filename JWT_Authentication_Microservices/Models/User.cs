﻿
namespace JWT_Authentication_Microservices.Models
{
	public class User
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public UserRole UserRole { get; set; }
	}
}