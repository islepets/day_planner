using System;
using System.Windows.Forms.VisualStyles;

public class Task : Form
{
    private bool checkBoxStatus = false;
    public Task()
    {
        this.Icon = Ежедневник.Properties.Resources.diary_icon;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(500, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = ColorTranslator.FromHtml("#374785");

        MPanel panel = new MPanel(500, this, "Создание задачи");
        this.Controls.Add(panel);

        GroupBox gb = CreateGroupBox();
        this.Controls.Add(gb);
    }

    private GroupBox CreateGroupBox()
    {
        GroupBox gb = new GroupBox();
        gb.Text = "Задача";
        gb.Font = new Font("Arial", 12, FontStyle.Bold);
        gb.BackColor = ColorTranslator.FromHtml("#A8D0E6");
        gb.ForeColor = ColorTranslator.FromHtml("#24305E");
        gb.Location = new Point(55, 70);
        gb.Size = new Size(400, 400);

        ComboBox cb = CreateComboBox(10, 30, 160, 25);
        cb.Enabled = false;
        gb.Controls.Add(cb);

        CheckBox chb = CreateCheckBox(200, 35, 170, 25);
        gb.Controls.Add(chb);

        TextBox txt = CreateTextBox(10, 100, 380, 25);
        gb.Controls.Add(txt);

        Label lbl = CreateLabel(10, 75, 180, 25, "Заголовок");
        gb.Controls.Add(lbl);

        Label lbl1 = CreateLabel(10, 140, 180, 25, "Описание");
        gb.Controls.Add(lbl1);

        RichTextBox rtb = CreateRichTextBox(10, 170, 380, 150);
        gb.Controls.Add(rtb);

        Button b1 = CreateButton(10, 330, 190, 40, "Заметка");
        gb.Controls.Add(b1);

        Button b2 = CreateButton(200, 330, 190, 40, "Готово");
        gb.Controls.Add(b2);

        return gb;
    }

    public static Button CreateButton(int x, int y, int width, int height, string content)
    {
        Button b = new Button();
        b.Font = new Font("Arial", 12, FontStyle.Bold);
        b.Location = new Point(x, y);
        b.Size = new Size(width, height);
        b.Text = content;
        b.BackColor = ColorTranslator.FromHtml("#FF6B6B");

        b.MouseEnter += (s, e) =>
        {
            b.BackColor = ColorTranslator.FromHtml("#A62323");

        };

        b.MouseLeave += (s, e) =>
        {
            b.BackColor = ColorTranslator.FromHtml("#FF6B6B");
        };

        return b;
    }
    public static Label CreateLabel(int x, int y, int width, int height, string str)
    {
        Label label = new Label();
        label.Font = new Font("Arial", 12, FontStyle.Bold);
        label.Location = new Point(x, y);
        label.Size = new Size(width, height);
        label.Text = str;
        return label;
    }

    public static TextBox CreateTextBox(int x, int y, int width, int height)
    {
        TextBox txt = new TextBox();
        txt.Font = new Font("Arial", 12, FontStyle.Bold);
        txt.Location = new Point(x, y);
        txt.Size = new Size(width, height);
        return txt;
    }

    public CheckBox CreateCheckBox(int x, int y, int width, int height)
    {
        CheckBox chb = new CheckBox();
        chb.Font = new Font("Arial", 12, FontStyle.Bold);
        chb.Text = "Уведомление";
        chb.Location = new Point(x, y);
        chb.Size = new Size(width, height);
        checkBoxStatus = chb.Checked;

        return chb;
    }

    public static ComboBox CreateComboBox(int x, int y, int width, int height)
    {
        ComboBox cb = new ComboBox();
        cb.Font = new Font("Arial", 12, FontStyle.Bold);
        cb.ForeColor = ColorTranslator.FromHtml("#24305E");
        cb.Location = new Point(x, y);
        cb.Size = new Size(width, height);
        cb.DropDownStyle = ComboBoxStyle.DropDownList;
        cb.Items.Add("✅ Готово");
        cb.Items.Add("❌ Не готово");
        cb.SelectedIndex = 1;

        return cb;
    }

    public static RichTextBox CreateRichTextBox(int x, int y, int width, int height)
    {
        RichTextBox rtb = new RichTextBox();
        rtb.Font = new Font("Arial", 12, FontStyle.Bold);
        rtb.Location = new Point(x, y);
        rtb.Size = new Size(width, height);
        rtb.BackColor = ColorTranslator.FromHtml("#F7F9FB");
        return rtb;
    }
}
