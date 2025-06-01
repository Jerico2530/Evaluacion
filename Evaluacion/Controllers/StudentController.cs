using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepo, IMapper mapper)
        {
            _logger = logger;
            _studentRepo = studentRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetStudent()
        {
            try
            {
                _logger.LogInformation("Getting all Students");

                IEnumerable<Student> studentList = await _studentRepo.GettAll();
                _response.Result= _mapper.Map<IEnumerable<StudentDto>>(studentList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Students: {ex.Message}", ex);
                _response.IsSuccessfull = false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("Invalid Student ID: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }

                var student = await _studentRepo.Gett(v => v.StudentId == id);
                if (student == null)
                {
                    _logger.LogWarning("Student with ID {id} not found.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessfull = false;
                    return NotFound(_response);
                }

                _response.Result= _mapper.Map<StudentDto>(student);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving Student with ID {id}: {ex.Message}", ex);
                _response.IsSuccessfull = false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateStudent([FromBody] StudentCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model when trying to create a Student.");
                    return BadRequest(ModelState);
                }

                if (await _studentRepo.Gett(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Student", "A Student with that Name already exists!");
                    _logger.LogWarning("Student with Name {Name} already exists.", createDto.Name);
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Student model = _mapper.Map<Student>(createDto);
                await _studentRepo.Create(model);
                _response.Result= model;
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("Student created successfully: {StudentId}", model.StudentId);
                return CreatedAtRoute("GetStudent", new { id = model.StudentId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating Student: {ex.Message}", ex);
                _response.IsSuccessfull = false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var student = await _studentRepo.Gett(v => v.StudentId == id);
                if (student == null)
                {
                    _response.IsSuccessfull = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _studentRepo.Remove(student);
                _response.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Student with ID {id} successfully deleted.", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Student with ID {id}: {ex.Message}", ex);
                _response.IsSuccessfull= false;
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.StudentId)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Student model = _mapper.Map<Student>(updateDto);
            await _studentRepo.ActualizarStudent(model);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

    }
}
