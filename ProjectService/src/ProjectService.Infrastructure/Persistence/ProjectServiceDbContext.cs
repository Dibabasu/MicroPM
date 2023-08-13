using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Entity;
using ProjectService.Infrastructure.Common;
using ProjectService.Infrastructure.Persistence.Configurations;
using ProjectService.Infrastructure.Persistence.Interceptors;

namespace ProjectService.Infrastructure.Persistence;

public class ProjectServiceDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly IMediator _mediator;
    public ProjectServiceDbContext(DbContextOptions<ProjectServiceDbContext> options,
    AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
    IMediator mediator)
    : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        _mediator = mediator;
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ComponentConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectUserConfiguration());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(cancellationToken);
    }

}