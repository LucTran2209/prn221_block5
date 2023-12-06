using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Categories
{
    public class DeleteModel : PageModel
    {

        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public DeleteModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            try
            {
                var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
                if (category == null)
                {
                    return NotFound();
                }
                if(await _context.Documents.Where(d=> d.CategoryId == id).AnyAsync())
                {
                    ViewData["Error"] = "Có các văn bản khác thuộc thể loại " + category.CategoryName + ", không thể xóa";
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Xóa loại văn bản thành công";

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.ToString();
            }
            return RedirectToPage("Index");
        }
    }
}
