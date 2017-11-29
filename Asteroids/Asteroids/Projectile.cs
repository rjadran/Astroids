using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class Projectile : Sprite
    {
        public Ship Owner { get; set; }
        public Vector2 Prior { get; set; }
        public Vector2 Velocity { get; set; }
        public bool collision { get; set; }
        public float speed { get; set; }
        public float theta { get; set; }
        public float distanceValue { get; set; }
        public float DISTANCE { get; set; }

        public Projectile(Ship owner)
        {
            Owner = owner;
            X = owner.X;
            Y = owner.Y;
            Prior = new Vector2(X, Y);
            spriteIndexX = 0;
            spriteIndexY = 0;
            columnCount = 1;
            rowCount = 1;
            Rotation = owner.Rotation;
            speed = 600f;
            theta = Rotation * (float)(3.14 / 180);
            distanceValue = 0;
            DISTANCE = 700;
            collision = false;
            Velocity = new Vector2(speed * (float)Math.Cos(theta), speed * (float)Math.Sin(theta));
            LoadTexture("projectile.png");
        }

        public void Update(float elapsedTime, dynamic game)
        {
            X += Velocity.X * elapsedTime;
            Y += Velocity.Y * elapsedTime;
            Location = new Vector2(X, Y);
            distanceValue += Math.Abs(Location.Distance(Prior));
            Prior = new Vector2(X, Y);
            if (X < 0)
            {
                X = game.Width;
                Prior = new Vector2(X, Y);
            }
            if (X > game.Width)
            {
                X = 0;
                Prior = new Vector2(X, Y);
            }
            if (Y < 0)
            {
                Y = game.Height;
                Prior = new Vector2(X, Y);
            }
            if (Y > game.Height)
            {
                Y = 0;
                Prior = new Vector2(X, Y);
            }
        }

        public void CheckCollision(List<Asteroid> bigAsteroids, List<Asteroid> mediumAsteroids, List<Asteroid> smallAsteroids, List<Destroy> destroy)
        {
            Location = new Vector2(X, Y);
            for (int i = 0; i < bigAsteroids.Count; i++)
            {
                if (Math.Abs(bigAsteroids[i].Location.Distance(Location)) <= (bigAsteroids[i].width / 2 + width / 4))
                {
                    destroy.Add(new Destroy(X, Y));
                    mediumAsteroids.Add(new Asteroid(bigAsteroids[i].X, bigAsteroids[i].Y, "asteroid_medium0.png"));
                    mediumAsteroids.Add(new Asteroid(bigAsteroids[i].X, bigAsteroids[i].Y, "asteroid_medium0.png"));
                    bigAsteroids.Remove(bigAsteroids[i]);
                    collision = true;
                }
            }
            for (int i = 0; i < mediumAsteroids.Count; i++)
            {
                if (Math.Abs(mediumAsteroids[i].Location.Distance(Location)) <= (mediumAsteroids[i].width / 2 + width / 4))
                {
                    destroy.Add(new Destroy(X, Y));
                    smallAsteroids.Add(new Asteroid(mediumAsteroids[i].X, mediumAsteroids[i].Y, "asteroid_small0.png"));
                    smallAsteroids.Add(new Asteroid(mediumAsteroids[i].X, mediumAsteroids[i].Y, "asteroid_small0.png"));
                    mediumAsteroids.Remove(mediumAsteroids[i]);
                    collision = true;
                }
            }
            for (int i = 0; i < smallAsteroids.Count; i++)
            {
                if (Math.Abs(smallAsteroids[i].Location.Distance(Location)) <= (smallAsteroids[i].width / 2 + width / 4))
                {
                    destroy.Add(new Destroy(X, Y));
                    smallAsteroids.Remove(smallAsteroids[i]);
                    collision = true;
                }
            }
        }
    }
}
