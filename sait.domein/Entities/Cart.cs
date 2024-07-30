using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sait.domein.Entities
{
   public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Clothe clothe, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Clothe.Id == clothe.Id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Clothe = clothe,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Clothe clothe)
        {
            lineCollection.RemoveAll(l => l.Clothe.Id == clothe.Id);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Clothe.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Clothe Clothe { get; set; }
        public int Quantity { get; set; }
    }
}
