using ClickBait.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickBait.Infra.Data.Mappings
{
    internal class ClicksCountMapping : IEntityTypeConfiguration<ClickCount>
    {
        public void Configure(EntityTypeBuilder<ClickCount> builder)
        {
            builder.HasKey(x => x.PostId);

            builder
               .Property(x => x.PostId)
               .HasColumnType("Varchar(36)")
               .IsRequired();

            builder
               .Property(x => x.Qtd)
               .HasColumnType("BIGINT")
               .IsRequired();

            builder.HasOne<Post>()
                .WithMany()
                .HasForeignKey(x => x.PostId);
        }
    }
}
