using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Book Book { get; set; }
        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id == null)
            {
                return View(Book);
            }
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }
        #region
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFormDB = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFormDB == null)
            {
                return Json(new { success = false, message = "error while deleting" });
            }
            _db.Books.Remove(bookFormDB);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delet Successfull" });

        }
        #endregion
    }
}
