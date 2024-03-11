using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            int score = 5;
            Random randomNumber = new Random();
            bool gameOver = false;
            Pixel snakePixel = new Pixel(screenWidth/2, screenHeight/2, ConsoleColor.Red);
            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();
            int berryx = randomNumber.Next(0, screenWidth);
            int berryy = randomNumber.Next(0, screenHeight);
            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";
            while (true)
            {
                Console.Clear();
                if (snakePixel.XPos == screenWidth-1 || snakePixel.XPos == 0 ||snakePixel.YPos == screenHeight-1 || snakePixel.YPos == 0)
                { 
                    gameOver = true;
                }
                GenerateScreenBorder(screenWidth,screenHeight);
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryx == snakePixel.XPos && berryy == snakePixel.YPos)
                {
                    score++;
                    berryx = randomNumber.Next(1, screenWidth-2);
                    berryy = randomNumber.Next(1, screenHeight-2);
                } 
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == snakePixel.XPos && yposlijf[i] == snakePixel.YPos)
                    {
                        gameOver = true;
                    }
                }
                if (gameOver)
                {
                    break;
                }
                Console.SetCursorPosition(snakePixel.XPos, snakePixel.YPos);
                Console.ForegroundColor = snakePixel.GameConsoleColor;
                Console.Write("■");
                Console.SetCursorPosition(berryx, berryy);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                tijd = DateTime.Now;
                buttonpressed = "no";
                while (true)
                {
                    tijd2 = DateTime.Now;
                    if (tijd2.Subtract(tijd).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo toets = Console.ReadKey(true);
                        //Console.WriteLine(toets.Key.ToString());
                        if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonpressed == "no")
                        {
                            movement = "UP";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonpressed == "no")
                        {
                            movement = "DOWN";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonpressed == "no")
                        {
                            movement = "LEFT";
                            buttonpressed = "yes";
                        }
                        if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonpressed == "no")
                        {
                            movement = "RIGHT";
                            buttonpressed = "yes";
                        }
                    }
                }
                xposlijf.Add(snakePixel.XPos);
                yposlijf.Add(snakePixel.YPos);
                switch (movement)
                {
                    case "UP":
                        snakePixel.YPos--;
                        break;
                    case "DOWN":
                        snakePixel.YPos++;
                        break;
                    case "LEFT":
                        snakePixel.XPos--;
                        break;
                    case "RIGHT":
                        snakePixel.XPos++;
                        break;
                }
                if (xposlijf.Count() > score)
                {
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 +1);
        }

        static void GenerateScreenBorder(int screenWidth, int screenHeight)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition (i, 0);
                Console.Write ("■");
            }

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition (i, screenHeight - 1);
                Console.Write ("■");
            }

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition (0, i);
                Console.Write ("■");
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition (screenWidth - 1, i);
                Console.Write ("■");
            }
        }

        class Pixel
        {
            public Pixel(int xPos, int yPos, ConsoleColor color)
            {
                XPos = xPos;
                YPos = yPos;
                GameConsoleColor = color;
            }

            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor GameConsoleColor { get; set; }
        }
    }
}