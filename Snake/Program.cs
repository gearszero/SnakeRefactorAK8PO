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
            Random randomNumber = new Random();
            ScreenArea screenArea = new ScreenArea(32, 16);
            //Pixel Positions
            PixelsCoordination snakePixelsCoordination = new PixelsCoordination(screenArea.ScreenWidth/2, screenArea.ScreenHeight/2, ConsoleColor.Red);
            PixelsCoordination berryPosition = new PixelsCoordination(randomNumber.Next(0, screenArea.ScreenWidth),
                randomNumber.Next(0, screenArea.ScreenHeight), ConsoleColor.Yellow);
            int score = 5;
            bool gameOver = false;
            string movement = "RIGHT";
            List<int> xposlijf = new List<int>();
            List<int> yposlijf = new List<int>();

            DateTime tijd = DateTime.Now;
            DateTime tijd2 = DateTime.Now;
            string buttonpressed = "no";
            while (true)
            {
                Console.Clear();
                gameOver = CheckSnakeOutOfBounds(snakePixelsCoordination, screenArea);
                GenerateScreenBorder(screenArea);
                Console.ForegroundColor = ConsoleColor.Green;
                score += CheckIfBerryWasPickedUp(snakePixelsCoordination, berryPosition, screenArea);
                for (int i = 0; i < xposlijf.Count(); i++)
                {
                    Console.SetCursorPosition(xposlijf[i], yposlijf[i]);
                    Console.Write("■");
                    if (xposlijf[i] == snakePixelsCoordination.XPosition && yposlijf[i] == snakePixelsCoordination.YPosition)
                    {
                        gameOver = true;
                    }
                }
                if (gameOver)
                {
                    break;
                }
                Console.SetCursorPosition(snakePixelsCoordination.XPosition, snakePixelsCoordination.YPosition);
                Console.ForegroundColor = snakePixelsCoordination.GameConsoleColor;
                Console.Write("■");
                Console.SetCursorPosition(berryPosition.XPosition, berryPosition.YPosition);
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
                xposlijf.Add(snakePixelsCoordination.XPosition);
                yposlijf.Add(snakePixelsCoordination.YPosition);
                switch (movement)
                {
                    case "UP":
                        snakePixelsCoordination.YPosition--;
                        break;
                    case "DOWN":
                        snakePixelsCoordination.YPosition++;
                        break;
                    case "LEFT":
                        snakePixelsCoordination.XPosition--;
                        break;
                    case "RIGHT":
                        snakePixelsCoordination.XPosition++;
                        break;
                }
                if (xposlijf.Count() > score)
                {
                    xposlijf.RemoveAt(0);
                    yposlijf.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenArea.ScreenWidth / 5, screenArea.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenArea.ScreenWidth / 5, screenArea.ScreenHeight / 2 +1);
        }

        private static int CheckIfBerryWasPickedUp(PixelsCoordination berryPosition, PixelsCoordination snakePixelsCoordination,ScreenArea screenArea)
        {
            Random randomNumber = new Random();
            if (berryPosition.XPosition == snakePixelsCoordination.XPosition && berryPosition.YPosition == snakePixelsCoordination.YPosition)
            {
                berryPosition.XPosition = randomNumber.Next(1, screenArea.ScreenWidth-2);
                berryPosition.YPosition = randomNumber.Next(1, screenArea.ScreenHeight-2);
                return 1;
            }

            return 0;
        }

        static void GenerateScreenBorder(ScreenArea screenArea)
        {
            for (int i = 0; i < screenArea.ScreenWidth; i++)
            {
                Console.SetCursorPosition (i, 0);
                Console.Write("■");

                Console.SetCursorPosition (i, screenArea.ScreenHeight - 1);
                Console.Write ("■");
            }

            for (int i = 0; i < screenArea.ScreenHeight; i++)
            {
                Console.SetCursorPosition (0, i);
                Console.Write ("■");

                Console.SetCursorPosition (screenArea.ScreenWidth - 1, i);
                Console.Write ("■");
            }
        }

        static bool CheckSnakeOutOfBounds(PixelsCoordination currentSnakePosition, ScreenArea screenArea)
        {
            if (currentSnakePosition.XPosition == screenArea.ScreenWidth-1 || currentSnakePosition.XPosition == 0 ||currentSnakePosition.YPosition == screenArea.ScreenWidth-1 || currentSnakePosition.YPosition == 0)
            { 
                return true;
            }

            return false;
        }

        class PixelsCoordination
        {
            public PixelsCoordination(int xPosition, int yPosition, ConsoleColor color)
            {
                XPosition = xPosition;
                YPosition = yPosition;
                GameConsoleColor = color;
            }

            public int XPosition { get; set; }
            public int YPosition { get; set; }
            public ConsoleColor GameConsoleColor { get; set; }
        }

        class ScreenArea
        {
            public ScreenArea(int screenWidth, int screenHeight)
            {
                ScreenWidth = screenWidth;
                ScreenHeight = screenHeight;
                SetScreenWidth(ScreenWidth,ScreenHeight);
            }

            public int ScreenWidth { get; private set; }
            public int ScreenHeight { get; private set; }
            
            //functions
            private static void SetScreenWidth(int screenWidth, int screenHeight)
            {
                Console.WindowWidth = screenWidth;
                Console.WindowHeight = screenHeight;
            }
        }
    }
}