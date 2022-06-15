using System;
using System.Collections.Generic;

namespace MVCInterProject.Models
{
    public partial class Person
    {
        public int PerId { get; set; }
        public string PerName { get; set; } = null!;
        public string PerPassword { get; set; } = null!;
        public bool PerIsAdmin { get; set; }
    }
}