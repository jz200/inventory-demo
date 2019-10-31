using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BBMDemo.Data.Data;
using BBMDemo.Data.Data.Entities;
using BBMDemo.UI.Models.ProductInventoriesViewModels;
using Microsoft.AspNetCore.Html;

namespace BBMDemo.UI.Controllers
{
    public class ProductInventoriesController : Controller
    {
        private readonly BBMContext _context;
      
        public ProductInventoriesController(BBMContext context)
        {
            _context = context;
        }

        // GET: ProductInventories
        public IActionResult Index(string itemNumber, int catId=0, int locationId=0, int styleId = 0 )
        {
            var inventories = _context.ProductInventory.Include(p => p.Location).Include(p => p.Product).Include(p => p.Style).AsQueryable();
            var products = _context.Product.Include(p => p.Category).AsQueryable();

            //Prepare data for drop downs
            StoreSelectListsInViewData();

            if (styleId > 0)
            {
                inventories = inventories.Where(pv => pv.StyleId == styleId);
            }
            if (locationId > 0)
            {
                inventories = inventories.Where(pv => pv.LocationId == locationId);
            }
            if (catId > 0)
            {
                 var productsInCat = products.Where(p => p.CategoryId == catId)
                                            .Select(p => p.ProductId);
                if (productsInCat.Count() > 0)
                {
                    inventories = inventories.Where(pv => productsInCat.Contains(pv.ProductId));
                }
                else
                {
                    //never true, we do this because we cannot set this collection to null directly
                    inventories = inventories.Where(pv => pv.ProductId < 0);
                }
            }
            if (!string.IsNullOrEmpty(itemNumber))
            {
                var product = products.Where(p => p.ItemNumber.Equals(itemNumber))
                                .FirstOrDefault();
                if (product != null)
                {
                    inventories = inventories.Where(pv => pv.ProductId == product.ProductId);
                }
                else
                {
                    //never true in database
                    inventories = inventories.Where(pv => pv.ProductId < 0);
                }
            }
            //sort items by category, itemNumber, style and location
            inventories = inventories.OrderBy(pi => pi.Product.CategoryId).ThenBy(pi => pi.ProductId)
                        .ThenBy(pi => pi.StyleId).ThenBy(pi => pi.LocationId);

            var model = new InventoryListViewModel()
            {
                Inventories = inventories,
                Products = products
            };

            return View(model);
        }

