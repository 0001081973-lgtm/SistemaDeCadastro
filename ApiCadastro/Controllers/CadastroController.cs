using ApiCadastro.Config;
using ApiCadastro.Data;
using ApiCadastro.DTO;
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

        // recebe o DTO, converte para a entidade e salva no banco
        [HttpPost]
        public async Task<IActionResult> Cadastrar(CadastroDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Nome))
            {
                return BadRequest("Nome nao pode ser vazio!");
            }

            if (string.IsNullOrEmpty(dto.Email))
            {
                return BadRequest("Email nao pode ser vazio!");
            }

            // converte o DTO para a entidade
            var cadastro = new CadastroData
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Departamento = dto.Departamento,
                Timestamp = DateTime.Now
            };

            _db.Cadastros.Add(cadastro);
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

        // atualiza um cadastro existente usando DTO
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, CadastroDTO dto)
        {
            var cadastro = await _db.Cadastros.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound("Cadastro nao encontrado!");
            }

            cadastro.Nome = dto.Nome;
            cadastro.Email = dto.Email;
            cadastro.Telefone = dto.Telefone;
            cadastro.Departamento = dto.Departamento;
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
