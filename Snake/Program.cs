using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Program
{
    static void Main(string[] args)
    {
        Random randomNumber = new Random();
        ScreenArea screenArea = new ScreenArea(32, 16);

        //Pixel Positions
        PixelsCoordination snakeHead = new PixelsCoordination(screenArea.ScreenWidth / 2,
            screenArea.ScreenHeight / 2, ConsoleColor.Red);
        PixelsCoordination berryPosition = new PixelsCoordination(randomNumber.Next(0, screenArea.ScreenWidth),
            randomNumber.Next(0, screenArea.ScreenHeight), ConsoleColor.Yellow);
        List<PixelsCoordination> snakeBody = new List<PixelsCoordination>();

        Direction currentDirection = Direction.Right;
        int gameScore = 5;


        while (true)
        {
            Console.Clear();
            GenerateScreenBorder(screenArea);
            if (CheckIfBerryWasPickedUp(berryPosition, snakeHead))
            {
                berryPosition = GenerateNewBerryPosition(berryPosition, screenArea);
                ++gameScore;
            }

            var gameOver = CheckSnakeOutOfBounds(snakeHead, screenArea) ||
                           CheckSnakeSelfCollisionAndGenerateBody(snakeBody, snakeHead);
            if (gameOver)
            {
                break;
            }

            DrawPixel(snakeHead);
            DrawPixel(berryPosition);

            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds <= 500)
            {
                currentDirection = KeyInputChangeDirection(currentDirection);
            }

            snakeBody.Add(new PixelsCoordination(snakeHead.XPosition, snakeHead.YPosition, ConsoleColor.Green));

            ChangeDirectionOfSnake(snakeHead, currentDirection);

            if (snakeBody.Count() > gameScore)
            {
                snakeBody.RemoveAt(0);
            }
        }

        GenerateGameOverTag(screenArea, gameScore);
    }

    private static bool CheckIfBerryWasPickedUp(PixelsCoordination berryPosition,
        PixelsCoordination snakePixelsCoordination)
    {
        if (berryPosition.XPosition == snakePixelsCoordination.XPosition &&
            berryPosition.YPosition == snakePixelsCoordination.YPosition)
        {
            return true;
        }

        return false;
    }

    private static bool CheckSnakeOutOfBounds(PixelsCoordination currentSnakeHeadPosition, ScreenArea screenArea)
    {
        if (currentSnakeHeadPosition.XPosition == screenArea.ScreenWidth - 1 ||
            currentSnakeHeadPosition.XPosition == 0 ||
            currentSnakeHeadPosition.YPosition == screenArea.ScreenHeight - 1 ||
            currentSnakeHeadPosition.YPosition == 0)
        {
            return true;
        }

        return false;
    }

    private static bool CheckSnakeSelfCollisionAndGenerateBody(List<PixelsCoordination> snakeBody, PixelsCoordination snakeHead)
    {
        bool headCollide = false;
        for (int bodyPixel = 0; bodyPixel < snakeBody.Count(); ++bodyPixel)
        {
            DrawPixel(snakeBody[bodyPixel]);
            headCollide = (snakeBody[bodyPixel].XPosition == snakeHead.XPosition &&
                           snakeBody[bodyPixel].YPosition == snakeHead.YPosition);
        }

        return headCollide;
    }

    private static PixelsCoordination GenerateNewBerryPosition(PixelsCoordination berryPosition, ScreenArea screenArea)
    {
        Random randomNumber = new Random();
        berryPosition = new PixelsCoordination(randomNumber.Next(2, screenArea.ScreenWidth - 2),
            randomNumber.Next(2, screenArea.ScreenHeight - 2), ConsoleColor.Yellow);
        DrawPixel(berryPosition);
        return berryPosition;
    }

    private static void GenerateScreenBorder(ScreenArea screenArea)
    {
        PixelsCoordination borderCoordination = new PixelsCoordination(0, 0, ConsoleColor.Cyan);
        for (int currentPosition = 0; currentPosition < screenArea.ScreenWidth; currentPosition++)
        {
            DrawPixel(borderCoordination.ChangePixelsPosition(currentPosition, 0));
            DrawPixel(borderCoordination.ChangePixelsPosition(currentPosition, screenArea.ScreenHeight - 1));
        }

        for (int currentPosition = 0; currentPosition < screenArea.ScreenHeight; currentPosition++)
        {
            DrawPixel(borderCoordination.ChangePixelsPosition(0, currentPosition));
            DrawPixel(borderCoordination.ChangePixelsPosition(screenArea.ScreenWidth - 1, currentPosition));
        }
    }

    private static void GenerateGameOverTag(ScreenArea screenArea, int score)
    {
        Console.SetCursorPosition(screenArea.ScreenWidth / 5, screenArea.ScreenHeight / 2);
        Console.WriteLine("Game over, Score: " + score);
        Console.SetCursorPosition(screenArea.ScreenWidth / 5, screenArea.ScreenHeight / 2 + 1);
    }

    private static void DrawPixel(PixelsCoordination pixel)
    {
        Console.SetCursorPosition(pixel.XPosition, pixel.YPosition);
        Console.ForegroundColor = pixel.PixelColor;
        Console.Write("■");
        Console.SetCursorPosition(0, 0);
    }

    private static Direction KeyInputChangeDirection(Direction currentDirection)
    {
        if (Console.KeyAvailable)
        {
            var readKey = Console.ReadKey(true).Key;
            if (readKey == ConsoleKey.UpArrow && currentDirection != Direction.Down)
            {
                currentDirection = Direction.Up;
            }

            else if (readKey == ConsoleKey.DownArrow && currentDirection != Direction.Up)
            {
                currentDirection = Direction.Down;
            }

            else if (readKey == ConsoleKey.LeftArrow && currentDirection != Direction.Right)
            {
                currentDirection = Direction.Left;
            }

            else if (readKey == ConsoleKey.RightArrow && currentDirection != Direction.Left)
            {
                currentDirection = Direction.Right;
            }
        }

        return currentDirection;
    }

    private static void ChangeDirectionOfSnake(PixelsCoordination snakeHead, Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                snakeHead.YPosition--;
                break;
            case Direction.Down:
                snakeHead.YPosition++;
                break;
            case Direction.Left:
                snakeHead.XPosition--;
                break;
            case Direction.Right:
                snakeHead.XPosition++;
                break;
        }
    }

    class PixelsCoordination
    {
        public PixelsCoordination(int xPosition, int yPosition, ConsoleColor color)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            PixelColor = color;
        }

        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public ConsoleColor PixelColor { get; set; }

        //functions
        public PixelsCoordination ChangePixelsPosition(int newPositionX, int newPositionY)
        {
            return new PixelsCoordination(newPositionX, newPositionY, this.PixelColor);
        }

        public void ChangePixelLocation(int newLocationX, int newLocationY)
        {
            XPosition = newLocationX;
            YPosition = newLocationY;
        }
    }

    class ScreenArea
    {
        public ScreenArea(int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            SetScreenWidth(ScreenWidth, ScreenHeight);
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