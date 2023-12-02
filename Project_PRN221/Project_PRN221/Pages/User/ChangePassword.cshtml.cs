using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Project_PRN221.Pages.User
{
	[Authorize(Roles = "User, Admin")]
	public class ChangePasswordModel : PageModel
	{
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public ChangePasswordModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
		[BindProperty]
		[Required(ErrorMessage = "Không để trống")]
		public string password { get; set; }
		[BindProperty]
		[Required(ErrorMessage = "Không để trống")]
		public string newpassword { get; set; }
		[Required(ErrorMessage = "Không để trống")]
		[BindProperty]
		public string repassword { get; set; }



		public void OnGet()
		{
			
		}
		public async Task<IActionResult> OnPostAsync()
		{
			string email = User.FindFirstValue(ClaimTypes.Email);
			var account = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(email));
			if (account != null)
			{
				if(account.Password.Equals(password))
				{
					if (newpassword.Equals(repassword))
					{
						account.Password = newpassword;
						_context.Users.Update(account);
						_context.SaveChanges();
						return RedirectToPage("/index");
					}
					ViewData["Error"] = "Mật khẩu mới không khớp";
				}
				else
				{
					ViewData["Error"] = "Mật khẩu không chính xác";
				}

			}
			else
			{
				ViewData["Error"] = "Tài khoản với email này không tồn tại";
			}
			return Page();
		}
	}
}
