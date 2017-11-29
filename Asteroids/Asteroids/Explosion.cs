using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class Explosion : Sprite
    {
        public bool showExplosion { get; set; }
        public bool okToResetShip { get; set; }
        private float time;

        public Explosion()
        {
            spriteIndexX = 0;
            spriteIndexY = 2;
            columnCount = 4;
            rowCount = 3;
            showExplosion = false;
            time = 0;
            LoadTexture("explosion.png");
        }

        public void Update(float elapsedTime)
        {
            time += elapsedTime;
            if (time > 0.05f)
            {
                spriteIndexY -= 1;
                if (spriteIndexY < 0 && spriteIndexX == 0)
                {
                    spriteIndexX += 1;
                    spriteIndexY = 2;
                
                }
                if (spriteIndexY < 0 && spriteIndexX == 1)
                {
                    spriteIndexX += 1;
                    spriteIndexY = 2;                    
                }
                if (spriteIndexY < 0 && spriteIndexX == 2)
                {
                    spriteIndexX += 1;
                    spriteIndexY = 2;
                }
                if (spriteIndexY < 0 && spriteIndexX == 3)
                {
                    spriteIndexY = 2;
                    spriteIndexX = 0;
                    showExplosion = false;
                    okToResetShip = true;
                }
                time = 0;
            }
        }
    }
}
