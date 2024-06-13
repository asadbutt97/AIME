using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AIME.Pages
{
    public class logoutModel : PageModel
    {
        public void OnGet()
        {
            RedirectToPage("./Index");
        }
    }
}
