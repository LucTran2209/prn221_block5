using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Text;
using System;

namespace Project_PRN221.Pages.User
{
	public class ForgetPasswordModel : PageModel
	{
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public ForgetPasswordModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}

		[BindProperty]
		public string email { get; set; }
		[BindProperty]
		public string username { get; set; }

		public void OnGet()
		{

		}
		public async Task<IActionResult> OnPostAsync()
		{

			var account = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(email));
			if (account != null)
			{
				//Xử Lý tìm mật khẩu
				if(account.UserName.Equals(username))
				{
					string newpass = RandomPassword(8);
					account.Password = newpass;
					_context.Users.Update(account);
					_context.SaveChanges();

					ViewData["Error"] = "Mật khẩu mới: " + newpass;
				}
				else
				{
					ViewData["Error"] = "Tên đăng nhập không khớp";
				}

			}
			else
			{
				ViewData["Error"] = "Tài khoản với email này không tồn tại";
			}
			return Page();

		}
		private static Random random = new Random();
		private string RandomPassword(int length)
		{

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder stringBuilder = new StringBuilder(length);

			for (int i = 0; i < length; i++)
			{
				stringBuilder.Append(chars[random.Next(chars.Length)]);
			}

			return stringBuilder.ToString();
		}
	}
}
