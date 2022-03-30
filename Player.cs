using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg;

namespace SkullIsland
{
	public class Player
	{
		private Vector2 _position = new Vector2(Game1.Width / 2, Game1.Height / 2);
		private Vector2 _offset = new Vector2(48, 48);
		private int _speed = 300;
		private Dir _direction = Dir.Down;
		private bool _isMoving = false;
		public SpriteAnimation Animation;
		public SpriteAnimation[] Animations = new SpriteAnimation[4];
		private KeyboardState _kStateOld = Keyboard.GetState();
		public bool IsDead = false;
		public int Radius = 32;

		public Vector2 Position
		{
			get => _position;
			set
			{
				_position.X = value.X;
				_position.Y = value.Y;
			}
		}

		public void Update(GameTime gameTime)
		{
			KeyboardState kState = Keyboard.GetState();
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

			//Player movement
			_isMoving = false;
			
			if (kState.IsKeyDown(Keys.W)) 
			{ 
				_direction = Dir.Up;
				_isMoving = true;
			}

			if (kState.IsKeyDown(Keys.S))
			{
				_direction = Dir.Down;
				_isMoving = true;
			}

			if (kState.IsKeyDown(Keys.A))
			{
				_direction = Dir.Left;
				_isMoving = true;
			}

			if (kState.IsKeyDown(Keys.D))
			{
				_direction = Dir.Right;
				_isMoving = true;
			}

			//updates the animation and its direction
			//updates every frame even if not moving
			Animation = Animations[(int)_direction];
			Animation.Position = _position - _offset;
			Animation.Update(gameTime);
			
			if (IsDead) return; //can't shoot or move when dead
			
			if (kState.IsKeyDown(Keys.Space) && _kStateOld.IsKeyUp(Keys.Space))
			{
				Projectile.Projectiles.Add(new Projectile(_position, _direction));
			}
			
			if (kState.IsKeyDown(Keys.Space)) _isMoving = false;
			_kStateOld = kState;
			
			if (!_isMoving)
			{
				if (kState.IsKeyDown(Keys.Space))
				{
					Animation.setFrame(0); //throw frame when shoots
				}
				else
				{
					Animation.setFrame(1); //sets the idle frame when not moving
				}

				return;
			}

			switch (_direction)
			{
				case Dir.Up:
					if (_position.Y > 200)
					_position.Y -= _speed * deltaTime;
					break;
				case Dir.Down:
					if (_position.Y < 1250)
					_position.Y += _speed * deltaTime;
					break;
				case Dir.Left:
					if (_position.X > 225)
					_position.X -= _speed * deltaTime;
					break;
				case Dir.Right:
					if (_position.X < 1275)
					_position.X += _speed * deltaTime;
					break;
			}
		}
	}
}
