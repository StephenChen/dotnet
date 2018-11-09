namespace EFDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_Customer
    {
        public int ID { get; set; }

        [StringLength(32)]
        public string UserName { get; set; }

        public int? Age { get; set; }

        [StringLength(64)]
        public string Address { get; set; }
    }
}
