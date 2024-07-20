using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend_examen.Data;
using backend_examen.Models;

namespace backend_examen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioData _usuarioData;
        public UsuarioController(UsuarioData usuarioData)
        {
            _usuarioData = usuarioData;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Usuario> Lista = await _usuarioData.Lista();
            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            Usuario objeto = await _usuarioData.Obtener(id);
            return StatusCode(StatusCodes.Status200OK, objeto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody]Usuario objeto)
        {
            bool respuesta= await _usuarioData.Crear(objeto);
            return StatusCode(StatusCodes.Status200OK, new {isSuccess = respuesta});
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Usuario objeto)
        {
            bool respuesta = await _usuarioData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _usuarioData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            bool usuarioExiste = await _usuarioData.VerificarUsuarioLogin(request.Login, request.Password);

            if (usuarioExiste)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Usuario encontrado con éxito" });
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Usuario no encontrado" });
            }
        }

    }


}

