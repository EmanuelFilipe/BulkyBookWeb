using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            //IEnumerable<CoverType> model = _unitOfWork.CoverType.GetAll();
            //return View(model);
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Product product = new Product();

            if (id == null || id == 0)
                return View(product);

            product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            GetLists();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product model, IFormFile file)
        {            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (model.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));
                        
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    model.ImageUrl = @"images\products\" + fileName + extension;
                }


                if (model.Id == 0)
                    _unitOfWork.Product.Add(model);
                else
                    _unitOfWork.Product.Update(model);

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //public IActionResult Delete(int? id)
        //{
        //    if (id == 0) return NotFound();

        //    var coverType = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

        //    if (coverType == null) return NotFound();
            
        //    return View(coverType);
        //}

        

        private void GetLists()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });

            IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll()
               .Select(c => new SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               });

            ViewBag.CategoryList = CategoryList;
            ViewData["CoverTypeList"] = CoverTypeList;
        }

        #region API CALS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            
            if (product == null)
                return Json(new { success = false, message = "Error while deleting" });

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
                System.IO.File.Delete(oldImagePath);

            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful!" });
        }

        #endregion
    }
}
