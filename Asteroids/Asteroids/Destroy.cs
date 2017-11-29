using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class Destroy : Sprite
    {
        public bool showDestroy { get; set; }
        private float time;

        public Destroy(float x, float y)
        {
            X = x;
            Y = y;
            spriteIndexX = 0;
            spriteIndexY = 0;
            columnCount = 3;
            rowCount = 3;
            time = 0;
            showDestroy = true;
            LoadTexture("burst.png");
        }

        public void Update(float elapsedTime)
        {
            time += elapsedTime;
            if (time > 0.05f)
            {
                spriteIndexX += 1;
                if (spriteIndexX > 2)
                {
                    spriteIndexX = 0;
                    spriteIndexY += 1;
                }
                if (spriteIndexY > 2)
                {
                    spriteIndexX = 0;
                    spriteIndexY = 0;
                    showDestroy = false;
                }
                time = 0;
            }
        }
    }
}
