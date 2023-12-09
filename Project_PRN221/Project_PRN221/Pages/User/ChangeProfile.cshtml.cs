using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Project_PRN221.Pages.User
{
    [Authorize(Roles = "User, Admin")]
    public class ChangeProfileModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public ChangeProfileModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Không được trống")]
        public string Fullname { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Không được trống")]
        [RegularExpression(@"^(\+)?[0-9]{9,15}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Không được trống")]
        public int AgenceId { get; set; }
        [BindProperty]
        public IFormFile? ImageAvatar { get; set; }

        public void OnGet()
        {
            LoadForm();
        }

        private void LoadForm()
        {
            ViewData["AgenceId"] = new SelectList(_context.Agences, "AgenceId", "AgenceName");
            var profile = GetProfile();
            Fullname = profile.FullName;
            Phone = profile.Phone;
            AgenceId = profile.AgenceId;
        }

        private Models.User? GetProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var profile = _context.Users.FirstOrDefault(p => p.Email == userEmail);
            return profile;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var profile = GetProfile();
            if (profile != null)
            {
                try
                {
                    if (ImageAvatar != null && ImageAvatar.Length > 0)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await ImageAvatar.CopyToAsync(memoryStream);
                            byte[] bytes = memoryStream.ToArray();
                            var avatar = Convert.ToBase64String(bytes);
                            profile.Avatar = avatar;
                        }
                    }
                    profile.FullName = Fullname;
                    profile.Phone = Phone;
                    profile.AgenceId = AgenceId;
                    _context.Users.Update(profile);
                    await _context.SaveChangesAsync();
                    ViewData["Message"] = "Thay đổi thông tin tài khoản thành công";
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Thay đổi thông tin tài khoản thất bại";
                }
                LoadForm();
                return Page();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
