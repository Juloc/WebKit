using WebKit.Core.Pagination;
using WebKit.Core.Results;
using WebKit.Core.Validation;

namespace WebKit.ExampleApp.Features.Catalog;

public sealed record Product(Guid Id, string Name, string Category, string Status);

public sealed record ProductDraft(Guid? Id, string Name, string Category, string Status)
{
    public ValidationResult Validate()
    {
        ValidationResult result = new();
        result.AddIf(string.IsNullOrWhiteSpace(Name), nameof(Name), "Enter a product name.");
        result.AddIf(Name.Length > 80, nameof(Name), "Use 80 characters or fewer.");
        result.AddIf(string.IsNullOrWhiteSpace(Category), nameof(Category), "Enter a category.");
        result.AddIf(Status is not ("Active" or "Draft"), nameof(Status), "Choose Active or Draft.");
        return result;
    }
}

public interface IProductCatalog
{
    PageResult<Product> Search(string? search, WebKit.Core.Pagination.PageRequest request);

    Result<Product> Find(Guid id);

    Result<Product> Save(ProductDraft draft);
}
