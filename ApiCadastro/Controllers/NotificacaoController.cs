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

        // lista todas as notificacoes geradas
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var dados = await _db.Notificacoes.ToListAsync();
            return Ok(dados);
        }

        // busca notificacoes de um cadastro especifico
        [HttpGet("por-cadastro/{cadastroId}")]
        public async Task<IActionResult> BuscarPorCadastro(int cadastroId)
        {
            var dados = await _db.Notificacoes
                .Where(n => n.CadastroId == cadastroId)
                .ToListAsync();

            return Ok(dados);
        }
    }
}
