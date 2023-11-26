using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    public class SnakeController
    {
        private SnakePlayer player;
        private SnakeFood food;
        private bool UpPressed = false;
        private bool DownPressed = false;
        private bool LeftPressed = false;
        private bool RightPressed = false;
        private bool addSegment = false;
        private int screenSize;
        private Timer gameTimer;

        public delegate void UpdateHandler();
        public delegate void EndGameHandler();

        public event UpdateHandler UpdateArrived;
        public event EndGameHandler EndGame;

        public SnakeController()
        {
            screenSize = 500;
            food = new SnakeFood(100, 100);
            player = new SnakePlayer();
            
            gameTimer = new Timer();
            gameTimer.Interval = 50;
            gameTimer.Elapsed += UpdateCameFromServer;
        }

        public SnakePlayer GetSnake()
        {
            return player;
        }

        public SnakeFood GetFood()
        {
            return food;
        }

        public void ProcessUpdates()
        {
            player.KillSnake();

            gameTimer.Start();
        }

        private void UpdateCameFromServer(object sender, ElapsedEventArgs e)
        {
            int lastSegX = player.GetLastSegment().x;
            int lastSegY = player.GetLastSegment().y;

            foreach (SnakePlayer.Segment seg in player.GetSegmentList())
            {
                if(seg.nextSeg == null)
                {
                    switch (player.direction)
                    {
                        case 0:
                            if (seg.x > screenSize / 2)
                                seg.x = ((screenSize / 2) * -1);
                            else
                                seg.x += 10;
                            break;

                        case 90:
                            if (seg.y < (screenSize / 2) * -1)
                                seg.y = screenSize / 2;
                            else
                                seg.y -= 10;
                            break;

                        case 180:
                            if (seg.x < (screenSize / 2) * -1)
                                seg.x = screenSize / 2;
                            else
                                seg.x -= 10;
                            break;

                        case 270:
                            if (seg.y > screenSize / 2)
                                seg.y = (screenSize / 2) * -1;
                            else
                                seg.y += 10;
                            break;
                    }
                }
                else
                {
                    if (player.GetFirstSegment().x == seg.x && player.GetFirstSegment().y == seg.y)
                    {
                        gameTimer.Stop();
                        EndGame();
                        break;
                    }


                    seg.x = seg.nextSeg.x;
                    seg.y = seg.nextSeg.y;
                }

            }

            if (addSegment)
            {
                Random rand = new Random();
                player.AddSegment(lastSegX, lastSegY);

                int newX = rand.Next((screenSize / 2) * -1, screenSize / 2);
                int newY = rand.Next((screenSize / 2) * -1, screenSize / 2);
                newX = newX - (newX % 10);
                newY = newY - (newY % 10);

                food.x = newX;
                food.y = newY;
                addSegment = false;
            }

            if (player.GetFirstSegment().x == food.x && player.GetFirstSegment().y == food.y)
                addSegment = true;

            // Notify any listeners (the view) that a new game world has arrived from the server
            UpdateArrived();

            // For whatever user inputs happened during the last frame,
            // process them.
            ProcessInputs();
        }

        /// <summary>
        /// Checks which inputs are currently held down
        /// Normally this would send a message to the server
        /// </summary>
        private void ProcessInputs()
        {
            if (UpPressed)
                player.direction = 90;
            if (DownPressed)
                player.direction = 270;
            if (LeftPressed)
                player.direction = 180;
            if (RightPressed)
                player.direction = 0;

            UpPressed = false;
            DownPressed = false;
            LeftPressed = false;
            RightPressed = false;
        }

        /// <summary>
        /// Handle direction change
        /// </summary>
        /// <param name="directionDegree"></param>
        public void HandleDirectionChange(int directionDegree)
        {
            switch (directionDegree)
            {
                case 0:
                    if(player.direction != 180)
                        RightPressed = true;
                    break;
                case 90:
                    if(player.direction != 270)
                        UpPressed = true;
                    break;
                case 180:
                    if(player.direction != 0)
                        LeftPressed = true;
                    break;
                case 270:
                    if(player.direction != 90)
                        DownPressed = true;
                    break;
            }
        }

        public void HandleSnakeAddition()
        {
            addSegment = true;
        }

        public void Restart()
        {

        }


    }
}
