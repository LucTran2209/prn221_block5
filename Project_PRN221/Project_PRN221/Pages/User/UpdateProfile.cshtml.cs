using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace Project_PRN221.Pages.User
{
    public class UpdateProfileModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHubContext<SignalRHub> _signalRHub;

        public UpdateProfileModel(PROJECT_SENT_DOCUMENTContext context, IWebHostEnvironment hostingEnvironment, IHubContext<SignalRHub> signalRHub)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _signalRHub = signalRHub;
        }
        public async Task OnGet()
        {
            var userid = Int32.Parse(User.FindFirstValue("AccountId"));
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);
            string path;
            try
            {
                if (String.IsNullOrEmpty(user.Avatar))
                {
                    throw new Exception();
                }
                byte[] imageBytes = Convert.FromBase64String(user.Avatar);
                string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", $"avatar{user.UserId}.png");
                if (System.IO.File.Exists(imagePath))
                {
                    // Nếu tệp tồn tại, xóa nó
                    System.IO.File.Delete(imagePath);
                }
                System.IO.File.WriteAllBytes(imagePath, imageBytes);
                path = $"/images/avatar{user.UserId}.png";
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
