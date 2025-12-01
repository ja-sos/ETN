using ETN.Infrastructure;

namespace ETN.API.Tests.Helpers;

public class DbFixture(EtnDbContext dbContext) : IDisposable, IAsyncDisposable
{
    public EtnDbContext DbContext { get; } = dbContext;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        Dispose(false);
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        DbContext.Database.EnsureDeleted();
        await DbContext.DisposeAsync();
    }
}
