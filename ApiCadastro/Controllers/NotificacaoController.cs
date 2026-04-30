using ApiCadastro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCadastro.Controllers
{
    [ApiController]
    [Route("api/v1/notificacoes")]
    public class NotificacaoController : ControllerBase
    {
        private readonly CadastroDbContext _db;

        public NotificacaoController(CadastroDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Lista todas as notificacoes geradas pelo sistema.
        /// </summary>
        /// <returns>Retorna uma lista com todas as notificacoes</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var dados = await _db.Notificacoes.ToListAsync();
            return Ok(dados);
        }

        /// <summary>
        /// Busca todas as notificacoes vinculadas a um cadastro especifico.
        /// </summary>
        /// <param name="cadastroId">ID do cadastro para filtrar as notificacoes</param>
        /// <returns>Retorna a lista de notificacoes do cadastro informado</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        [HttpGet("por-cadastro/{cadastroId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> BuscarPorCadastro(int cadastroId)
        {
            var dados = await _db.Notificacoes
                .Where(n => n.CadastroId == cadastroId)
                .ToListAsync();

            return Ok(dados);
        }
    }
}
