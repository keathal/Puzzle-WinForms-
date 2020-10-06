using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace puzzle
{
    public partial class Form1 : Form
    {
        Point emptyPoint;
        Game game = new Game();
        Timer timer=new Timer();
        public Form1()
        {
            emptyPoint.X = game.settings.width* ((int)Math.Sqrt(game.settings.amount)-1);
            emptyPoint.Y = game.settings.width * ((int)Math.Sqrt(game.settings.amount)-1);
            InitializeComponent();
            timer.Interval = 3000;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            foreach (Button  btn in panel1.Controls)
            {
                btn.Enabled = true;
            }
            Image original = Image.FromFile(GetImage());
            game.cropImage(original, 420);
            addImagesToButtons(game.suffle(game.settings.amount), game.settings.amount);
        }

         void addImagesToButtons(Image[] curImages, int count)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < count-1; i++)
            {
                list.Add(i);
            }
            int j = 0;
            foreach(Button btn in panel1.Controls)
            {
                if(j<list.Count)
                {
                    btn.Image = curImages[list[j]];
                    j++;
                }
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if(game.moveButton(btn.Location.X, btn.Location.Y))
            {
                Point swap = btn.Location;
                btn.Location = emptyPoint;
                emptyPoint = swap;
                string text = "";
                for (int i = 0; i < game.settings.amount; i++)
                {
                    text += " Puzzle " + i + " goes to " + game.curPuzzles[i].id;
                }
               // t_test.Text = text;
                l_steps.Text = "Шагов сделано: " + game.settings.steps;
                if (game.checkValid() != "")
                    t_test.Text = game.checkValid();
            } 
            
        }

        private void check_Click(object sender, EventArgs e)
        {
            foreach (Button btn in panel1.Controls)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 2;
                if (game.checkButton(btn.Location.X, btn.Location.Y))
                    btn.FlatAppearance.BorderColor = Color.Green;
                else
                    btn.FlatAppearance.BorderColor = Color.Red;

                timer.Enabled = true;
                timer.Tick += Timer_Tick;

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (Button btn in panel1.Controls)
            {
                btn.FlatAppearance.BorderSize = 0;
            }
            timer.Enabled = false;
        }

        static string GetImage()
        {
            List<string> img = new List<string>();
            img.Add(@"D:\puzzle\puzzle\original.JPG");
            img.Add(@"D:\puzzle\puzzle\2.jpeg");
            img.Add(@"D:\puzzle\puzzle\3.jpg");
            img.Add(@"D:\puzzle\puzzle\4.jpg");
            img.Add(@"D:\puzzle\puzzle\5.jpg");
            img.Add(@"D:\puzzle\puzzle\6.png");
            Random rnd = new Random();
            return (img[rnd.Next(0, img.Count - 1)]);
        }
    }
}
