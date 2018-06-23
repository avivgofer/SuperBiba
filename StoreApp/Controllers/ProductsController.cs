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
        [Route("products/addToCart/{productID}/{user}")]
        public ActionResult addToCart(int productID, string user)

        {

            if (productID > 0 && !String.IsNullOrEmpty(user))
            {

                var products = from p in _context.Products
                               select p;

                bool flag = false;
                Product product = this.getProductFromDB(productID);
                StorageProducts storage = (from s in _context.StorageProducts
                                           where s.ProductName == product.ProductName
                                           select s).SingleOrDefault<StorageProducts>();
                storage.Amount--;

                User us = (from u in _context.Users
                           where u.UserName == user
                           select u).SingleOrDefault<User>();
                if (us != null)
                {
                    foreach (Product p in us.Cart)
                    {
                        if (p.ID == productID && p.Amount > 0)
                        {
                            p.Amount++;
                            flag = true;
                            _context.SaveChanges();
                        }
                    }
                    if (!flag)
                    {
                        us.Cart.Add(product);
                        _context.SaveChanges();
                    }
                }

                return View(us.Cart.ToList());

            }

            return null;
        }
        [Route("products/removeFromCart/{productID}/{userName}")]
        public ActionResult removeFromCart(int productID, string userName)
        {
            bool flag = false;
            Product product = this.getProductFromDB(productID);

            User user = (from u in _context.Users
                         where u.UserName == userName
                         select u).SingleOrDefault<User>();

            StorageProducts storage = (from s in _context.StorageProducts
                                       where s.ProductName == product.ProductName
                                       select s).SingleOrDefault<StorageProducts>();
            storage.Amount++;

            _context.SaveChanges();
            if (user != null)
            {
                foreach (Product p in user.Cart)
                {
                    if (p.ID == productID && p.Amount > 1)
                    {
                        p.Amount--;
                        flag = true;
                        _context.SaveChanges();
                    }
                }
                if (!flag)
                {
                    user.Cart.Remove(product);
                    _context.SaveChanges();
                }

                return View(user.Cart.ToList());
            }
            return BadRequest();
        }



        public double getTotalPrice(User user)
        {
            double total = 0;
            if (user != null)
            {
                foreach (Product p in user.Cart)
                {
                    total += (p.Amount * p.cost);
                }
                return total;
            }
            return 0;
        }
        [Route("products/sendOrder/{userName}")]
        public ActionResult sendOrder(string userName)
        {

            if (!String.IsNullOrEmpty(userName))
            {
                User user = (from u in _context.Users
                             where u.UserName == userName
                             select u).SingleOrDefault<User>();

                OrderDetails ord = new OrderDetails();

                ord.OrderID = orderId++;
                ord.userName = user.UserName;
                ord.orderTime = DateTime.Now;
                ord.total = this.getTotalPrice(user);
                _context.OrderDetails.Add(ord);
                var order = (from o in _context.OrderDetails
                             where o.OrderID == ord.OrderID
                             select o).SingleOrDefault<OrderDetails>();

                foreach (Product p in user.Cart.ToList())
                {
                    order.Cart.Add(p);
                    p.Amount = 1;
                    // user.Cart.Remove(p);

                }
                _context.SaveChanges();

                return View();

            }
            return BadRequest();

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
        public ActionResult getCart(string userName)
        {
            var user = (from u in _context.Users
                        where u.UserName == userName
                        select u).SingleOrDefault<User>();
            return View(user.Cart.ToList());
        }
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
        // POST:: api/SubmitOrcer
        [HttpGet]
        public IActionResult submitOrder(string jsonData) {
            JObject json = JObject.Parse(jsonData);

            foreach (var item in json)
            {
                Console.WriteLine("data ::: key ->{0} value->{1}", item.Key, item.Value);
                JObject js = JObject.Parse(item.Value.ToString());

                string productID = item.Key;
                string productName = "";
                string productAmount = "";
                foreach (var i in js) {
                    if (i.Key == "productName")
                    {
                        productName = i.Value.ToString();
                    }
                    if (i.Key == "productAmount")
                    {
                        productAmount = i.Value.ToString();
                    }
                }
                Console.WriteLine(productName + " " + productAmount + " saved.");
                Product p = new Product();
                p.ProductName = productName + "blabla";
                p.Amount = 999;
                p.SupplierID = 1;
                _context.Products.Add(p);
                _context.SaveChanges();
                Console.WriteLine("product saved !!");
            }
            Console.WriteLine("::::::::::::::::");
            Console.WriteLine("SEAN IM HERE !!!");

            
            return Redirect("/Index");
        }
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SupplierID,Title,Amount,Type,Supplier")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                StorageProducts storage = new StorageProducts(product.ID, product.ProductName, product.Supplier);
                _context.StorageProducts.Add(storage);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

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
