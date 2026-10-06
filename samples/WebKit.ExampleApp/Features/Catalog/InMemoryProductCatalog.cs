using WebKit.Core.Errors;
using WebKit.Core.Pagination;
using WebKit.Core.Results;
using WebKit.Core.Validation;

namespace WebKit.ExampleApp.Features.Catalog;

public sealed class InMemoryProductCatalog : IProductCatalog
{
    private readonly object gate = new();
    private readonly List<Product> products = Enumerable.Range(1, 14).Select(index => new Product(Guid.NewGuid(), $"Starter item {index}", index % 2 == 0 ? "Operations" : "Content", index % 3 == 0 ? "Draft" : "Active")).ToList();

    public PageResult<Product> Search(string? search, PageRequest request)
    {
        lock (gate)
        {
            IEnumerable<Product> matching = products;
            if (!string.IsNullOrWhiteSpace(search))
            {
                matching = matching.Where(product => product.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || product.Category.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            int totalCount = matching.Count();
            IReadOnlyList<Product> items = matching.Skip(request.Skip).Take(request.PageSize).ToArray();
            return new PageResult<Product>(items, request.Page, request.PageSize, totalCount);
        }
    }

    public Result<Product> Find(Guid id)
    {
        lock (gate)
        {
            Product? product = products.FirstOrDefault(item => item.Id == id);
            return product is null ? Result<Product>.Failure(WebKitError.NotFound()) : Result<Product>.Success(product);
        }
    }

    public Result<Product> Save(ProductDraft draft)
    {
        ValidationResult validation = draft.Validate();
        if (!validation.IsValid)
        {
            return Result<Product>.Failure(WebKitError.Validation(validation.Errors[0].Message));
        }

        lock (gate)
        {
            Product product = new(draft.Id ?? Guid.NewGuid(), draft.Name.Trim(), draft.Category.Trim(), draft.Status);
            int index = draft.Id is Guid id ? products.FindIndex(item => item.Id == id) : -1;
            if (index >= 0)
            {
                products[index] = product;
            }
            else
            {
                products.Insert(0, product);
            }

            return Result<Product>.Success(product);
        }
    }
}
