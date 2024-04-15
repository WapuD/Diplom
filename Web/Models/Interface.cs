using API.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Web.Models
{
    public interface InterfaceClient
    {
        //Users
        [Get("/users")]
        Task<IEnumerable<User>> GetAllUsers();
        [Get("/users/{id}")]
        Task<User> GetUser(int id);
        [Get("/users/authors")]
        Task<IEnumerable<User>> GetUserAuthors();
        [Post("/users")]
        Task<UserDTO> CreateUser(UserDTO user);
        [Put("/users")]
        Task UpdateUser(UserProfileDTO user);

        //Courses
        [Get("/courses/MyCourses")]
        Task<IEnumerable<Course>> GetCourses(int userId);
        [Get("/courses")]
        Task<IEnumerable<Course>> GetAllCourses();
        [Get("/courses/{id}")]
        Task<Course> GetCourse(int id);
        [Get("/courses/{id}")]
        Task<Course> DeleteCourse(int id);
        [Get("/courses")]
        Task<Course> CreateCourse(CourseDTO course);

        //Categories
        [Get("/categories")]
        Task<IEnumerable<Category>> GetAllCategories();
        [Get("/categories/{id}")]
        Task<Category> GetCategory(int id);

        //Roles
        [Get("/roles")]
        Task<IEnumerable<Role>> GetAllRoles();
        [Get("/roles/{id}")]
        Task<Role> GetRole(int id);

        //Images
        [Get("/images")]
        Task<IEnumerable<Image>> GetAllImages();
        [Get("/images/byte/{id}")]
        Task<string> GetImageByte(int id);

        //Favorites
        [Get("/favorites/{userId}")]
        Task<IEnumerable<Course>> GetFavorite(int userId);
        [Post("/favorites")]
        Task AddToFavorite(int userId, int courseId);

        /*[Post("/users")]
        Task<Users> CreateUser([Body] Users user);
        [Post("/tasks")]
        Task<Tasks> CreateTask([Body] Tasks task);

        [Put("/users/{id}")]
        Task RedactUser(int id, [Body] Users user);
        [Put("/tasks/{id}")]
        Task RedactTask(int id, [Body] Tasks task);

        [Delete("/users/{id}")]
        Task DeleteUser(int id);
        [Delete("/tasks/{id}")]
        Task DeleteTask(int id);

        [Post("/users/authorization")]
        Task<string> Authorize([Body] UserDTO user);*/
    }
}