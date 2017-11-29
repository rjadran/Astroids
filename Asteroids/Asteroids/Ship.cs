using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class Ship : Sprite
    {
        public bool showShip { get; set; }
        public bool Collided { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        private float ROTATION_SPEED;
        private float ACCELERATION_SPEED;
        private int MAX_SPEED = 300;


        public Ship(dynamic game)
        {
            X = game.Width / 2;
            Y = game.Height / 2;
            spriteIndexX = 0;
            spriteIndexY = 0;
            columnCount = 1;
            rowCount = 1;
            Rotation = -90;
            ROTATION_SPEED = 5;
            ACCELERATION_SPEED = 300;
            showShip = true;
            Acceleration = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Location = new Vector2(X, Y);
            LoadTexture("player.png");
        }

        public void CheckCollision(List<Asteroid> bigAsteroids, List<Asteroid> mediumAsteroids, List<Asteroid> smallAsteroids)
        {
            Location = new Vector2(X, Y);
            for (int i = 0; i < bigAsteroids.Count; i++)
            {
                if (Math.Abs(bigAsteroids[i].Location.Distance(Location)) <= (bigAsteroids[i].width / 2 + width / 4))
                {
                    showShip = false;
                }
            }
            for (int i = 0; i < mediumAsteroids.Count; i++)
            {
                if (Math.Abs(mediumAsteroids[i].Location.Distance(Location)) <= (mediumAsteroids[i].width / 2 + width / 4))
                {
                    showShip = false;
                }
            }
            for (int i = 0; i < smallAsteroids.Count; i++)
            {
                if (Math.Abs(smallAsteroids[i].Location.Distance(Location)) <= (smallAsteroids[i].width / 2 + width / 4))
                {
                    showShip = false;
                }
            }
        }

        public void Update(float elapsedTime, KeyboardDevice keyboard, dynamic game)
        {
            if (keyboard[Key.Left])
            {
                Rotation -= ROTATION_SPEED;
            }
            if (keyboard[Key.Right])
            {
                Rotation += ROTATION_SPEED;
            }
            if (keyboard[Key.Up])
            {
                var v = Acceleration;
                float theta = Rotation * (float)(3.14 / 180);
                v.X = (float)Math.Cos(theta) * ACCELERATION_SPEED * elapsedTime;
                v.Y = (float)Math.Sin(theta) * ACCELERATION_SPEED * elapsedTime;
                Velocity += v;
            }
            X += Velocity.X * elapsedTime;
            Y += Velocity.Y * elapsedTime;

            speedLimit();

            keepOnScreen(game);
        }

        public Ship ResetShip(float elapsedTime, Explosion explosion, int lives, dynamic game)
        {
            Ship ship = null;
            if (lives > 0)
            {
                float timer = 0;
                timer += elapsedTime;
                if (timer > 2)
                {
                    lives--;
                    ship = new Ship(game);
                    ship.showShip = true;
                    explosion.okToResetShip = false;
                }
            }
            return ship;
        }

        protected void speedLimit()
        {

            if (Math.Abs(Velocity.Length) > MAX_SPEED)
            {
                Velocity = (Velocity / Math.Abs(Velocity.Length)) * MAX_SPEED;
            }
        }

        public void keepOnScreen(dynamic game)
        {
            if (X < 0)
            {
                X = game.Width;
            }
            if (X > game.Width)
            {
                X = 0;
            }
            if (Y < 0)
            {
                Y = game.Height;
            }
            if (Y > game.Height)
            {
                Y = 0;
            }
        }
    }
}
