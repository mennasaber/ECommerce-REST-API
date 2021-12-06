using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int AvailableStock { get; set; }
        public ICollection<OrderProduct> OrderProduct { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}
