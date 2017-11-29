using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public class Sprite
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2 Location { get; set; }
        public Vector3 Position { get { return new Vector3(X, Y, 0f); } }
        public float Rotation { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        protected int columnCount;
        protected int rowCount;
        protected int spriteIndexX;
        protected int spriteIndexY;
        protected int texture;

        public Sprite()
        {
        }

        public Sprite(float x, float y, int columnCount, int rowCount, string path)
        {
            X = x;
            Y = y;
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            spriteIndexX = 0;
            spriteIndexY = 0;
            LoadTexture(path);
        }

        public Sprite(float x, float y, int columnCount, int rowCount, string path, float rotation)
        {
            X = x;
            Y = y;
            this.columnCount = columnCount;
            this.rowCount = rowCount;
            spriteIndexX = 0;
            spriteIndexY = 0;
            Rotation = rotation;
            LoadTexture(path);
        }

        public void LoadTexture(string path)
        {
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            Bitmap bitmap = new Bitmap(path);
            bitmap.MakeTransparent(Color.Magenta);

            width = bitmap.Width;
            height = bitmap.Height;

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height,
                0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        public virtual void Render()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Color3(Color.White);

            GL.Begin(BeginMode.Quads);

            //TOP LEFT
            GL.TexCoord2(1d / columnCount * spriteIndexX,
                            1d / rowCount * spriteIndexY);
            GL.Vertex2(0 - width / (rowCount * 2), 0 - height / (columnCount * 2));
            //TOP RIGHT
            GL.TexCoord2(1d / columnCount * (spriteIndexX + 1),
                            1d / rowCount * spriteIndexY);
            GL.Vertex2(width / (rowCount * 2), 0 - height / (columnCount * 2));
            //BOTTOM RIGHT
            GL.TexCoord2(1d / columnCount * (spriteIndexX + 1),
                            1d / rowCount * (spriteIndexY + 1));
            GL.Vertex2(width / (rowCount * 2), 0 + height / (columnCount * 2));
            //BOTTOM LEFT
            GL.TexCoord2(1d / columnCount * spriteIndexX,
                            1d / rowCount * (spriteIndexY + 1));
            GL.Vertex2(0 - width / (rowCount * 2), 0 + height / (columnCount * 2));
            GL.End();
        }
    }
}

