using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using ProjectService.Application.Common.Errors;
using ProjectService.Application.Common.Interfaces;
using ProjectService.Application.Common.Models;
using ProjectService.Application.DTO;
using ProjectService.Application.Projects.Queries.GetProjects;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entity;
using ProjectService.Infrastructure.Persistence;
using ProjectService.Infrastructure.Persistence.Interceptors;
using ProjectService.Infrastructure.Services;

namespace ProjectService.Application.UnitTest.GetPtojects_Test;
public class GetProjectsQueryHandlerTests
{
    private readonly GetProjectsQueryHandler _handler;
    private readonly IProjectService _projectService;
    private readonly ProjectServiceDbContext _context;
     private readonly ICustomClaimService _claimService;
    

    public GetProjectsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProjectServiceDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase") // Use InMemory database
            .Options;

        var mockMediator = Substitute.For<IMediator>();
        var mockDateTime = Substitute.For<IDateTime>();
        mockDateTime.UtcNow.Returns(DateTime.UtcNow);
        _claimService = Substitute.For<ICustomClaimService>(); 
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockDateTime,_claimService);

        _context = new ProjectServiceDbContext(options, auditableEntitySaveChangesInterceptor, mockMediator);
        _context.Projects.Add(new Project(new Details("Test", "Test"), Guid.NewGuid(), Guid.NewGuid()));
        _context.SaveChanges();

        _projectService = new ProjectServices(_context); // Use real service with InMemory database
        _handler = new GetProjectsQueryHandler(_projectService);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPagedList()
    {
        var query = new GetProjectsQuery { PageNumber = 1, PageSize = 10 };

        var result = await _handler.Handle(query, new CancellationToken());

        Assert.IsType<OneOf<PagedList<ProjectDto>, ProjectServiceException>>(result);
        Assert.True(result.IsT0);
    }

    [Fact]
    public async Task Handle_NoProjects_ThrowsProjectServiceException()
    {
        var query = new GetProjectsQuery
        {
            PageNumber = 1,
            PageSize = 10,
            Status = Arg.Any<ProjectStatus?>(),
            OwnerId = new Guid(),
            FromDate = DateTime.UtcNow.AddDays(7),
            ToDate = Arg.Any<DateTime?>(),
            OrderBy = Arg.Any<string>()
        };

        await Assert.ThrowsAsync<EmptyOrNullException>(() => _handler.Handle(query, new CancellationToken()));
    }
}
