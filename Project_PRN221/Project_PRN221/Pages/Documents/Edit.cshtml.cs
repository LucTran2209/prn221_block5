using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Documents
{
    public class EditModel : PageModel
    {
        private readonly Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext _context;

        public EditModel(Project_PRN221.Models.PROJECT_SENT_DOCUMENTContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Document Document { get; set; } = default!;

        [BindProperty]
        public IFormFile FileInput { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }
            Document = document;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["Base64Image"] = document.Content;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            

            try
            {
                if (Document != null)
                {
                    Document.CreateDate = DateTime.Now;
                    if (FileInput != null && FileInput.Length > 0)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await FileInput.CopyToAsync(memoryStream);
                            byte[] bytes = memoryStream.ToArray();
                            string base64String = Convert.ToBase64String(bytes);
                            Document.Content = base64String;
                        }
                    }
                }

                _context.Attach(Document).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToPage("/Index");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(Document.DocumentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.DocumentId == id)).GetValueOrDefault();
        }
    }
}
