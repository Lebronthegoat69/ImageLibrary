
using ImageLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ImageLibrary.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public GalleryModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public List<ImageRecord> Images { get; set; } = new List<ImageRecord>();

        public async Task OnGetAsync()
        {
            string jsonPath = Path.Combine(
                _environment.ContentRootPath,
                "data",
                "images.json");

            if (!System.IO.File.Exists(jsonPath))
            {
                return;
            }

            string json = await System.IO.File.ReadAllTextAsync(jsonPath);

            if (!string.IsNullOrWhiteSpace(json))
            {
                Images = JsonSerializer.Deserialize<List<ImageRecord>>(json)
                    ?? new List<ImageRecord>();
            }
        }
    }
}
