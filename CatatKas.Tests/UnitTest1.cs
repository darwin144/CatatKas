using Microsoft.Playwright;

namespace CatatKas.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Login_PositiveCase()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        // Arahkan ke halaman login
        await page.GotoAsync("https://localhost:7109/Home/Login");

        // Isi form dengan kredensial valid
        await page.FillAsync("input[name='user_id']", "darwinmanurung0110@gmail.com");
        await page.FillAsync("input[name='user_password']", "123123");

        await page.ClickAsync("button[type='submit']");

        // Verifikasi redirect ke Index
        await page.WaitForURLAsync("**/Index");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);


        await page.WaitForTimeoutAsync(2000);

        // Evidence screenshot
        var projectDir = Path.GetFullPath(Path.Combine(
                            Directory.GetCurrentDirectory(), "..", "..", ".."
                        ));

        var screenshotPath = Path.Combine(projectDir, "Evidence", "Positive", "Login_Positive.png");
        Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });

    }


    [Fact]
    public async Task Login_NegativeCase()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        var page = await browser.NewPageAsync();

        // Arahkan ke halaman login
        await page.GotoAsync("https://localhost:7109/Home/Login");

        // Isi form dengan kredensial salah
        await page.FillAsync("input[name='user_id']", "wronguser@gmail.com");
        await page.FillAsync("input[name='user_password']", "wrongpass");
        await page.ClickAsync("button[type='submit']");

        // Verifikasi pesan error muncul
        var errorText = await page.InnerTextAsync("body");
        Assert.Contains("username atau Password salah!", errorText);

        // Evidence screenshot
        var projectDir = Path.GetFullPath(Path.Combine(
                           Directory.GetCurrentDirectory(), "..", "..", ".."
                       ));

        var screenshotPath = Path.Combine(projectDir, "Evidence", "Negative", "Login_Negative.png");
        Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });
    }

}
