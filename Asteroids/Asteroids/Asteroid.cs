using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Asteroids
{
    public class Asteroid : Sprite
    {       
        private static Random random = new Random();
        Vector2 velocity = new Vector2();

        public Asteroid(dynamic game, string path)
        {
            X = GetXPosition(game);
            Y = GetYPosition(game);
            columnCount = 1;
            rowCount = 1;
            spriteIndexX = 0;
            spriteIndexY = 0;
            velocity.X = random.Next(-220, 220);
            velocity.Y = random.Next(-220, 220);
            LoadTexture(path);
        }

        public Asteroid(float x, float y, string path)
        {
            X = x;
            Y = y;
            columnCount = 1;
            rowCount = 1;
            spriteIndexX = 0;
            spriteIndexY = 0;
            velocity.X = random.Next(-200, 200);
            velocity.Y = random.Next(-200, 200);
            LoadTexture(path);
        }

        public void Update(float elapsedTime)
        {
            X += velocity.X * elapsedTime;
            Y += velocity.Y * elapsedTime;
            Location = new Vector2(X, Y);
        }

        public float GetXPosition(dynamic game)
        {
            float x = random.Next(0, game.Width);

            while (x < (game.Width / 2 + 200) && x > (game.Width / 2 - 200))
            {
                x = random.Next(0, game.Width);
            }

            return x;
        }

        public float GetYPosition(dynamic game)
        {
            float y = random.Next(0, game.Height);
            
            while (y < (game.Height / 2 + 200) && y > (game.Height / 2 - 200))
            {
                y = random.Next(0, game.Height);
            }

            return y;
        }
    }
}
