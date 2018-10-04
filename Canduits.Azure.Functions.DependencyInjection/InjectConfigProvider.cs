using Microsoft.Azure.WebJobs.Host.Config;

namespace Canduits.Azure.Functions.DependencyInjection
{
    public class InjectConfigProvider : IExtensionConfigProvider
    {
        private readonly InjectBindingProvider _injectBindingProvider;

        public InjectConfigProvider(InjectBindingProvider injectBindingProvider)
        {
            _injectBindingProvider = injectBindingProvider;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            context
                .AddBindingRule<InjectAttribute>()
                .Bind(_injectBindingProvider);
        }
    }
}