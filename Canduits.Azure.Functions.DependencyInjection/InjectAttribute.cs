using System;
using Microsoft.Azure.WebJobs.Description;

namespace Canduits.Azure.Functions.DependencyInjection
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}