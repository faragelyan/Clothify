using Clothify.Domain.Entities;
namespace Clothify.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<AppUser> AppUsers { get; }
        IGenericRepository<Brand> Brands { get; }
        IGenericRepository<CartItem> CartItems { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductSize> ProductSizes { get; }
        IGenericRepository<Report> Reports { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<ShoppingCart> ShoppingCarts { get; }
        IGenericRepository<Size> Sizes { get; }
        IGenericRepository<UserPhone> UserPhones { get; }
        IGenericRepository<PendingVerification> PendingVerifications { get; }

        Task<int> CommitAsync();
    }
}
