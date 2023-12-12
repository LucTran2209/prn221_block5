using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;

namespace Project_PRN221.Pages.User
{
    [Authorize(Roles = "User, Admin")]
    public class ChangeProfileModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<SignalRHub> _signalRHub;

        public ChangeProfileModel(PROJECT_SENT_DOCUMENTContext context, IWebHostEnvironment hostingEnvironment, IHubContext<SignalRHub> signalRHub)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _signalRHub = signalRHub;
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
            var profile = _context.Users.Include(user=>user.Role).FirstOrDefault(p => p.Email == userEmail);
            return profile;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = GetProfile();
            if (user != null)
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
                            user.Avatar = avatar;
                        }
                    }
                    user.FullName = Fullname;
                    user.Phone = Phone;
                    user.AgenceId = AgenceId;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    UpdateProfile(user);
                    var profile = new {
                        id = user.UserId,
                        name = user.FullName,
                        email = user.Email,
                        phone=user.Phone,
                        avatar = String.IsNullOrEmpty(user.Avatar)? "/images/default.png" : $"/images/avatar{user.UserId}.png"
                    };
                    await _signalRHub.Clients.All.SendAsync("LoadProfile", profile);
                    ViewData["Message"] = "Thay đổi thông tin tài khoản thành công";
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Thay đổi thông tin tài khoản thất bại";
                    LoadForm();
                    return Page();
                }
                return RedirectToPage();
            }
            else
            {
                return NotFound();
            }

        }

        private void UpdateProfile(Models.User user)
        {
            string path;
            try
            {
                if (String.IsNullOrEmpty(user.Avatar))
                {
                    throw new Exception();
                }
                byte[] imageBytes = Convert.FromBase64String(user.Avatar);
                string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "avatar.png");
                if (System.IO.File.Exists(imagePath))
                {
                    // Nếu tệp tồn tại, xóa nó
                    System.IO.File.Delete(imagePath);
                }
                System.IO.File.WriteAllBytes(imagePath, imageBytes);
                path = "/images/avatar.png";
            }
            catch (Exception ex)
            {
                path = "/images/default.png";
            }
            var accountClaims = new List<Claim>()
            {
                        new Claim("AccountId", user.UserId.ToString()),
                        new Claim("Avatar", path),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Role, user.Role.RoleName),
                        new Claim(ClaimTypes.MobilePhone, user.Phone)
                    };
            var accountIdentity = new ClaimsIdentity(accountClaims, "Account Identity");
            var accountPrincipal = new ClaimsPrincipal(new[] { accountIdentity });
            HttpContext.SignInAsync(accountPrincipal);
        }
    }
}
