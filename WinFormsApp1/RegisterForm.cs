using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            userNameField.Text = "Enter name";
            userSurnameField.Text = "Enter surname";
            userSurnameField.ForeColor = Color.Gray;
            userNameField.ForeColor = Color.Gray;
            this.Size = new Size(540, 540);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        Point lastPoint;
        private void panel1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //if the user clicked by left button of his mouse
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
            
        }

        private void panel1_MouseDown_1(object sender, MouseEventArgs e)
        {
           lastPoint = new Point(e.X, e.Y); //set our current position
        }

        private void userNameField_Enter(object sender, EventArgs e)
        {
            if (userNameField.Text == "Enter name")
            {
                userNameField.Text = "";
                userNameField.ForeColor = Color.Black;
            }
        }

        private void userSurnameField_Enter(object sender, EventArgs e)
        {
            if (userSurnameField.Text == "Enter surname")
            {
                userSurnameField.Text = "";
                userSurnameField.ForeColor = Color.Black;
            }
        }

        private void userNameField_Leave(object sender, EventArgs e)
        {
            if (userNameField.Text == "")
                userNameField.Text = "Enter name";
            
        }

        private void userSurnameField_Leave(object sender, EventArgs e)
        {
            if (userSurnameField.Text == "")
            {
                userSurnameField.Text = "Enter surname";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if(userNameField.Text == "Enter name")
            {
                MessageBox.Show("Enter name");
                return;
            }
            if (userSurnameField.Text == "Enter surname")
            {
                MessageBox.Show("Enter surname");
                return;
            }
            //checking if login is unique
            if (isUserExisted() == true)
                return; // we go out of funciton

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`login` , `pass` , `name` , `surname` ) VALUES (@login , @pass , @name , @surname)", db.getConnection());
            
            command.Parameters.Add("@login", MySqlDbType.VarChar).Value = loginField.Text;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = passField.Text;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = userNameField.Text;
            command.Parameters.Add("@surname", MySqlDbType.VarChar).Value = userSurnameField.Text;

            //connect to DB
            db.openConnection();
            //if our command was executed successfully 
            String title = "User was added";
            String message = "Do you want to continue?";
            DialogResult result =  MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (command.ExecuteNonQuery() == 1)
            {
                if(result == DialogResult.Yes)
                {
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.ShowDialog();
                }
                else
                {
                    return;
                }
            }
            else
                MessageBox.Show("Account was not created.");

            db.closeConnection();

        }



        public Boolean isUserExisted()
        {
            DB db = new DB(); //we connected to data base

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            //on the place of uL and uP we will write variables (for security)
            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL", db.getConnection());
            //db.getConnection() = we write the name of db we want to connect

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginField.Text;

            //adapter allows us to select data from DB
            adapter.SelectCommand = command;
            adapter.Fill(table); //we fill table with data we have achieved 

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Such login is already in the system");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm1 loginForm = new LoginForm1();
            loginForm.Show();
        }
    }
}
