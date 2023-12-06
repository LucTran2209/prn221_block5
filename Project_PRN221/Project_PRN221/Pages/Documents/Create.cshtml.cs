using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;

namespace Project_PRN221.Pages.Documents
{
    public class CreateModel : PageModel
    {
		public readonly PROJECT_SENT_DOCUMENTContext _context;

		public CreateModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}

		[BindProperty]
		public Document document { get; set; }

		[BindProperty]
		public IFormFile FileInput { get; set; }


		public async Task<IActionResult> OnGetAsync()
		{
			ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (document != null)
			{
				document.CreateDate = DateTime.Now;
				if (FileInput != null && FileInput.Length > 0)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						await FileInput.CopyToAsync(memoryStream);
						byte[] bytes = memoryStream.ToArray();
						string base64String = Convert.ToBase64String(bytes);

						document.Content = base64String;
					}
				}
			}

			_context.Documents.Add(document);
			_context.SaveChanges();

			return RedirectToPage("/Index");
		}
	}
}
