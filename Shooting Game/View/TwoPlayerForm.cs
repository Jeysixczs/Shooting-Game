﻿using Shooting_Game.Model;
using Shooting_Game.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Shooting_Game.View
{
    public partial class TwoPlayerForm : Form, ITwoPlayerGameView
    {
        public TwoPlayerForm()
        {
            //this.MinimizeBox = false;
            //this.MaximizeBox = false;
            //this.FormBorderStyle = FormBorderStyle.FixedDialog;

            InitializeComponent();


            KeyDown += (s, e) =>
            {
                presenter.OnKeyDown(e.KeyCode);
            };

            KeyUp += (s, e) =>
            {
                presenter.OnKeyUp(e.KeyCode);
            };

            player1 = new Player
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,

                    SizeMode = PictureBoxSizeMode.Zoom
                }
            };

            player2 = new Player
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,

                    SizeMode = PictureBoxSizeMode.Zoom
                }
            };

            player1.PictureBox.Location = new Point(150, 150);
            player2.PictureBox.Location = new Point(1069, 132);



            player1.PictureBox.BackColor = Color.Transparent;
            player2.PictureBox.BackColor = Color.Transparent;
            player1.PictureBox.BringToFront();
            player2.PictureBox.BringToFront();
            Controls.Add(player1.PictureBox);
            Controls.Add(player2.PictureBox);


        }

        private GamePresenter presenter;
        private Player player1, player2;


        public void SetPresenter(GamePresenter presenter) => this.presenter = presenter;


        public void RemoveEntity(GameEntity entity)
        {
            Controls.Remove(entity.PictureBox);
        }



        public void SpawnEntity(GameEntity entity)
        {
            Controls.Add(entity.PictureBox);
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            presenter.SetTwoPlayerView(this);
            presenter.StartTwoPlayerGame(player1, player2);


        }
        //test
        private bool player1Dead = false;
        private bool player2Dead = false;

        public void UpdatePlayer1Status(int health, int ammo)
        {
            health = Math.Max(0, Math.Min(health, 100));
            progressBar1.Value = health;
            label1.Text = $"P1 Ammo: {ammo}";

            if (health == 0 && !player1Dead)
            {
                player1Dead = true;

                CheckBothPlayersDead();
            }
        }

        public void UpdatePlayer2Status(int health, int ammo)
        {
            health = Math.Max(0, Math.Min(health, 100));
            progressBar2.Value = health;
            label2.Text = $"P2 Ammo: {ammo}";

            if (health == 0 && !player2Dead)
            {
                player2Dead = true;

                CheckBothPlayersDead();
            }
        }

        private void TwoPlayerForm_Load(object sender, EventArgs e)
        {
            AudioManager.PlayMusic(Properties.Resources.twoplayermusci, 0.5f);
        }

        private void TwoPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // AudioManager.StopMusic();
        }

        private void CheckBothPlayersDead()
        {
            if (player1Dead && player2Dead)
            {
                MessageBox.Show("Both players are dead! Game Over.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                MainMenuForm mainMenu = new MainMenuForm();
                mainMenu.Show();
            }
        }

        public List<PictureBox> GetWalls()
        {
            List<PictureBox> walls = new List<PictureBox>();
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pb && pb.Tag?.ToString() == "WALL")
                {
                    walls.Add(pb);
                }
            }
            return walls;
        }

        public void ShowGameOver()
        {
            this.Enabled = false;

            // Show game over panel/message
            MessageBox.Show("Game Over!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



    }
}
