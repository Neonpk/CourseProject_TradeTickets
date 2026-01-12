using CourseProject_SellingTickets.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseProject_SellingTickets.DbContexts.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDTO>
{
    public void Configure(EntityTypeBuilder<UserDTO> builder)
    {
        builder.Property(x => x.DiscountId).
                HasDefaultValueSql().
                ValueGeneratedOnAdd();
    }
}
