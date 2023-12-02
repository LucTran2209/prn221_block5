using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.User
{
	public class SignUpModel : PageModel
	{
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public SignUpModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
		[BindProperty]
		public Models.User User { get; set; }
		public void OnGet()
		{

		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (User == null)
			{
				return Page();
			}
			else
			{
				var account = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(User.Email));
				if (account == null)
				{
					await _context.AddAsync(User);
					await _context.SaveChangesAsync();
					return RedirectToPage("/User/login");
				}
				else
				{
					ViewData["Error"] = "Email is existed, pls choose another.";
					return Page();
				}
			}
		}
	}
}
