using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{

    public class AdminPanelController : Controller
    {

        private readonly StoreContext _context;

        public AdminPanelController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("AdminPanel/Products")]
        public async Task<IActionResult> Products()
        {
           return View(await _context.Products.ToListAsync());
        }
 
        //Get: AdminPanel/createProduct
        public IActionResult CreateProduct()
        {
            return View();
        }


        // POST: AdminPanel/CreateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("ID,ProductName,Amount,ProductType,cost,ImageURL,Description,SupplierID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // Get: AdminPanel/Users
        [Route("AdminPanel/Users")]
        public async Task<IActionResult> Users()
        {
            return View(await _context.Users.ToListAsync());
        }

        // Get: AdminPanel/CreateUser
        public IActionResult CreateUser()
        {
            return View();
        }

        // POST: AdminPanel/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("ID,FirstName,LastName,UserName,Password,Mail,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("ConfirmedCreateUser", new { ID = user.ID });

            }
            return View(user);
        }


        // Get: AdminPanel/ConfirmedCreateUser
        public IActionResult ConfirmedCreateUser(int ID)
        {
            //if (ID == null)
            //{
            //    return NotFound();
            //}

            //User user = await _context.Users
            //    .SingleOrDefaultAsync(m => m.ID == id);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            User user = (from u in _context.Users
                       where u.ID == ID
                       select u).SingleOrDefault<User>();
            
           
            return View(user);
        }







        [Route("AdminPanel/Suppliers")]
        public async Task<IActionResult> Suppliers()
        {
            return View(await _context.Suppliers.ToListAsync());
        }












        [Route("AdminPanel/check")]
        public IActionResult check()
        {
            return View();
        }
    }
}