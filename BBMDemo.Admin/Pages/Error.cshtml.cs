using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BBMDemo.Admin.Pages
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> logger;

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            this.logger = logger;
        }
        
        public void OnGet(int? statusCode)
        {
            ViewData["ErrorMessage"] = $"Status Code is {statusCode}";

            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewData["ExceptionPath"] = exceptionDetails.Path;
            ViewData["ExceptionMessage"] = exceptionDetails.Error.Message;

            logger.LogWarning($"Status code is {statusCode}");
            logger.LogError($"Exception happens at {exceptionDetails.Path}. " +
                $"Exceptoin is {exceptionDetails.Error}");
            
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
