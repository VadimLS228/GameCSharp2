using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;


namespace Game
{
    public partial class GameForm : Form
    {
        private GameModel model;
        private GameController controller;
        private Point pos;
        private bool dragging, coinCollected = false;
        private Image originalPlayerImage;
        private Image Background1;
        private Image Background2;
        public SoundPlayer backgroundMusic;
        public SoundPlayer lossSound;


        public GameForm()
        {
            InitializeComponent();
            model = new GameModel(this);
            controller = new GameController(model, timer, lableLose, this);
            InitializeEvents();
            KeyPreview = true;
            originalPlayerImage = shipStandart.Image;
            Background1 = portalBgStandart1.Image;
            Background2 = portalBgStandart2.Image;

            backgroundMusic = new SoundPlayer(Properties.Resources.BgMusic);
            lossSound = new SoundPlayer(Properties.Resources.GameOver);
            backgroundMusic.PlayLooping();
        }
        

        private void InitializeEvents()
        {
            // Привязка событий к элементам формы
            portalBgStandart1.MouseDown += MouseClickDown;
            portalBgStandart1.MouseUp += MouseClickUp;
            portalBgStandart1.MouseMove += MouseClickMove;
            portalBgStandart2.MouseDown += MouseClickDown;
            portalBgStandart2.MouseUp += MouseClickUp;
            portalBgStandart2.MouseMove += MouseClickMove;
            lableLose.Visible = false;
            btnRestart.Visible = false;
            btnRestart.Click += RestartGame_Click;
            timer.Tick += Timer_Tick;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            int speed = -10;
            portalBgStandart1.Left += speed;
            portalBgStandart2.Left += speed;

            portalGreen.Left += speed;
            portalRed.Left += speed;
            portalBlue.Left += speed;

            int stoneSpeed = -15;
            stone1.Left += stoneSpeed;
            metall.Left += stoneSpeed;
            stone2.Left += stoneSpeed;

            coin.Left += speed;


            if (coin.Left <= -10 || coinCollected)
            {
                MoveCoin(coin);
                coinCollected = false;
            }
            if (portalGreen.Left <= -500)
            {
                MovePortalG(portalGreen);
            }
            if (portalRed.Left <= -500)
            {
                MovePortalR(portalRed);
            }
            if (portalBlue.Left <= -500)
            {
                MovePortalB(portalBlue);
            }
            if (stone1.Left <= -110)
            {
                MoveObstacle(stone1);
            }
            if (stone2.Left <= -10)
            {
                MoveObstacle(stone2);
            }
            if (metall.Left <= -10)
            {
                MoveObstacle(metall);
            }
            if (portalBgStandart1.Left <= 0)
            {
                portalBgStandart1.Left = 840;
                portalBgStandart2.Left = 0;
            }

            controller.CheckCollisions(shipStandart, coin, stone1,
            stone2, metall, portalRed, portalBlue, portalGreen, shipBlue, shipGreen,
            shipPurple, portalBgRed1, portalBgRed2,
            portalBgBlue1, portalBgBlue2,portalBgGreen1, portalBgGreen2);

            shipStandart.Image = model.CurrentPlayerImage;
            portalBgStandart1.Image = model.CurrentBackgroundImage1;
            portalBgStandart2.Image = model.CurrentBackgroundImage2;

            if (shipStandart.Bounds.IntersectsWith(coin.Bounds))
            {
                model.CollectCoin(); // Увеличиваем счетчик монет
                MoveCoin(coin); // Перемещаем монетку на новую позицию
                UpdateCoinLabel(); // Обновляем отображение счетчика монет
                coinCollected = true;
            }
            else if (!shipStandart.Bounds.IntersectsWith(coin.Bounds))
            {
                coinCollected = false; // Сброс флага, если игрок покинул область монетки
            }

        }


        private void MouseClickDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            pos.X = e.X;
            pos.Y = e.Y;
        }

        private void MouseClickUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void MouseClickMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currPoint = PointToScreen(new Point(e.X, e.Y));
                this.Location = new Point(currPoint.X - pos.X + portalBgStandart1.Left, currPoint.Y);
            }
        }

        public void RestartGame_Click(object sender, EventArgs e)
        {
            model.RestartGame();
            ResetGame();
        }
        public void ShowRestartButton()
        {
            btnRestart.Visible = true;
        }
        public void ResetGame()
        {
            portalGreen.Left = 1400;
            portalRed.Left = 2000;
            portalBlue.Left = 2600;
            stone1.Left = 2000;
            stone2.Left = 940;
            metall.Left = 1400;
            lableLose.Visible = false;
            btnRestart.Visible = false;
            timer.Enabled = true;
            backgroundMusic.PlayLooping();
            labelCoins.Text = "Монеты: 0";
            coin.Left = 1400;
            model.RestartGame(); // Вызываем метод RestartGame из модели
            model.CurrentBackgroundImage1 = Background1;
            model.CurrentBackgroundImage2 = Background2;
            model.CurrentPlayerImage = originalPlayerImage;
            portalBgStandart1.Image = model.CurrentBackgroundImage1;
            portalBgStandart2.Image = model.CurrentBackgroundImage2;
        }

        private void MoveCoin(PictureBox coin)
        {
            Random rand = new Random();
            coin.Top = rand.Next(0, 640);
            coin.Left = rand.Next(1000, 1200);
        }

        private void MovePortalG(PictureBox portalGreen)
        {
            Random rand = new Random();
            portalGreen.Top = rand.Next(0, 640);
            portalGreen.Left = rand.Next(2000, 2500);
        }
        private void MovePortalR(PictureBox portalRed)
        {
            Random rand = new Random();
            portalRed.Top = rand.Next(0, 640);
            portalRed.Left = rand.Next(2000, 2500);
        }
        private void MovePortalB(PictureBox portalBlue)
        {
            Random rand = new Random();
            portalBlue.Top = rand.Next(0, 640);
            portalBlue.Left = rand.Next(2000, 2500);
        }

        private void UpdateCoinLabel()
        {
            labelCoins.Text = "Монеты: " + model.CountCoins.ToString();
        }

        private void MoveObstacle(PictureBox obstacle)
        {
            Random rand = new Random();
            obstacle.Top = rand.Next(0, 640);
            obstacle.Left = rand.Next(800, 1200);
        }
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundMusic.Stop();
            lossSound.Stop();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Escape)
                this.Close();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (model.Lose) return;

            int speed1 = 10;
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    if (shipStandart.Left > 10)
                    {
                        shipStandart.Left -= speed1;
                    }
                    break;
                case Keys.Right:
                case Keys.D:
                    if (shipStandart.Right < 820)
                    {
                        shipStandart.Left += speed1;
                    }
                    break;
                case Keys.Up:
                case Keys.W:
                    if (shipStandart.Top > 10)
                    {
                        shipStandart.Top -= speed1;
                    }
                    break;
                case Keys.Down:
                case Keys.S:
                    if (shipStandart.Bottom < 630)
                    {
                        shipStandart.Top += speed1;
                    }
                    break;
            }
        }
    }
}
