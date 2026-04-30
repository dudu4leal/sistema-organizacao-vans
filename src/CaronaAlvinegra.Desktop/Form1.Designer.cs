namespace CaronaAlvinegra.Desktop;

partial class Form1
{
    private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        webView = new Microsoft.Web.WebView2.WinForms.WebView2();
        ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
        SuspendLayout();

        // webView
        webView.AllowExternalDrop = true;
        webView.CreationProperties = null;
        webView.DefaultBackgroundColor = Color.White;
        webView.Dock = DockStyle.Fill;
        webView.Location = new Point(0, 0);
        webView.Name = "webView";
        webView.Size = new Size(1200, 800);
        webView.TabIndex = 0;
        webView.ZoomFactor = 1.0;

        // Form1
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 800);
        Controls.Add(webView);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Carona Alvinegra — Gestor de Logística";
        WindowState = FormWindowState.Maximized;

        ((System.ComponentModel.ISupportInitialize)webView).EndInit();
        ResumeLayout(false);
    }
}
