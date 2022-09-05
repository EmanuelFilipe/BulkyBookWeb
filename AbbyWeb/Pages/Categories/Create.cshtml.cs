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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context )
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
                ModelState.AddModelError("Category.Name", "O DisplayName não pode ser igual ao Name");

            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["success"] = "Categoria criado com sucesso!";

                return RedirectToPage(nameof(Index));
            }

            return Page();
        }
    }
}
