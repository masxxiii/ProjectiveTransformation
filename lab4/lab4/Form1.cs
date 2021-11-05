using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
		//Matrix for figure
		float[,] figure = new float[,] {
			{ -65, -65, 30, 1 },
			{ 65, -65, 30, 1 },
			{ -25, -25, 30, 1 },
			{ -65, 60, 30, 1},
			{ -65, -65, -30, 1 },
			{ 65, -65, -30, 1 },
			{ -25, -25, -30, 1 },
			{ -65, 60, -30, 1}
		};

		//Matrix for axonometric trimetic projection
		float[,] axioTriProject = new float[,] {
			{ 0.866F, 0.354F, 0, 0 },
			{ 0, 0.707F, 0, 0 },
			{ 0.5F, -0.612F, 0, 0 },
			{ 0, 0, 0, 1}
		};

		//Reflection matrix in space XOY
		float[,] XOY = new float[,]
		{
			{ 1, 0, 0, 0 },
			{ 0, 1, 0, 0 },
			{ 0, 0, -1, 0 },
			{ 0, 0, 0, 1 }
		};

		//Reflection matrix in space YOZ
		float[,] YOZ = new float[,]
		{
			{ -1, 0, 0, 0 },
			{ 0, 1, 0, 0 },
			{ 0, 0, 1, 0 },
			{ 0, 0, 0, 1 }
		};

		//Reflection matrix in space ZOX
		float[,] ZOX = new float[,]
		{
			{ 1, 0, 0, 0 },
			{ 0, -1, 0, 0 },
			{ 0, 0, 1, 0 },
			{ 0, 0, 0, 1 }
		};

		public Form1()
        {
            InitializeComponent();
			pictureBox1.Paint += PictureBox1_Paint;
		}

		private void PictureBox1_Paint(object sender, PaintEventArgs e)
		{
			int w = pictureBox1.ClientSize.Width / 2;
			int h = pictureBox1.ClientSize.Height / 2;
			e.Graphics.TranslateTransform(w, h);
			DrawXAxis(new Point(0, 0), new Point(w, 0), e.Graphics);
			DrawYAxis(new Point(0, 0), new Point(0, -h), e.Graphics);
			DrawZAxis(e.Graphics);
			e.Graphics.FillEllipse(Brushes.Red, -2, -2, 4, 4);
		}

		//Drawing X-axis
		private void DrawXAxis(Point start, Point end, Graphics g)
		{
			//Отрисовка оси
			g.DrawLine(Pens.Black, start, end);
			//Отрисовка делений по оси
			for (int i = start.X; i < end.X; i += 10)
			{
				g.DrawLine(Pens.Black, i, -3, i, 3);
			}
		}

		//Drawing Y-axis
		private void DrawYAxis(Point start, Point end, Graphics g)
		{
			g.DrawLine(Pens.Black, start, end);
			for (int i = start.Y; i > end.Y; i -= 10)
			{
				g.DrawLine(Pens.Black, -3, i, 3, i);
			}
		}

		//Drawing Z-axis
		private void DrawZAxis(Graphics g)
		{
			g.DrawLine(Pens.Black, 0, 0, -(pictureBox1.ClientSize.Width / 2), (pictureBox1.ClientSize.Height / 2));
			for (int i = 0; i > -(pictureBox1.ClientSize.Width / 2); i -= 10)
			{
				for (int j = 0; j < (pictureBox1.ClientSize.Height / 2); j += 10)
				{
					if (i == -j)
					{
						g.DrawLine(Pens.Black, i - 2, j - 2, i + 2, j + 2);
					}
				}

			}
		}

		//Method for drawing figure
		private void DrawFigure()
		{
			Graphics g = pictureBox1.CreateGraphics();
			Pen pen = new Pen(Color.Red);
			int w = pictureBox1.ClientSize.Width / 2;
			int h = pictureBox1.ClientSize.Height / 2;
			g.TranslateTransform(w, h);
			for (int i = 0; i < 4; i++)
			{
				if (i == 3)
				{
					g.DrawLine(pen, figure[i, 0], figure[i, 1], figure[0, 0], figure[0, 1]);
				}
				else
				{
					g.DrawLine(pen, figure[i, 0], figure[i, 1], figure[i + 1, 0], figure[i + 1, 1]);
				}

			}
			for (int i = 4; i < 8; i++)
			{
				if (i == 7)
				{
					g.DrawLine(pen, figure[i, 0] - figure[i, 2], figure[i, 1] + figure[i, 2], figure[4, 0] - figure[i, 2], figure[4, 1] + figure[i, 2]);
				}
				else
				{
					g.DrawLine(pen, figure[i, 0] - figure[i, 2], figure[i, 1] + figure[i, 2], figure[i + 1, 0] - figure[i + 1, 2], figure[i + 1, 1] + figure[i + 1, 2]);
				}

			}
			for (int i = 0; i < 4; i++)
			{
				g.DrawLine(pen, figure[i, 0], figure[i, 1], figure[4 + i, 0] - figure[4 + i, 2], figure[4 + i, 1] + figure[4 + i, 2]);
			}
			g.Dispose();
		}

		//Method for multiplying two matrixes
		static float[,] Multiplication(float[,] a, float[,] b)
		{
			float[,] result = new float[a.GetLength(0), b.GetLength(1)];
			for (int i = 0; i < a.GetLength(0); i++)
			{
				for (int j = 0; j < b.GetLength(1); j++)
				{
					for (int k = 0; k < b.GetLength(0); k++)
					{
						result[i, j] += a[i, k] * b[k, j];
					}
				}
			}
			return result;
		}

		//Method for performing Axiometric projection
		private void PerformAxio()
        {
			Refresh();
			figure = Multiplication(figure, axioTriProject);
			DrawFigure();
		}

		//Button for drawing
		private void button1_Click(object sender, EventArgs e)
		{
			DrawFigure();
		}

		//Button for applying Axiometric projection
        private void button2_Click(object sender, EventArgs e)
        {
			PerformAxio();
        }

		//Button for XOY
        private void button3_Click(object sender, EventArgs e)
        {
			Refresh();
			figure = Multiplication(figure, XOY);
			DrawFigure();
		}

		//Button for YOZ
        private void button4_Click(object sender, EventArgs e)
        {
			Refresh();
			figure = Multiplication(figure, YOZ);
			DrawFigure();
		}

		//Button for ZOX
		private void button5_Click(object sender, EventArgs e)
        {
			Refresh();
			figure = Multiplication(figure, ZOX);
			DrawFigure();
		}
    }
}
