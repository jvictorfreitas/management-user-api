using domain;
using feature.user;
using Infrastructure.database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace management_user_tests.Handlers;

public class CreateUserHandlerTests
{
    private static AppDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsCreated()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var loggerMock = new Mock<ILogger<CreateUserHandler>>();
        var cacheServiceMock = new Mock<ICacheService>();
        var dbContext = CreateInMemoryDbContext();
        var outboxService = new OutboxService(dbContext);

        var request = new CreateUserRequest("John Doe", "12345678901");
        var createdUser = new User(Guid.NewGuid(), request.Name, request.Cpf, AccountStatus.Active);

        userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(createdUser);

        cacheServiceMock
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<TimeSpan?>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateUserHandler(
            userRepositoryMock.Object,
            loggerMock.Object,
            cacheServiceMock.Object,
            outboxService,
            dbContext
        );

        // Act
        var result = await handler.Handle(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(createdUser.Name, result.Value.response.Name);
        Assert.Equal(createdUser.Cpf, result.Value.response.Cpf);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrows()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var loggerMock = new Mock<ILogger<CreateUserHandler>>();
        var cacheServiceMock = new Mock<ICacheService>();
        var dbContext = CreateInMemoryDbContext();
        var outboxService = new OutboxService(dbContext);

        userRepositoryMock
            .Setup(r => r.Add(It.IsAny<User>()))
            .ThrowsAsync(new Exception("DB error"));

        var handler = new CreateUserHandler(
            userRepositoryMock.Object,
            loggerMock.Object,
            cacheServiceMock.Object,
            outboxService,
            dbContext
        );

        var request = new CreateUserRequest("John Doe", "12345678901");

        // Act
        var result = await handler.Handle(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("500", result.Errors.First().Status);
    }
}
