using System;
using System.Collections.Generic;

namespace Snake
{

    public class SnakePlayer
    {
        private LinkedList<Segment> segments;
        public int direction { get; set; }

        public SnakePlayer()
        {
            direction = 0;
            segments = new LinkedList<Segment>();

            AddSegment(50, 50);
        }

        /// <summary>
        /// Add a segment to the existing snake
        /// </summary>
        /// <param name="direction">Which direction this segment is moving</param>
        public void AddSegment(int x, int y)
        {
            if(segments.Count > 0)
                segments.AddFirst(new Segment(x, y, segments.First.Value));
            else
                segments.AddFirst(new Segment(x, y, null));
        }

        public LinkedList<Segment> GetSegmentList()
        {
            return segments;
        }

        public Segment GetLastSegment()
        {
            return segments.First.Value;
        }


        public Segment GetFirstSegment()
        {
            return segments.Last.Value;
        }

        /// <summary>
        /// Represents a square segment of the snake. 
        /// </summary>
        public class Segment
        {
            public int x
            { get; set; }
            public int y
            { get; set; }
            public Segment nextSeg
            { get; }

            public Segment(int _x, int _y, Segment seg)
            {
                x = _x;
                y = _y;
                nextSeg = seg;
            }
        }

        /// <summary>
        /// Reset snake to initial values. Used when restarting game. 
        /// </summary>
        public void KillSnake()
        {
            direction = 0;
            segments = new LinkedList<Segment>();

            AddSegment(50, 50);
        }
    }
}
