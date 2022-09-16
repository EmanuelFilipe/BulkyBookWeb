using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> model = _context.Categories.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                TempData["success"] = "Categoria criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            return View(_context.Categories.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (model.Name == model.DisplayOrder.ToString())
                ModelState.AddModelError("name", "O DisplayName não pode ser igual ao Name");

            if (ModelState.IsValid)
            {
                _context.Categories.Update(model);
                _context.SaveChanges();
                TempData["success"] = "Categoria Alterada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0) return NotFound();

            var categoria = _context.Categories.Find(id);

            if (categoria == null) return NotFound();
            
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoria = _context.Categories.Find(id);
            if (categoria == null) return NotFound();

            _context.Categories.Remove(categoria);
            _context.SaveChanges();
            TempData["success"] = "Categoria Apagada com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
