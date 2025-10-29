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
            .Setup(s => s.CreateShortUrlModel(
                       It.Is<ShortUrlDto>(a => a.Alias == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostAsync_ShouldReturnRedirectToPage_WhenModelDoesNotExist()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };
        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.FindShortUrlModelByAlias(
                       It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns((ShortUrlDto?)null)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s => s.CreateShortUrlModel(
                       It.Is<ShortUrlDto>(a => a.Alias == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostAsync_ShouldReturnPage_WhenModelExistsAndAliasIsNotEmpty()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };
        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.FindShortUrlModelByAlias(
                       It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s => s.CreateShortUrlModel(
                       It.Is<ShortUrlDto>(a => a.Alias == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Never());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostAsync();

        Assert.IsType<PageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task IndexOnPostDeleteAsync_ShouldReturnRedirectToPage()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.DeleteShortUrlModel(It.Is<string>(a => a == shortUrl.Alias)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());

        IndexModel indexModel = new(serviceMock.Object, logger) { ShortUrl = shortUrl };
        var result = await indexModel.OnPostDeleteAsync(shortUrl.Alias);

        Assert.IsType<RedirectToPageResult>(result);
        serviceMock.Verify();
    }

    [Fact]
    public async Task RedirectOnGetAsync_ShouldReturnRedirect_WhenModelIsNotNull()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.FindShortUrlModelByAlias(
                       It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns(shortUrl)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s => s.IncrementShortUrlAccessCount(
                       It.Is<string>(a => a == shortUrl.Alias)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once());


        RedirectModel redirectModel = new(serviceMock.Object, logger) { };
        var returnedModel = await redirectModel.OnGetAsync(shortUrl.Alias);

        var result = Assert.IsType<RedirectResult>(returnedModel);
        Assert.Equal(shortUrl.Url, result.Url);
        Assert.True(result.Permanent);
        serviceMock.Verify();
    }

    [Fact]
    public async Task RedirectOnGetAsync_ShouldReturnNotFound_WhenModelNull()
    {
        ShortUrlDto shortUrl = new() { Alias = "alias", Url = "https://example.com" };

        var logger = Mock.Of<ILogger<IndexModel>>();

        var serviceMock = new Mock<IUrlShortenerService>();
        serviceMock
            .Setup(s => s.FindShortUrlModelByAlias(
                       It.Is<string>(a => a == shortUrl.Alias)).Result)
            .Returns((ShortUrlDto?)null)
            .Verifiable(Times.Once());
        serviceMock
            .Setup(s => s.IncrementShortUrlAccessCount(
                       It.Is<string>(a => a == shortUrl.Alias)))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Never());


        RedirectModel redirectModel = new(serviceMock.Object, logger) { };
        var returnedModel = await redirectModel.OnGetAsync(shortUrl.Alias);

        Assert.IsType<NotFoundResult>(returnedModel);
        serviceMock.Verify();
    }
}