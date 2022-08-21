using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http.Headers;
using TestProjectWthAngular.Services;

namespace TestProjectWthAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly ILogger<UploadController> _logger;
        private readonly IXmlConverter _xmlConterter;
        public UploadController(ILogger<UploadController> logger, IXmlConverter xmlConverter)
        {
            _logger = logger;
            _xmlConterter = xmlConverter;
        }

        [EnableCors("Policy1")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();

            string pathToSave = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
                await stream.FlushAsync();
                stream.Position = 0;
            }
            
            await _xmlConterter.Convert(fullPath, System.IO.Path.GetFileNameWithoutExtension(fileName));

            return Ok();

        }
    }
}