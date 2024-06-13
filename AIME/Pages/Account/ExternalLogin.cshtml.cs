using AIME.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AIME.Pages.Account
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExternalLoginModel(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult OnGet(string provider)
        {
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./SignIn");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return RedirectToPage("/Documents/Index");
            }
            if (signInResult.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
               
                if (email != null)
                {
                    var user = new User { UserName = email, Email = email, FirstName = firstName ?? "", LastName = lastName ?? "", EmailConfirmed=true };

                
                   var result = await _userManager.CreateAsync(user);
                 
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "user");
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToPage("/Documents/Index");
                        }
                    }
                }
                return RedirectToPage("./SignIn");
            }
        }
    }
}
