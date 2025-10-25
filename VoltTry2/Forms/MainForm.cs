using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VoltTry2.Contracts.Entities;
using VoltTry2.Contracts.Presenters;

namespace VoltTry2.Forms
{
    public partial class MainForm : Form
    {
        private readonly char[] _alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".ToCharArray();
        private IMainPresenter _presenter;
        private Panel _selectedContactPanel;
        private Label _activeLetterLabel;
        private List<Label> _letterLabels = new List<Label>();

        public MainForm()
        {
            InitializeComponent();
            InitializeAlphabetLadder();
        }

        public IMainPresenter Presenter
        {
            set
            {
                _presenter = value;

                _presenter?.LoadContacts();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeShowAllButton();

            btnAdd.Click += (s, args) => AddContact();
            btnDelete.Click += (s, args) => DeleteContact();
        }

        private void InitializeShowAllButton()
        {
            var btnShowAll = new Button
            {
                Location = new Point(230, 10),
                Size = new Size(100, 30),
                Text = "Показать все"
            };

            btnShowAll.Click += (s, e) =>
            {
                if (_activeLetterLabel != null)
                {
                    _activeLetterLabel.BackColor = Color.White;
                    _activeLetterLabel = null;
                }
                _presenter?.LoadContacts();
            };

            this.Controls.Add(btnShowAll);
        }

        private void InitializeAlphabetLadder()
        {
            panelLadder.Controls.Clear();
            _letterLabels.Clear();

            int totalHeight = panelLadder.Height;
            int labelHeight = totalHeight / _alphabet.Length;

            for (int i = 0; i < _alphabet.Length; i++)
            {
                char letter = _alphabet[i];

                var label = new Label
                {
                    Location = new Point(0, i * labelHeight),
                    Size = new Size(panelLadder.Width, labelHeight),
                    Text = $"{letter} (0)",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Arial", 10, FontStyle.Regular),
                    Cursor = Cursors.Hand,
                    Tag = letter,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                label.Click += (s, e) =>
                {
                    if (_activeLetterLabel != null)
                    {
                        _activeLetterLabel.BackColor = Color.White;
                    }

                    label.BackColor = Color.LightGreen;
                    _activeLetterLabel = label;

                    _presenter?.LoadContactsByLetter(letter);
                };

                panelLadder.Controls.Add(label);
                _letterLabels.Add(label);
            }
        }

        public void UpdateContactCount(char letter, int count)
        {
            var label = _letterLabels.FirstOrDefault(l => (char)l.Tag == letter);
            if (label != null)
            {
                label.Text = $"{letter} ({count})";
            }
        }

        private void AddContact()
        {
            using (var contactForm = new ContactForm())
            {
                if (contactForm.ShowDialog() == DialogResult.OK)
                {
                    var contact = contactForm.GetContact();
                    _presenter?.AddContact(contact);
                }
            }
        }

        private void DeleteContact()
        {
            var selectedContact = GetSelectedContact();
            if (selectedContact != null)
            {
                var result = MessageBox.Show(
                    $"Точно хотите удалить контакт:\n{selectedContact.FullName}?\n\nТелефон: {selectedContact.PhoneNumber}?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _presenter?.DeleteContact(selectedContact.Id);

                    if (_selectedContactPanel != null)
                    {
                        _selectedContactPanel.BackColor = Color.White;
                        _selectedContactPanel.BorderStyle = BorderStyle.FixedSingle;
                        _selectedContactPanel = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите контакт для удаления", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DisplayContacts(object contacts)
        {
            panelMain.Controls.Clear();
            _selectedContactPanel = null;

            var contactsList = contacts as IEnumerable<IContact>;
            if (contactsList == null) return;

            if (!contactsList.Any())
            {
                var lblNoContacts = new Label
                {
                    Text = "Контакты не найдены",
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Font = new Font("Arial", 12, FontStyle.Italic),
                    ForeColor = Color.Gray
                };
                panelMain.Controls.Add(lblNoContacts);
                return;
            }

            int yPosition = 10;
            foreach (var contact in contactsList)
            {
                var contactPanel = CreateContactPanel(contact, yPosition);
                panelMain.Controls.Add(contactPanel);
                yPosition += 60;
            }
        }

        private Panel CreateContactPanel(IContact contact, int yPosition)
        {
            var panel = new Panel
            {
                Location = new Point(10, yPosition),
                Size = new Size(580, 50),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = contact.Id
            };

            var lblName = new Label
            {
                Location = new Point(5, 5),
                Size = new Size(300, 20),
                Text = contact.FullName,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            var lblPhone = new Label
            {
                Location = new Point(5, 25),
                Size = new Size(200, 15),
                Text = $"Тел: {contact.PhoneNumber}",
                Font = new Font("Arial", 9)
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPhone);

            panel.Click += (s, e) => SelectContactPanel(panel, contact);
            panel.DoubleClick += (s, e) => EditContact(contact);

            return panel;
        }

        private void SelectContactPanel(Panel panel, IContact contact)
        {
            if (_selectedContactPanel != null)
            {
                _selectedContactPanel.BackColor = Color.White;
                _selectedContactPanel.BorderStyle = BorderStyle.FixedSingle;
            }

            panel.BackColor = Color.LightBlue;
            panel.BorderStyle = BorderStyle.Fixed3D;
            _selectedContactPanel = panel;
        }

        private IContact GetSelectedContact()
        {
            if (_selectedContactPanel != null && _selectedContactPanel.Tag is int contactId)
            {
                var contacts = _presenter?.GetAllContacts();
                return contacts?.FirstOrDefault(c => c.Id == contactId);
            }
            return null;
        }

        private void EditContact(IContact contact)
        {
            using (var contactForm = new ContactForm(contact))
            {
                if (contactForm.ShowDialog() == DialogResult.OK)
                {
                    var updatedContact = contactForm.GetContact();
                    _presenter?.UpdateContact(updatedContact);
                }
            }
        }
    }
}