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
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            Response.Redirect("/index");
		}
	}
}
