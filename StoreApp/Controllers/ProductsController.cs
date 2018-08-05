using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class ProductsController : Controller
    {
        static int orderId = 1;
        private readonly StoreContext _context;

        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View();
        }
        [Route("products/Meat")]
        public ActionResult Meat()
        {
            var meat = (from m in _context.Products
                        where m.ProductType == "meat"
                        select m);
            return View(meat.ToList());
        }
        [Route("products/Milky")]
        public ActionResult Milky()
        {
            var milky = (from m in _context.Products
                         where m.ProductType == "milky"
                         select m);
            return View(milky.ToList());
        }
        [Route("products/General")]
        public ActionResult General()
        {
            var general = (from g in _context.Products
                           where g.ProductType == "general"
                           select g);
            return View(general.ToList());
        }
        [Route("products/Drinks")]
        public ActionResult Drinks()
        {
            var drinks = (from d in _context.Products
                          where d.ProductType == "drinks"
                          select d);
            return View(drinks.ToList());
        }
        [Route("products/vegatable")]
        public ActionResult vegatable()
        {
            var vegatable = (from v in _context.Products
                             where v.ProductType == "vegatable"
                             select v);
            return View(vegatable.ToList());
        }
        [Route("products/snacks")]
        public ActionResult snacks()
        {
            var snacks = (from s in _context.Products
                          where s.ProductType == "snacks"
                          select s);
            return View(snacks.ToList());
        }

        public Product getProductFromDB(int productID)
        {
            return (from p in _context.Products
                    where p.ID == productID
                    select p).SingleOrDefault<Product>();
        }
        [Route("products/getOrder/{orderID}")]
        public ActionResult getOrder(int orderID)
        {
            var order = (from o in _context.OrderDetails
                         where o.OrderID == orderID
                         select o).SingleOrDefault<OrderDetails>();

            return View(order.Cart.ToList());
        }
        [Route("products/getCart/{userName}")]

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(string jsonData)
        {
            return View();
        }
        // GET:: api/SubmitOrcer
        [HttpGet]
        public IActionResult submitOrder(string jsonData) {
            string username;
            // convert stringJSON to objectJSON:
            JObject json = JObject.Parse(jsonData);

            foreach (var item in json) {
                // if current item equals to username then dont try to drown deeper
                var isUserField = false;

                if (item.Key == "username") {
                    username = item.Value.ToString();
                    Console.WriteLine("Shopping cart belongs to: {0}, starting orderDetail ..", username);
                    isUserField = true;
                }
            
                if (isUserField == false) {
                    // parsing product details ..           
                    JObject product = JObject.Parse(item.Value.ToString());
                    string productID = item.Key;
                    string productName = "";
                    string productAmount = "";

                    foreach (var p in product) {

                        if (p.Key == "productName") {
                            productName = p.Value.ToString();
                        }

                        if (p.Key == "productAmount") {
                            productAmount = p.Value.ToString();
                        }
                    }
                }
            }
            return Redirect("/Index");
        }
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SupplierID,Title,Amount,Type,Supplier")] Product product)
        {
            ViewBag.SupplierId = new SelectList(_context.Suppliers, "ID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SupplierID,Title,Amount,Type,Supplier")] Product product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
        
    }
}
