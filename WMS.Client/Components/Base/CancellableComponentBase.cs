using Microsoft.AspNetCore.Components;

namespace WMS.Client.Components.Base
{
    public abstract class CancellableComponentBase : ComponentBase, IDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        protected CancellationToken CancellationToken => _cts.Token;

        public virtual void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
