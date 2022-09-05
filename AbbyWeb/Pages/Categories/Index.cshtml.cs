using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IEnumerable<Category> Categories { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == 0) return NotFound();

        //    var categoria = _context.Categories.Find(id);

        //    if (categoria == null) return NotFound();

        //    return Page();
        //}

        public async Task<IActionResult> OnPostDelete()
        {
            int id = 0;
            var categoria = _context.Categories.Find(id);
            if (categoria == null) return NotFound();

            _context.Categories.Remove(categoria);
            await _context.SaveChangesAsync();

            return RedirectToPage(nameof(Index));
        }
    }
}
