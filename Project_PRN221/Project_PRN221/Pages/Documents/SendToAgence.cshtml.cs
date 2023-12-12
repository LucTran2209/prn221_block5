using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Documents
{
    public class SendToAgenceModel : PageModel
    {
        private readonly Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext _context;
        private readonly IHubContext<SignalRHub> _signalRHub;


        public SendToAgenceModel(Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext context, IHubContext<SignalRHub> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //ViewData["AgenceId"] = new SelectList(_context.Documents, "AgenceId", "AgenceName");
            ViewData["AgenceId"] = new SelectList(await _context.Agences.ToListAsync(), "AgenceId", "AgenceName");

            return Page();
        }

        [BindProperty]
        public SendDocument SendDocument { get; set; } = default!;

        [BindProperty]
        public int? AgenceId { get; set; }

        [BindProperty]
        public string? DocumentNumber { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            var listUser = _context.Users.Where(x => x.AgenceId == AgenceId).ToList();
            var document = _context.Documents.FirstOrDefault(x => x.DocumentNumber.Equals(DocumentNumber));
            if (document != null)
            {
                foreach (var item in listUser)
                {
                    var send = new SendDocument();
                    send.SentDate = DateTime.Now;
                    send.UserIdSend = Int16.Parse(User.FindFirstValue("AccountId"));
                    send.UserIdReceive = item.UserId;
                    send.DocumentId = document.DocumentId;
                    send.Message = Message;
                    send.IsRead = false;

                    _context.SendDocuments.Add(send);
                    await _context.SaveChangesAsync();
                    await _signalRHub.Clients.All.SendAsync("LoadReceiveDocs", send.UserIdReceive, send.UserIdSend);
                }
            }

            return RedirectToPage("SendDocument");
        }
    }
}