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
			int score = 0;
            Console.WriteLine("Kirjuta oma nimi >>> ");
            string name = Console.ReadLine();
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

			//работа с файлом
            //string[] v = new string[5];
            //v[0] = name;
            //v[1] = "ZXC";
            //StreamWriter to_file = new StreamWriter("Gamers.txt", true);
            //for (int i = 0; i < v.Length; i++)
            //{
            //    to_file.WriteLine(v[i]);
            //}

            //to_file.Close();
            //StreamReader from_file = new StreamReader("Gamers.txt");
            //string text = from_file.ReadToEnd();
            //Console.WriteLine(text);
            //from_file.Close();

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
			WriteText($" {name}, your score: {score}", xOffset, yOffset++);
			WriteText("=====================", xOffset, yOffset++);
		}
		static void WriteText(String text, int xOffset, int yOffset)
		{
			Console.SetCursorPosition(xOffset, yOffset);
			Console.WriteLine(text);
		}
	}
}
