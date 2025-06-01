using AutoMapper;
using BiblotecaClase.Model.Dto;
using BiblotecaClase.Model;
using Evaluacion.Models;
using Evaluacion.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    public class StateTuitionController : Controller
    {
        private readonly ILogger<StateTuitionController> _logger;
        private readonly IStateTuitionRepository _tuitRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public StateTuitionController(ILogger<StateTuitionController> logger, IStateTuitionRepository tuitRepo, IMapper mapper)
        {
            _logger = logger;
            _tuitRepo = tuitRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetStateTuition()
        {
            try
            {
                _logger.LogInformation("Getting all StateTuitions");

                IEnumerable<StateTuition> areaList = await _tuitRepo.GettAll();
                _response.Result= _mapper.Map<IEnumerable<StateTuitionDto>>(areaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting StateTuitions");
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetStateTuition")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetStateTuition(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("Invalid StateTuition ID: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccessfull= false;
                    return BadRequest(_response);
                }
                var StateTuition = await _tuitRepo.Gett(v => v.StateId == id);
                if (StateTuition == null)
                {
                    _logger.LogWarning("StateTuition with ID {id} not found.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccessfull= false;
                    return NotFound(_response);
                }
                _response.Result= _mapper.Map<StateTuitionDto>(StateTuition);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting StateTuition with ID {id}", id);
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CreateStateTuition([FromBody] StateTuitionCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model when trying to create a StateTuition.");
                    return BadRequest(ModelState);
                }

                if (await _tuitRepo.Gett(v => v.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("StateTuition", "A StateTuition with that Name already exists.");
                    _logger.LogWarning("StateTuition with Name {Name} already exists.", createDto.Name);
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                StateTuition modelo = _mapper.Map<StateTuition>(createDto);

                await _tuitRepo.Create(modelo);
                _response.Result= modelo;
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("StateTuition created successfully: {StateTuitionId}", modelo.StateId);
                return CreatedAtRoute("GetStateTuition", new { id = modelo.StateId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating StateTuition");
                _response.IsSuccessfull= false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStateTuition(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccessfull = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var StateTuition = await _tuitRepo.Gett(v => v.StateId == id);
                if (StateTuition == null)
                {
                    _response.IsSuccessfull = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _tuitRepo.Remove(StateTuition);
                _response.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("StateTuition with ID {id} deleted successfully.", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting StateTuition with ID {id}", id);
                _response.IsSuccessfull = false;
                _response.ErrorMessages= new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStateTuition(int id, [FromBody] StateTuitionUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.StateId)
            {
                _response.IsSuccessfull = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            StateTuition modelo = _mapper.Map<StateTuition>(updateDto);

            await _tuitRepo.ActualizarStateTuition(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialStateTuition(int id, JsonPatchDocument<StateTuitionUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning("Invalid StateTuition ID: {id}", id);
                return BadRequest();
            }

            var area = await _tuitRepo.Gett(v => v.StateId == id, tracked: false);
            StateTuitionUpdateDto areaDto = _mapper.Map<StateTuitionUpdateDto>(area);

            if (area == null) return BadRequest();

            patchDto.ApplyTo(areaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            StateTuition model = _mapper.Map<StateTuition>(areaDto);

            await _tuitRepo.ActualizarStateTuition(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _logger.LogInformation("StateTuition with ID {id} partially updated successfully.", id);
            return Ok(_response);
        }
    }
}
