using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Configurations
{
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>, IEntityTypeConfiguration<Department>
    {
        public new void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(d => d.Id).UseIdentityColumn(100,100);
            builder.Property(d => d.Name).HasColumnType("varchar(30)").IsRequired();
            builder.Property(d => d.Code).HasColumnType("varchar(30)").IsRequired();
            builder.Property(d => d.IsDeleted).HasDefaultValue(false);

            builder.HasMany(d => d.Employees)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            base.Configure(builder);
        }
    }
}
