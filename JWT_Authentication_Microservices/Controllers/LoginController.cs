using System.Net;
using System.Net.Http;
using System.Web.Http;
using JWT_Authentication_Microservices.Models;
using JWT_Authentication_Microservices.Repositories;

namespace JWT_Authentication_Microservices.Controllers
{
    public class LoginController : ApiController
    {
		/// <summary>
		/// check if username and password are valid and then generate a token based on the username
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[System.Web.Mvc.HttpPost]
		public HttpResponseMessage Login(User user)
		{
			User u = new UserRepository().GetUser(user.Username);
			if (u == null)
			{
				return Request.CreateResponse(HttpStatusCode.NotFound,"The user was not found.");
			}
			bool credentials = u.Password.Equals(user.Password);
			return !credentials ? 
				Request.CreateResponse(HttpStatusCode.Forbidden, "The username/password combination was wrong.") : 
				Request.CreateResponse(HttpStatusCode.OK,TokenManager.GenerateToken(user.Username));
		}

	    [HttpGet]
	    public HttpResponseMessage Validate(string token, string username)
	    {
		    bool exists = new UserRepository().GetUser(username) != null;
		    if (!exists)
		    {
			    return Request.CreateResponse(HttpStatusCode.NotFound,"The user was not found.");
		    }
		    return Request.CreateResponse(TokenManager.ValidateToken(token, username) ? 
			    HttpStatusCode.OK : 
			    HttpStatusCode.BadRequest);
	    }
	}
}