using ApiCadastro.Config;
using ApiCadastro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared;

namespace ApiCadastro.Controllers
{
    [ApiController]
    [Route("api/v1/cadastros")]
    public class CadastroController : ControllerBase
    {
        private readonly CadastroDbContext _db;
        private readonly IOptions<ApiConfig> _config;

        public CadastroController(CadastroDbContext db, IOptions<ApiConfig> config)
        {
            _db = db;
            _config = config;
        }

        // recebe um novo cadastro, gera notificacao e salva no banco
        [HttpPost]
        public async Task<IActionResult> Cadastrar(CadastroData cadastro)
        {
            if (string.IsNullOrEmpty(cadastro.Nome))
            {
                return BadRequest("Nome nao pode ser vazio!");
            }

            if (string.IsNullOrEmpty(cadastro.Email))
            {
                return BadRequest("Email nao pode ser vazio!");
            }

            cadastro.Timestamp = DateTime.Now;
            _db.Cadastros.Add(cadastro);
            await _db.SaveChangesAsync();

            // gera a notificacao automaticamente apos o cadastro
            var notificacao = new NotificacaoData
            {
                CadastroId = cadastro.Id,
                NomeDestinatario = cadastro.Nome,
                Email = cadastro.Email,
                Mensagem = $"Bem-vindo(a) {cadastro.Nome}! Seu cadastro foi realizado com sucesso.",
                Status = "Enviado",
                Timestamp = DateTime.Now
            };
            _db.Notificacoes.Add(notificacao);
            await _db.SaveChangesAsync();

            return Ok(cadastro);
        }

        // lista todos os cadastros salvos
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var dados = await _db.Cadastros.ToListAsync();
            return Ok(dados);
        }

        // busca cadastro por id
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var cadastro = await _db.Cadastros.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound("Cadastro nao encontrado!");
            }
            return Ok(cadastro);
        }

        // atualiza um cadastro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, CadastroData atualizado)
        {
            var cadastro = await _db.Cadastros.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound("Cadastro nao encontrado!");
            }

            cadastro.Nome = atualizado.Nome;
            cadastro.Email = atualizado.Email;
            cadastro.Telefone = atualizado.Telefone;
            cadastro.Departamento = atualizado.Departamento;
            await _db.SaveChangesAsync();

            return Ok(cadastro);
        }

        // deleta um cadastro pelo id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var cadastro = await _db.Cadastros.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound("Cadastro nao encontrado!");
            }

            _db.Cadastros.Remove(cadastro);
            await _db.SaveChangesAsync();
            return Ok("Deletado com sucesso!");
        }
    }
}
