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

        //pattern values
        bool moveRight = true;
        int xChange = 20;

        int newBoxCounter = 0;

        int speedChangeTimer = 200;
        int gapTimer = 250;

        int boxSpeed = 10;

        //hero values
        Box hero;


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

            hero = new Box(this.Width / 2 - 15, this.Height - 100, 30, 10, new SolidBrush(Color.Gold));
        }

        public void CreateBox(int x)
        {// coloring boxes
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
            Box b = new Box(x, 0, 30, boxSpeed, boxBrush);
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
            if (rightArrowDown)
            {
                hero.move("Right");
            }
            else if (leftArrowDown)
            {
                hero.move("Left");
            }
            //update location of all boxes (drop down screen)
            foreach (Box b in boxes)
            {
                b.move();
            }

            //remove box if it has gone of screen
            if (boxes[0].y > this.Height)
            {
                boxes.RemoveAt(0);
            }

            //add new box if it is time
            if (newBoxCounter == 6)
            {
                if (moveRight == true)
                {
                    xLeft += xChange;
                }
                else
                {
                    xLeft -= xChange;
                }
                CreateBox(xLeft);
                CreateBox(xLeft + gap);

                xChange--;

                if (xChange == 0)
                {
                    moveRight = !moveRight;
                    xChange = randGen.Next(15, 30);
                }
                newBoxCounter = 0;
            }
            //Check for colisions between hero and boxes

            foreach (Box b in boxes)
            {
                if (hero.colision(b))
                {
                    gameLoop.Enabled = false;
                }
            }
            //check to see if its time to increase speed
            if(speedChangeTimer == 0 && boxSpeed < 20)
            {
                boxSpeed += 2;
                speedChangeTimer = 200;
            }
            //chcek to see if time to decrease gap size
            if (gapTimer == 0 && gap > 100)
            {
                gap -= 50;
                gapTimer = 250;
            }

            Refresh();
            speedChangeTimer--;
            gapTimer--;
        }
        
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = !true;
                    break;
                case Keys.Right:
                    rightArrowDown = !true;
                    break;
            }
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw boxes to screen
            foreach (Box b in boxes)
            {
                e.Graphics.FillRectangle(b.brushColour, b.x, b.y, b.size, b.size);
            }

            e.Graphics.FillRectangle(hero.brushColour, hero.x, hero.y, hero.size, hero.size);
        }
    }
}
