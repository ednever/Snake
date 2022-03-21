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
}
