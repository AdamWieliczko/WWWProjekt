using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCInterProject.Models
{
    public partial class Shipping
    {
        public int ShiId { get; set; }
        public int ShiPackageId { get; set; }
        public string ShiName { get; set; } = null!;
        public string ShiAddress { get; set; } = null!;
        public DateTime ShiCreateDate { get; set; }
        public int ShiMass { get; set; }

        public virtual Package? ShiPackage { get; set; }

        [NotMapped]
        public bool ToBeSaved { get; set; } = true;
        [NotMapped]
        public bool IsInDb { get; set; } = true;
    }
}