using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoxField
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys
        Boolean leftArrowDown, rightArrowDown;

        //create a list to hold a column of boxes   
        List<Box> boxes = new List<Box>();

        //starting x positions for boxes
        int xLeft = 250;
        int gap = 300;

        int newBoxCounter = 0;


        Random randGen = new Random();

        public GameScreen()
        {
            InitializeComponent();
            OnStart();
        }

        /// <summary>
        /// Set initial game values here
        /// </summary>
        public void OnStart()
        {
            CreateBox(xLeft);
            CreateBox(xLeft + gap);
        }

        public void CreateBox(int x)
        {
            SolidBrush boxBrush = new SolidBrush(Color.White);
            int colourValue = randGen.Next(1, 4);

            if (colourValue == 1)
            {
                boxBrush = new SolidBrush(Color.Red);
            }
            else if (colourValue == 2)
            {
                boxBrush = new SolidBrush(Color.Yellow);
            }
            else
            {
                boxBrush = new SolidBrush(Color.Orange);
            }

            //Box(x, y, size, speed, brush)
            Box b = new Box(x, 0, 50, 10, boxBrush);
            boxes.Add(b);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;           
            }
        }


        private void gameLoop_Tick(object sender, EventArgs e)
        {
            newBoxCounter++;

            //update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.y += b.speed;
            }

            //remove box if it has gone of screen
            if (boxes[0].y > 200)
            {
                boxes.RemoveAt(0);   
            }

            //add new box if it is time
            if (newBoxCounter == 10)
            {
                CreateBox(xLeft);
                CreateBox(xLeft + gap);

                newBoxCounter = 0;
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach(Box b in boxes)
            {
                e.Graphics.FillRectangle(b.brushColour, b.x, b.y, b.size, b.size);          
            }
        }
    }
}
