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
		private readonly IWebHostEnvironment _hostingEnvironment;

        public LoginModel(PROJECT_SENT_DOCUMENTContext context, IWebHostEnvironment hostingEnvironment)
		{
			_context = context;
			_hostingEnvironment = hostingEnvironment;
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
					string avatar = account.Avatar;
					try
					{
                        byte[] imageBytes = Convert.FromBase64String(avatar);
                        string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "avatar.png");
                        System.IO.File.WriteAllBytes(imagePath, imageBytes);
                    }catch(Exception ex)
					{

					}
                    HttpContext.Session.SetString("AvatarPath", $"/images/avatar.png");
                    var accountClaims = new List<Claim>()
                    {
                        new Claim("AccountId", account.UserId.ToString()),
                        new Claim(ClaimTypes.Email,email),
                        new Claim(ClaimTypes.Name, fullName),
                        new Claim(ClaimTypes.Role, role),
						new Claim(ClaimTypes.MobilePhone, phone)
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
