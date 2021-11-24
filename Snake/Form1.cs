using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private SnakeController controller;

        // World is a simple container for Players and Powerups
        // The controller owns the world, but we have a reference to it
        private SnakePlayer player;
        private SnakeFood food;

        // This simple form only has two components
        SnakePanel drawingPanel;
        Button startButton;
        Button restartButton;
        Label nameLabel;
        TextBox nameText;
        Label gameOver;

        private const int viewSize = 500;
        private const int menuSize = 40;

        public Form1(SnakeController ctl)
        {
            InitializeComponent();
            controller = ctl;
            player = controller.GetSnake();
            food = controller.GetFood();
            controller.UpdateArrived += OnFrame;

            // Set the window size
            ClientSize = new Size(viewSize, viewSize + menuSize);

            // Place and add the button
            startButton = new Button();
            startButton.Location = new Point(215, 5);
            startButton.Size = new Size(70, 20);
            startButton.Text = "Start";
            startButton.Click += StartClick;
            this.Controls.Add(startButton);
            
            // Place and add the button
            restartButton = new Button(); 
            restartButton.Location = new Point(240, 5);
            restartButton.Size = new Size(70, 20);
            restartButton.Text = "Restart";
            restartButton.MouseClick += RestartHandler;
            this.Controls.Add(startButton);

            // Place and add the name label
            nameLabel = new Label();
            nameLabel.Text = "Name:";
            nameLabel.Location = new Point(5, 10);
            nameLabel.Size = new Size(40, 15);
            this.Controls.Add(nameLabel);

            // Place and add the name textbox
            nameText = new TextBox();
            nameText.Text = "player";
            nameText.Location = new Point(50, 5);
            nameText.Size = new Size(70, 15);
            this.Controls.Add(nameText);

            // Game over label
            gameOver = new Label();
            gameOver.Text = "Game Over.";
            gameOver.Location = new Point(viewSize / 2, 10);
            gameOver.Size = new Size(40, 15);
            this.Controls.Add(gameOver);
            gameOver.Visible = false;
            controller.EndGame += GameOver;

            // Place and add the drawing panel
            drawingPanel = new SnakePanel(player, food);
            drawingPanel.Location = new Point(0, menuSize);
            drawingPanel.Size = new Size(viewSize, viewSize);
            this.Controls.Add(drawingPanel);

            // Set up key and mouse handlers
            this.KeyDown += HandleKeyDown;
        }

        /// <summary>
        /// Simulates connecting to a "server"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartClick(object sender, EventArgs e)
        {
            // Disable the form controls
            startButton.Enabled = false;
            nameText.Enabled = false;
            // Enable the global form to capture key presses
            KeyPreview = true;
            // "connect" to the "server"
            controller.ProcessUpdates();
        }

        /// <summary>
        /// Handler for the controller's UpdateArrived event
        /// </summary>
        private void OnFrame()
        {
            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can
            if (!(drawingPanel.IsDisposed))
            {
                MethodInvoker m = new MethodInvoker(() => Invalidate(true));
                this.Invoke(m);
            }
        }

        private void GameOver()
        {
            MethodInvoker m = new MethodInvoker(() =>
            { 
            gameOver.Visible = true;
            drawingPanel.Visible = false;
            startButton.Enabled = true;
            });
            this.Invoke(m);
        }

        /// <summary>
        /// Key down handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            else if (e.KeyCode == Keys.W)
            {
                controller.HandleDirectionChange(90);
            }
            else if (e.KeyCode == Keys.A)
            {
                controller.HandleDirectionChange(180);
            }

            else if (e.KeyCode == Keys.S)
            {
                controller.HandleDirectionChange(270);
            }
            else if (e.KeyCode == Keys.D)
            {
                controller.HandleDirectionChange(0);
            }
            else if (e.KeyCode == Keys.Space)
                controller.HandleSnakeAddition();

            // Prevent other key handlers from running
            //e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void RestartHandler(object sender, MouseEventArgs e)
        {

        }
    }
}
