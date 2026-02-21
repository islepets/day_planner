using System;
using System.Runtime.InteropServices;
using Ежедневник;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class MPanel : Panel
{
    private Form parent;

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 2;

    public MPanel(int width, Form form, string title)
    {
        parent = form;

        this.Height = 40;
        this.Dock = DockStyle.Top;
        this.BackColor = ColorTranslator.FromHtml("#434B4D");

        Label lb = CreateLabel(title, 10, 8);
        Button bt = CreateButton(width);

        bt.Click += CloseButton;

        this.Controls.Add(bt);
        this.Controls.Add(lb);

        lb.MouseDown += Panel_MouseDown;
        this.MouseDown += Panel_MouseDown;
    }

    private void Panel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(parent.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
    }

private Label CreateLabel(string text, int pos_x, int pos_y)
    {
        Label testLabel = new Label();
        testLabel.Text = text;
        testLabel.ForeColor = Color.White;
        testLabel.Font = new Font("Arial", 12, FontStyle.Bold);
        testLabel.Location = new Point(pos_x, pos_y);
        testLabel.AutoSize = true;

        return testLabel;
    }
    private Button CreateButton(int width)
    {
        Button button = new Button();
        button.Text = "X";
        button.ForeColor = Color.White;
        button.Location = new Point(width - 30, 0);
        button.Size = new Size(30, 40);
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 2;

        button.MouseEnter += (s, e) =>
        {
            button.BackColor = Color.Red;
            button.ForeColor = Color.Black;
            button.FlatAppearance.BorderColor = Color.Black;
        };

        button.MouseLeave += (s, e) =>
        {
            button.BackColor = ColorTranslator.FromHtml("#434B4D");
            button.ForeColor = Color.White;
            button.FlatAppearance.BorderColor = Color.White;
        };

        return button;
    }
    public void CloseButton(object sender, EventArgs e)
    {
        parent.Close();
    }

}
