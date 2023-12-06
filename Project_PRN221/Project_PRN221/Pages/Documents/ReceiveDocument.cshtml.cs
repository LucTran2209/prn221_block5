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

        public string DoccumentNumber { get; set; }
        public int CategoryID { get; set; }
        public string HumanSign {  get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;
        public List<SendDocument> documents {  get; set; }

        public async Task<IActionResult> OnGetAsync(string? doccumentNumber, int? categoryId, string? humanSign, DateTime? startDate, DateTime? endDate, string? username, string? title)
        {
            LoadForm();
            string userId = User.FindFirstValue("AccountId");
            var query = _context.SendDocuments
                .Include(d => d.Document).Include(d=>d.UserIdSendNavigation)
                .Where(d => d.UserIdReceive == int.Parse(userId))
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).AsQueryable();
            if (doccumentNumber != null) { query = query.Where(d => d.Document.DocumentNumber.ToLower().Contains(doccumentNumber.ToLower())); }
            if (categoryId != null) { query = query.Where(d => d.Document.CategoryId == categoryId); }
            if (humanSign != null) { query = query.Where(d => d.Document.HumanSign.ToLower().Contains(humanSign.ToLower())); }
            if (startDate != null) {query = query.Where(d => d.SentDate >= startDate); }
            if (endDate != null) {query = query.Where(d => d.SentDate <= endDate); }
            if (username != null) { query = query.Where(d => d.UserIdSendNavigation.FullName.ToLower().Contains(username.ToLower())); }
            if (title != null) { query = query.Where(d => d.Document.Title.ToLower().Contains(title.ToLower())); }
            documents = query.ToList();
            return Page();
        }

        public void LoadForm()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "FullName");
        }
    }
}
