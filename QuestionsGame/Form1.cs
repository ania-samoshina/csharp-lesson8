using QuestionsApp;
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

namespace QuestionsGame
{
    public partial class Form1 : Form
    {
        private TrueFalse database;
        private bool currentAnswer;
        private int playerScore = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void GameRound()
        {
            Random random = new Random();
            Question currentQuestion = database[random.Next(0, database.Count - 1)];

            rtbQuestion.Text = currentQuestion.Text;
            currentAnswer = currentQuestion.TrueFalse;
        }

        private void CheckPlayerAnswer(bool playerAnswer)
        {
            if (currentAnswer == playerAnswer)
            {
                playerScore++;
                lblScore.Text = playerScore.ToString();
            }

            GameRound();            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xml файлы|*.xml";
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
  
                if (database.Count == 0)
                {
                    MessageBox.Show("В файле отсутсвуют вопросы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnRestart.Enabled = false;
                btnStart.Enabled = true;
            }
        }

        private void btnIsTrue_Click(object sender, EventArgs e)
        {
            CheckPlayerAnswer(true);
        }

        private void btnIsFalse_Click(object sender, EventArgs e)
        {
            CheckPlayerAnswer(false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnRestart.Enabled = true;
            btnStart.Enabled = false;

            btnIsTrue.Enabled = true;
            btnIsFalse.Enabled = true;

            GameRound();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            playerScore = 0;
            lblScore.Text = playerScore.ToString();
        }
    }
}
