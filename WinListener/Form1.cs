using Microsoft.AspNetCore.SignalR.Client;

namespace WinListener
{
    public partial class Form1 : Form
    {
        private HubConnection _connection;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var uri = "http://localhost:5000/notificationhub"; 

            _connection = new HubConnectionBuilder()
                .WithUrl(uri)
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                this.Invoke((Action)(() =>
                {
                    textBox1.AppendText($"Nova postagem de {user}: {message}{Environment.NewLine}");
                }));
            });

            try
            {
                await _connection.StartAsync();
                textBox1.AppendText("Conectado ao Hub SignalR. Aguardando notificações..." + Environment.NewLine);
            }
            catch (Exception ex)
            {
                textBox1.AppendText($"Erro ao conectar: {ex.Message}" + Environment.NewLine);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connection != null)
            {
                _connection.DisposeAsync();
            }
        }
    }
}
