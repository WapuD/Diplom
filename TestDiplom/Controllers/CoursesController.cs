using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using Npgsql.Internal;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly CourseContext _context;

        public CoursesController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseDTO course)
        {
            var lastCourse = _context.Courses.OrderBy(u => u.CourseId).LastOrDefault();
            var item = new Course();
            if (lastCourse != null)
                item.CourseId = lastCourse.CourseId + 1;
            else
                item.CourseId = 1;


            item.Author = await _context.Users.FindAsync(course.CourseAuthor);
            item.CourseAuthor = course.CourseAuthor;
            item.Category = await _context.Categories.FindAsync(course.CourseCategory);
            item.CourseCategory = course.CourseCategory;


            item.CourseName = course.CourseName;
            item.CourseText = course.CourseText;
            item.CourseDesciption = course.CourseDesciption;
            item.CourseTextRecom = course.CourseTextRecom;
            item.CourseTextWarning = course.CourseTextWarning;
            item.CourseData = DateTime.Today.ToString();


            _context.Courses.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = item.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        // POST: api/Courses/DocumentCourse
        [HttpPost("DocumentCourse")]
        public async Task<IActionResult> DocumentCourse()
        {
            var courses = _context.Courses.Include(c => c.Category).Include(c => c.Author).ToList();
            var categories = _context.Categories.ToList();
            int totalCourses = _context.Courses.Count();
            // Получаем количество курсов за текущий год
            var countThisYear = 0;
            var coursesThisYear = _context.Courses.ToList();
            for (int i = 0; i < coursesThisYear.Count; i++)
            {
                var str = coursesThisYear[i].CourseData;
                var year = str.Substring(str.Length - 4);
                if (Convert.ToInt32(year) == DateTime.Now.Year)
                    countThisYear += 1;
            }
            // Получаем количество курсов для каждой категории
            var coursesByCategory = _context.Courses
                .GroupBy(c => c.CourseCategory)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToList();
            var memoryStream = new MemoryStream();
            var document = new Document();
            PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Добавление заголовка
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18f);
            var title = new Paragraph("Statistic course", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Добавляем общее количество курсов в отчет
            document.Add(new Paragraph($"Count course: {totalCourses}"));

            // Добавляем количество курсов за текущий год в отчет
            document.Add(new Paragraph($"Courses on {DateTime.Now.Year} year: {countThisYear}"));

            // Добавляем количество курсов для каждой категории в отчет
            document.Add(new Paragraph("Course category:"));
            foreach (var category in coursesByCategory)
            {
                foreach (var item in categories)
                {
                    if (category.CategoryId == item.CategoryId)
                        document.Add(new Paragraph($"Course name: {item.CategoryName}, course count: {category.Count}"));
                }
            }

            // Добавление таблицы для статистики курсов
            var table = new PdfPTable(5); // 5 столбцов для свойств курса
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.SpacingBefore = 20f;
            // Добавление заголовков столбцов
            var columnHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);
            AddCell(table, "Course name", columnHeaderFont);
            AddCell(table, "Description", columnHeaderFont);
            AddCell(table, "Category", columnHeaderFont);
            AddCell(table, "Author", columnHeaderFont);
            AddCell(table, "Date", columnHeaderFont);

            // Добавление данных о курсах в таблицу
            var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10f);
            foreach (var course in courses)
            {
                AddCell(table, course.CourseName, dataFont);
                AddCell(table, course.CourseDesciption, dataFont);
                AddCell(table, course.Category.CategoryName, dataFont); // Если есть свойство CategoryName в модели Category
                AddCell(table, (course.Author.UserFirstName + " " + course.Author.UserSecondName), dataFont); // Если есть свойство UserName в модели User
                AddCell(table, course.CourseData, dataFont); // Добавьте данные о дате, если это нужно
            }

            document.Add(table);

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "CourseStatistics.pdf");
        }
        private void AddCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 5;
            table.AddCell(cell);
        }
    }
}
