using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public static class Vector2Extensions
    {
        public static float Distance(this Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(((a.Y - b.Y) * (a.Y - b.Y)) + ((a.X - b.X) * (a.X - b.X))); 
        }
    }
}
