using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    public static class DependencyInjector
    {
        private static readonly ServiceCollection serviceCollection = new ServiceCollection();
        public static void AddSingleton<T, Q>()
            where T : class where Q : class, T
        {
            serviceCollection.AddSingleton<T, Q>();
        }

        public static T CreateInstance<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(serviceCollection.BuildServiceProvider());
        }
    }
}
