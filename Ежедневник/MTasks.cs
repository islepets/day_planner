using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Ежедневник;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

public class MTasks : Form
{
    private Panel BigPanel;
    private Button button1;
    private Button button;
    private Button button2;

    private List<int> taskIds;
    public MTasks()
    {
        this.Icon = Ежедневник.Properties.Resources.diary_icon;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = ColorTranslator.FromHtml("#A8D0E6");
            
        MPanel panel = new MPanel(1000, this, "Задачи - Проектирование программных систем");
        this.Controls.Add(panel);

        Panel pnl = MNotes.CreatePanel(0, 0, 300, 600, "#374785");
        button = Task.CreateButton(10, 500, 275, 48, "Создать");
        button.Click += CreateTask;

        button1 = Task.CreateButton(10, 550, 275, 48, "Редактировать");
        button1.Click += EditTask;

        button2 = Task.CreateButton(10, 550, 275, 48, "Сохранить");
        button2.Click += SaveTask;
        button2.Visible = false;

        pnl.Controls.Add(button);
        pnl.Controls.Add(button1);
        pnl.Controls.Add(button2);
        this.Controls.Add(pnl);

        BigPanel = CreateDynamicPanel(350, 50, 610, 540, "#FFFFFF");
        this.Controls.Add(BigPanel);  
    }

    private void SaveAllTasksData()
    {
        DatabaseHelper bd = new DatabaseHelper();


        int i = 0;
        foreach (Control control in BigPanel.Controls)
        {
            if (control is GroupBox gb)
            {
                string title = "";
                int status = 1;
                string date = "";
                string time = "";
                string description = "";

                foreach (Control innerControl in gb.Controls)
                {
                    if (innerControl is TextBox txt)
                    {
                        if (txt.Location.Y == 20) // Заголовок
                            title = txt.Text;
                        else if (txt.Location.Y == 90) // Дата
                            date = txt.Text;
                        else if (txt.Location.Y == 125) // Время
                            time = txt.Text;
                    }
                    else if (innerControl is ComboBox cmb)
                    {
                        // Статус
                        status = cmb.SelectedIndex == 0 ? 1 : 2;
                    }
                    else if (innerControl is RichTextBox rtb)
                    {
                        // Описание
                        description = rtb.Text;
                    }
                }

                bd.UpdateTask(taskIds[i], title, status, date, time, description);
                i++;
            }
        }
    }

    public void SaveTask(object sender,  EventArgs e)
    {
        button1.Visible = true;
        button.Visible = true;
        button2.Visible = false;
        FillAllControls(BigPanel, false);
        SaveAllTasksData();
    }

    public void FillAllControls(Control parent, bool enabled)
    {
        foreach (Control control in parent.Controls)
        {
            control.Enabled = enabled;
            FillAllControls(control, true);
        }
    }

    public void EditTask(object sender,  EventArgs e)
    {
        button1.Visible = false;
        button.Visible = false;
        button2.Visible = true;
        FillAllControls(BigPanel, true);
    }

    public void CreateTask(object sender, EventArgs e)
    {
        Task task = new Task();
        task.Show();
    }

    public Panel CreateDynamicPanel(int x, int y, int width, int height, string color)
    {
        Panel panel = new Panel();
        panel.Location = new Point(x, y);
        panel.Size = new Size(width, height);
        panel.BackColor = ColorTranslator.FromHtml(color);
        panel.AutoScroll = true;

        DatabaseHelper bd = new DatabaseHelper();

        taskIds = bd.GetAllTaskIds();

        int margin = 10;
        int gbWidth = 270;
        int gbHight = 280;

        for(int i = 0; i < taskIds.Count; i++)
        {

            int taskId = taskIds[i];

            GroupBox gb = new GroupBox();
            gb.BackColor = ColorTranslator.FromHtml("#A8D0E6");

            TextBox txtTitle = CreateTaskTextBox(taskId, 10, 20, 250, 50, 10, "Заголовок");
            gb.Controls.Add(txtTitle);

            ComboBox cmb = CreateTaskComboBox(taskId, 10, 55, 250, 50, 10, "Статус");
            gb.Controls.Add(cmb);

            TextBox txtDate = CreateTaskTextBox(taskId, 10, 90, 250, 50, 10, "Дата");
            gb.Controls.Add(txtDate);

            TextBox txtTime = CreateTaskTextBox(taskId, 10, 125, 250, 50, 10, "Время");
            gb.Controls.Add(txtTime);

            RichTextBox rtb = CreateTaskRichTextBox(taskId, 10, 160, 250, 80, 8, "Описание");
            gb.Controls.Add(rtb);

            Button b = Task.CreateButton(10, 245, 250, 35, "Заметка");
            b.Enabled = false;
            gb.Controls.Add(b);

            int row = i / 2;
            int column = i % 2;
            gb.Location = new Point(margin + (column * (gbWidth + margin)), margin + (row * (gbHight + margin)));
            gb.Size = new Size(gbWidth, gbHight);

            panel.Controls.Add(gb);
        }

        return panel;
    }

    private RichTextBox CreateTaskRichTextBox(int ind, int x, int y, int width, int height, int font, string column)
    {
        DatabaseHelper bd = new DatabaseHelper();
        RichTextBox rtb = Task.CreateRichTextBox(x, y, width, height);
        rtb.Font = new System.Drawing.Font("Arial", font, FontStyle.Bold);
        rtb.Text = bd.GetRowString(ind, column);
        rtb.Enabled = false;

        return rtb;
    }

    private TextBox CreateTaskTextBox(int ind, int x, int y, int width, int height, int font, string column)
    {
        DatabaseHelper bd = new DatabaseHelper();
        TextBox txt = Task.CreateTextBox(x, y, width, height);
        txt.Font = new System.Drawing.Font("Arial", font, FontStyle.Bold);
        txt.Text = bd.GetRowString(ind, column);
        txt.Enabled = false;
        return txt;
    }

    private ComboBox CreateTaskComboBox(int ind, int x, int y, int width, int height, int font, string column)
    {
        DatabaseHelper bd = new DatabaseHelper();
        ComboBox cmb = Task.CreateComboBox(x, y, width, height);
        cmb.Font = new System.Drawing.Font("Arial", font, FontStyle.Bold);
        string str = bd.GetRowString(ind, column);
        if (str == "1") cmb.SelectedIndex = 0;
        else cmb.SelectedIndex = 1;
        cmb.Enabled = false;

        return cmb;
    }

}
