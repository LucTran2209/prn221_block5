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
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var account = await _context.Users.SingleOrDefaultAsync(acc => acc.Email.Equals(User.Email));
                if (account == null)
                {
                    var new_user = new Models.User()
                    {
                        UserName = User.UserName,
                        FullName = User.FullName,
                        Email = User.Email,
                        Password = User.Password,
                        Phone = User.Phone,
                        Avatar = User.Avatar,
                        RoleId = 2,

                    };
                    await _context.AddAsync(new_user);
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
}
