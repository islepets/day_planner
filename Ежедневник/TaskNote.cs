using System;
using Ежедневник;

public class TaskNote : Form
{
    private int taskId;
    private RichTextBox rt;
    private DatabaseHelper bd;

    public TaskNote(int taskId)
    {
        this.taskId = taskId;

        this.Icon = Ежедневник.Properties.Resources.diary_icon;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(400, 400);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = ColorTranslator.FromHtml("#374785");

        MPanel panel = new MPanel(400, this, "Заметка");
        this.Controls.Add(panel);

        rt = Task.CreateRichTextBox(10, 50, 380, 270);
        this.Controls.Add(rt);

        Button btn = Task.CreateButton(10, 330, 380, 50, "Сохранить");
        btn.Click += SaveData;
        this.Controls.Add(btn);

        bd = new DatabaseHelper();
        LoadExistingNote();
    }

    private void LoadExistingNote()
    {
        string existingNote = bd.GetTaskComment(taskId);
        rt.Text = existingNote;
    }

    public void SaveData(object sender, EventArgs e)
    {
        bd.SaveTaskComment(taskId, rt.Text);
        MessageBox.Show("Заметка сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
    }
}