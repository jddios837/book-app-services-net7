using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class GoResourceAdmin : PageTest
{
    [Test]
    public async Task MyTest()
    {
        
        await Page.GotoAsync("https://resadmin.azurewebsites.net/");

        await Page.GotoAsync("https://resadmin.azurewebsites.net/signin");

        await Page.GotoAsync("https://api.empoweriam.com/oauth/v2/ui/authorize?client_id=b8043f62-ac3c-41e5-88e9-58f52cbf2c94&redirect_uri=https%3A%2F%2Fresadmin.azurewebsites.net%2Fcallback&response_type=code&scope=openid%20mscommon%20application.all&state=940180a456c94e3a89ee51c397897151&code_challenge=OtiwAdNGi1ludc3QhFGzML_kuASIkvYIFRuZxikBXRw&code_challenge_method=S256&response_mode=query");

        await Page.GetByPlaceholder("User Name").ClickAsync();

        await Page.GetByPlaceholder("User Name").FillAsync("slavko.bosancic@empowerid.com");

        // await Page.GetByRole(AriaRole.Button, new() { Name = "Enter password" }).ClickAsync();
        var passwordButton = Page.Locator("button[class^='eid-password-button']");
        await passwordButton.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 0});
        await Page.Locator("button[class^='eid-password-button']").ClickAsync();
        

        await Page.GetByRole(AriaRole.Button, new() { Name = "OK" }).ClickAsync();

        var inputPassword = Page.GetByPlaceholder("Password");
        await inputPassword.WaitForAsync(new LocatorWaitForOptions() { State = WaitForSelectorState.Visible, Timeout = 0});
        await Page.Locator("//*[@id='eid-password-input']").ClickAsync();

        await Page.Locator("//*[@id='eid-password-input']").FillAsync("1q2w3e4r%T");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Slavko Bosancic Login As Slavko Bosancic Softwareentwickler Standard Employee in All Business Locations" }).ClickAsync();

        await Page.GotoAsync("https://resadmin.azurewebsites.net/applications");

        await Page.GetByRole(AriaRole.Row, new() { Name = "Application Image 01-testRegressionAppPBAC 01-testRegressionAppPBAC - - Testing Details" }).GetByRole(AriaRole.Button).First.ClickAsync();
    }
}