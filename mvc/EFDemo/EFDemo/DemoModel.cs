namespace EFDemo
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DemoModel : DbContext
    {
        public DemoModel()
            : base("name=DemoSiteEntities")
        {
        }

        public virtual DbSet<T_Customer> T_Customer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
