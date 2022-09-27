using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> model = _unitOfWork.Category.GetAll();
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
                _unitOfWork.Category.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Categoria criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            //return View(_context.Categories.Find(id));

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (model.Name == model.DisplayOrder.ToString())
                ModelState.AddModelError("name", "O DisplayName não pode ser igual ao Name");

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(model);
                _unitOfWork.Save();
                TempData["success"] = "Categoria Alterada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0) return NotFound();

            var categoria = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);

            if (categoria == null) return NotFound();
            
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoria = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (categoria == null) return NotFound();

            _unitOfWork.Category.Remove(categoria);
            _unitOfWork.Save();
            TempData["success"] = "Categoria Apagada com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
