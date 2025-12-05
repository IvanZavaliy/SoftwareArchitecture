using DemoAPI.Data;
using DemoAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<StudentsController> _logger;

    // Впровадження залежностей (Dependency Injection) для БД та Логера
    public StudentsController(ApplicationDbContext context, ILogger<StudentsController> logger)
    {
        _db = context;
        _logger = logger;
    }

    // 1. GET: Отримати всіх студентів
    [HttpGet]
    [Route("GetStudents")]
    public IActionResult GetStudents()
    {
        _logger.LogInformation("Fetching all student list"); // Логування
        var students = _db.StudentRegister.ToList();
        return Ok(students);
    }

    // 2. GET: Отримати студента за ID
    [HttpGet("GetStudentById")]
    public IActionResult GetStudentById(int id)
    {
        if (id == 0)
        {
            _logger.LogError("Student Id was not passed");
            return BadRequest();
        }

        var student = _db.StudentRegister.FirstOrDefault(x => x.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    // 3. POST: Додати нового студента
    [HttpPost]
    public IActionResult AddStudent([FromBody] StudentEntity studentDetails)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Додавання запису
        _db.StudentRegister.Add(studentDetails);
        // Збереження змін (Commit)
        _db.SaveChanges();

        return Ok(studentDetails);
    }

    // 4. PUT: Оновити існуючого студента
    [HttpPut]
    public IActionResult UpdateStudent(int id, [FromBody] StudentEntity studentDetails)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingStudent = _db.StudentRegister.FirstOrDefault(x => x.Id == id);

        if (existingStudent == null)
        {
            return NotFound();
        }

        // Оновлення полів
        existingStudent.Name = studentDetails.Name;
        existingStudent.Age = studentDetails.Age;
        existingStudent.Standard = studentDetails.Standard;
        existingStudent.EmailAddress = studentDetails.EmailAddress;

        _db.SaveChanges();

        return Ok(existingStudent);
    }

    // 5. DELETE: Видалити студента
    [HttpDelete]
    public IActionResult DeleteStudent(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var student = _db.StudentRegister.FirstOrDefault(x => x.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        _db.StudentRegister.Remove(student);
        _db.SaveChanges();

        return Ok("Student Deleted Successfully"); // У відео повертається Ok або NoContent
    }
}