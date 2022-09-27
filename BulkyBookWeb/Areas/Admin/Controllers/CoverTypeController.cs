using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> model = _unitOfWork.CoverType.GetAll();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Categoria criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            //return View(_context.Categories.Find(id));

            if (coverType == null) return NotFound();

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType model)
        {            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(model);
                _unitOfWork.Save();
                TempData["success"] = "Categoria Alterada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0) return NotFound();

            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null) return NotFound();
            
            return View(coverType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (coverType == null) return NotFound();

            _unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.Save();
            TempData["success"] = "CoverType Apagada com sucesso!";

            return RedirectToAction(nameof(Index));
        }
    }
}
