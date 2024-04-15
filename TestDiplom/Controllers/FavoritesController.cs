using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly CourseContext _context;

        public FavoritesController(CourseContext context)
        {
            _context = context;
        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorite()
        {
            return await _context.Favorite.ToListAsync();
        }

        // GET: api/Favorites/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetFavorite(int userId)
        {
            var favoriteCourses = await _context.Favorite
                .Where(f => f.FavoriteUser == userId) // Получаем записи избранного для заданного пользователя
                .Select(f => f.FavoriteCourse) // Выбираем только courseId
                .ToListAsync();

            if (favoriteCourses == null || favoriteCourses.Count == 0)
            {
                return NotFound("Записи в избранном для данного пользователя не найдены.");
            }

            // Получаем все курсы, чьи courseId содержатся в списке favoriteCourses
            var courses = await _context.Courses
                .Where(c => favoriteCourses.Contains(c.CourseId))
                .ToListAsync();

            if (courses == null)
            {
                return NotFound("Курсы из избранного не найдены.");
            }

            return courses;
        }

        // PUT: api/Favorites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavorite(int id, Favorite favorite)
        {
            if (id != favorite.FavoriteId)
            {
                return BadRequest();
            }

            _context.Entry(favorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteExists(id))
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

        // POST: api/Favorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> AddToFavorite(int userId, int courseId)
        {
            // Проверяем, существует ли уже запись в таблице Favorite с такими userId и courseId
            var existingFavorite = await _context.Favorite
                .FirstOrDefaultAsync(f => f.FavoriteUser == userId && f.FavoriteCourse == courseId);

            // Если запись уже существует, возвращаем BadRequest
            if (existingFavorite != null)
            {
                return BadRequest("Запись уже существует в избранном.");
            }

            var item = new Favorite();

            var lastitem = _context.Users.OrderBy(u => u.UserId).LastOrDefault();
            if (lastitem != null)
                item.FavoriteId = lastitem.UserId + 1;
            else
                item.FavoriteId = 1;

            item.FavoriteUser = userId;
            item.FavoriteCourse = courseId;
            _context.Favorite.Add(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Favorites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            var favorite = await _context.Favorite.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }

            _context.Favorite.Remove(favorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favorite.Any(e => e.FavoriteId == id);
        }
    }
}
