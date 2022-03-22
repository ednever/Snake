using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;
using System.IO;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            
            

            Thread.Sleep(2000);

            string[] menuItems = new string[] { "Start", "Author", "Players", "Quit" };
            Console.SetWindowSize(80, 25);
            Console.WriteLine("SNAKE GAME \n");
            int row = Console.CursorTop;
            int col = Console.CursorLeft;
            int index = 0;
            while (true)
            {
                Console.SetCursorPosition(col, row);
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == index)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(menuItems[i]);
                    Console.ResetColor();
                }
                Console.WriteLine();
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < menuItems.Length - 1)
                            index++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (index > 0)
                            index--;
                        break;
                    case ConsoleKey.Enter:
                        switch (index)
                        {
                            case 0:
                                int score = 0;
                                Console.Write("Kirjuta oma nimi >>> ");
                                string name = Console.ReadLine();
                                Console.Clear();


                                Walls walls = new Walls(80, 25);
                                walls.Draw();

                                //Прорисовка фигуры "змейка"			
                                Point p = new Point(4, 5, '&');
                                Console.ForegroundColor = ConsoleColor.Green;
                                Snake snake = new Snake(p, 4, Direction.RIGHT);

                                snake.Draw();

                                //создание точки "еда"
                                FoodCreator foodCreator = new FoodCreator(80, 25, '*');
                                Point food = foodCreator.CreateFood();
                                food.Draw();

                                //процесс игры
                                while (true)
                                {
                                    //если фигура "стена" и фигура "хвост змеи" касается фигуры "змейка" игра прекращается
                                    if (walls.IsHit(snake) || snake.IsHitTail())
                                    {
                                        break;
                                    }
                                    //если змея ест еду очки увеличиваются
                                    if (snake.Eat(food))
                                    {
                                        food = foodCreator.CreateFood();
                                        food.Draw();
                                        score++;
                                    }
                                    //если события не происходят, то фигура "змейка" движется
                                    else
                                    {
                                        snake.Move();
                                    }

                                    Thread.Sleep(100);
                                    if (Console.KeyAvailable)
                                    {
                                        ConsoleKeyInfo key = Console.ReadKey();
                                        snake.HandleKey(key.Key);
                                    }
                                }
                                WriteGameOver(score, name);
                                if (score >= 10)
                                {
                                    StreamWriter to_file = new StreamWriter("Gamers.txt", true);
                                    to_file.WriteLine(name + " - " + score);
                                    to_file.Close();
                                }                                
                                Thread.Sleep(2000);
                                Console.Clear();
                                break;
                            case 1:
                                Console.Clear();
                                Console.WriteLine($"Edgar Neverovski TARpv21");
                                Thread.Sleep(2000);
                                Console.Clear();
                                break;
                            case 2:
                                Console.Clear();
                                StreamReader from_file = new StreamReader("Gamers.txt");
                                string text = from_file.ReadToEnd();
                                Console.WriteLine(text);
                                from_file.Close();
                                Thread.Sleep(2000);
                                Console.Clear();
                                break;
                            case 3:
                                Console.WriteLine("Выбран выход из приложения");
                                return;
                        }
                        break;
                }
            }

            Console.ReadLine();
		}

		//функция вывода окончания игры
		static void WriteGameOver(int score, string name)
		{
			int xOffset = 25;
			int yOffset = 8;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(xOffset, yOffset++);
			WriteText("=====================", xOffset, yOffset++);
			WriteText(" G A M E    O V E R ", xOffset, yOffset++);
			WriteText($"{name}, your score: {score}", xOffset, yOffset++);
			WriteText("=====================", xOffset, yOffset++);
		}
        //функция вывода текста
        static void WriteText(String text, int xOffset, int yOffset)
		{
			Console.SetCursorPosition(xOffset, yOffset);
			Console.WriteLine(text);
		}
	}
}
