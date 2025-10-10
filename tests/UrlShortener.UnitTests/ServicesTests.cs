namespace UrlShortener.UnitTests;

public class ServicesTests
{
    [Fact]
    public async Task FindShortUrlModelByAlias_ShouldReturnShortUrlDto_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.Is<string>(
                            alias => alias == shortUrlModel.Alias)).Result)
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.FindShortUrlModelByAlias(shortUrlModel.Alias);

        Assert.NotNull(returnedObject);
        Assert.IsType<ShortUrlDto>(returnedObject);
        Assert.Equal(shortUrlModel.Alias, returnedObject.Alias);
        Assert.Equal(shortUrlModel.Url, returnedObject.Url);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task FindShortUrlModelByAlias_ShouldReturnNull_WhenModelDoesNotExist()
    {
        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.IsAny<string>()).Result)
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.FindShortUrlModelByAlias("string");

        Assert.Null(returnedObject);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteShortUrlModel_ShouldCallRepositoryDelete_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.Is<string>(
                            alias => alias == shortUrlModel.Alias)).Result)
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteShortUrlModel(It.Is<ShortUrlModel>(
                            m => m.Url == shortUrlModel.Url && m.Alias == shortUrlModel.Alias)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        await service.DeleteShortUrlModel(shortUrlModel.Alias);

        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteShortUrlModel_ShouldNotCallRepositoryDelete_WhenModelDoesNotExist()
    {

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.IsAny<string>()).Result)
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteShortUrlModel(It.IsAny<ShortUrlModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        await service.DeleteShortUrlModel("string");

        repositoryMock.Verify();
    }

    [Fact]
    public async Task IncrementShortUrlAccessCount_ShouldCallRepositoryIncrement_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.Is<string>(
                            alias => alias == shortUrlModel.Alias)).Result)
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.IncrementShortUrlAccessCount(It.Is<ShortUrlModel>(
                            m => m.Url == shortUrlModel.Url && m.Alias == shortUrlModel.Alias)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        await service.IncrementShortUrlAccessCount(shortUrlModel.Alias);

        repositoryMock.Verify();
    }

    [Fact]
    public async Task IncrementShortUrlAccessCount_ShouldNotCallRepositoryIncrement_WhenModelDoesNotExist()
    {
        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.FindShortUrlModelByAlias(It.IsAny<string>()).Result)
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.IncrementShortUrlAccessCount(It.IsAny<ShortUrlModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        await service.IncrementShortUrlAccessCount("string");

        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllShortUrls_ShouldReturnList_WhenAtLeastOneExist()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };
        List<ShortUrlModel> models = [shortUrlModel];

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.GetAllShortUrls().Result)
            .Returns(models)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.GetAllShortUrls();

        Assert.Single(returnedObject);
        Assert.IsType<List<ShortUrlDto>>(returnedObject);
        Assert.Equal(shortUrlModel.Alias, returnedObject.ElementAt(0).Alias);
        Assert.Equal(shortUrlModel.Url, returnedObject.ElementAt(0).Url);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllShortUrls_ShouldReturnEmptyList_WhenDoesNotHaveAny()
    {
        List<ShortUrlModel> models = [];

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.GetAllShortUrls().Result)
            .Returns(models)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.GetAllShortUrls();

        Assert.Empty(returnedObject);
        Assert.IsType<List<ShortUrlDto>>(returnedObject);
        repositoryMock.Verify();
    }
}