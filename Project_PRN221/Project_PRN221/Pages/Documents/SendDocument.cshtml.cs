using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
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

        public async Task<IActionResult> OnGetAsync(string? doccumentNumber, int? categoryId, 
                                                    string? humanSign, DateTime? startDate, DateTime? endDate, 
                                                    string? username, string? title)
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
            var documentSendDetail = from Document in _context.Documents
                                     join sendDocument in _context.SendDocuments on Document.DocumentId equals sendDocument.DocumentId                                 
                                     select new SendDocumentDetailDto
                                     {
                                         DocumentNumber = Document.DocumentNumber,
                                         DocumentTitle = Document.Title,
                                         DocumentDescription = Document.Description,
                                         DocumentType = _context.Categories.SingleOrDefault(x => x.CategoryId == Document.CategoryId).CategoryName,
                                         DocumentUrl = Document.Content,
                                         UserReceive = _context.Users.SingleOrDefault(x => x.UserId == sendDocument.UserIdReceive).FullName,
                                         UserSend = _context.Users.SingleOrDefault(x => x.UserId == sendDocument.UserIdSend).FullName,
                                         AgenceReceive = _context.Users.Include(x => x.Agence).SingleOrDefault(x => x.UserId == sendDocument.UserIdReceive).Agence.AgenceName,
                                         SentDate = sendDocument.SentDate,
                                         IssueDate = Document.CreateDate,
                                         HumanSign = Document.HumanSign,
                                     };
           // ViewData["ListDocumentSent"] = list.ToList();
            return Page();
        }

        public void LoadForm()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "FullName");
        }
    }
}