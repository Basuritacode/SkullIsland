using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rpg;

namespace SkullIsland
{
	public class Enemy
	{
		public static List<Enemy> Enemies = new List<Enemy>();
		
		private Vector2 _position;
		public Vector2 Offset = new Vector2(48, 66);
		private int _speed = 150;
		public int Radius = 25;
		private bool _dead = false;
		
		public Vector2 Position => _position;
		public bool Dead
		{
			get => _dead;
			set => _dead = value;
		}
		
		public SpriteAnimation Animation;

		public Enemy(Vector2 position, Texture2D texture)
		{
			_position = position;
			Animation = new SpriteAnimation(texture, 10, 6);
		}

		public void Update(GameTime gameTime, Vector2 playerPos, bool playerDead)
		{
			float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

			Animation.Position = _position - Offset;
			Animation.Update(gameTime);
			
			if(playerDead) return;
			Chase(playerPos, deltaTime);
		}

		private void Chase(Vector2 playerPos, float deltaTime)
		{
			Vector2 enemyToPlayer = this._position - playerPos;
			enemyToPlayer.Normalize();
			_position -= enemyToPlayer * _speed * deltaTime;
		}
	}
}