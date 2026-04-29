namespace CadastroInterface.DTO
{
    // DTO usado para exibir os dados do cadastro na interface
    // desacopla a tela do modelo de dados do Shared
    public class CadastroDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}