        // GET: ProductInventories/Create
        public IActionResult Create()
        {
            //prepare data for dropdowns
            StoreSelectListsInViewData();
            return View();
        }

        
        // POST: ProductInventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductInventory Input)
        {
            StoreSelectListsInViewData();

            //retrive name details for status message or error message
            var piDetail = GetNameDetails(Input);

            //check if composite primary key already exists in the database
            var pi = _context.ProductInventory.Find(Input.ProductId, Input.StyleId, Input.LocationId);
            if (pi != null)
            {
                ModelState.AddModelError("DuplicateKey", "Duplicate Keys");

                var ErrorMsg = "There is already an inventory entry for " +
                    $"item # <strong>{piDetail.itemNumber}</strong> with style of <strong>{piDetail.style}</strong> at <strong>{piDetail.location}</strong>." +
                    " Please select a different item number, style, and location combination or click " +
                    $@"<a href='/ProductInventories/Edit?ProductId={Input.ProductId}&StyleId={Input.StyleId}&LocationId={Input.StyleId}'>here</a>" +
                    " to edit the existing entry.";
                ViewData["ErrorMsg"] = ErrorMsg;
                ViewData["htmlStringError"] = new HtmlString(ErrorMsg);
                return View(Input);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.ProductInventory.AddAsync(Input);
                    int result = await _context.SaveChangesAsync();
                    if (result >= 0)
                    {
                        //Message sent back to Index razor page
                        TempData["StatusMessage"] = $"Created a new inventory entry for Item {piDetail.itemNumber}, Style: {piDetail.style}, Location: {piDetail.location}.";
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //something failed, redisplay the form
            return View(Input);
        }

        // GET: ProductInventories/Edit/5
        public async Task<IActionResult> Edit(int productId, int styleId, int locationId)
        {
            var productInventory = await _context.ProductInventory.FindAsync(productId, styleId, locationId);
            if (productInventory == null)
            {
                return View("NotFound");
            }
            else
            {
                AddNavigationFields(productInventory);
            }
            ViewData["Category"] = _context.Product.Where(p => p.ProductId == productId)
                                  .Include(p => p.Category).SingleOrDefault().Category?.Name;
            return View(productInventory);
        }

        // POST: ProductInventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductInventory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                   
                    int result = await _context.SaveChangesAsync();
                    if (result >= 0)
                    {
                    //retrive item number and style, location name for status message
                         var piDetail = GetNameDetails(model);
                        //Message sent back to Index razor page
                        TempData["StatusMessage"] = $"Updated inventory entry for item # {piDetail.itemNumber} " +
                                $"of style {piDetail.style} at {piDetail.location}.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //something failed, redisplay the form
            return View(model);
        }

        // GET: ProductInventories/Delete/5
        public async Task<IActionResult> Delete(int productId, int styleId, int locationId)
        {
            var productInventory = await _context.ProductInventory.FindAsync(productId, styleId, locationId);
            if (productInventory == null)
            {
                return View("NotFound");
            }
            else
            {
                AddNavigationFields(productInventory);
                ViewData["Category"] = GetCategoryNameByProductId(productId);
                return View(productInventory);
            }
        }      

        // POST: ProductInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ProductInventory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Remove(model);

                    int result = await _context.SaveChangesAsync();
                    if (result >= 0)
                    {
                        //retrive item number and style, location name for status message
                        var piDetail = GetNameDetails(model);
                        //Message sent back to Index razor page
                        TempData["StatusMessage"] = $"Deleted inventory entry for item # {piDetail.itemNumber} " +
                                $"of style {piDetail.style} at {piDetail.location}.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            //something failed, redisplay the form
            return View(model);
        }

        #region HelperMethods
        //retreive data needed for dropdown lists and store them in viewdata for use in view
        private void StoreSelectListsInViewData()
        {
            ViewData["Locations"] = new SelectList(_context.Location, "LocationId", "Name");
            ViewData["Categories"] = new SelectList(_context.Category, "CategoryId", "Name");
            ViewData["Styles"] = new SelectList(_context.Style, "StyleId", "Name");
            ViewData["Products"] = new SelectList(_context.Product, "ProductId", "ItemNumber");
        }     

        //Load navigation field for productinventory entry
        private void AddNavigationFields(ProductInventory productInventory)
        {
            _context.Entry(productInventory).Reference("Product").Load();
            _context.Entry(productInventory).Reference("Style").Load();
            _context.Entry(productInventory).Reference("Location").Load();
        }

        //Get Category name for display based on product Id
        private string GetCategoryNameByProductId(int Id)
        {
            return _context.Product.Include(p => p.Category)
                .SingleOrDefault(p => p.ProductId == Id).Category?.Name;
        }

        //retrieve ItemNumber, Style Name, and Location Name for a ProductInventory object
        private (string itemNumber, string style, string location) GetNameDetails(ProductInventory model)
        {
            var itemNumber = _context.Product.Find(model.ProductId)?.ItemNumber;
            var style = _context.Style.Find(model.StyleId)?.Name;
            var location = _context.Location.Find(model.LocationId)?.Name;
            return (itemNumber, style, location);
        }

        //This products method is used to provide list of products to UI through ajax request
        [HttpGet, HttpPost]
        public JsonResult Products(int catId)
        {
            var products = _context.Product.AsQueryable();
            if (catId > 0)
            {
                products = products.Where(p => p.CategoryId == catId);
            }
            return Json(products.Select(p => new { p.ProductId, p.ItemNumber }));
        }

        #endregion
    }
}
