using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN221.Pages.User
{
	public class LogoutModel : PageModel
	{
		public async Task OnGetAsync()
		{
			var schema = CookieAuthenticationDefaults.AuthenticationScheme;
			await HttpContext.SignOutAsync(schema);

			var check_cus_login = HttpContext.Session.GetString("CusSession");
			var check_emp_login = HttpContext.Session.GetString("EmpSession");
			if (check_cus_login != null)
			{
				HttpContext.Session.Remove("CusSession");
				Response.Redirect("/index");
			}
			if (check_emp_login != null)
			{
				HttpContext.Session.Remove("EmpSession");
				Response.Redirect("/index");
			}
			Response.Redirect("/index");
		}
	}
}
