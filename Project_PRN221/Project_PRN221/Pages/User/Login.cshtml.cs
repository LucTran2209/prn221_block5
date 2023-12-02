using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Project_PRN221.Models;
using Microsoft.EntityFrameworkCore;

namespace Project_PRN221.Pages.User
{
	public class LoginModel : PageModel
	{
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public LoginModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
		[BindProperty]
		public Models.User Account { get; set; }
		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var check_email = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(Account.Email));
			if (check_email != null)
			{
				var account = await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(Account.Email) && u.Password.Equals(Account.Password));
				if (account != null)
				{
					if (account.RoleId == 2 || account.RoleId == 3)
					{
						HttpContext.Session.SetString("CusSession", JsonSerializer.Serialize(account));
						return RedirectToPage("/Index");
					}
					else
					{
						if (account.RoleId == 1)
						{
							var schema = CookieAuthenticationDefaults.AuthenticationScheme;
							var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, Account.Email) }, schema);
							var user = new ClaimsPrincipal(identity);
							await HttpContext.SignInAsync(schema, user);
							HttpContext.Session.SetString("EmpSession", JsonSerializer.Serialize(account));
							return RedirectToPage("/admin/dashboard");
						}
						ViewData["Error"] = "Password is not correct. ";
						return Page();
					}
				}
				else
				{
					ViewData["Error"] = "Password is incorrect.";
					return Page();
				}
			}
			else
			{
				ViewData["Error"] = "Wrong email or password, pls try again";
				return Page();
			}
		}
	}
}
