using domain;
using feature.user;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace management_user_tests.Handlers;

public class GetAllUsersByFiltersHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<GetAllUsersByFiltersHandler>> _loggerMock;
    private readonly GetAllUsersByFiltersHandler _handler;

    public GetAllUsersByFiltersHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<GetAllUsersByFiltersHandler>>();

        _handler = new GetAllUsersByFiltersHandler(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUsers_WhenRepositoryReturnsResults()
    {
        var cancelationToken = It.IsAny<CancellationToken>();

        // Arrange
        var request = new GetAllUsersByFiltersRequest("John", null, 1, 10);
        var users = new List<User>
        {
            new User(Guid.NewGuid(), "John Doe", "12345678901", AccountStatus.Active),
            new User(Guid.NewGuid(), "John Smith", "98765432100", AccountStatus.Active),
        };

        _userRepositoryMock
            .Setup(r => r.GetAllByFilter("John", null, 1, 10, cancelationToken))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Count());
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersFound()
    {
        var cancelationToken = It.IsAny<CancellationToken>();

        // Arrange
        var request = new GetAllUsersByFiltersRequest("Unknown", null, 1, 10);

        _userRepositoryMock
            .Setup(r => r.GetAllByFilter("Unknown", null, 1, 10, cancelationToken))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _handler.Handle(request, cancelationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrows()
    {
        var cancelationToken = It.IsAny<CancellationToken>();

        // Arrange
        var request = new GetAllUsersByFiltersRequest(null, null, 1, 10);

        _userRepositoryMock
            .Setup(r => r.GetAllByFilter(null, null, 1, 10, cancelationToken))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(request, cancelationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("500", result.Errors.First().Status);
    }
}
