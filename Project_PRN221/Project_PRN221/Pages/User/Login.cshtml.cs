using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Project_PRN221.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Không được để trống")]
		[EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "Không được để trống")]
		public string Password { get; set; }

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			var account = await _context.Users.Include(acc => acc.Role).SingleOrDefaultAsync(acc => acc.Email.Equals(Email));
			if (account != null)
			{
				if (account.Password == Password)
				{
					string email = account.Email;
                    string fullName = account.FullName;
                    string role = account.Role.RoleName;
					string phone = account.Phone;
					string avatar = account.Avatar;
					string path;
					try
					{
                        byte[] imageBytes = Convert.FromBase64String(avatar);
                        string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "avatar.png");
                        if (System.IO.File.Exists(imagePath))
                        {
                            // Nếu tệp tồn tại, xóa nó
                            System.IO.File.Delete(imagePath);
                        }
                        System.IO.File.WriteAllBytes(imagePath, imageBytes);
						path = "/images/avatar.png";
					}
					catch(Exception ex)
					{
						path = "/images/default.png";
					}
                    var accountClaims = new List<Claim>()
                    {
                        new Claim("AccountId", account.UserId.ToString()),
                        new Claim("Avatar", path),
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
