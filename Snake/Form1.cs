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
        private SnakePlayer player;
        private SnakeFood food;

        private SnakePanel drawingPanel;
        private Button startButton;
        private Button restartButton;
        private Label nameLabel;
        private TextBox nameText;
        private Label gameOver;

        private const int viewSize = 500;
        private int width = Screen.PrimaryScreen.Bounds.Width;
        private int height = Screen.PrimaryScreen.Bounds.Height;
        private const int menuSize = 40;

        public Form1(SnakeController ctl)
        {
            InitializeComponent();
            controller = ctl;
            player = controller.GetSnake();
            food = controller.GetFood();
            controller.UpdateArrived += OnFrame;

            // Set the window size
            ClientSize = new Size(width, height);

            // Place and add the button
            startButton = new Button();
            startButton.Location = new Point(width / 2, 5);
            startButton.Size = new Size(70, 20);
            startButton.Text = "Start";
            startButton.Click += StartClick;
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

            // Place and add the drawing panel
            drawingPanel = new SnakePanel(player, food);
            drawingPanel.Location = new Point(0, menuSize);
            drawingPanel.Size = new Size(width, height - menuSize);
            this.Controls.Add(drawingPanel);

            // Game over label
            gameOver = new Label
            {
                Text = "Game Over.",
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))),
                BackColor = Color.Red,
                Location = new Point(width / 2, height / 2),
                Size = new Size(50, 50)
            };

            this.Controls.Add(gameOver);
            gameOver.Visible = false;
            controller.EndGame += GameOver;
            gameOver.BringToFront();
            
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
            nameText.Enabled = false;
            // Enable the global form to capture key presses
            KeyPreview = true;

            gameOver.Visible = false;
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
            startButton.Text = "Restart";
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
    }
}
