using Microsoft.JSInterop;

namespace Northwind.BlazorWasm.Client.Services;

public class CarouselService : IAsyncDisposable
{
    private readonly IJSRuntime jsRuntime;
    private Lazy<IJSObjectReference> jsModule = new();
    
    public CarouselService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }
    
    public async ValueTask DisposeAsync()
    {
        if (jsModule.IsValueCreated)
        {
            await jsModule.Value.DisposeAsync();
        }
    }
    
    public async Task InitializeCarousel()
    {
        await WaitForReference();
        await jsModule.Value.InvokeVoidAsync("initialize");
    }
    
    private async Task WaitForReference()
    {
        if (!jsModule.IsValueCreated)
        {

            jsModule = new(await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./js/carousel.js"));
        }
    }
}