using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GRID_OFFSET = 25; 
        private const int GRID_LENGTH = 200; 
        private const int NUM_CELLS = 3;
        private const int CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
        private bool[,] grid;
        private Random rand;

        public MainForm()
        {
            InitializeComponent();

            rand = new Random();
            grid = new bool[NUM_CELLS, NUM_CELLS];

            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    grid[r, c] = true;
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    Brush brush;
                    Pen pen;

                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }

                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);

                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 1, CELL_LENGTH - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * NUM_CELLS + GRID_OFFSET ||
                e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * NUM_CELLS + GRID_OFFSET)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
            int c = (e.X - GRID_OFFSET) / CELL_LENGTH;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < NUM_CELLS && j >= 0 && j < NUM_CELLS)
                        grid[i, j] = !grid[i, j];
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }

        private bool PlayerWon()
        {
            bool winning = true;

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (grid[r, c])
                    {
                        winning = false;
                    }
                }
            }

            return winning;
        }

        private void newGameBtn_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < NUM_CELLS; r++)
                for (int c = 0; c < NUM_CELLS; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
            this.Invalidate(); 
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameBtn_Click(sender, e);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
    }
}
