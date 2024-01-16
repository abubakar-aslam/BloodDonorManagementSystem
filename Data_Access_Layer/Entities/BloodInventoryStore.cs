using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class BloodInventoryStore
    {
        public int Id { get; set; }
        public string BloodTypeName { get; set; }
        public int Quantity { get; set; }
    }
}
