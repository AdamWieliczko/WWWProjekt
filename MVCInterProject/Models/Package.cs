using System;
using System.Collections.Generic;

namespace MVCInterProject.Models
{
    public partial class Package
    {
        public Package()
        {
            Shippings = new();
        }

        public int PacId { get; set; }
        public string PacName { get; set; } = null!;
        public DateTime PacCreateDate { get; set; }
        public bool PacIsOpen { get; set; }
        public DateTime? PacCloseDate { get; set; }
        public string PacCity { get; set; } = null!;

        public List<Shipping> Shippings { get; set; }
    }
}
