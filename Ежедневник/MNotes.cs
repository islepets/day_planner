using System;
using System.Globalization;
using Ежедневник;
using Ежедневник.Properties;

public class MNotes : Form
{
    private Label lb1, lb2;
    private Button bt, bt0, bt1, bt2, bt3, bt4, bt5, bt6;
    private MPanel panel;
    private RichTextBox rtb, rtb1;
    private Panel pnl;
    private PictureBox pcb;

    private bool isInterfaceVisible = false;
    private bool isInterfaceVisibleCreate = false;
    private bool isEdit = true;

    public MNotes()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.Size = new Size(1450, 800);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = ColorTranslator.FromHtml("#374785");
        this.Font = new Font("Arial", 16, FontStyle.Bold);
        CultureInfo culture = new CultureInfo("ru-Ru");
        DateTime now = DateTime.Now;

        pcb = CreatePictureBox();
        this.Controls.Add(pcb);

        lb1 = CreateDaysLabel(culture, now);
        this.Controls.Add(lb1);

        lb2 = CreateTimeLabel(culture, now);
        this.Controls.Add(lb2);

        bt = CreateButton("Создать", 50, 650, 200, 50);
        this.Controls.Add(bt);

        bt0 = CreateButton("Просмотр", 50, 720, 200, 50);
        this.Controls.Add(bt0);

        bt1 = CreateButton("<", 1300, 150, 50, 50);
        this.Controls.Add(bt1);

        bt2 = CreateButton(">", 1350, 150, 50, 50);
        this.Controls.Add(bt2);

        panel = new MPanel(1450, this, "Заметки - Проектирование программных систем");
        this.Controls.Add(panel);

        rtb = MTasks.CreateRichTextBox(350, 150, 900, 600);
        this.Controls.Add(rtb);

        rtb1 = MTasks.CreateRichTextBox(350, 150, 900, 600);
        this.Controls.Add(rtb1);

        pnl = CreatePanel(0, 0, 300, 800, "#A8D0E6");
        this.Controls.Add(pnl);

        bt3 = CreateButton("Редактировать", 1250, 600, 200, 50);
        this.Controls.Add(bt3);

        bt4 = CreateButton("Удалить", 1250, 650, 200, 50);
        this.Controls.Add(bt4);

        bt5 = CreateButton("Сохранить", 1250, 700, 200, 50);
        this.Controls.Add(bt5);

        bt6 = CreateButton("Создать", 1250, 700, 200, 50);
        this.Controls.Add(bt6);

        HideViewInterface();
        bt0.Click += SwitchView;
        bt.Click += SwitchViewCreate;
        bt5.Click += SaveChanges;
        bt3.Click += EditView; 
        bt6.Visible = false;
        rtb1.Visible = false;

        rtb.ReadOnly = isEdit;
        rtb.BackColor = Color.Gray;
    }

    public void EditView(object sender, EventArgs e)
    {
        isEdit = false;
        UpdateEditMode();
    }

    public void SaveChanges(object sender, EventArgs e)
    {
        isEdit = true; 
        UpdateEditMode();

    }

    private void UpdateEditMode()
    {
        if (isEdit == true)
        {
            rtb.ReadOnly = true;
            rtb.BackColor = Color.Gray;
            bt5.Visible = false; 
            bt3.Visible = true;
            bt4.Visible = true;  
        }
        else
        {
            rtb.ReadOnly = false;
            rtb.BackColor = SystemColors.Window;
            bt5.Visible = true;
            bt3.Visible = false; 
            bt4.Visible = false; 
        }
    }

    public void SwitchViewCreate(object sender, EventArgs e)
    {
        HideViewInterface();
        isInterfaceVisible = false;
        isInterfaceVisibleCreate = !isInterfaceVisibleCreate;

        if (isInterfaceVisibleCreate == true)
        {
            rtb1.Visible = true;
            pcb.Visible = false;
            bt6.Visible = true;
        }
        else
        {
            rtb1.Visible = false;
            pcb.Visible = true;
            bt6.Visible = false;
        }

        if (!isEdit)
        {
            isEdit = true;
            rtb.ReadOnly = true;
            rtb.BackColor = Color.Gray;
        }
    }

    private void SwitchView(object sender, EventArgs e)
    {
        isInterfaceVisible = !isInterfaceVisible;
        isInterfaceVisibleCreate = false;
        bt6.Visible = false;
        rtb1.Visible = false;

        if (isInterfaceVisible)
        {
            VisibleViewInterface();
            if (!isEdit)
            {
                isEdit = true;
                rtb.ReadOnly = true;
                rtb.BackColor = Color.Gray;
                bt5.Visible = false;
            }
        }
        else
        {
            HideViewInterface();
            if (!isEdit)
            {
                isEdit = true;
                rtb.ReadOnly = true;
                rtb.BackColor = Color.Gray;
                bt5.Visible = false;
            }
        }
    }

    public void HideViewInterface()
    {
        pcb.Visible = true;
        bt1.Visible = false;
        bt2.Visible = false;
        bt3.Visible = false;
        bt4.Visible = false;
        bt5.Visible = false;
        bt6.Visible = false;
        rtb.Visible = false;
        rtb1.Visible = false;
    }

    public void VisibleViewInterface()
    {
        pcb.Visible = false;
        bt1.Visible = true;
        bt2.Visible = true;
        bt3.Visible = true;
        bt4.Visible = true;
        bt5.Visible = false;
        rtb.Visible = true;
    }

    public PictureBox CreatePictureBox()
    {
        PictureBox pictureBox = new PictureBox();
        pictureBox.Size = new Size(700, 700);
        pictureBox.Location = new Point(500, 75);
        pictureBox.Image = Ежедневник.Properties.Resources.notes;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

        return pictureBox;
    }

    public Panel CreatePanel(int x, int y, int width, int height, string color)
    {
        Panel panel = new Panel();
        panel.Location = new Point(x, y);
        panel.Size = new Size(width, height);
        panel.BackColor = ColorTranslator.FromHtml(color);

        return panel;
    }

    public Label CreateDaysLabel(CultureInfo culture, DateTime now)
    {
        Label label = new Label();
        label.Text = culture.TextInfo.ToTitleCase(now.ToString("dddd", culture));
        label.Font = new Font("Arial", 14, FontStyle.Bold);
        label.ForeColor = ColorTranslator.FromHtml("#374785");
        label.BackColor = ColorTranslator.FromHtml("#A8D0E6");
        label.Location = new Point(50, 50);
        label.AutoSize = true;

        return label;
    }

    public Label CreateTimeLabel(CultureInfo culture, DateTime now)
    {
        Label label = new Label();
        label.Text = now.ToString("dd MMM yyyy", culture);
        label.Font = new Font("Arial", 14, FontStyle.Bold);
        label.ForeColor = ColorTranslator.FromHtml("#374785");
        label.BackColor = ColorTranslator.FromHtml("#A8D0E6");
        label.Location = new Point(50, 80);
        label.AutoSize = true;

        return label;
    }

    public Button CreateButton(string text, int x, int y, int width, int height)
    {
        Button button = new Button();
        button.Text = text;
        button.Font = new Font("Arial", 14, FontStyle.Bold);
        button.Location = new Point(x, y);
        button.BackColor = ColorTranslator.FromHtml("#FF6B6B");
        button.Size = new Size(width, height);

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