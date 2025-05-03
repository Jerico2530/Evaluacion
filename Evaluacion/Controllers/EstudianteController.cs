using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    public class EstudianteController : Controller
    {
        private readonly ILogger<EstudianteController> _logger;
        private readonly IEstudianteRepositorio _estuRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public EstudianteController(ILogger<EstudianteController> logger, IEstudianteRepositorio estuRepo, IMapper mapper)
        {
            _logger = logger;
            _estuRepo = estuRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetEstudiante()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los estudiantes");

                IEnumerable<Estudiante> areaList = await _estuRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<EstudianteDto>>(areaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener los estudiantes: {ex.Message}", ex);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }

        }


        [HttpGet("{id:int}", Name = "GetEstudiante")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetEstudiante(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("ID de estudiantes inválido: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Estudiante = await _estuRepo.Obtener(v => v.EstudianteId == id);
                if (Estudiante == null)
                {
                    _logger.LogWarning("Estudiantes con ID {id} no encontrado.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudianteDto>(Estudiante);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el estudiantes con ID {id}: {ex.Message}", ex);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CrearEstudiante([FromBody] EstudianteCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Modelo no válido al intentar crear un estudiantes.");
                    return BadRequest(ModelState);
                }

                if (await _estuRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("Estudiante", "La Estudiante con ese nombre ya existe!");
                    _logger.LogWarning("El estudiantes con el nombre {Nombre} ya existe.", createDto.Nombre);
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Estudiante modelo = _mapper.Map<Estudiante>(createDto);



                await _estuRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("Estudiante creado con éxito: {EstudianteId}", modelo.EstudianteId);
                return CreatedAtRoute("GetEstudiante", new { id = modelo.EstudianteId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el estudiantes: {ex.Message}", ex);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }

        }



        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Estudiante = await _estuRepo.Obtener(v => v.EstudianteId == id);
                if (Estudiante == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _estuRepo.Remover(Estudiante);
                _response.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Estudiante con ID {id} eliminado con éxito.", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el estudiantes con ID {id}: {ex.Message}", ex);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }

        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEstudiante(int id, [FromBody] EstudianteUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.EstudianteId)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Estudiante modelo = _mapper.Map<Estudiante>(updateDto);

            await _estuRepo.ActualizarEstudiante(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialEstudiante(int id, JsonPatchDocument<EstudianteUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning("ID de estudiante  no válidos: {id}", id);
                return BadRequest();
            }

            var area = await _estuRepo.Obtener(v => v.EstudianteId == id, tracked: false);

            EstudianteUpdateDto areaDto = _mapper.Map<EstudianteUpdateDto>(area);


            if (area == null) return BadRequest();

            patchDto.ApplyTo(areaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Estudiante model = _mapper.Map<Estudiante>(areaDto);

            await _estuRepo.ActualizarEstudiante(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _logger.LogInformation("Estudiante con ID {id} actualizado parcialmente con éxito.", id);
            return Ok(_response);
        }
    }
}

