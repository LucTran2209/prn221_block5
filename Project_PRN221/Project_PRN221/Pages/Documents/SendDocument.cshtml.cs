using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Project_PRN221.Dto;
using Project_PRN221.Models;
using System.Collections.Generic;
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
        public bool ShowPrevious { get; set; }
        public bool ShowNext { get; set; }
        public int PageIndex { get; set; }
        public string DoccumentNumber { get; set; }
        public int CategoryID { get; set; }
        public string HumanSign { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now;

        public async Task<IActionResult> OnGetAsync(string? doccumentNumber, int? categoryId,
                                                    string? humanSign, DateTime? startDate, DateTime? endDate,
                                                    string? username, string? title,
                                                    int pageIndex = 1)
        {
            LoadForm();
            int userId = Int32.Parse(@User.FindFirstValue("AccountId"));

            const int pageSize = 3;

            var list = from Document in _context.Documents
                       join sendDocument in _context.SendDocuments on Document.DocumentId equals sendDocument.DocumentId
                       where sendDocument.UserIdSend == userId
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
                           CategoryId = _context.Categories.SingleOrDefault(x => x.CategoryId == Document.CategoryId).CategoryId,
                           SendId = sendDocument.SendId,
                       };
            if (doccumentNumber != null) { list = list.Where(d => d.DocumentNumber.Contains(doccumentNumber));  DoccumentNumber = doccumentNumber; }
            if (categoryId != null) { list = list.Where(d => d.CategoryId == categoryId); CategoryID = (int)categoryId;}
            if (humanSign != null) { list = list.Where(d => d.HumanSign.Contains(humanSign)); HumanSign = humanSign; }
            if (startDate != null) { list = list.Where(d => d.SentDate >= startDate); StartDate = (DateTime)startDate; }
            if (endDate != null) { list = list.Where(d => d.SentDate <= endDate); EndDate = (DateTime)endDate; }
            if (username != null) { list = list.Where(d => d.UserReceive.Contains(username)); Username = username; }
            if (title != null) { list = list.Where(d => d.DocumentTitle.Contains(title)); Title = title; }
            var totalAlbums = list.Count();
            var totalPages = (int)Math.Ceiling((double)totalAlbums / pageSize);

            // Update the ShowPrevious and ShowNext properties based on pageIndex
            ShowPrevious = pageIndex > 1;
            ShowNext = pageIndex < totalPages;

            // Apply paging to the query
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            PageIndex = pageIndex;
            ViewData["ListDocumentSent"] = list.OrderByDescending(x => x.SentDate).ToList();
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