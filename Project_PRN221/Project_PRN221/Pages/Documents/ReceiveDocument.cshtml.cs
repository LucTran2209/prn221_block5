using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.Security.Claims;
using System.Threading;

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
        public string DoccumentNumber { get; set; }
        [BindProperty]
        public int? CategoryID { get; set; } = null!;
        [BindProperty]
        public string HumanSign {  get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public DateTime StartDate { get; set; }
        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now!;
        [BindProperty]
        public bool? IsRead { get; set; } = null;
        [BindProperty]
        public int? IndexPage { get; set; }
        public List<SendDocument> documents { get; set; }
         //public PaginatedList<SendDocument> documents {  get; set; }

        public async Task<IActionResult> OnGetAsync(string? doccumentNumber, int? categoryId, string? humanSign, DateTime? startDate, DateTime? endDate, string? username, string? title, bool? isRead, int? pageIndex)
        {
            LoadForm();
            string userId = User.FindFirstValue("AccountId");
            var query = _context.SendDocuments
                .Include(d => d.Document).Include(d=>d.UserIdSendNavigation)
                .Where(d => d.UserIdReceive == int.Parse(userId))
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).AsQueryable();
            if (doccumentNumber != null) { query = query.Where(d => d.Document.DocumentNumber.ToLower().Contains(doccumentNumber.ToLower())); DoccumentNumber = doccumentNumber; }
            if (categoryId != null) { query = query.Where(d => d.Document.CategoryId == categoryId); CategoryID = categoryId; }
            if (humanSign != null) { query = query.Where(d => d.Document.HumanSign.ToLower().Contains(humanSign.ToLower())); HumanSign = humanSign; }
            if (startDate != null) {query = query.Where(d => d.SentDate >= startDate); StartDate = startDate.Value; }
            if (endDate != null) {query = query.Where(d => d.SentDate <= endDate); EndDate = endDate.Value; }
            if (username != null) { query = query.Where(d => d.UserIdSendNavigation.FullName.ToLower().Contains(username.ToLower())); Username = username; }
            if (title != null) { query = query.Where(d => d.Document.Title.ToLower().Contains(title.ToLower())); Title = title; }
            if (isRead != null) { query = query.Where(d => d.IsRead == isRead); IsRead = isRead; }
            //documents = await PaginatedList<SendDocument>.CreateAsync(query.AsNoTracking(), pageIndex??1, 4);
            documents = query.ToList();
            //IndexPage = documents.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? doccumentNumber, int? categoryId, string? humanSign, DateTime? startDate, DateTime? endDate, string? username, string? title, bool? isRead, int? pageIndex)
        {
            string userId = User.FindFirstValue("AccountId");
            var query = _context.SendDocuments
                .Include(d => d.Document).Include(d => d.UserIdSendNavigation)
                .Where(d => d.UserIdReceive == int.Parse(userId))
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).AsQueryable();
            if (doccumentNumber != null) { query = query.Where(d => d.Document.DocumentNumber.ToLower().Contains(doccumentNumber.ToLower())); DoccumentNumber = doccumentNumber; }
            if (categoryId != null) { query = query.Where(d => d.Document.CategoryId == categoryId); CategoryID = categoryId; }
            if (humanSign != null) { query = query.Where(d => d.Document.HumanSign.ToLower().Contains(humanSign.ToLower())); HumanSign = humanSign; }
            if (startDate != null) { query = query.Where(d => d.SentDate >= startDate); StartDate = startDate.Value; }
            if (endDate != null) { query = query.Where(d => d.SentDate <= endDate); EndDate = endDate.Value; }
            if (username != null) { query = query.Where(d => d.UserIdSendNavigation.FullName.ToLower().Contains(username.ToLower())); Username = username; }
            if (title != null) { query = query.Where(d => d.Document.Title.ToLower().Contains(title.ToLower())); Title = title; }
            if (isRead != null) { query = query.Where(d => d.IsRead == isRead); IsRead = isRead; }
            //documents = await PaginatedList<SendDocument>.CreateAsync(query.AsNoTracking(), pageIndex??1, 4);
            documents = await query.ToListAsync();
            return new JsonResult(documents);
        }

        public void LoadForm()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "FullName");
        }
    }
}
