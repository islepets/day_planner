using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Ежедневник
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Font = new Font("Arial", 12, FontStyle.Bold);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#A8D0E6");

            MPanel mPanel = new MPanel(1000, this, "Главная - Проектирование программных систем");
            this.Controls.Add(mPanel);

            panel1.BackColor = ColorTranslator.FromHtml("#374785");
            panel1.Location = new Point(0, 40);
            panel1.Height = this.ClientSize.Height - 40;

            this.Icon = Properties.Resources.diary_icon;
            pictureBox2.Image = Properties.Resources.mainform;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + 40);

            button1 = BeautifulButton(button1);
            button2 = BeautifulButton(button2);
            button3 = BeautifulButton(button3);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MNotes notes = new MNotes();
            notes.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MTasks tasks = new MTasks();
            tasks.Show();
        }
        private Button BeautifulButton(Button button)
        {
            button.BackColor = ColorTranslator.FromHtml("#FF6B6B");

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ColorTranslator.FromHtml("#A62323");

            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = ColorTranslator.FromHtml("#FF6B6B");
            };

            return button;
        }
    }
}
