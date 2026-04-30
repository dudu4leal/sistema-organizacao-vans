using Microsoft.Web.WebView2.Core;

namespace CaronaAlvinegra.Desktop;

public partial class Form1 : Form
{
    private readonly string _apiUrl;

    public Form1(string apiUrl)
    {
        _apiUrl = apiUrl;
        InitializeComponent();
        Load += Form1_Load;
    }

    private async void Form1_Load(object? sender, EventArgs e)
    {
        Text = $"Carona Alvinegra — Gestor de Logística";

        try
        {
            // Inicializa o ambiente WebView2
            await webView.EnsureCoreWebView2Async(null);

            // Habilita console de depuração (F12)
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;

            // Navega para a aplicação web (Swagger UI)
            webView.CoreWebView2.Navigate(_apiUrl);
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(
                $"Erro ao inicializar WebView2: {ex.Message}\n\n" +
                $"Certifique-se de que o WebView2 Runtime está instalado.\n" +
                $"Baixe em: https://developer.microsoft.com/pt-br/microsoft-edge/webview2/",
                "Erro",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        webView?.Dispose();
        base.OnFormClosing(e);
    }
}
