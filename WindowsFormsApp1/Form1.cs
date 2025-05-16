using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int ballXspeed = 4;
        int ballYspeed = 4;
        int speed = 2;
        Random rand = new Random();
        bool goDown, goUp;
        int computer_speed_change = 50;
        int playerScore = 0;
        int computerScore = 0;
        int playerSpeed = 8;
        int[] computerSpeeds = { 5, 6, 8, 9 };
        int[] ballSpeeds = { 10, 9, 8, 11, 12 };
        int topOffset = 50;
        int secondsElapsed = 0;

        public Form1()
        {
            InitializeComponent();
            label1.Parent = pictureBox1;
            label2.Parent = pictureBox2;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ensure initial positions respect topOffset
            if (ball.Top < topOffset)
                ball.Top = topOffset;
            if (player.Top < topOffset)
                player.Top = topOffset;
            if (computer.Top < topOffset)
                computer.Top = topOffset;

            // Initialize score labels
            label1.Text = $"Player: {playerScore}";
            label2.Text = $"Computer: {computerScore}";

            // Ensure label2 (Computer) is aligned to the right
            label2.Left = pictureBox2.Width - label2.Width - 10; // 10-pixel padding from the right
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Adjust label2 position on form resize to keep it aligned to the right
            label2.Left = pictureBox2.Width - label2.Width - 10;
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            ball.Top -= ballYspeed;
            ball.Left -= ballXspeed;

            // Ball boundaries: Respect topOffset for top boundary
            if (ball.Top < topOffset || ball.Bottom > this.ClientSize.Height)
            {
                ballYspeed = -ballYspeed;
            }

            if (ball.Left < -2)
            {
                ball.Left = this.ClientSize.Width / 2;
                ballXspeed = -ballXspeed;
                computerScore++;
                label2.Text = $"Computer: {computerScore}"; // Update score
            }

            if (ball.Left > this.ClientSize.Width + 2)
            {
                ball.Left = this.ClientSize.Width / 2;
                ballXspeed = -ballXspeed;
                playerScore++;
                label1.Text = $"Player: {playerScore}"; // Update score
            }

            // Computer paddle boundaries: Respect topOffset
            if (computer.Top < topOffset)
            {
                computer.Top = topOffset;
            }
            else if (computer.Bottom >= this.ClientSize.Height)
            {
                computer.Top = this.ClientSize.Height - computer.Height;
            }

            // Computer AI
            if (ball.Top < computer.Top + (computer.Height / 2) && ball.Left > (this.ClientSize.Width / 2))
            {
                computer.Top -= speed;
            }
            if (ball.Top > computer.Top + (computer.Height / 2) && ball.Left > (this.ClientSize.Width / 2))
            {
                computer.Top += speed;
            }

            computer_speed_change -= 1;

            if (computer_speed_change < 0)
            {
                speed = computerSpeeds[rand.Next(computerSpeeds.Length)];
                computer_speed_change = 50;
            }

            // Player movement: Respect topOffset
            if (goDown && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += playerSpeed;
            }

            if (goUp && player.Top > topOffset)
            {
                player.Top -= playerSpeed;
            }

            CheckCollision(ball, player, player.Right + 5);
            CheckCollision(ball, computer, computer.Left - 35);

            if (computerScore > 5)
            {
                GameOver("Sorry, you lost the game");
            }
            else if (playerScore > 5)
            {
                GameOver("You won!");
            }
        }

        private void KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
        }

        private void KeyisUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
        }

        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offset)
        {
            if (PicOne.Bounds.IntersectsWith(PicTwo.Bounds))
            {
                PicOne.Left = offset;

                int x = ballSpeeds[rand.Next(ballSpeeds.Length)];
                int y = ballSpeeds[rand.Next(ballSpeeds.Length)];

                if (ballXspeed < 0)
                {
                    ballXspeed = x;
                }
                else
                {
                    ballXspeed = -x;
                }

                if (ballYspeed < 0)
                {
                    ballYspeed = -y;
                }
                else
                {
                    ballYspeed = y;
                }
            }
        }

        private void timer1Tick(object sender, EventArgs e)
        {
            secondsElapsed++; // Increment seconds
            int minutes = secondsElapsed / 60; // Calculate minutes
            int seconds = secondsElapsed % 60; // Calculate remaining seconds
            label3.Text = $"{minutes:D2}:{seconds:D2}"; // Update label (e.g., "01:23")
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void GameOver(string message)
        {
            GameTimer.Stop();
            MessageBox.Show(message, "Game: ");
            computerScore = 0;
            playerScore = 0;
            ballXspeed = ballYspeed = 4;
            computer_speed_change = 50;
            label1.Text = $"Player: {playerScore}"; // Reset score display
            label2.Text = $"Computer: {computerScore}"; // Reset score display
            GameTimer.Start();
        }
    }
}