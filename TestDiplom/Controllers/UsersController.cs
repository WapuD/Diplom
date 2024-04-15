using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using Refit;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CourseContext _context;

        public UsersController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/Authors
        [HttpGet("Authors")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.Where(u => u.UserRole == 2).ToListAsync();
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO user)
        {
            if (!UserExistsLogin(user.UserLogin))
            {
                var lastUser = _context.Users.OrderBy(u => u.UserId).LastOrDefault();
                var item = new User();
                if (lastUser != null)
                    item.UserId = lastUser.UserId + 1;
                else
                    item.UserId = 1;

                item.UserFirstName = user.UserFirstName;
                item.UserSecondName = user.UserSecondName;
                item.UserLogin = user.UserLogin;
                item.UserPassword = user.UserPassword;
                item.UserImage = 1;
                item.UserRole = 1;
                item.UserCountry = 1;

                var role = await _context.Roles.FindAsync(item.UserRole);
                item.Role = role;
                var image = await _context.Image.FindAsync(item.UserImage);
                item.Image = image;
                var country = await _context.Country.FindAsync(item.UserCountry);
                item.Country = country;

                _context.Users.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = item.UserId }, user);
            }
            else
                return Conflict("Пользователь с таким логином уже существует");
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        private bool UserExistsLogin(string login)
        {
            return _context.Users.Any(e => e.UserLogin == login);
        }


        // POST: api/Users/DocumentUser
        [HttpPost("DocumentUser")]
        public IActionResult DocumentUser()
        {
            // Получаем список пользователей из базы данных
            var users = _context.Users.OrderBy(user => user.UserId).ToList();

            var memoryStream = new MemoryStream();
            var document = new Document();
            PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            // Adding title
            var titleFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 18f, Font.BOLD);
            var title = new Paragraph("User Report", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            // Adding total number of users
            var totalUsers = users.Count;
            document.Add(new Paragraph($"Total number of users: {totalUsers}"));

            // Counting number of users with each role
            var readerCount = users.Count(u => u.UserRole == 1);
            var writerCount = users.Count(u => u.UserRole == 2);
            // Adding number of users with each role
            document.Add(new Paragraph($"Number of users with role 'Reader': {readerCount}"));
            document.Add(new Paragraph($"Number of users with role 'Writer': {writerCount}"));

            // Adding user data to table
            var table = new PdfPTable(5); // 4 columns for user properties
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.SpacingBefore = 20f;

            // Adding column headers
            var columnHeaderFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 12f, Font.BOLD);
            AddCell(table, "ID", columnHeaderFont);
            AddCell(table, "First Name", columnHeaderFont);
            AddCell(table, "Second Name", columnHeaderFont);
            AddCell(table, "Third Name", columnHeaderFont);
            AddCell(table, "Gender", columnHeaderFont);

            // Adding user data to table
            var dataFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.EMBEDDED, 10f);
            foreach (var user in users)
            {
                AddCell(table, user.UserId.ToString(), dataFont);
                AddCell(table, user.UserFirstName, dataFont);
                AddCell(table, user.UserSecondName, dataFont);
                AddCell(table, user.UserThirdName ?? "NULL", dataFont);
                AddCell(table, user.UserGender.ToString() ?? "NULL", dataFont);
            }

            document.Add(table);

            // Adding user data to table
            var tableZero = new PdfPTable(5); // 4 columns for user properties
            tableZero.WidthPercentage = 100;
            tableZero.HorizontalAlignment = Element.ALIGN_CENTER;
            tableZero.SpacingBefore = 20f;

            // Adding column headers
            AddCell(tableZero, "Login", columnHeaderFont);
            AddCell(tableZero, "Password", columnHeaderFont);
            AddCell(tableZero, "Role", columnHeaderFont);
            AddCell(tableZero, "Country", columnHeaderFont);
            AddCell(tableZero, "Age", columnHeaderFont);

            // Adding user data to table
            foreach (var user in users)
            {
                AddCell(tableZero, user.UserLogin, dataFont);
                AddCell(tableZero, user.UserPassword, dataFont);
                AddCell(tableZero, user.Role.RoleName ?? "NULL", dataFont);
                AddCell(tableZero, user.UserCountry.ToString() ?? "NULL", dataFont);
                AddCell(tableZero, user.UserImage.ToString() ?? "NULL", dataFont);
                AddCell(tableZero, user.UserAge.ToString() ?? "NULL", dataFont);
            }

            document.Add(tableZero);

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", "UserReport.pdf");
        }

        private void AddCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 5;
            table.AddCell(cell);
        }
    }
}
