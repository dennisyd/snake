using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class SnakePanel : UserControl
    {
        private SnakePlayer player;
        private SnakeFood food;

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        public SnakePanel(SnakePlayer p, SnakeFood f)
        {
            player = p;
            food = f;
            DoubleBuffered = true;
        }

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            e.Graphics.TranslateTransform((int)worldX, (int)worldY);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void SegmentDrawer(object o, PaintEventArgs e)
        {
            SnakePlayer p = o as SnakePlayer;

            int width = 10;
            int height = 10;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (System.Drawing.SolidBrush blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
            {
                // Rectangles are drawn starting from the top-left corner.
                // So if we want the rectangle centered on the player's location, we have to offset it
                // by half its size to the left (-width/2) and up (-height/2)
                e.Graphics.FillRectangle(blackBrush, new Rectangle(-(width / 2), -(height / 2), width, height));
            }
        }

        /// <summary>
        /// Acts as a drawing delegate for DrawObjectWithTransform
        /// After performing the necessary transformation (translate/rotate)
        /// DrawObjectWithTransform will invoke this method
        /// </summary>
        /// <param name="o">The object to draw</param>
        /// <param name="e">The PaintEventArgs to access the graphics</param>
        private void FoodDrawer(object o, PaintEventArgs e)
        {
            SnakeFood p = o as SnakeFood;

            int width = 10;
            int height = 10;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {
                // Rectangles are drawn starting from the top-left corner.
                // So if we want the rectangle centered on the player's location, we have to offset it
                // by half its size to the left (-width/2) and up (-height/2)
                e.Graphics.FillRectangle(redBrush, new Rectangle(-(width / 2), -(height / 2), width, height));
            }
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            // Center the view on the middle of the world,
            // since the image and world use different coordinate systems
            int viewSize = Size.Width; // view is square, so we can just use width
            e.Graphics.TranslateTransform(viewSize / 2, viewSize / 2);

            // Draw the player
            lock (player)
            {
                foreach (SnakePlayer.Segment seg in player.GetSegmentList())
                {
                    DrawObjectWithTransform(e, seg, seg.x, seg.y,
                        0, SegmentDrawer);
                }
            }

            DrawObjectWithTransform(e, food, food.x, food.y, 0, FoodDrawer);


            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
