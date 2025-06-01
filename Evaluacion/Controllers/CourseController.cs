using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseRepository _courseRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public CourseController(ILogger<CourseController> logger, ICourseRepository courseRepo, IMapper mapper)
        {
            _logger = logger;
            _courseRepo = courseRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetCourses()
        {
            try
            {
                _logger.LogInformation("Retrieving all courses");

                IEnumerable<Course> courseList = await _courseRepo.GettAll();
                _response.Result = _mapper.Map<IEnumerable<CourseDto>>(courseList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving courses");
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCourse(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("Invalid course ID: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessfull = false;
                    return BadRequest(_response);
                }
                var course = await _courseRepo.Gett(c => c.CourseId == id);

                if (course == null)
                {
                    _logger.LogWarning("Course with ID {id} not found.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessfull = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<CourseDto>(course);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course with ID {id}", id);
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateCourse([FromBody] CourseCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model when attempting to create a course.");
                    return BadRequest(ModelState);
                }

                if (await _courseRepo.Gett(c => c.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("Course", "A course with that name already exists.");
                    _logger.LogWarning("Course with the name {Name} already exists.", createDto.Name);
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Course model = _mapper.Map<Course>(createDto);

                await _courseRepo.Create(model);
                _response.Result = model;
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("Course created successfully: {CourseId}", model.CourseId);
                return CreatedAtRoute("GetCourse", new { id = model.CourseId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course");
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessfull = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var course = await _courseRepo.Gett(c => c.CourseId == id);
                if (course == null)
                {
                    _response.IsSuccessfull = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _courseRepo.Remove(course);
                _response.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Course with ID {id} deleted successfully.", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course with ID {id}", id);
                _response.IsSuccessfull = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.CourseId)
            {
                _response.IsSuccessfull= false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Course model = _mapper.Map<Course>(updateDto);

            await _courseRepo.ActualizarCourse(model);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartialUpdateCourse(int id, JsonPatchDocument<CourseUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning("Invalid course ID: {id}", id);
                return BadRequest();
            }

            var course = await _courseRepo.Gett(c => c.CourseId == id, tracked: false);
            CourseUpdateDto courseDto = _mapper.Map<CourseUpdateDto>(course);

            if (course == null) return BadRequest();

            patchDto.ApplyTo(courseDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Course model = _mapper.Map<Course>(courseDto);

            await _courseRepo.ActualizarCourse(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _logger.LogInformation("Course with ID {id} partially updated successfully.", id);
            return Ok(_response);
        }
    }
}
