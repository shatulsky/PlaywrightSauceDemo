using System.Collections.Concurrent;
using Common.Playwright.Browser.Context;
using Microsoft.Playwright;

namespace Common.Playwright.Browser;

public class PlaywrightBrowserProvider : IAsyncDisposable
{
    private readonly SemaphoreSlim _lock = new(1);
    private readonly PlaywrightFactory _playwrightFactory;
    private readonly PlaywrightBrowserFactory _browserFactory;
    private readonly PlaywrightContextFactory _contextFactory;
    private readonly ContextCreationStrategyStorage _scopeCreationStrategyStorage;

    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly ConcurrentDictionary<string, Task<IBrowserContext>> _contexts = new();


    public PlaywrightBrowserProvider(PlaywrightFactory playwrightFactory,
        PlaywrightBrowserFactory browserFactory,
        PlaywrightContextFactory contextFactory,
        ContextCreationStrategyStorage scopeCreationStrategyStorage)
    {
        _playwrightFactory = playwrightFactory;
        _browserFactory = browserFactory;
        _contextFactory = contextFactory;
        _scopeCreationStrategyStorage = scopeCreationStrategyStorage;
    }

    public async Task<IPlaywright> GetPlaywrightAsync()
    {
        await EnsureBrowserCreatedAsync();
        return _playwright!;
    }

    public async Task<IBrowserContext> GetInstanceAsync(string identifier)
    {
        await EnsureBrowserCreatedAsync();
        return await _contexts.GetOrAdd(identifier, async _ =>
        {
            var creationStrategy = _scopeCreationStrategyStorage.GetStrategy();
            return await _contextFactory.Create(_browser!, creationStrategy);
        });
    }

    public async Task DisposeInstanceAsync(string identifier)
    {
        if (_contexts.TryRemove(identifier, out var contextTask))
        {
            var context = await contextTask.ConfigureAwait(false);
            await context.CloseAsync();
        }
    }

    private async Task EnsureBrowserCreatedAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_playwright == null)
            {
                _playwright = await _playwrightFactory.Create();
                _browser = await _browserFactory.Create(_playwright);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var kv in _contexts)
        {
            try
            {
                var ctx = await kv.Value.ConfigureAwait(false);
                await ctx.CloseAsync().ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }
        }

        _contexts.Clear();

        if (_browser is not null)
        {
            await _browser.CloseAsync().ConfigureAwait(false);
            _browser = null;
        }

        _playwright?.Dispose();
        _playwright = null;
    }
}