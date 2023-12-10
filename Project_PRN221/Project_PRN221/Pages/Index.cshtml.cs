using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Project_PRN221.Pages
{
	[Authorize(Roles = "User, Admin")]

	public class IndexModel : PageModel
	{
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public IndexModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
		public IList<Document> ListDocument { get; set; }
		public Models.User GetUser { get; set; }
		public async Task OnGetAsync()
		{
			int userId = Int32.Parse(@User.FindFirstValue("AccountId"));

			var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

			ListDocument = await _context.Documents.ToListAsync();
			//var list = ListDocument
		}
	}
}