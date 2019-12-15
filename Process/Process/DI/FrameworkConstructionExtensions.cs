using Dna;
using Process.DI;
using Process.ViewModel.App;
using Microsoft.Extensions.DependencyInjection;

namespace Process.DI
{
    /// <summary>
    /// Extension methods for the <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        public static FrameworkConstruction AddAppViewModels(this FrameworkConstruction construction)
        {
            // Bind to a single instance of Application view model
            construction.Services.AddSingleton<ApplicationViewModel>();

            // Return the construction for chaining
            return construction;
        }
    }
}