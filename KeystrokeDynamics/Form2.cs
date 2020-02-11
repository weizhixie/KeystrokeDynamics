using System;
using System.IO;
using System.Windows.Forms;

namespace KeystrokeDynamics
{
    public partial class Form2 : Form
    {
        private int counter = 0;
        private int errorCounter = 0;
        private int attempCounter = 0;//total attemps for each individual question
        private int totalAttemps = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Question();
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            //check if the input is correct
            if (InputTextBox.Text == QuestionLbl.Text)
            {
                InputTextBox.Text = "";
                CentreForm("Correct, next");
                totalAttemps += 1;
                attempCounter += 1;
                Question();
            }
            else
            {
                CentreForm("Incorrect, write again");
                totalAttemps += 1;
                attempCounter += 1;
                errorCounter += 1;
            }
        }
        
        private double Accuracy()
        {
            double error = Convert.ToDouble(errorCounter);
            double attemp = Convert.ToDouble(attempCounter);

            //accuracy = (correctly predicted class / total testing class) × 100%
            double accuracy = 1 / attemp * 100;

            if (error == 0 && attemp == 1)
            {
                accuracy = 100;
            }

            return accuracy;
        }

        private void LogStream()
        {
            if (counter == 0)
            { }
            else
            {
                StreamWriter writel = new StreamWriter(Application.StartupPath + @"\accuracy.txt", true);
                writel.Write("Wrong attemps for " + "Q" + counter + ": " + errorCounter + "," + " Attemps for " +
                    "Q" + counter + ": " + attempCounter + "," + " Total attemps: " + totalAttemps + "," +
                    " Accuracy = " + Accuracy() + "%" + Environment.NewLine);
                writel.Close();

                //reset the errorcounter for next question
                errorCounter = 0;
                //reset total attemps for individual question
                attempCounter = 0;
            }
        }

        private void Question()
        {
            //read all text from file and store it in a string array
            string[] psLines = File.ReadAllLines(Application.StartupPath + @"\password.txt");
            QNumlbl.Text = "Q." + Convert.ToString(counter + 1);

            if(counter <= psLines.Length-1)
            {
                //dispaly text
                QuestionLbl.Text = psLines[counter];
                StreamWriter output = new StreamWriter(Application.StartupPath + @"\log.txt", true);
                output.Write(Environment.NewLine + "**************" + QuestionLbl.Text + "***************" + Environment.NewLine);
                output.Close();
                LogStream();
            }
            
            if (counter == psLines.Length)
            {
                LogStream();
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
                CentreForm("Thank you for your cooperation and support");
            }
            counter += 1;
        }

        //make message box  appear centre on main form.
        private void CentreForm(string text)
        {
            using (new CentreWinDialog(this))
            {
                MessageBox.Show(text);
            }
        }

        //close the application, if X button on the top right being pressed
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            Application.Exit();
        }

    }
}
