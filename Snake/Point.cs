using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Point
    {
		public int x;
		public int y;
		public char sym;

		public Point()
		{
		}

		public Point(int x, int y, char sym)
		{
			this.x = x;
			this.y = y;
			this.sym = sym;
		}

		public Point(Point p)
		{
			x = p.x;
			y = p.y;
			sym = p.sym;
		}
		//изменение координаты от значениях направления
		public void Move(int offset, Direction direction)
		{
			if (direction == Direction.RIGHT)
			{
				x = x + offset;
			}
			else if (direction == Direction.LEFT)
			{
				x = x - offset;
			}
			else if (direction == Direction.UP)
			{
				y = y - offset;
			}
			else if (direction == Direction.DOWN)
			{
				y = y + offset;
			}
		}
		//функция столкновения точек
		public bool IsHit(Point p)
		{
			return p.x == this.x && p.y == this.y;
		}
		//функция прорисовки символа в заданной позиции
		public void Draw()
		{
			Console.SetCursorPosition(x, y);
			Console.Write(sym);
		}
		//функция замещения символа на пробел
		public void Clear()
		{
			sym = ' ';
			Draw();
		}
		//функция преобразования переменных в текст
		public override string ToString()
		{
			return x + ", " + y + ", " + sym;
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	enum Direction
	{
		LEFT,
		RIGHT,
		UP,
		DOWN
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class Figure
	{
		protected List<Point> pList;
		//функция прорисовки точек в листе
		public void Draw()
		{
			foreach (Point p in pList)
			{
				p.Draw();
			}
		}
		//функция столкновения фигуры "змейка" сама с собой
		internal bool IsHit(Figure figure)
		{
			foreach (var p in pList)
			{
				if (figure.IsHit(p))
					return true;
			}
			return false;
		}
		//функция столкновения фигуры "змейка" с фигурой точка
		private bool IsHit(Point point)
		{
			foreach (var p in pList)
			{
				if (p.IsHit(point))
					return true;
			}
			return false;
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class FoodCreator
	{
		int mapWidht;
		int mapHeight;
		char sym;

		Random random = new Random();
		public FoodCreator(int mapWidth, int mapHeight, char sym)
		{
			this.mapWidht = mapWidth;
			this.mapHeight = mapHeight;
			this.sym = sym;
		}
		//определение местоположения точки "еда" и перезапись точки
		public Point CreateFood()
		{
			int x = random.Next(2, mapWidht - 2);
			int y = random.Next(2, mapHeight - 2);
			return new Point(x, y, sym);
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class HorizontalLine : Figure
	{
		public HorizontalLine(int xLeft, int xRight, int y, char sym)
		{
			//добавление точек в лист с точками по координате x
			pList = new List<Point>();
			for (int x = xLeft; x <= xRight; x++)
			{
				Point p = new Point(x, y, sym);
				pList.Add(p);
			}
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class VerticalLine : Figure
	{
		public VerticalLine(int yUp, int yDown, int x, char sym)
		{
			//добавление точек в лист с точками по координате y
			pList = new List<Point>();
			for (int y = yUp; y <= yDown; y++)
			{
				Point p = new Point(x, y, sym);
				pList.Add(p);
			}
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class Snake : Figure
	{
		Direction direction;
		public int points;
		//структура фигуры "змейка"
		public Snake(Point tail, int length, Direction _direction)
		{
			direction = _direction;
			pList = new List<Point>();
			for (int i = 0; i < length; i++)
			{
				Point p = new Point(tail);
				p.Move(i, direction);
				pList.Add(p);
			}
		}
		//функция изменения направления хвоста и головы фигуры "змейка" 
		public void Move()
		{
			Point tail = pList.First();
			pList.Remove(tail);
			Point head = GetNextPoint();
			pList.Add(head);

			tail.Clear();
			head.Draw();
		}
		//функция изменения местоположения головы
		public Point GetNextPoint()
		{
			Point head = pList.Last();
			Point nextPoint = new Point(head);
			nextPoint.Move(1, direction);
			return nextPoint;
		}
		//функция столкновения головы с хвостом
		public bool IsHitTail()
		{
			var head = pList.Last();
			for (int i = 0; i < pList.Count - 2; i++)
			{
				if (head.IsHit(pList[i]))
					return true;
			}
			return false;
		}
		//обозначение клавиш в системе
		public void HandleKey(ConsoleKey key)
		{
			if (key == ConsoleKey.LeftArrow)
				direction = Direction.LEFT;
			else if (key == ConsoleKey.RightArrow)
				direction = Direction.RIGHT;
			else if (key == ConsoleKey.DownArrow)
				direction = Direction.DOWN;
			else if (key == ConsoleKey.UpArrow)
				direction = Direction.UP;
		}
		//функция увеличения фигуры "змейка" при столкновении с фигурой "еда"
		public bool Eat(Point food)
		{
			Point head = GetNextPoint();
			if (head.IsHit(food))
			{
				food.sym = head.sym;
				pList.Add(food);
				points++;
				return true;
			}
			else
				return false;
		}
	}
////////////////////////////////////////////////////////////////////////////////////////////////
	class Walls
	{
		List<Figure> wallList;
		//создание стен
		public Walls(int mapWidth, int mapHeight)
		{
			wallList = new List<Figure>();
			HorizontalLine upLine = new HorizontalLine(0, mapWidth - 2, 0, '+');
			HorizontalLine downLine = new HorizontalLine(0, mapWidth - 2, mapHeight - 1, '+');
			VerticalLine leftLine = new VerticalLine(0, mapHeight - 1, 0, '+');
			VerticalLine rightLine = new VerticalLine(0, mapHeight - 1, mapWidth - 2, '+');

			wallList.Add(upLine);
			wallList.Add(downLine);
			wallList.Add(leftLine);
			wallList.Add(rightLine);
		}
		//если фигура сталкивается с стеной, то возвращает True
		internal bool IsHit(Figure figure)
		{
			foreach (var wall in wallList)
			{
				if (wall.IsHit(figure))
				{
					return true;
				}
			}
			return false;
		}
		//Отрисовка рамочки
		public void Draw()
		{
			foreach (var wall in wallList)
			{
				wall.Draw();
			}
		}
	}
}
