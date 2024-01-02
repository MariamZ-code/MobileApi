using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MediConsultMobileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public UploadImageController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public class FileUpload
        {
            public IFormFile imageUrl { get; set; }
        }

        [HttpPost]
        public async Task<string> uploadPhoto([FromQuery] FileUpload file)
        {
            try
            {
                if (file.imageUrl.Length > 0)
                {
                    if (!Directory.Exists(webHostEnvironment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(webHostEnvironment.WebRootPath + "\\Upload\\");
                    }

                    using (FileStream fileStream = System.IO.File.Create(webHostEnvironment.WebRootPath + "\\Upload\\" + file.imageUrl.FileName))
                    {
                        file.imageUrl.CopyTo(fileStream);
                        fileStream.Flush();
                        return "\\Upload\\" + file.imageUrl.FileName;
                    }
                }

                return "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        [HttpGet("getPhoto/{filePath}")]
        public IActionResult GetPhoto(string filePath)
        {
            try
            {
                // Combine the web root path with the requested file path
                string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, filePath);

                // Check if the file exists
                if (System.IO.File.Exists(absolutePath))
                {
                    // Read the file content
                    var fileContent = System.IO.File.ReadAllBytes(absolutePath);

                    // Determine the file content type
                    string contentType = GetContentType(filePath);

                    // Return the file with appropriate content type
                    return File(fileContent, contentType);
                }

                // Return not found if the file does not exist
                return NotFound("File not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

   
    

    private string GetContentType(string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = "application/octet-stream"; // Default content type
        }
        return contentType;
    }

       
    }
}
