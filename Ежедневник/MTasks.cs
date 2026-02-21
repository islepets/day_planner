using System;

public class MTasks : Form
{
    public string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

    public MTasks()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(1450, 800);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = ColorTranslator.FromHtml("#374785");

        for (int i = 0; i < days.Length; i++)
        {
            GroupBox gb = CreateGroupBox(i);
            this.Controls.Add(gb);
        }

        MPanel panel = new MPanel(1450, this, "Задачи - Проектирование программных систем");
        this.Controls.Add(panel);
    }

    private GroupBox CreateGroupBox(int iter)
    {
        GroupBox gb = new GroupBox();
        gb.Text = days[iter];
        gb.Font = new Font("Arial", 12, FontStyle.Bold);
        gb.BackColor = ColorTranslator.FromHtml("#A8D0E6");
        gb.ForeColor = ColorTranslator.FromHtml("#24305E");
        if (iter < 4)
        {
            gb.Location = new Point(55 + 340 * iter, 70);
            gb.Size = new Size(300, 300);
        }
        else
        {
            gb.Location = new Point(220 + 340 * (iter - 4), 440);
            gb.Size = new Size(300, 300);
        }

        RichTextBox rtb = CreateRichTextBox(10, 30, 280, 250);
        gb.Controls.Add(rtb);

        return gb;
    }


    public static RichTextBox CreateRichTextBox(int x, int y, int width, int height)
    {
        RichTextBox rtb = new RichTextBox();
        rtb.Location = new Point(x, y);
        rtb.Size = new Size(width, height);
        rtb.BackColor = ColorTranslator.FromHtml("#F7F9FB");
        return rtb;
    }
}
