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

        /// <summary>
        /// Cadastra um novo usuario no sistema e salva no banco de dados.
        /// </summary>
        /// <param name="dto">Dados do cadastro recebidos pela requisicao</param>
        /// <returns>Retorna o cadastro criado com o ID gerado</returns>
        /// <response code="200">Cadastro realizado com sucesso</response>
        /// <response code="400">Nome ou Email nao podem ser vazios</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Lista todos os cadastros salvos no banco de dados.
        /// </summary>
        /// <returns>Retorna uma lista com todos os cadastros</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar()
        {
            var dados = await _db.Cadastros.ToListAsync();
            return Ok(dados);
        }

        /// <summary>
        /// Busca um cadastro especifico pelo ID.
        /// </summary>
        /// <param name="id">ID do cadastro a ser buscado</param>
        /// <returns>Retorna o cadastro correspondente ao ID informado</returns>
        /// <response code="200">Cadastro encontrado com sucesso</response>
        /// <response code="404">Cadastro nao encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var cadastro = await _db.Cadastros.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound("Cadastro nao encontrado!");
            }
            return Ok(cadastro);
        }

        /// <summary>
        /// Atualiza os dados de um cadastro existente.
        /// </summary>
        /// <param name="id">ID do cadastro a ser atualizado</param>
        /// <param name="dto">Novos dados para atualizar o cadastro</param>
        /// <returns>Retorna o cadastro com os dados atualizados</returns>
        /// <response code="200">Cadastro atualizado com sucesso</response>
        /// <response code="404">Cadastro nao encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Remove um cadastro do banco de dados pelo ID.
        /// </summary>
        /// <param name="id">ID do cadastro a ser removido</param>
        /// <returns>Retorna mensagem de confirmacao da remocao</returns>
        /// <response code="200">Cadastro deletado com sucesso</response>
        /// <response code="404">Cadastro nao encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
