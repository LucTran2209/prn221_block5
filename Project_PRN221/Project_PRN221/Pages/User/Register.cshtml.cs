using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IFormFile? FileInput { get; set; }

        private string? avatar = null;


        public void OnGetAsync()
        {
            LoadForm();
        }

        private void LoadForm()
        {
            ViewData["AgenceId"] = new SelectList(_context.Agences, "AgenceId", "AgenceName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadForm();
            if (!ModelState.IsValid)
            {

                return Page();

            }

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
                if(!String.IsNullOrEmpty(avatar))
                {
                    User.Avatar = avatar;
                    
                }
                _context.Users.Add(User);
                await _context.SaveChangesAsync();
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
