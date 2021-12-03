using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderTime { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCanceled { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
