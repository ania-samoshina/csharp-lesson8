using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionsApp
{
    public partial class MainForm : Form
    {
        private TrueFalse database;

        public MainForm()
        {
            InitializeComponent();
            ToggleControlsEnableState(false);
        }

        private void ToggleControlsEnableState(bool currentState)
        {
            btnAdd.Enabled = currentState;
            btnDelete.Enabled = currentState;
            btnSave.Enabled = currentState;

            nudNumber.Enabled = currentState;
            cbTrue.Enabled = currentState;
            tbQuestion.Enabled = currentState;

            menuItemSave.Enabled = currentState;
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                database = new TrueFalse(saveFileDialog.FileName);
                database.Add("Земля круглая?", true);
                database.Save();

                nudNumber.Minimum = 1;
                nudNumber.Maximum = 1;
                nudNumber.Value = 1;

                ToggleControlsEnableState(true);
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                double fileSizeInMb = fileInfo.Length / 1024.0;

                if (fileSizeInMb >= 10)
                {
                    MessageBox.Show("Файл больше 10 Мегабайт", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                database = new TrueFalse(openFileDialog.FileName);
                database.Load();

                nudNumber.Minimum = 1;
                nudNumber.Maximum = database.Count;
                nudNumber.Value = 1;

                if (database.Count > 0)
                {
                    tbQuestion.Text = database[0].Text;
                    cbTrue.Checked = database[0].TrueFalse;
                }

                ToggleControlsEnableState(true);
            }
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            database.Save();
            MessageBox.Show("Вы успешно сохранили базу вопросов", "База вопросов",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void nudNumber_ValueChanged(object sender, EventArgs e)
        {
            tbQuestion.Text = database[(int)nudNumber.Value - 1].Text;
            cbTrue.Checked = database[(int)nudNumber.Value - 1].TrueFalse;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            database[(int)nudNumber.Value - 1].Text = tbQuestion.Text;
            database[(int)nudNumber.Value - 1].TrueFalse = cbTrue.Checked;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            database.Add($"Вопрос #{database.Count + 1}", true);
            nudNumber.Maximum = database.Count;
            nudNumber.Value = database.Count;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (nudNumber.Maximum == 1)
                return;
            database.Remove((int)nudNumber.Value - 1);
            nudNumber.Maximum--;
            MessageBox.Show("Вопрос успешно удален", "База вопросов",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
