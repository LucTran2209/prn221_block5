using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Data;
using System.Security.Claims;

namespace Project_PRN221.Pages.Documents
{
	//[Authorize(Roles = "User, Admin")]
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
                document.UserId = 1;
                document.AgenceId = 1;

				string userId = User.FindFirstValue("AccountId");


				if (FileInput != null && FileInput.Length > 0)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						await FileInput.CopyToAsync(memoryStream);
						byte[] bytes = memoryStream.ToArray();
						string base64String = Convert.ToBase64String(bytes);

						// Thực hiện xử lý với chuỗi base64 ở đây
						// Ví dụ: lưu vào cơ sở dữ liệu hoặc hiển thị trên trang
						//ViewData["Base64Image"] = base64String;
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
