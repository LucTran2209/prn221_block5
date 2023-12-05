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
using NuGet.Protocol.Plugins;

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
			var account = await _context.Users.Include(acc => acc.Role).SingleOrDefaultAsync(acc => acc.Email.Equals(Account.Email));
			if (account != null)
			{
				if (account.Password == Account.Password)
				{
					string email = account.Email;
                    string fullName = account.FullName;
                    string role = account.Role.RoleName;
					string phone = account.Phone;

                    var accountClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim(ClaimTypes.Name, account.FullName),
                        new Claim(ClaimTypes.Role, account.Role.RoleName),
						new Claim(ClaimTypes.MobilePhone, account.Phone)
                    };

					var accountIdentity = new ClaimsIdentity(accountClaims, "Account Identity");
                    var accountPrincipal = new ClaimsPrincipal(new[] { accountIdentity });
                    HttpContext.SignInAsync(accountPrincipal);
                    return RedirectToPage("/Index");
                }
				else
				{
					ViewData["Error"] = "Mật khẩu không chính xác";
					return Page();
				}
			}
			else
			{
				ViewData["Error"] = "Tài khoản với Email không tồn tại";
				return Page();
			}
		}
	}
}
