using System;
using PharmaSphere.Models;
using PharmaSphere.Data.Repositories.Implementations;

namespace PharmaSphere.Data.Repositories.Specifications
{
    public class ProductWithCategoryAndBrandSpecification : BaseSpecification<Product>
    {
        public ProductWithCategoryAndBrandSpecification()
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
            ApplyOrderByDescending(p => p.Id);
        }

        public ProductWithCategoryAndBrandSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
        }
    }

    public class ProductsByCategorySpecification : BaseSpecification<Product>
    {
        public ProductsByCategorySpecification(int categoryId) : base(p => p.CategoryId == categoryId)
        {
            AddInclude(p => p.Category);
            AddInclude(p => p.Brand);
        }
    }

    public class LowStockProductsSpecification : BaseSpecification<Product>
    {
        public LowStockProductsSpecification(int threshold) : base(p => p.Stock <= threshold)
        {
            AddInclude(p => p.Category);
            ApplyOrderBy(p => p.Stock);
        }
    }

    public class OrderWithItemsAndCustomerSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsAndCustomerSpecification()
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.Items);
            ApplyOrderByDescending(o => o.OrderDate);
        }

        public OrderWithItemsAndCustomerSpecification(int id) : base(o => o.Id == id)
        {
            AddInclude(o => o.Customer);
            AddInclude(o => o.Items);
        }
    }
}
