using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly ILogger<MatriculaController> _logger;
        private readonly MatriculaService _matriculaService;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public MatriculaController(ILogger<MatriculaController> logger, MatriculaService matriculaService, IMapper mapper)
        {
            _logger = logger;
            _matriculaService = matriculaService;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> CrearMatricula([FromBody] MatriculaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid || createDto == null)
                {
                    _logger.LogWarning("Datos inválidos para crear matrícula.");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                await _matriculaService.CrearMatriculaAsync(createDto.EstudianteId, createDto.CursoId, createDto.Estado);

                _logger.LogInformation($"Matrícula creada: EstudianteId={createDto.EstudianteId}, CursoId={createDto.CursoId}");
                _response.Resultado = createDto;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(GetMatricula), new { id = createDto.EstudianteId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear matrícula.");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }

        [HttpPut("estado/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                await _matriculaService.CambiarEstadoAsync(id, nuevoEstado);
                _logger.LogInformation($"Estado de matrícula {id} cambiado a {nuevoEstado}.");
                _response.StatusCode = HttpStatusCode.OK;
                _response.Resultado = $"Estado actualizado a {nuevoEstado}";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar estado de la matrícula con ID {id}.");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> EliminarMatricula(int id)
        {
            try
            {
                await _matriculaService.EliminarMatriculaAsync(id);
                _logger.LogInformation($"Matrícula con ID {id} eliminada.");
                _response.StatusCode = HttpStatusCode.OK;
                _response.Resultado = $"Matrícula con ID {id} eliminada correctamente";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar la matrícula con ID {id}.");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetMatricula(int id)
        {
            try
            {
                var matricula = await _matriculaService.ObtenerMatriculaPorIdAsync(id);
                if (matricula == null)
                {
                    _logger.LogWarning($"Matrícula con ID {id} no encontrada.");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _logger.LogInformation($"Matrícula obtenida con ID {id}.");
                _response.Resultado = _mapper.Map<MatriculaDto>(matricula);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la matrícula con ID {id}.");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }
    }
}
