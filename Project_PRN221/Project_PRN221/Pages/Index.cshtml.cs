using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Project_PRN221.Pages
{
	public class IndexModel : PageModel
	{
		//private readonly PROJECT_SENT_DOCUMENTContext _context;
		//public IndexModel(PROJECT_SENT_DOCUMENTContext context)
		//{
		//	_context = context;
		//}
		//public IList<Product> ListProducts { get; set; }
		//public Models.User GetUser { get; set; }
		//public async Task OnGetAsync()
		//{
		//	ListProducts = await _context.Products.Include(c => c.Category).ToListAsync();
		//	String contact = HttpContext.Session.GetString("CusSession");
		//	if (contact != null)
		//	{
		//		GetUser = JsonSerializer.Deserialize<Models.User>(contact);
		//		ViewData["Contact"] = GetUser.ContactName;
		//	}
		//}
	}
}