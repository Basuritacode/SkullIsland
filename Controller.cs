using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkullIsland
{
	public class Controller
	{
		private double _timer = 4;
		private double _maxTime = 4;
		private static Random _random = new Random();
		private Vector2 _borders = new Vector2(-500, 2000);
		
		public void Update(GameTime gameTime, Texture2D spriteSnap)
		{
			_timer -= gameTime.ElapsedGameTime.TotalSeconds;
			if (_timer <= 0)
			{
				var newPosition = new Vector2(_random.Next((int) _borders.X, (int) _borders.Y),
					_random.Next((int) _borders.X, (int) _borders.Y));
				
				Enemy.Enemies.Add(new Enemy(newPosition, spriteSnap));
				
				_timer = _maxTime;
				if (_maxTime > 0.5)
				{
					_maxTime -= 0.05;
				}
			}
		}
	}
}