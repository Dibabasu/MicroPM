using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entity;

namespace ProjectService.Infrastructure.Persistence.Configurations;

public class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.ToTable("components");
        builder.HasKey(c => new { c.Id, c.ProjectId });
        builder.Property(c => c.Id)
            .HasColumnName("componentid")
            .ValueGeneratedNever();
        builder.Property(c => c.ProjectId)
            .HasColumnName("projectid");

        builder.OwnsOne(p => p.Details,
            navigationBuilder =>
                {
                    navigationBuilder.Property(d => d.Name)
                                    .HasColumnName("componentName")
                                    .HasMaxLength(50)
                                    .IsRequired();
                    navigationBuilder.Property(d => d.Description)
                                    .HasColumnName("componentDescription")
                                    .HasMaxLength(100);
                });

        builder.Property(p => p.Created)
            .HasColumnName("created")
            .HasColumnType("datetime2");
        builder.Property(p => p.LastModified)
            .HasColumnName("modified")
            .HasColumnType("datetime2");
        builder.Property(p => p.CreatedBy)
            .HasColumnName("createdby")
            .HasMaxLength(50);
        builder.Property(p => p.LastModifiedBy)
            .HasColumnName("modifiedby")
            .HasMaxLength(50);;

        builder.HasOne(c => c.Project)
            .WithMany(p => p.Components)
            .HasForeignKey(c => c.ProjectId);

        builder.HasIndex(c => c.ProjectId).HasDatabaseName("idx_components_projectid");
        // builder.HasIndex(c => c.Details.Name).HasDatabaseName("idx_components_componentname");
    }
}