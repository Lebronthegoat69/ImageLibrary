
using ImageLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ImageLibrary.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public UploadModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter an image title.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
            }
            else
            {
                const long maxFileSize = 2 * 1024 * 1024;

                if (ImageFile.Length >= maxFileSize)
                {
                    ModelState.AddModelError("ImageFile", "The image must be smaller than 2 MB.");
                }

                string extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

                if (extension != ".jpg" && extension != ".png")
                {
                    ModelState.AddModelError("ImageFile", "Only JPG and PNG files are allowed.");
                }

                if (extension == ".jpg" && ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "The selected file is not a valid JPG image.");
                }

                if (extension == ".png" && ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "The selected file is not a valid PNG image.");
                }

                byte[] signature = new byte[8];

                using var stream = ImageFile.OpenReadStream();
                int bytesRead = await stream.ReadAsync(signature.AsMemory(0, 8));

                bool validJpg = bytesRead >= 3 &&
                    signature[0] == 0xFF &&
                    signature[1] == 0xD8 &&
                    signature[2] == 0xFF;

                bool validPng = bytesRead == 8 &&
                    signature.SequenceEqual(new byte[]
                    {
                        0x89, 0x50, 0x4E, 0x47,
                        0x0D, 0x0A, 0x1A, 0x0A
                    });

                if (!validJpg && !validPng)
                {
                    ModelState.AddModelError("ImageFile", "The file contents are not a valid JPG or PNG image.");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string uploadsFolder = Path.Combine(
                _environment.WebRootPath, "uploads");

            Directory.CreateDirectory(uploadsFolder);

            string extensionToSave =
                Path.GetExtension(ImageFile!.FileName).ToLowerInvariant();

            string newFileName = Guid.NewGuid().ToString() + extensionToSave;

            string filePath = Path.Combine(uploadsFolder, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            string dataFolder = Path.Combine(
                _environment.ContentRootPath, "data");

            Directory.CreateDirectory(dataFolder);

            string jsonPath = Path.Combine(dataFolder, "images.json");

            List<ImageRecord> images = new List<ImageRecord>();

            if (System.IO.File.Exists(jsonPath))
            {
                string existingJson = await System.IO.File.ReadAllTextAsync(jsonPath);

                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    images = JsonSerializer.Deserialize<List<ImageRecord>>(existingJson)
                        ?? new List<ImageRecord>();
                }
            }

            images.Add(new ImageRecord
            {
                FileName = newFileName,
                Title = Title
            });

            string updatedJson = JsonSerializer.Serialize(
                images,
                new JsonSerializerOptions { WriteIndented = true });

            await System.IO.File.WriteAllTextAsync(jsonPath, updatedJson);

            SuccessMessage = "Your image was uploaded successfully!";

            ModelState.Clear();
            Title = string.Empty;
            ImageFile = null;

            return Page();
        }
    }
}
