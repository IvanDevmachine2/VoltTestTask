using System;
using System.Windows.Forms;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Forms
{
    public partial class ContactForm : Form
    {
        private readonly IContact _contact;
        private readonly bool _isEditMode;

        public ContactForm() : this(null) { }

        public ContactForm(IContact contact)
        {
            InitializeComponent();
            _contact = contact;
            _isEditMode = contact != null;

            InitializeForm();
        }

        private void InitializeForm()
        {
            Text = _isEditMode ? "Редактировать контакт" : "Добавить контакт";

            if (_isEditMode)
            {
                txtLastName.Text = _contact.LastName;
                txtFirstName.Text = _contact.FirstName;
                txtMiddleName.Text = _contact.MiddleName;
                txtPhone.Text = _contact.PhoneNumber;
                txtEmail.Text = _contact.Email;
                txtAddress.Text = _contact.Address;
                txtNotes.Text = _contact.Notes;
            }
        }

        public IContact GetContact()
        {
            return new Models.Entities.Contact
            {
                Id = _isEditMode ? _contact.Id : 0,
                LastName = txtLastName.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                Address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Фамилия обязательна для заполнения", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Имя обязательно для заполнения", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Телефон обязателен для заполнения", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            return true;
        }
    }
}