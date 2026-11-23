# ImageLibrary

## Overview
ImageLibrary is an ASP.NET Core application that allows users to securely upload images and view them in an image gallery.

The application validates uploaded files to help prevent unsupported or oversized files from being saved.

## Features
- Upload images using an upload form.
- Accept JPG and PNG image formats.
- Validate file size (maximum 2 MB).
- Save uploaded images in the `wwwroot/uploads` folder.
- Store image information in a JSON file.
- Display uploaded images in a gallery with their titles.
- Validate uploaded files before saving them.

## Technologies Used
- C#
- ASP.NET Core
- Razor Pages
- HTML
- CSS
- JSON

## Project Structure
- `Models/ImageRecord.cs` - Stores image information.
- `Pages/Upload.cshtml` - Image upload page.
- `Pages/Gallery.cshtml` - Displays uploaded images.
- `data/images.json` - Stores image metadata.
- `wwwroot/uploads/` - Stores uploaded image files.
- `Program.cs` - Configures the application.

## How to Run
1. Clone or download this repository.
2. Open the project in Visual Studio.
3. Build the project.
4. Run the application.
5. Navigate to the Upload page to upload an image.
6. Visit the Gallery page to view uploaded images.

<img width="728" height="468" alt="image" src="https://github.com/user-attachments/assets/29b3902a-df3f-4b38-b07d-390d3d254889" />
<img width="489" height="372" alt="image" src="https://github.com/user-attachments/assets/41bd1208-ece8-408d-9632-86efa93ff906" />
<img width="726" height="120" alt="image" src="https://github.com/user-attachments/assets/19201586-c8cf-453f-ac27-fd52ace982b2" />

## Author
Sam
