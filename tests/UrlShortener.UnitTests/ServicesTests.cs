namespace UrlShortener.UnitTests;

public class ServicesTests
{
    [Fact]
    public async Task CreateShortUrlModel_ShouldReturnBindToDto_WhenAliasIsNotNullOrEmpty()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.CreateShortUrlModel(
                    It.Is<ShortUrlModel>(m =>
                        m.Alias == shortUrlModel.Alias && m.Url == shortUrlModel.Url
                    )
                ).Result
            )
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.CreateShortUrlModel(Utils.ShortUrlToDto(shortUrlModel));

        Assert.IsType<ShortUrlDto>(returnedObject);
        Assert.Equal(shortUrlModel.Alias, returnedObject.Alias);
        Assert.Equal(shortUrlModel.Url, returnedObject.Url);
        Assert.Equal(shortUrlModel.Accesses, returnedObject.Accesses);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task CreateShortUrlModel_ShouldReturnDtoWithRandomAlias_WhenAliasIsNullOrEmpty()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r => r.GetShortUrlModelByAlias(It.IsAny<string>()).Result)
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.CreateShortUrlModel(It.Is<ShortUrlModel>(m => m.Url == shortUrlModel.Url)).Result
            )
            .Returns<ShortUrlModel>(m => m)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.CreateShortUrlModel(Utils.ShortUrlToDto(shortUrlModel));

        Assert.IsType<ShortUrlDto>(returnedObject);
        Assert.Equal(shortUrlModel.Accesses, returnedObject.Accesses);
        Assert.Equal(shortUrlModel.Url, returnedObject.Url);
        Assert.NotNull(returnedObject.Alias);
        Assert.NotEmpty(returnedObject.Alias);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetShortUrlModelByAlias_ShouldReturnDto_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.GetShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.GetShortUrlModelByAlias(shortUrlModel.Alias);

        Assert.NotNull(returnedObject);
        Assert.IsType<ShortUrlDto>(returnedObject);
        Assert.Equal(shortUrlModel.Alias, returnedObject.Alias);
        Assert.Equal(shortUrlModel.Url, returnedObject.Url);
        Assert.Equal(shortUrlModel.Accesses, returnedObject.Accesses);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetShortUrlModelByAlias_ShouldReturnNull_WhenModelDoesNotExist()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.GetShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var returnedObject = await service.GetShortUrlModelByAlias(shortUrlModel.Alias);

        Assert.Null(returnedObject);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteShortUrlModel_ShouldCallReturnTrue_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.FindShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteShortUrlModel(It.Is<ShortUrlModel>(m => m == shortUrlModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var result = await service.DeleteShortUrlModel(shortUrlModel.Alias);

        Assert.True(result);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task DeleteShortUrlModel_ShouldCallReturnFalse_WhenModelDoesNotExist()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.FindShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r => r.DeleteShortUrlModel(It.Is<ShortUrlModel>(m => m == shortUrlModel)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var result = await service.DeleteShortUrlModel(shortUrlModel.Alias);

        Assert.False(result);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task IncrementShortUrlModelAccessCount_ShouldReturnTrue_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new()
        {
            Alias = "alias",
            Url = "string",
            Accesses = 0,
        };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.FindShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.IncrementShortUrlAccessCount(It.Is<ShortUrlModel>(m => m == shortUrlModel))
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var result = await service.IncrementShortUrlModelAccessCount(shortUrlModel.Alias);

        Assert.True(result);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task IncrementShortUrlModelAccessCount_ShouldReturnFalse_WhenModelExists()
    {
        ShortUrlModel shortUrlModel = new()
        {
            Alias = "alias",
            Url = "string",
            Accesses = 0,
        };

        var logger = Mock.Of<ILogger<UrlShortenerService>>();

        var repositoryMock = new Mock<IUrlShortenerRepository>();
        repositoryMock
            .Setup(r =>
                r.FindShortUrlModelByAlias(
                    It.Is<string>(alias => alias == shortUrlModel.Alias)
                ).Result
            )
            .Returns((ShortUrlModel?)null)
            .Verifiable(Times.Once());
        repositoryMock
            .Setup(r =>
                r.IncrementShortUrlAccessCount(It.Is<ShortUrlModel>(m => m == shortUrlModel))
            )
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var result = await service.IncrementShortUrlModelAccessCount(shortUrlModel.Alias);

        Assert.False(result);
        repositoryMock.Verify();
    }

    [Fact]
    public async Task GetAllShortUrls_ShouldReturnList()
    {
        ShortUrlModel shortUrlModel = new() { Alias = "alias", Url = "string" };
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
        repositoryMock.Verify();
    }
}
