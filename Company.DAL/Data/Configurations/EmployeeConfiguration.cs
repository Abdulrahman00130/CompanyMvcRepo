using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.DAL.Data.Configurations
{
    public class EmployeeConfiguration : BaseEntityConfiguration<Employee>, IEntityTypeConfiguration<Employee>
    {
        public new void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name).HasColumnType("varchar(50)").IsRequired();
            builder.Property(e => e.Address).HasColumnType("varchar(150)");
            builder.Property(e => e.Salary).HasColumnType("decimal(10,2)");
            builder.Property(e => e.Gender)
                .HasConversion((gender) => gender.ToString(),
                               (gender) => (Gender) Enum.Parse(typeof(Gender), gender));
            builder.Property(e => e.EmployeeType)
                .HasConversion((empType) => empType.ToString(),
                               (empType) => (EmployeeType) Enum.Parse(typeof(EmployeeType), empType));
            base.Configure(builder);
        }
    }
}
