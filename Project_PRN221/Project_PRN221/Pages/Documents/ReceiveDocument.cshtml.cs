using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;

namespace Project_PRN221.Pages.Documents
{
	[Authorize(Roles = "User, Admin")]
	public class ReceiveDocumentModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public ReceiveDocumentModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int CategoryID { get; set; }
        [BindProperty]
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;

        [BindProperty]
        public List<SendDocument> documents {  get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoryId, int? userID, DateTime? startDate, DateTime? endDate)
        {
            LoadForm();
            string userId = User.FindFirstValue("AccountId");
            documents = _context.SendDocuments
                .Include(d => d.Document).Include(d=>d.UserIdSendNavigation)
                .Where(d => d.UserIdReceive == int.Parse(userId))
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).ToList();
            if (categoryId != null) {CategoryID = categoryId.Value; documents = documents.Where(d => d.Document.CategoryId == categoryId).ToList(); }
            if (userID != null) { UserID = userID.Value; documents = documents.Where(d => d.UserIdSend == userID).ToList(); }
            if (startDate != null) { StartDate = startDate.Value; documents = documents.Where(d => d.SentDate >= startDate).ToList(); }
            if (endDate != null) { EndDate = endDate.Value;  documents = documents.Where(d => d.SentDate <= endDate).ToList(); }
            return Page();
        }

        public void LoadForm()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "FullName");
        }
    }
}
