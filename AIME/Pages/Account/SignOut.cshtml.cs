using AIME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Dynamic;

namespace AIME.Pages.Account
{
    public class SignOutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<SignOutModel> _logger;

        public SignOutModel(SignInManager<User> signInManager, ILogger<SignOutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task OnGet()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            _logger.LogInformation("Redirecting to sign-in page...");
            HttpContext.Response.Redirect("/Account/SignIn");
            _logger.LogInformation("Redirected successfully.");
        }


    }
}
