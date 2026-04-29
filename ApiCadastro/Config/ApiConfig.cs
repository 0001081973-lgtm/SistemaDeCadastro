namespace ApiCadastro.Config
{
    /// <summary>
    /// Classe de configuracao para a API, contendo propriedades que podem ser definidas
    /// no arquivo appsettings.json ou em variaveis de ambiente. Neste exemplo, temos a
    /// propriedade MaxCadastrosPorMinuto, que pode ser usada para definir um limite maximo
    /// de cadastros aceitos por minuto pela API.
    /// 
    /// </summary>
    public class ApiConfig
    {
        public int MaxCadastrosPorMinuto { get; set; }
    }
}
