using AIME.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.DirectoryServices.AccountManagement;

namespace AIME.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;

        public IndexModel(UserManager<User> userManager, ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public string Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                var roles = await _userManager.GetRolesAsync(user);
                Roles = string.Join(", ", roles);
            }
            else
            {
                Roles = "No user found";
            }
            HttpContext.Response.Redirect("/Documents/Index");
        }
   

        
    }
}
