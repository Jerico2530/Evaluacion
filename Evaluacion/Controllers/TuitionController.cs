using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TuitionController : ControllerBase
    {
        private readonly ILogger<TuitionController> _logger;
        private readonly ITuitionRepository _tuiRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public TuitionController(ILogger<TuitionController> logger, ITuitionRepository tuiRepo, IMapper mapper)
        {
            _logger = logger;
            _tuiRepo = tuiRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetAllTuitions()
        {
            try
            {
                _logger.LogInformation("Retrieving all tuitions");

                IEnumerable<Tuition> tuitionList = await _tuiRepo.GettAll();
                _response.Result= _mapper.Map<IEnumerable<TuitionDto>>(tuitionList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tuitions");
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetTuition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTuition(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("Invalid tuition ID: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessfull= false;
                    return BadRequest(_response);
                }

                var tuition = await _tuiRepo.Gett(v => v.TuitionId == id);
                if (tuition == null)
                {
                    _logger.LogWarning("Tuition with ID {id} not found.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessfull= false;
                    return NotFound(_response);
                }

                _response.Result= _mapper.Map<TuitionDto>(tuition);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tuition with ID {id}", id);
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateTuition([FromBody] TuitionCreateDto createDto)
        {
            var response = new ApiResponse();

            try
            {
                if (createDto == null)
                {
                    return BadRequest("The createDto object cannot be null.");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model when trying to create a tuition.");
                    return BadRequest(ModelState);
                }

                var existingTuition = await _tuiRepo.Gett(v => v.StudentId == createDto.StudentId && v.CourseId == createDto.CourseId);
                if (existingTuition != null)
                {
                    ModelState.AddModelError("Tuition", "This student is already enrolled in the selected course.");
                    _logger.LogWarning("Student ID {StudentId} is already enrolled in Course ID {CourseId}.", createDto.StudentId, createDto.CourseId);
                    return BadRequest(ModelState);
                }

                var (resultCode, message) = await _tuiRepo.CreateMatriculaAsync(
                    createDto.StudentId, createDto.CourseId, createDto.StateId);

                if (resultCode != 0)
                {
                    ModelState.AddModelError("StoredProcedure", message);
                    _logger.LogWarning("Stored procedure error: {Message}", message);
                    return BadRequest(ModelState);
                }

                response.IsSuccessfull= true;
                response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("Tuition successfully created via stored procedure for StudentId {StudentId} and CourseId {CourseId}.", createDto.StudentId, createDto.CourseId);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tuition");
                response.IsSuccessfull= false;
                response.ErrorMessages= new List<string>() { ex.ToString() };
                response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> DeleteTuition(int id)
        {
            try
            {
                var tuition = await _tuiRepo.Gett(v => v.TuitionId == id);
                if (tuition == null)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages= new List<string> { "Tuition not found." };
                    return NotFound(_response);
                }

                var (resultCode, message) = await _tuiRepo.EliminarMatriculaAsync(id);

                if (resultCode != 0)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages= new List<string> { message };
                    return BadRequest(_response);
                }

                _response.IsSuccessfull= true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result= "The tuition was successfully deleted.";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tuition with ID {id}", id);
                _response.IsSuccessfull= false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages= new List<string> { ex.ToString() };
                return StatusCode(500, _response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> UpdateTuition(int id, [FromBody] TuitionUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.TuitionId)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages= new List<string> { "The object is null or ID mismatch." };
                    return BadRequest(_response);
                }

                if (!ModelState.IsValid)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                var (resultCode, message) = await _tuiRepo.ActualizarEstadoMatriculaAsync(updateDto.TuitionId, updateDto.StateId);

                if (resultCode != 0)
                {
                    _response.IsSuccessfull= false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages= new List<string> { message };
                    return BadRequest(_response);
                }

                _response.IsSuccessfull= true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = message;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTuition");
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(500, _response);
            }
        }
    }
}
