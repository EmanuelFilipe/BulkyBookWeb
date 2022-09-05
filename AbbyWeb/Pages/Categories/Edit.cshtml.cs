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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context )
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public void OnGet(int id)
        {
            //if (id == 0) return NotFound();

            Category = _context.Categories.Find(id);
            //return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Category.Name == Category.DisplayOrder.ToString())
                ModelState.AddModelError("Category.Name", "O DisplayName não pode ser igual ao Name");

            if (ModelState.IsValid)
            {
                _context.Categories.Update(Category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Categoria alterada com sucesso!";

                return RedirectToPage(nameof(Index));
            }

            return Page();
        }
    }
}
