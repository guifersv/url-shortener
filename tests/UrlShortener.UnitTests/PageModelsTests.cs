using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortener.UnitTests;

public class PageModelsTests
{
    [Fact]
    public async Task IndexOnPostAsync_ShouldReturnRedirectToPage_WhenAliasIsEmpty()
    {
        ShortUrlDto shortUrl = new() { Alias = "", Url = "https://example.com" };
        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.CreateShortUrlModel(It.Is<ShortUrlDto>(a => a == shortUrl)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostAsync_ShouldReturnRedirectToPage_WhenAliasDoesNotExists()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };
        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.GetShortUrlModelByAlias(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns((ShortUrlDto?)null)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s => s.CreateShortUrlModel(It.Is<ShortUrlDto>(a => a == shortUrl)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostAsync_ShouldReturnPage_WhenAliasAlreadyExists()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };
        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.GetShortUrlModelByAlias(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<PageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostDeleteAsync_ShouldReturnRedirectToPage_WhenModelExists()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.DeleteShortUrlModel(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(true)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostDeleteAsync(shortUrl.Alias);

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostDeleteAsync_ShouldReturnBadRequest_WhenModelDoesNotExist()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.DeleteShortUrlModel(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(false)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostDeleteAsync(shortUrl.Alias);

        Assert.IsType<BadRequestResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task RedirectOnGetAsync_ShouldReturnRedirectPermanent_WhenModelIsNotNull()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.GetShortUrlModelByAlias(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s =>
                s.IncrementShortUrlModelAccessCount(It.Is<string>(a => a == shortUrl.Alias)).Result
            )
            .Returns(true)
            .Verifiable(Times.Once());

        RedirectModel redirectModel = new(serviceMock.Object, logger) { };
        var returnedModel = await redirectModel.OnGetAsync(shortUrl.Alias);

        var result = Assert.IsType<RedirectResult>(returnedModel);
        Assert.Equal(shortUrl.Url, result.Url);
        Assert.True(result.Permanent);
        serviceMock.Verify();
    }

    [Fact]
    public async Task RedirectOnGetAsync_ShouldReturnNotFound_WhenModelIsNull()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.GetShortUrlModelByAlias(It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns((ShortUrlDto?)null)
            .Verifiable(Times.Once());

        RedirectModel redirectModel = new(serviceMock.Object, logger) { };
        var returnedModel = await redirectModel.OnGetAsync(shortUrl.Alias);

        Assert.IsType<NotFoundResult>(returnedModel);
        serviceMock.Verify();
    }
}
