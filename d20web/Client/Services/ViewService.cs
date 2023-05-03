using Microsoft.JSInterop;

namespace d20Web.Services
{
    /// <summary>
    /// Class for managing the current view
    /// </summary>
    public sealed class ViewService
    {
        /// <summary>
        /// Constructs a new <see cref="ViewService"/>
        /// </summary>
        /// <param name="runtime">Javascript runtime to interface with</param>
        public ViewService(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        private readonly IJSRuntime _runtime;
        public async Task ScrollIntoView(string id, bool smooth = true, ScrollIntoViewOption verticalAlignment = ScrollIntoViewOption.Center, ScrollIntoViewOption horizontalAlignment = ScrollIntoViewOption.Center)
        {
            await _runtime.InvokeVoidAsync("ScrollIntoView", id, smooth ? "smooth" : "instant", verticalAlignment.ToString().ToLower(), horizontalAlignment.ToString().ToLower());
        }
    }
}
