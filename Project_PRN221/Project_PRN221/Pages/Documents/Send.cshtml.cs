using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Documents
{
    public class SendModel : PageModel
    {
		public readonly PROJECT_SENT_DOCUMENTContext _context;

		public SendModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
	
		public void OnGet() {
		}
	}
}
