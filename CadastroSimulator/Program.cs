using Microsoft.Data.Sqlite;
using Shared;
using System.Net.Http.Json;

var http = new HttpClient();
int index = 1;

//cria o banco local do simulador se nao existir
CriarBanco();

var nomes = new List<string> { "Ana Lima", "Carlos Souza", "Fernanda Oliveira", "Pedro Alves", "Juliana Costa",
                                "Rafael Mendes", "Mariana Rocha", "Bruno Ferreira", "Camila Dias", "Lucas Martins" };
var departamentos = new List<string> { "TI", "RH", "Financeiro", "Comercial", "Logistica", "Producao" };

while (true)
{
    var rng = new Random();
    var nome = nomes[rng.Next(nomes.Count)];
    var depto = departamentos[rng.Next(departamentos.Count)];

    var cadastro = new CadastroData
    {
        Id = index,
        Nome = nome,
        Email = $"{nome.ToLower().Replace(" ", ".")}@empresa.com",
        Telefone = $"(31) 9{rng.Next(1000, 9999)}-{rng.Next(1000, 9999)}",
        Departamento = depto,
        Timestamp = DateTime.Now
    };

    // salva localmente antes de enviar
    SalvarLocal(cadastro);

    var response = await http.PostAsJsonAsync(
        "https://localhost:7210/api/v1/cadastros", cadastro);

    if (!response.IsSuccessStatusCode)
    {
        var erro = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Erro: {response.StatusCode} - {erro}");
    }
    else
    {
        Console.WriteLine($"Cadastrado: {cadastro.Nome} | {cadastro.Departamento} | {cadastro.Email}");
    }

    await Task.Delay(2000);
    index++;
}

void CriarBanco()
{
    using var conn = new SqliteConnection("Data Source=simulator_log.db");
    conn.Open();
    var cmd = conn.CreateCommand();
    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS CadastroData (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Nome TEXT,
        Email TEXT,
        Telefone TEXT,
        Departamento TEXT,
        Timestamp TEXT
    )";
    cmd.ExecuteNonQuery();
}

void SalvarLocal(CadastroData cadastro)
{
    using var conn = new SqliteConnection("Data Source=simulator_log.db");
    conn.Open();
    var cmd = conn.CreateCommand();
    cmd.CommandText = @"INSERT INTO CadastroData (Nome, Email, Telefone, Departamento, Timestamp)
                        VALUES ($nome, $email, $telefone, $depto, $ts)";
    cmd.Parameters.AddWithValue("$nome", cadastro.Nome);
    cmd.Parameters.AddWithValue("$email", cadastro.Email);
    cmd.Parameters.AddWithValue("$telefone", cadastro.Telefone);
    cmd.Parameters.AddWithValue("$depto", cadastro.Departamento);
    cmd.Parameters.AddWithValue("$ts", cadastro.Timestamp.ToString());
    cmd.ExecuteNonQuery();
}
