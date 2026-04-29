using Microsoft.EntityFrameworkCore;
using Shared;

namespace ApiCadastro.Data
{
    public class CadastroDbContext : DbContext
    {
        public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options)
        {
        }

        public DbSet<CadastroData> Cadastros { get; set; }
        public DbSet<NotificacaoData> Notificacoes { get; set; }
    }
}
