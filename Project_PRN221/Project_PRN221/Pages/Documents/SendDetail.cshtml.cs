using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Dto;
using Project_PRN221.Models;
using System.Security.Claims;
using System.Xml.Linq;

namespace Project_PRN221.Pages.Documents
{
    public class SendDetailModel : PageModel
    {
        private readonly PROJECT_SENT_DOCUMENTContext _context;
        public SendDetailModel(PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SendDocumentDetailDto SendDocumentDetailDtoSend { get; set; }

        public IActionResult OnGet(int? sendId)
        {
            if (sendId == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue("AccountId");

            var documentSendDetail = from Document in _context.Documents
                                     join sendDocument in _context.SendDocuments on Document.DocumentId equals sendDocument.DocumentId
                                     where sendDocument.SendId == sendId
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

            var list = documentSendDetail.ToList();
            SendDocumentDetailDtoSend = list[0];

            return Page();

        }
    }
}