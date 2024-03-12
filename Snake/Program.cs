using System.Diagnostics;
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

static class Program
{
    static void Main(string[] args)
    {
        var randomNumber = new Random();
        var screenArea = new ScreenArea(32, 16);

        //Pixel Positions
        var snakeHeadPosition = new PixelsCoordination(screenArea.ScreenWidth / 2,
            screenArea.ScreenHeight / 2, ConsoleColor.Red);
        var berryPosition = new PixelsCoordination(randomNumber.Next(0, screenArea.ScreenWidth),
            randomNumber.Next(0, screenArea.ScreenHeight), ConsoleColor.Yellow);
        var snakeBodyPosition = new List<PixelsCoordination>();

        var currentDirection = Direction.Right;
        var gameScore = 5;


        while (true)
        {
            Console.Clear();
            GenerateScreenBorder(screenArea);
            if (CheckIfBerryWasPickedUp(berryPosition, snakeHeadPosition))
            {
                berryPosition = GenerateNewBerryPosition(screenArea);
                ++gameScore;
            }

            var gameOver = CheckSnakeOutOfBounds(snakeHeadPosition, screenArea) ||
                           CheckSnakeSelfCollisionAndGenerateBody(snakeBodyPosition, snakeHeadPosition);
            if (gameOver)
            {
                break;
            }

            DrawPixel(snakeHeadPosition);
            DrawPixel(berryPosition);

            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds <= 200)
            {
                currentDirection = KeyInputChangeDirection(currentDirection);
            }

            snakeBodyPosition.Add(new PixelsCoordination(snakeHeadPosition.XPosition, snakeHeadPosition.YPosition, ConsoleColor.Green));

            ChangeDirectionOfSnake(snakeHeadPosition, currentDirection);

            if (snakeBodyPosition.Count() > gameScore)
            {
                snakeBodyPosition.RemoveAt(0);
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

    private static PixelsCoordination GenerateNewBerryPosition(ScreenArea screenArea)
    {
        var randomNumber = new Random();
        var berryPosition = new PixelsCoordination(randomNumber.Next(2, screenArea.ScreenWidth - 2),
            randomNumber.Next(2, screenArea.ScreenHeight - 2), ConsoleColor.Yellow);
        DrawPixel(berryPosition);
        return berryPosition;
    }

    private static void GenerateScreenBorder(ScreenArea screenArea)
    {
        var borderCoordination = new PixelsCoordination(0, 0, ConsoleColor.Cyan);
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

    private class PixelsCoordination
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
    }

    private class ScreenArea
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