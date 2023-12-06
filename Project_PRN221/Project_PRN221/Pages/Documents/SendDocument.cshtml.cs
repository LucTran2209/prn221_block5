using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public List<SendDocumentDto> ListDocument { get; set; }

        [BindProperty]
        public SendDocument Send { get; set; }

   

        public async Task OnGetAsync()
        {

            int userId = Int32.Parse(@User.FindFirstValue("AccountId"));
            var list = from document in _context.Documents
                       join sentDocument in _context.SendDocuments on document.DocumentId equals sentDocument.DocumentId
                       where sentDocument.UserIdSend == userId
                       select new SendDocumentDto
                       {
                           DocumentNumber = document.DocumentNumber,
                           CreateDate = document.CreateDate.ToString("dd/MM/yyyy"),
                           Description = document.Description,
                           SendDate = sentDocument.SentDate.ToString("dd/MM/yyyy"),
                           AgenceReceive = _context.Agences.FirstOrDefault(h => h.AgenceId == (_context.Users.FirstOrDefault(u => u.UserId == sentDocument.UserIdReceive)).AgenceId).AgenceName,
                       };

            ViewData["ListDocumentSent"] = list.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {



            return RedirectToPage("/SendDocument");
        }
    }
}
