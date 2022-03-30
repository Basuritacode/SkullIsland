using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SkullIsland
{
	public class Projectile
	{
		public static List<Projectile> Projectiles = new List<Projectile>();
		
		private Vector2 _position;
		public Vector2 Offset = new Vector2(48, 48);
		private int _speed = 1000;
		public int Radius = 18;
		private Dir _direction;
		private bool _collided = false;
		
		public Vector2 Position => _position;
		public bool Collided
		{
			get => _collided;
			set => _collided = value;
		}

		public Projectile(Vector2 position, Dir direction)
		{
			_position = position;
			_direction = direction;
		}

		public void Update(GameTime gameTime)
		{
			var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
			
			switch (_direction)
			{
				case Dir.Up:
					_position.Y -= _speed * deltaTime;
					break;
				case Dir.Down:
					_position.Y += _speed * deltaTime;
					break;
				case Dir.Left:
					_position.X -= _speed * deltaTime;
					break;
				case Dir.Right:
					_position.X += _speed * deltaTime;
					break;
			}
		}
	}
}