﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Extensions.DependencyInjection;

namespace Canduits.Azure.Functions.DependencyInjection
{
    internal class InjectBinding : IBinding
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type _type;

        internal InjectBinding(IServiceProvider serviceProvider, Type type)
        {
            _type = type;
            _serviceProvider = serviceProvider;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            return Task.FromResult((IValueProvider) new InjectValueProvider(value));
        }

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            await Task.Yield();
            var scope = InjectBindingProvider.Scopes.GetOrAdd(context.FunctionInstanceId,
                _ => _serviceProvider.CreateScope());
            var value = scope.ServiceProvider.GetRequiredService(_type);
            return await BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor();
        }

        private class InjectValueProvider : IValueProvider
        {
            private readonly object _value;

            public InjectValueProvider(object value)
            {
                _value = value;
            }

            public Type Type => _value.GetType();

            public Task<object> GetValueAsync()
            {
                return Task.FromResult(_value);
            }

            public string ToInvokeString()
            {
                return _value.ToString();
            }
        }
    }
}