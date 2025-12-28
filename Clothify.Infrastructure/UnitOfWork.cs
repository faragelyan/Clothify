using Clothify.Domain.Entities;
using Clothify.Domain.Interfaces;
using Clothify.Infrastructure.Peresistence;
using Clothify.Infrastructure.Repositories;

namespace Clothify.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Addresses = new GenericRepository<Address>(_context);
            AppUsers = new GenericRepository<AppUser>(_context);
            Brands = new GenericRepository<Brand>(_context);
            CartItems = new GenericRepository<CartItem>(_context);
            Categories = new GenericRepository<Category>(_context);
            Orders = new GenericRepository<Order>(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
            Payments = new GenericRepository<Payment>(_context);
            Products = new GenericRepository<Product>(_context);
            ProductSizes = new GenericRepository<ProductSize>(_context);
            Reports = new GenericRepository<Report>(_context);
            Reviews = new GenericRepository<Review>(_context);
            ShoppingCarts = new GenericRepository<ShoppingCart>(_context);
            Sizes = new GenericRepository<Size>(_context);
            UserPhones = new GenericRepository<UserPhone>(_context);
        }

        public IGenericRepository<Address> Addresses { get; private set; }
        public IGenericRepository<AppUser> AppUsers { get; private set; }
        public IGenericRepository<Brand> Brands { get; private set; }
        public IGenericRepository<CartItem> CartItems { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<Order> Orders { get; private set; }
        public IGenericRepository<OrderItem> OrderItems { get; private set; }
        public IGenericRepository<Payment> Payments { get; private set; }
        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<ProductSize> ProductSizes { get; private set; }
        public IGenericRepository<Report> Reports { get; private set; }
        public IGenericRepository<Review> Reviews { get; private set; }
        public IGenericRepository<ShoppingCart> ShoppingCarts { get; private set; }
        public IGenericRepository<Size> Sizes { get; private set; }
        public IGenericRepository<UserPhone> UserPhones { get; private set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
