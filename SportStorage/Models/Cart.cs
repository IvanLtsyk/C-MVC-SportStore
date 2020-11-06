using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SportStorage.Models
{
    public class Cart
    {
        private List<CartLine> _lineColection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = _lineColection
                .FirstOrDefault(m => m.Product.ProductId == product.ProductId);

            if (line == null)
            {
                _lineColection.Add(
                    new CartLine
                    {
                        Product = product,
                        Quantity = quantity
                    });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
        {
            _lineColection.RemoveAll(m => m.Product.ProductId == product.ProductId);
        }

        public virtual void Clear()
        {
            _lineColection.Clear();
        }

        public virtual decimal ComputeTotalValue()
        {
            return _lineColection.Sum(m => m.Product.Price * m.Quantity);
        } 

        public virtual IEnumerable<CartLine> Lines => _lineColection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}