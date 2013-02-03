using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using LeapWrapper;
using Leap;

namespace LeapPaint
{
    public partial class LeapPaintMain : Form
    {
        bool isDrawing;
        bool isDeleting= false;
        // our collection of strokes for drawing
        //List<List<Point>> _strokes = new List<List<Point>>();
        List<List<stroke>> _strokes = new List<List<stroke>>();
        // the current stroke being drawn
        //List<Point> _currStroke;
        List<stroke> _currStroke;
        // our pen
        Pen _pen = new Pen(Color.Red, 2);
        Leap.Controller controller;
        LeapListerner listener;
        int zTrigger;
        public LeapPaintMain()
        {
            InitializeComponent();
            // Create a sample listener and controller
            listener = new LeapWrapper.LeapListerner();
            listener.sensetivity = 3;
            hScrollBar1.Value = 100;
            zTrigger = hScrollBar1.Value;
            lblzTrigger.Text = hScrollBar1.Value.ToString();
            controller = new Leap.Controller();
            controller.AddListener(listener);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LeapPaintMain_Paint(object sender, PaintEventArgs e)
        {
        //       System.Drawing.Graphics graphicsObj;

        //    graphicsObj = this.CreateGraphics();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // now handle and redraw our strokes on the paint event
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen myPen = new Pen(Color.Red);
            foreach (List<stroke> stroke in _strokes)
            {
                foreach(stroke MyStroke in stroke)
                {
                    //Pen myPen = new Pen(MyStroke.PenColor, MyStroke.PenWidth[stroke.IndexOf(MyStroke)]);
                    //e.Graphics.DrawLines(myPen,MyStroke._stroke.ToArray());
                    myPen.Color = MyStroke.PenColor;
                        for (int i = 0; i < MyStroke._stroke.Count-1; i = i + 1)
                        {
                            myPen.Width = MyStroke.PenWidth[i];
                            e.Graphics.DrawLines(myPen, MyStroke._stroke.GetRange(i, 2).ToArray()); 
                        }

                }
                //Pen MyPen = new Pen(
                //e.Graphics.DrawLines(_pen, stroke.ToArray());
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawing == false)
            {
                isDrawing = true;
                // mouse is down, starting new stroke
                //_currStroke = new List<Point>();
                _currStroke = new List<stroke>();
                stroke MyStroke = new stroke();
                MyStroke._stroke = new List<Point>();
                MyStroke.PenWidth = new List<float>();
                MyStroke._stroke.Add(e.Location);
                MyStroke.PenColor = _pen.Color;
                //MyStroke.PenWidth = _pen.Width;
                MyStroke.PenWidth.Add(_pen.Width);

                // add the initial point to the new stroke
                _currStroke.Add(MyStroke);
                // add the new stroke collection to our strokes collection
                _strokes.Add(_currStroke); 
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //Frame frame = controller.Frame(0);
            //Tool tool = frame.Tools[0];
            //Leap.Screen screen = controller.CalibratedScreens[0];
            //Vector vic = screen.Intersect(tool, true);
            
            //int iScreenX = screen.WidthPixels;
            //int iScreenY = screen.HeightPixels;
            //double z = screen.DistanceToPoint(tool.TipPosition);
            double z = listener.zDistance;
            //lblX.Text = (vic.x * iScreenX).ToString();
            lblX.Text = System.Windows.Forms.Cursor.Position.X.ToString();
            lblY.Text = System.Windows.Forms.Cursor.Position.Y.ToString();
            //lblY.Text = (iScreenY - vic.y * iScreenY).ToString();
            lblZ.Text = z.ToString();
            if (z < zTrigger && z != 0.0)
            {
                pictureBox1_MouseDown(sender, e);
            }
            else
            {

                pictureBox1_MouseUp(sender,e);
            }
            //if (z < -0.96 && z != 0)
            //{
            //    pictureBox1_MouseUp(sender, e);
            //}

            if (isDrawing)
            {
                // record stroke point if we're in drawing mode
                stroke MyStroke = _currStroke[_currStroke.Count - 1];
                //MyStroke._stroke = new List<Point>();
                MyStroke._stroke.Add(e.Location);
                MyStroke.PenColor = _pen.Color;
                int MAX_DEPTH = zTrigger - 40;
                int MAX_PEN_WIDTH = 10;
                float iWidth;
                if (z <= zTrigger)
                {
                    if (!isDeleting)
                        iWidth = Math.Abs((float)(zTrigger - z) / (zTrigger - MAX_DEPTH) * MAX_PEN_WIDTH + 2);
                    else
                        iWidth = 20;
                   
                }
                else
                    iWidth = 2;

                //MyStroke.PenWidth = iWidth;
                MyStroke.PenWidth.Add(iWidth);
                
                Refresh(); // refresh the drawing to see the latest section
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //isDrawing = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            //isDrawing = false;
        }

        private void LeapPaintMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Remove the sample listener when done
            controller.RemoveListener(listener);
            controller.Dispose();
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            isDeleting = false;
            double z = listener.zDistance;
            if (z < zTrigger) { _pen.Color = Color.Blue; pictureBox2.BackColor = Color.GhostWhite; }
            else pictureBox2.BackColor = Color.Blue;
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            isDeleting = false;
            double z = listener.zDistance;
            if (z < zTrigger) { _pen.Color = Color.Red; pictureBox3.BackColor = Color.GhostWhite; }
            else pictureBox3.BackColor = Color.Red;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_MouseMove(object sender, MouseEventArgs e)
        {
            isDeleting = false;
            double z = listener.zDistance;
            if (z < zTrigger) { _pen.Color = Color.Yellow; pictureBox4.BackColor = Color.GhostWhite; }
            else pictureBox4.BackColor = Color.Yellow;
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            isDeleting = false;
            double z = listener.zDistance;
            if (z < zTrigger) { _pen.Color = Color.Lime; pictureBox5.BackColor = Color.GhostWhite; }
            else pictureBox5.BackColor = Color.Lime;
        }

        private void pictureBox6_MouseMove(object sender, MouseEventArgs e)
        {
            isDeleting = false;
            double z = listener.zDistance;
            _pen.Color = Color.Red;

            if (z < zTrigger)
            {
                _strokes = new List<List<stroke>>(); 
                pictureBox1.Refresh();
                pictureBox6.BackColor = Color.Black;
                label6.BackColor = Color.Black;
                label6.ForeColor = Color.White;
            }
            else
            {
                pictureBox6.BackColor = Color.DarkGray;
                label6.BackColor = Color.DarkGray;
                label6.ForeColor = Color.Blue;
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            zTrigger = hScrollBar1.Value;
            lblzTrigger.Text = hScrollBar1.Value.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            double z = listener.zDistance;
            isDeleting = true;
            if (z < zTrigger)
            {
                _pen.Color = Color.Snow;
                pictureBox7.BackColor = Color.Black;
                label2.BackColor = Color.Black;
                label2.ForeColor = Color.White;
            }
            else
            {
                pictureBox7.BackColor = Color.DarkGray;
                label2.BackColor = Color.DarkGray;
                label2.ForeColor = Color.Blue;
            }

        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
             pictureBox2.BackColor = Color.Blue;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Red;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Yellow;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Lime;
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox7.BackColor = Color.DarkGray;
            label2.BackColor = Color.DarkGray;
            label2.ForeColor = Color.Blue;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.BackColor = Color.DarkGray;
            label6.BackColor = Color.DarkGray;
            label6.ForeColor = Color.Blue;
        }
    }
}
