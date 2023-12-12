using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Dto;
using Project_PRN221.Models;
using System.Security.Claims;

namespace Project_PRN221.Pages.Documents
{
    public class SendDocumentModel : PageModel
    {
        public readonly PROJECT_SENT_DOCUMENTContext _context;

        public SendDocumentModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        public string DoccumentNumber { get; set; }
        public int CategoryID { get; set; }
        public string HumanSign { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;

        public async Task<IActionResult> OnGetAsync(string? doccumentNumber, int? categoryId, string? humanSign, DateTime? startDate, DateTime? endDate, string? username, string? title)
        {
            LoadForm();
            int userId = Int32.Parse(@User.FindFirstValue("AccountId"));
            var query = _context.SendDocuments
                .Include(d => d.Document)
                .Where(d => d.UserIdSend == userId)
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).AsQueryable();
            if (doccumentNumber != null) { query = query.Where(d => d.Document.DocumentNumber.ToLower().Contains(doccumentNumber.ToLower())); }
            if (categoryId != null) { query = query.Where(d => d.Document.CategoryId == categoryId); }
            if (humanSign != null) { query = query.Where(d => d.Document.HumanSign.ToLower().Contains(humanSign.ToLower())); }
            if (startDate != null) { query = query.Where(d => d.SentDate >= startDate); }
            if (endDate != null) { query = query.Where(d => d.SentDate <= endDate); }
            if (title != null) { query = query.Where(d => d.Document.Title.ToLower().Contains(title.ToLower())); }
            var list = from document in query
                       select new SendDocumentDto
                       {
                           DocumentNumber = document.Document.DocumentNumber,
                           CreateDate = document.Document.CreateDate.ToString("dd/MM/yyyy"),
                           Description = document.Document.Description,
                           SendDate = document.SentDate.ToString("dd/MM/yyyy"),
                           AgenceReceive = _context.Agences.FirstOrDefault(h => h.AgenceId == (_context.Users.FirstOrDefault(u => u.UserId == document.UserIdReceive)).AgenceId).AgenceName,
                       };
            ViewData["ListDocumentSent"] = list.ToList();
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(string? doccumentNumber, int? categoryId, string? humanSign, DateTime? startDate, DateTime? endDate, string? username, string? title)
        {
            int userId = Int32.Parse(@User.FindFirstValue("AccountId"));
            var query = _context.SendDocuments
                .Include(d => d.Document)
                .Where(d => d.UserIdSend == userId)
                .OrderBy(d => d.IsRead).ThenBy(d => d.SentDate).AsQueryable();
            if (doccumentNumber != null) { query = query.Where(d => d.Document.DocumentNumber.ToLower().Contains(doccumentNumber.ToLower())); }
            if (categoryId != null) { query = query.Where(d => d.Document.CategoryId == categoryId); }
            if (humanSign != null) { query = query.Where(d => d.Document.HumanSign.ToLower().Contains(humanSign.ToLower())); }
            if (startDate != null) { query = query.Where(d => d.SentDate >= startDate); }
            if (endDate != null) { query = query.Where(d => d.SentDate <= endDate); }
            if (title != null) { query = query.Where(d => d.Document.Title.ToLower().Contains(title.ToLower())); }
            var list = from document in query
                       select new SendDocumentDto
                       {
                           DocumentNumber = document.Document.DocumentNumber,
                           CreateDate = document.Document.CreateDate.ToString("dd/MM/yyyy"),
                           Description = document.Document.Description,
                           SendDate = document.SentDate.ToString("dd/MM/yyyy"),
                           AgenceReceive = _context.Agences.FirstOrDefault(h => h.AgenceId == (_context.Users.FirstOrDefault(u => u.UserId == document.UserIdReceive)).AgenceId).AgenceName,
                       };
            return new JsonResult(list.ToList());
        }

        public void LoadForm()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "FullName");
        }
    }
}