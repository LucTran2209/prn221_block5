using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;

namespace Project_PRN221.Pages.Documents
{
	[Authorize(Roles = "User, Admin")]

	public class ReceiveDetailModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public ReceiveDetailModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        public Document Document { get; set; } = default!;
        public SendDocument SendDocument { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue("AccountId");
            var sendDocument =await _context.SendDocuments.FirstOrDefaultAsync(sd=>sd.UserIdReceive == int.Parse(userId) && sd.DocumentId == id);
            var document = await _context.Documents.Include(d=>d.User).Include(d=>d.Agence).FirstOrDefaultAsync(d=>d.DocumentId == id);
            if (document == null || sendDocument == null)
            {
                return NotFound();
            }
            Document = document;
            SendDocument = sendDocument;
            return Page();

        }
    }
}
