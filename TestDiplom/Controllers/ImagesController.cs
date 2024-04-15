using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using System.Drawing;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly CourseContext _context;

        public ImagesController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<List<FileContentResult>> GetImage()
        {
            var images = await _context.Image.ToListAsync();
            // Создаем список файлов изображений
            var files = new List<FileContentResult>();
            foreach (var image in images)
            {
                files.Add(File(image.ImageData, "image/jpeg")); // Можете изменить MIME-тип, если требуется
            }

            return files;
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<FileContentResult> GetImage(int id)
        {
            var image = _context.Image.Find(id);
            if (image == null)
            {
                var imageDefault = _context.Image.Find(1);
                return File(imageDefault.ImageData, "image/jpeg");
            }
            else
                return File(image.ImageData, "image/jpeg");
        }

        // GET: api/Images/5
        [HttpGet("byte/{id}")]
        public async Task<string> GetImageByte(int id)
        {
            var image = _context.Image.Find(id);

            var base64Image = Convert.ToBase64String(image.ImageData);
            return base64Image;
        }

        // POST: api/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Image>> PostImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("Пожалуйста, выберите файл для загрузки.");
            }

            // Проверяем расширение файла
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Пожалуйста, загрузите изображение формата .png, .jpeg или .jpg");
            }

            // Преобразуем данные изображения в квадратную форму
            System.Drawing.Image originalImage;
            using (var stream = image.OpenReadStream())
            {
                originalImage = System.Drawing.Image.FromStream(stream);
            }

            int newSize = Math.Min(originalImage.Width, originalImage.Height);
            Bitmap squareImage = new Bitmap(newSize, newSize);
            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                graphics.DrawImage(originalImage, 0, 0, newSize, newSize);
            }

            // Преобразуем квадратное изображение в байтовый массив
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                squareImage.Save(memoryStream, originalImage.RawFormat);
                imageData = memoryStream.ToArray();
            }

            // Сохраняем данные изображения в базу данных
            var newImage = new Models.Image
            {
                ImageData = imageData,
                // Другие свойства изображения, если есть
            };

            _context.Image.Add(newImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { id = newImage.ImageId }, newImage);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Image.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.ImageId == id);
        }
    }
}
