using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace puzzle
{
    class Game
    {
        public Settings settings = new Settings();
        public Puzzle[] puzzles;
        public Puzzle[] curPuzzles;
        public int maxX { get; set; }
        ArrayList images = new ArrayList();

        public Game()
        {
            puzzles = new Puzzle[settings.amount];
            curPuzzles = new Puzzle[settings.amount];
            maxX = settings.width * (int)Math.Sqrt(settings.amount);
        }
        public ArrayList cropImage(Image img, int width)
        {
            Bitmap bmp = new Bitmap(width, width);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.DrawImage(img, 0, 0, width, width);
            graphic.Dispose();

            int movr = 0, movd = 0;
            for (int x = 0; x < settings.amount; x++)
            {
                Bitmap piece = new Bitmap(settings.width, settings.width);
                for (int i = 0; i < settings.width; i++)
                    for (int j = 0; j < settings.width; j++)
                    {
                        piece.SetPixel(i, j, bmp.GetPixel(i + movr, j + movd));
                    }

                images.Add(piece);
                puzzles[x] = new Puzzle(piece, x);
                puzzles[x].X = x % (int)Math.Sqrt(settings.amount);
                puzzles[x].Y = x / (int)Math.Sqrt(settings.amount);
                movr += settings.width;
                if (movr == maxX)
                {
                    movr = 0;
                    movd += settings.width;
                }
            }
            return images;
        }
        public Image[] suffle(int count)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            Random rnd = new Random();
            list = list.OrderBy(x => rnd.Next()).ToList();
            Image[] returnImgs = new Image[puzzles.Length];
            foreach (int i in list)
            {
                curPuzzles[i] = puzzles[list[i]];
                curPuzzles[i].X = i % (int)Math.Sqrt(settings.amount);
                curPuzzles[i].Y = i / (int)Math.Sqrt(settings.amount);
                returnImgs[i] = curPuzzles[i].img;
            }
            curPuzzles[settings.amount - 1].isempty = true;
            curPuzzles[settings.amount - 1].X = (int)Math.Sqrt(settings.amount) - 1;
            curPuzzles[settings.amount - 1].Y = (int)Math.Sqrt(settings.amount) - 1;
            return returnImgs;
        }
        public Image[] getImages()
        {
            Image[] returnImgs = new Image[puzzles.Length];
            int i = 0;
            foreach (Puzzle item in curPuzzles)
            {
                if (item != null)
                    returnImgs[i] = item.img;
                else
                    returnImgs[i] = null;
                i++;
            }
            return returnImgs;
        }
        public string checkValid()
        {
            int count = 0;

            for (int i = 0; i < settings.amount; i++)
            {
                if (i == curPuzzles[i].id)
                    count++;
            }

            if (count >= settings.amount-1)
                return "you win!";
            else
                return "";
        }
        public bool moveButton(int x, int y)
        {
            x = x / settings.width;
            y = y / settings.width;
            int id = y * 3 + x;
            Puzzle item = curPuzzles[id];
            Puzzle empty = curPuzzles.Where(b => b.isempty == true).FirstOrDefault();
            if (item != null && empty!=null)
            {
                if (((x == empty.X - 1 || x == empty.X + 1) &&
                y == empty.Y)
                || ((y == empty.Y - 1 || y == empty.Y + 1) &&
                x == empty.X))
                {
                    int emptyID = empty.Y * (int)Math.Sqrt(settings.amount) + empty.X;
                    curPuzzles[id] = empty;
                    curPuzzles[id].X = x; curPuzzles[id].Y = y;

                    curPuzzles[emptyID] = item;
                    curPuzzles[emptyID].X = empty.X; curPuzzles[emptyID].Y = empty.Y;
                    settings.steps++;
                    return true;
                }
                else
                    return false;
            }
                
            else return false;
        }
        public bool checkButton(int x, int y)
        {
            x = x / settings.width;
            y = y / settings.width;
            int id = y * 3 + x;
            if (id == curPuzzles[id].id)
                return true;
            else
                return false;
        }
    }
}
