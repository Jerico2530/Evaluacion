using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using Evaluacion.Models;
using Evaluacion.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Serilog;

namespace Evaluacion.Controllers
{
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly ILogger<CursoController> _logger;
        private readonly ICursoRepositorio _cursRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public CursoController(ILogger<CursoController> logger, ICursoRepositorio cursRepo, IMapper mapper)
        {
            _logger = logger;
            _cursRepo = cursRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetCurso()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los cursos");

                IEnumerable<Curso> areaList = await _cursRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<CursoDto>>(areaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los cursos");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpGet("{id:int}", Name = "GetCurso")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetCurso(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogWarning("ID de curso inválido: {id}", id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Curso = await _cursRepo.Obtener(v => v.CursoId == id);
                if (Curso == null)
                {
                    _logger.LogWarning("Curso con ID {id} no encontrado.", id);
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<CursoDto>(Curso);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el curso con ID {id}", id);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CrearCurso([FromBody] CursoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Modelo no válido al intentar crear un curso.");
                    return BadRequest(ModelState);
                }

                if (await _cursRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("Curso", "El curso con ese nombre ya existe.");
                    _logger.LogWarning("El curso con el nombre {Nombre} ya existe.", createDto.Nombre);
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Curso modelo = _mapper.Map<Curso>(createDto);

                await _cursRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("Curso creado con éxito: {CursoId}", modelo.CursoId);
                return CreatedAtRoute("GetCurso", new { id = modelo.CursoId }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el curso");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Curso = await _cursRepo.Obtener(v => v.CursoId == id);
                if (Curso == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _cursRepo.Remover(Curso);
                _response.StatusCode = HttpStatusCode.NoContent;

                _logger.LogInformation("Curso con ID {id} eliminado con éxito.", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el curso con ID {id}", id);
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCurso(int id, [FromBody] CursoUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.CursoId)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Curso modelo = _mapper.Map<Curso>(updateDto);

            await _cursRepo.ActualizarCurso(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialCurso(int id, JsonPatchDocument<CursoUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning("ID de curso no válidos: {id}", id);
                return BadRequest();
            }

            var area = await _cursRepo.Obtener(v => v.CursoId == id, tracked: false);
            CursoUpdateDto areaDto = _mapper.Map<CursoUpdateDto>(area);

            if (area == null) return BadRequest();

            patchDto.ApplyTo(areaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Curso model = _mapper.Map<Curso>(areaDto);

            await _cursRepo.ActualizarCurso(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _logger.LogInformation("Curso con ID {id} actualizado parcialmente con éxito.", id);
            return Ok(_response);
        }
    }
}
