using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class NotificacaoData
    {
        public int Id { get; set; }
        public int CadastroId { get; set; }
        public string NomeDestinatario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
