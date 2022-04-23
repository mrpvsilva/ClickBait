using ClickBait.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Bogus;

namespace ClickBait.Infra.Data.Mappings
{
    internal class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnType("Varchar(36)")
                .IsRequired();

            builder
                .Property(x => x.Title)
                .HasColumnType("Varchar(100)")
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasColumnType("Varchar(250)")
                .IsRequired();

            builder
                .HasIndex(x => x.Title)
                .IsUnique();

            var posts = new Faker<Post>()
                    .RuleFor(x => x.Id, opt => Guid.NewGuid())
                    .RuleFor(x => x.Title, opt => opt.Lorem.Sentence(3))
                    .RuleFor(x => x.Description, opt => opt.Lorem.Sentence(20))
                    .Generate(10);

            builder.HasData(posts);
        }
    }
}
