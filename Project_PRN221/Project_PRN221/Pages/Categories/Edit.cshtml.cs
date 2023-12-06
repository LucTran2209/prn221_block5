using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Categories
{
    public class EditModel : PageModel
    {
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public EditModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> OnPostAsync(int? id, string categoryName)
		{
			if(id == null || _context.Categories == null) {
				return NotFound();
			}
			try
			{
				var category = await _context.Categories.SingleOrDefaultAsync(c=>c.CategoryId == id);
				if(category == null)
				{
					return NotFound();
				}
				category.CategoryName = categoryName;
				_context.Categories.Update(category);
				await _context.SaveChangesAsync();
				ViewData["Message"] = "Chỉnh sửa loại văn bản thành công";

			}
			catch (Exception ex)
			{
				ViewData["Error"] = ex.ToString();
			}
			return RedirectToPage("Index");
		}
	}
}
