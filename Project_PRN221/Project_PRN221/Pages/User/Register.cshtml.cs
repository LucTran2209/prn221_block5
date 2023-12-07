using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public RegisterModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.User User { get; set; }

        [BindProperty]
        public IFormFile FileInput { get; set; }

        private string? avatar = null;

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {

            var account = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(User.Email));
            if (account == null)
            {
                if (FileInput != null && FileInput.Length > 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await FileInput.CopyToAsync(memoryStream);
                        byte[] bytes = memoryStream.ToArray();
                        avatar = Convert.ToBase64String(bytes);
                        
                    }
                }
                if(avatar != null)
                {
                    var new_user = new Models.User()
                    {
                        UserName = User.UserName,
                        FullName = User.FullName,
                        Avatar = avatar,
                        Email = User.Email,
                        Password = User.Password,
                        Phone = User.Phone,
                        RoleId = 2,
                    };
                    _context.Users.Add(new_user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ViewData["Error"] = "Định dạng ảnh không hợp lệ";
                    return Page();
                }


                return RedirectToPage("/user/login");
            }
            else
            {
                ViewData["Error"] = "Email is existed, pls choose another.";
                return Page();
            }

        }
    }
}
