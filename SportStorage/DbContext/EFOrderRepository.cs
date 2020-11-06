using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportStorage.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _ctx;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Order> Orders => _ctx.Orders
            .Include(m => m.Lines)
            .ThenInclude(l => l.Product);

        public void Save(Order order)
        {
            if (order.OrderId == 0)
            {
                _ctx.Orders.Add(order);
            }
            _ctx.AttachRange(order.Lines.Select(l=>l.Product));

            _ctx.SaveChanges();
        }
    }
}