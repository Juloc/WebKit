using Microsoft.AspNetCore.Mvc.RazorPages;

namespace __Page__;

public sealed class __Page__Model : PageModel
{
    public Guid Id { get; private set; }

    public void OnGet(Guid id)
    {
        Id = id;
    }
}
