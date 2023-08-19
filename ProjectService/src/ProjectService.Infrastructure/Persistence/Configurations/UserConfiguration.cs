using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entity;

namespace ProjectService.Infrastructure.Persistence.Configurations;

public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.ToTable("projectusers");
        builder.HasKey(pu => new { pu.Id, pu.ProjectId });
        builder.Property(pu => pu.Id)
            .HasColumnName("userid");
        builder.Property(pu => pu.ProjectId)
            .HasColumnName("projectid");
        builder.Property(pu => pu.UserRole)
            .HasColumnName("userrole");
        builder.Property(p => p.Created)
            .HasColumnName("created")
            .HasColumnType("datetime2");
        builder.Property(p => p.LastModified)
            .HasColumnName("modified")
            .HasColumnType("datetime2");
        builder.Property(p => p.CreatedBy)
            .HasColumnName("createdby")
            .HasMaxLength(20);
        builder.Property(p => p.LastModifiedBy)
            .HasColumnName("modifiedby")
            .HasMaxLength(20);
        builder.HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId);
        builder.HasIndex(pu => pu.ProjectId).HasDatabaseName("idx_projectusers_projectid");
        builder.HasIndex(pu => pu.Id).HasDatabaseName("idx_projectusers_userid");
    }
}