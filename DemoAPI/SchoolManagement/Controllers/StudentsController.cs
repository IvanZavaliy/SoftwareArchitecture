using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagement.Models;
using System.Text;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        // Адреса вашого API (змініть порт на ваш)
        private string _baseurl = "http://localhost:5150/api/Students/";

        // 1. GET: Список студентів
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<StudentEntity> students = new List<StudentEntity>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Виклик API: GET api/Students/GetStudents
                HttpResponseMessage response = await client.GetAsync("GetStudents");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    students = JsonConvert.DeserializeObject<List<StudentEntity>>(result);
                }
            }
            return View(students);
        }

        // 2. CREATE: Відображення форми
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. CREATE: Відправка даних (POST)
        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentEntity student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseurl);
                var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                
                // Виклик API: POST api/Students
                var response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Create");
        }

        // 4. DETAILS/EDIT: Отримання даних для форми
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            StudentEntity student = new StudentEntity();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseurl);
                // Виклик API: GET api/Students/GetStudentById?id=X
                var response = await client.GetAsync($"GetStudentById?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<StudentEntity>(result);
                }
            }
            return View(student);
        }

        // 5. EDIT: Оновлення даних (PUT)
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(StudentEntity student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseurl);
                var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");

                // Виклик API: PUT api/Students?id=X
                var response = await client.PutAsync($"?id={student.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Details", student);
        }

        // 6. DELETE: Видалення даних
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Для видалення ми показуємо ту ж форму Details, але в режимі ReadOnly (або окрему view)
            // Логіка отримання така ж як у Details
            return await Details(id); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseurl);
                // Виклик API: DELETE api/Students?id=X
                var response = await client.DeleteAsync($"?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}