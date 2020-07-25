using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRexRunnerGame
{
    public partial class Form1 : Form
    {
        bool jumping = false;
        int jumpSpeed;
        int force = 0;
        int obstSpeed = 6;
        int score = 0;
        Random rand = new Random();
        int position;
        bool isGameOver = false;

        public Form1()
        {
            InitializeComponent();
            GameReset();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            TRexImage.Top += jumpSpeed;

            txtScore.Text = "Score: " + score;

            if(jumping == true && force <0)
            {
                jumping = false;
            }
            if(jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            if( TRexImage.Top > 281 && jumping== false)
            {
                force = 12;
                TRexImage.Top = 282;
                jumpSpeed = 0;
            }

            foreach (Control x in this.Controls)
            {
                if ( x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstSpeed;
                    if(x.Left <-100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                        score++;
                    }

                    if (TRexImage.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        TRexImage.Image = Properties.Resources.dead;
                        txtScore.Text = "press R to restart the game";
                        isGameOver = true;
                    }
                }
            }

            if (score>10)
            {
                obstSpeed = 8;
            }
            if (score > 20)
            {
                obstSpeed = 12;
            }
            if (score > 30)
            {
                obstSpeed = 15;
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }

            if(e.KeyCode== Keys.R && isGameOver == true)
            {
                GameReset();
            }
        }

        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstSpeed = 10;
            txtScore.Text = "Score: "+ score ;
            TRexImage.Image = Properties.Resources.running;
            isGameOver = false;
            TRexImage.Top = 282;

            foreach (Control x in this.Controls)
            {
                if(x is PictureBox  && 
                    (string)x.Tag == "obstacle")
                {
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);
                    x.Left = position;
                }
            }

            gameTimer.Start();

        }
    }
}
