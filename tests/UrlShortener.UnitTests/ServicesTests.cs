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
            .Setup(r => r.FindShortUrlModelByAlias(It.Is<string>(alias => alias == shortUrlModel.Alias)).Result)
            .Returns(shortUrlModel)
            .Verifiable(Times.Once());

        UrlShortenerService service = new(repositoryMock.Object, logger);
        var resultedObject = await service.FindShortUrlModelByAlias(shortUrlModel.Alias);

        Assert.NotNull(resultedObject);
        Assert.IsType<ShortUrlDto>(resultedObject);
        Assert.Equal(shortUrlModel.Alias, resultedObject.Alias);
        Assert.Equal(shortUrlModel.Url, resultedObject.Url);
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
        var resultedObject = await service.FindShortUrlModelByAlias("string");

        Assert.Null(resultedObject);
        repositoryMock.Verify();
    }
}