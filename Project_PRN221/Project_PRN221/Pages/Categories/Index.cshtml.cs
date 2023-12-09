using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN221.Models;
using System.ComponentModel.DataAnnotations;

namespace Project_PRN221.Pages.Categories
{
    public class IndexModel : PageModel
    {
		private readonly PROJECT_SENT_DOCUMENTContext _context;
		public IndexModel(PROJECT_SENT_DOCUMENTContext context)
		{
			_context = context;
		}

		public List<Category> Categories { get; set; } = default!;
		[BindProperty]
		[Required(ErrorMessage = "Không được để trống")]
		public string categoryName { get; set; } = default!;

		public IActionResult OnGet()
        {
			GetCategories();
			return Page();
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			var category = new Category { CategoryName = categoryName};
			try
			{
				_context.Categories.Add(category);
				await _context.SaveChangesAsync();
				ViewData["Message"] = "Thêm loại văn bản mới thành công";

			}catch (Exception ex)
			{
				ViewData["Error"] = ex.ToString();
			}
			GetCategories();
			return Page();
		}

		public void GetCategories()
		{
			Categories = _context.Categories.ToList();

		}
	}
}
