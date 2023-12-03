using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN221.Pages.Documents
{
	[Authorize(Roles = "User, Admin")]

	public class ReceiveDetailModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
