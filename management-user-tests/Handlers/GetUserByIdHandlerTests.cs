using domain;
using feature.user;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace management_user_tests.Handlers;

public class GetUserByIdHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<GetUserByIdHandler>> _loggerMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<GetUserByIdHandler>>();
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new GetUserByIdHandler(
            _userRepositoryMock.Object,
            _loggerMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnUserFromCache_WhenCacheHit()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(userId, "John Doe", "12345678901", AccountStatus.Active);

        _cacheServiceMock
            .Setup(c => c.GetAsync<User>(userId.ToString()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userId.ToString(), result.Value.id);
        Assert.Equal("John Doe", result.Value.response.Name);

        _userRepositoryMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldFetchFromRepository_WhenCacheMiss()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(userId, "Jane Doe", "12345678901", AccountStatus.Active);

        _cacheServiceMock
            .Setup(c => c.GetAsync<User>(userId.ToString()))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(r => r.GetById(userId))
            .ReturnsAsync(user);

        _cacheServiceMock
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<TimeSpan?>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Jane Doe", result.Value.response.Name);
        _userRepositoryMock.Verify(r => r.GetById(userId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrows()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _cacheServiceMock
            .Setup(c => c.GetAsync<User>(userId.ToString()))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(r => r.GetById(userId))
            .ThrowsAsync(new KeyNotFoundException("User not found"));

        // Act
        var result = await _handler.Handle(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("500", result.Errors.First().Status);
    }
}
