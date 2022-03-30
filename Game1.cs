using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Comora;
using rpg;

namespace SkullIsland
{
    public enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public static class MySounds
    {
        public static SoundEffect ProjectileSound;
        public static Song BackMusic;
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        //Window size
        public static int Width = 1920;
        public static int Height = Width * 9 / 16;
        
        //Assets: there are 8 in total
        private Texture2D _playerSprite;
        private Texture2D _walkDown;
        private Texture2D _walkLeft;
        private Texture2D _walkRight;
        private Texture2D _walkUp;

        private Texture2D _background;
        private Texture2D _ballSprite;
        private Texture2D _skullSprite;
        
        //the player
        private Player _player = new Player();
        
        //Camera
        private Camera _camera;
        
        //Game controller
        private Controller _gameController = new Controller();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //change window size
            _graphics.PreferredBackBufferWidth = Width;
            _graphics.PreferredBackBufferHeight = Height;
            _graphics.ApplyChanges();

            this._camera = new Camera(_graphics.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _playerSprite = Content.Load<Texture2D>("Player/player");
            _walkDown = Content.Load<Texture2D>("Player/walkDown");
            _walkLeft = Content.Load<Texture2D>("Player/walkLeft");
            _walkRight = Content.Load<Texture2D>("Player/walkRight");
            _walkUp = Content.Load<Texture2D>("Player/walkUp");
            _background = Content.Load<Texture2D>("background");
            _ballSprite = Content.Load<Texture2D>("ball");
            _skullSprite = Content.Load<Texture2D>("skull");

            //fps: stands for how fast it cycles trough, 4 frames at 8fps = 0.5s cycle
            _player.Animations[0] = new SpriteAnimation(_walkDown, 4, 8);
            _player.Animations[1] = new SpriteAnimation(_walkUp, 4, 8);
            _player.Animations[2] = new SpriteAnimation(_walkLeft, 4, 8);
            _player.Animations[3] = new SpriteAnimation(_walkRight, 4, 8);

            _player.Animation = _player.Animations[0];
            
            //Sounds
            MySounds.ProjectileSound = Content.Load<SoundEffect>("Sounds/blip");
            MySounds.BackMusic = Content.Load<Song>("Sounds/nature");
            MediaPlayer.Play(MySounds.BackMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _player.Update(gameTime);
            this._camera.Position = _player.Position;
            this._camera.Update(gameTime);
            
            _gameController.Update(gameTime, _skullSprite);

            foreach (var enemy in Enemy.Enemies)
            {
                enemy.Update(gameTime, _player.Position, _player.IsDead);
                //check enemy to player collision
                var distance = Vector2.Distance(_player.Position, enemy.Position);
                if (distance <= _player.Radius)
                    _player.IsDead = true;

            }
            foreach (var projectile in Projectile.Projectiles)
            {
                projectile.Update(gameTime);
            }
            
            //Collision Detection for every bullet over every enemy
            foreach (var projectile in Projectile.Projectiles)
            {
                foreach (var enemy in Enemy.Enemies)
                {
                    var distance = Vector2.Distance(enemy.Position, projectile.Position);
                    var maxDist = projectile.Radius + enemy.Radius;
                    if (distance <= maxDist)
                    {
                        projectile.Collided = true;
                        enemy.Dead = true;
                    }
                }
            }
            //Check for this property in List<T>.RemoveAll()
            Projectile.Projectiles.RemoveAll(p => p.Collided);
            Enemy.Enemies.RemoveAll(e => e.Dead);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(this._camera);

            _spriteBatch.Draw(_background, new Vector2(-500, -500), Color.White);
            foreach (var projectile in Projectile.Projectiles)    
            {
                _spriteBatch.Draw(_ballSprite,projectile.Position - projectile.Offset, Color.White);
            }

            foreach (var enemy in Enemy.Enemies)
            {
                enemy.Animation.Draw(_spriteBatch);
            }
            if(!_player.IsDead)
                _player.Animation.Draw(_spriteBatch);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
