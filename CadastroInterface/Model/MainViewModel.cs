using CadastroInterface.Command;
using CadastroInterface.ViewModels;
using Microsoft.Data.Sqlite;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CadastroInterface.Model
{
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Atributos gerais de binding
        /// </summary>
        public ObservableCollection<CadastroData> Cadastros { get; set; }
        public ObservableCollection<NotificacaoData> Notificacoes { get; set; }

        private string _status = "";
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Comandos
        /// </summary>
        public ICommand CarregarCadastrosCommand { get; }
        public ICommand CarregarNotificacoesCommand { get; }

        public MainViewModel()
        {
            Cadastros = new ObservableCollection<CadastroData>();
            Notificacoes = new ObservableCollection<NotificacaoData>();

            //comandos
            CarregarCadastrosCommand = new RelayCommand(CarregarCadastros);
            CarregarNotificacoesCommand = new RelayCommand(CarregarNotificacoes);

            //cria o banco local da interface
            CriarBanco();
        }

        private async void CarregarCadastros()
        {
            Status = "Carregando cadastros...";
            var http = new HttpClient();
            var dados = await http.GetFromJsonAsync<List<CadastroData>>(
                "https://localhost:7210/api/v1/cadastros");

            Cadastros.Clear();
            foreach (var cadastro in dados)
            {
                Cadastros.Add(cadastro);
                SalvarLocal(cadastro);
            }

            Status = $"Total carregado: {dados.Count} registros";
        }

        private async void CarregarNotificacoes()
        {
            Status = "Carregando notificacoes...";
            var http = new HttpClient();
            var dados = await http.GetFromJsonAsync<List<NotificacaoData>>(
                "https://localhost:7210/api/v1/notificacoes");

            Notificacoes.Clear();
            foreach (var notificacao in dados)
            {
                Notificacoes.Add(notificacao);
            }

            Status = $"Total de notificacoes: {dados.Count}";
        }

        private void CriarBanco()
        {
            using var conn = new SqliteConnection("Data Source=interface_log.db");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS CadastroData (
                Id INTEGER PRIMARY KEY,
                Nome TEXT,
                Email TEXT,
                Telefone TEXT,
                Departamento TEXT,
                Timestamp TEXT
            )";
            cmd.ExecuteNonQuery();
        }

        private void SalvarLocal(CadastroData cadastro)
        {
            using var conn = new SqliteConnection("Data Source=interface_log.db");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT OR IGNORE INTO CadastroData (Id, Nome, Email, Telefone, Departamento, Timestamp)
                                VALUES ($id, $nome, $email, $telefone, $depto, $ts)";
            cmd.Parameters.AddWithValue("$id", cadastro.Id);
            cmd.Parameters.AddWithValue("$nome", cadastro.Nome);
            cmd.Parameters.AddWithValue("$email", cadastro.Email);
            cmd.Parameters.AddWithValue("$telefone", cadastro.Telefone);
            cmd.Parameters.AddWithValue("$depto", cadastro.Departamento);
            cmd.Parameters.AddWithValue("$ts", cadastro.Timestamp.ToString());
            cmd.ExecuteNonQuery();
        }
    }
}
