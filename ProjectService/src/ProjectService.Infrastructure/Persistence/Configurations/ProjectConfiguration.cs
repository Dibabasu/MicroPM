using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Entity;

namespace ProjectService.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("projectid")
            .IsRequired()
            .ValueGeneratedNever();
        builder.Property(p => p.Id)
            .HasColumnName("projectid")
            .IsRequired();

        builder.OwnsOne(p => p.Details,
            navigationBuilder =>
                {
                    navigationBuilder.Property(d => d.Name)
                                    .HasColumnName("projectName")
                                    .HasMaxLength(50)
                                    .IsRequired();
                    navigationBuilder.Property(d => d.Description)
                                    .HasColumnName("projectDescription")
                                    .HasMaxLength(100);
                });

        builder.Property(p => p.WorkflowId)
            .HasColumnName("workflowid");
        builder.Property(p => p.ProjectStatus)
            .HasColumnName("projectstatus")
            .HasMaxLength(20);
        builder.Property(p => p.OwnerId)
            .HasColumnName("ownerid");
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
            .HasMaxLength(50);

        builder.HasMany(p => p.Components)
            .WithOne(c => c.Project)
            .HasForeignKey(c => c.ProjectId);
        builder.HasMany(p => p.ProjectUsers)
            .WithOne(pu => pu.Project)
            .HasForeignKey(pu => pu.ProjectId);
            
        //indexes
       // builder.HasIndex(p => p.Details.Name).HasDatabaseName("idx_projects_projectname");
        builder.HasIndex(p => p.WorkflowId).HasDatabaseName("idx_projects_workflowid");
        builder.HasIndex(p => p.ProjectStatus).HasDatabaseName("idx_projects_projectstatus");
    }
}
