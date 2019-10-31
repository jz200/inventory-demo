using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BBMDemo.Admin.Models;
using BBMDemo.Admin.Services;
using BBMDemo.Data.Data;
using BBMDemo.Data.Data.Entities;
using BBMDemo.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMDemo.Admin.Pages.ProductInventories
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private IDbReadService _dbReadService;
        private IDbWriteService _dbWriteService;
        [BindProperty]
        public ProductInventory Input { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        public string ErrorMsg { get; set; }
        public HtmlString MsgInHtmlString { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(IDbWriteService dbWriteService, IDbReadService dbReadService)
        {
            _dbWriteService = dbWriteService;
            _dbReadService = dbReadService;
        }
        public void OnGet()
        {
            //Retrive data for drop downs and store them in view data
            StoreSelectListInViewData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Retrive data for drop downs and store them in view data
            StoreSelectListInViewData();

            //Retreive the names for item, style, and location based on Ids
            var itemNumber = _dbReadService.Get<Product>(Input.ProductId)?.ItemNumber;
            var style = _dbReadService.Get<Style>(Input.StyleId)?.Name;
            var location = _dbReadService.Get<Location>(Input.LocationId)?.Name;

            //check if composite primary key already exists in the database
            var pi = _dbReadService.Get<ProductInventory>(Input.ProductId, Input.StyleId, Input.LocationId);
                if (pi != null)
                {
                    ModelState.AddModelError("DuplicateKey", "Duplicate Keys");

                    ErrorMsg = "There is already an inventory entry for " +
                        $"item # <strong>{itemNumber}</strong> with style of <strong>{style}</strong> at <strong>{location}</strong>."  + 
                        " Please select a different item number, style, and location combination or click " +
                        $@"<a href='/ProductInventories/Edit?ProductId={Input.ProductId}&StyleId={Input.StyleId}&LocationId={Input.StyleId}'>here</a>" +
                        " to edit the existing entry.";

                    MsgInHtmlString = new HtmlString(ErrorMsg);
                    return Page();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var result = await _dbWriteService.Add(Input);

                        if (result)
                        {
                            //Message sent back to Index razor page
                            StatusMessage = $"Created a new inventory entry for Item {itemNumber}, Style: {style}, Location: {location}.";
                            return RedirectToPage("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }                       
            //something failed, redisplay the form
            return Page();
        }

        private void StoreSelectListInViewData()
        {
            ViewData["Categories"] = _dbReadService.GetSelectList<Category>("CategoryId", "Name");
            ViewData["Products"] = _dbReadService.GetSelectList<Product>("ProductId", "ItemNumber");
            ViewData["Styles"] = _dbReadService.GetSelectList<Style>("StyleId", "Name");
            ViewData["Locations"] = _dbReadService.GetSelectList<Location>("LocationId", "Name");
        }
    }
}