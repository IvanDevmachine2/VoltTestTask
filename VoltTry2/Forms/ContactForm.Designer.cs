using System.Drawing;
using System.Windows.Forms;

namespace VoltTry2.Forms
{
    partial class ContactForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtLastName;
        private TextBox txtFirstName;
        private TextBox txtMiddleName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private TextBox txtNotes;
        private Button btnSave;
        private Button btnCancel;
        private Label lblLastName;
        private Label lblFirstName;
        private Label lblMiddleName;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblAddress;
        private Label lblNotes;

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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Text = "Контакт";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeUIComponents();
        }

        private void InitializeUIComponents()
        {
            // Labels
            lblLastName = new Label { Text = "Фамилия*:", Location = new Point(10, 15), Size = new Size(80, 20) };
            lblFirstName = new Label { Text = "Имя*:", Location = new Point(10, 45), Size = new Size(80, 20) };
            lblMiddleName = new Label { Text = "Отчество:", Location = new Point(10, 75), Size = new Size(80, 20) };
            lblPhone = new Label { Text = "Телефон*:", Location = new Point(10, 105), Size = new Size(80, 20) };
            lblEmail = new Label { Text = "Email:", Location = new Point(10, 135), Size = new Size(80, 20) };
            lblAddress = new Label { Text = "Адрес:", Location = new Point(10, 165), Size = new Size(80, 20) };
            lblNotes = new Label { Text = "Заметки:", Location = new Point(10, 195), Size = new Size(80, 20) };

            // TextBoxes
            txtLastName = new TextBox { Location = new Point(100, 12), Size = new Size(280, 20) };
            txtFirstName = new TextBox { Location = new Point(100, 42), Size = new Size(280, 20) };
            txtMiddleName = new TextBox { Location = new Point(100, 72), Size = new Size(280, 20) };
            txtPhone = new TextBox { Location = new Point(100, 102), Size = new Size(280, 20) };
            txtEmail = new TextBox { Location = new Point(100, 132), Size = new Size(280, 20) };
            txtAddress = new TextBox { Location = new Point(100, 162), Size = new Size(280, 20) };
            txtNotes = new TextBox { Location = new Point(100, 192), Size = new Size(280, 60), Multiline = true, ScrollBars = ScrollBars.Vertical };

            // Buttons
            btnSave = new Button { Text = "Сохранить", Location = new Point(200, 270), Size = new Size(80, 30) };
            btnCancel = new Button { Text = "Отмена", Location = new Point(300, 270), Size = new Size(80, 30) };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            // Add controls to form
            Controls.AddRange(new Control[] {
                lblLastName, lblFirstName, lblMiddleName, lblPhone, lblEmail, lblAddress, lblNotes,
                txtLastName, txtFirstName, txtMiddleName, txtPhone, txtEmail, txtAddress, txtNotes,
                btnSave, btnCancel
            });
        }
    }
}