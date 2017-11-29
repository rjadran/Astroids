using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace Asteroids
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new GameWindow())
            {
                game.ClientSize = new Size(1600, 950);
                game.X = 150;
                game.Y = 20;
                float resetTimer = 0;
                float startTimer = 0;
                bool startShowStartScreen = true;
                bool startGame = false;
                bool spacePressed = false;
                bool endGame = false;
                int lives = 3;
                System.Media.SoundPlayer playerExplosionSound = new System.Media.SoundPlayer("snd_explosion_player.wav");
                System.Media.SoundPlayer asteroidExplosionSound = new System.Media.SoundPlayer("snd_explosion_asteroid.wav");
                System.Media.SoundPlayer projectileSound = new System.Media.SoundPlayer("snd_shoot.wav");
                List<Asteroid> bigAsteroids = new List<Asteroid>();
                List<Asteroid> mediumAsteroids = new List<Asteroid>();
                List<Asteroid> smallAsteroids = new List<Asteroid>();
                List<Sprite> livesSprites = new List<Sprite>();
                Sprite background = new Sprite(game.Width / 2, game.Height / 2, 1, 1, "space.png");
                Sprite gameLabel = new Sprite(game.Width / 2, game.Height / 2, 1, 1, "game_label.png");
                Ship ship = new Ship(game);
                SetGame(game, lives, bigAsteroids, mediumAsteroids, smallAsteroids, livesSprites, ref ship);
                Explosion explosion = new Explosion();
                List<Projectile> projectiles = new List<Projectile>();
                List<Destroy> destroy = new List<Destroy>();
                Text renderer = new Text(game.Width, game.Height); 
                Font serif = new Font(FontFamily.GenericSerif, 100);
                int score = 0;

                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds, etc
                    GL.LoadIdentity();
                    GL.Ortho(0, 1600, 950, 0, -1, 1);
                    GL.Viewport(0, 0, 1600, 950);
                    GL.ClearColor(Color.Black);                  
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    game.VSync = VSyncMode.On;
                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };

                game.UpdateFrame += (sender, e) =>
                {
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                    if (game.Keyboard[Key.Enter])
                    {
                        startShowStartScreen = false;
                    }

                    if (!startShowStartScreen)
                    {
                        startTimer += (float)e.Time;

                        if (startTimer > 4.0f)
                        {
                            startGame = true;
                        }

                        if (startGame)
                        {
                            foreach (var a in bigAsteroids)
                            {
                                a.Update((float)e.Time);
                                if (a.X < -100)
                                {
                                    a.X = game.Width + 100;
                                }
                                if (a.X > game.Width + 100)
                                {
                                    a.X = -100;
                                }
                                if (a.Y < -100)
                                {
                                    a.Y = game.Height + 100;
                                }
                                if (a.Y > game.Height + 100)
                                {
                                    a.Y = -100;
                                }
                            }
                            foreach (var a in mediumAsteroids)
                            {
                                a.Update((float)e.Time);
                                if (a.X < -50)
                                {
                                    a.X = game.Width + 50;
                                }
                                if (a.X > game.Width + 50)
                                {
                                    a.X = -50;
                                }
                                if (a.Y < -50)
                                {
                                    a.Y = game.Height + 50;
                                }
                                if (a.Y > game.Height + 50)
                                {
                                    a.Y = -50;
                                }
                            }
                            foreach (var a in smallAsteroids)
                            {
                                a.Update((float)e.Time);
                                if (a.X < -30)
                                {
                                    a.X = game.Width + 30;
                                }
                                if (a.X > game.Width + 30)
                                {
                                    a.X = -30;
                                }
                                if (a.Y < -30)
                                {
                                    a.Y = game.Height + 30;
                                }
                                if (a.Y > game.Height + 30)
                                {
                                    a.Y = -30;
                                }
                            }
                            if (ship != null)
                            {
                                ship.Update((float)e.Time, game.Keyboard, game);
                                ship.CheckCollision(bigAsteroids, mediumAsteroids, smallAsteroids);
                            }
                            if (ship != null && !ship.showShip)
                            {
                                explosion.showExplosion = true;
                                explosion.X = ship.X;
                                explosion.Y = ship.Y;
                                playerExplosionSound.Play();
                                lives--;
                                startTimer = 0;
                            }
                            if (explosion.showExplosion)
                            {
                                ship = null;
                                explosion.Update((float)e.Time);
                            }

                            if (explosion.okToResetShip)
                            {
                                if (lives >= 0)
                                {
                                    Ship checkCollisionShip = new Ship(game);
                                    checkCollisionShip.CheckCollision(bigAsteroids, mediumAsteroids, smallAsteroids);
                                    if (checkCollisionShip.showShip != false)
                                    {
                                        checkCollisionShip = null;
                                        ship = new Ship(game);
                                        ship.showShip = true;
                                        explosion.okToResetShip = false;
                                    }
                                }
                            }

                            if (game.Keyboard[Key.Space] && ship != null)
                            {
                                if (!spacePressed)
                                {
                                    projectiles.Add(new Projectile(ship));
                                    projectileSound.Play();
                                }
                                spacePressed = true;
                            }
                            else
                            {
                                spacePressed = false;
                            }

                            if (projectiles != null)
                            {
                                for (int i = 0; i < projectiles.Count; i++)
                                {
                                    projectiles[i].Update((float)e.Time, game);
                                    if (projectiles[i].distanceValue > projectiles[i].DISTANCE)
                                    {
                                        projectiles.Remove(projectiles[i]);
                                    }
                                }
                                for (int i = 0; i < projectiles.Count; i++)
                                {
                                    projectiles[i].CheckCollision(bigAsteroids, mediumAsteroids, smallAsteroids, destroy);
                                    if (projectiles[i].collision)
                                    {
                                        projectiles.Remove(projectiles[i]);
                                        score += 100;
                                        asteroidExplosionSound.Play();
                                    }
                                }
                            }

                            if (destroy != null)
                            {
                                for (int i = 0; i < destroy.Count; i++)
                                {

                                    destroy[i].Update((float)e.Time);
                                    if (!destroy[i].showDestroy)
                                    {
                                        destroy.Remove(destroy[i]);
                                    }

                                }
                            }
                            if (!bigAsteroids.Any() && !mediumAsteroids.Any() && !smallAsteroids.Any())
                            {
                                resetTimer += (float)e.Time;
                                if (resetTimer > 3)
                                {
                                    SetGame(game, lives, bigAsteroids, mediumAsteroids, smallAsteroids, livesSprites, ref ship);
                                    startTimer = 0;
                                    resetTimer = 0;
                                    startGame = false;
                                }
                            }
                        }
                        if (lives < 0 && startTimer > 5)
                        {
                            endGame = true;
                        }
                        if (endGame)
                        {
                            explosion.okToResetShip = false;
                            SetGame(game, lives, bigAsteroids, mediumAsteroids, smallAsteroids, livesSprites, ref ship);
                            endGame = false;
                            lives = 3;
                            startShowStartScreen = true;
                            startTimer = 0;
                            startGame = false;
                            score = 0;
                        }
                    }
                };

                game.RenderFrame += (sender, e) =>
                {
                    renderer.Clear(Color.Transparent);
                    
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.Enable(EnableCap.Texture2D);

                    GL.PushMatrix();
                    GL.Translate(background.Position);
                    GL.Rotate(background.Rotation, Vector3d.UnitZ);
                    background.Render();
                    GL.PopMatrix();

                    if (!startShowStartScreen)
                    {
                        renderer.DrawString(score.ToString(), serif, Brushes.White, new PointF(0.0f, 0.0f));
                        foreach (Asteroid a in bigAsteroids)
                        {
                            GL.PushMatrix();
                            GL.Translate(a.Position);
                            GL.Rotate(a.Rotation, Vector3d.UnitZ);
                            a.Render();
                            GL.PopMatrix();
                        }

                        foreach (Asteroid a in mediumAsteroids)
                        {
                            GL.PushMatrix();
                            GL.Translate(a.Position);
                            GL.Rotate(a.Rotation, Vector3d.UnitZ);
                            a.Render();
                            GL.PopMatrix();
                        }

                        foreach (Asteroid a in smallAsteroids)
                        {
                            GL.PushMatrix();
                            GL.Translate(a.Position);
                            GL.Rotate(a.Rotation, Vector3d.UnitZ);
                            a.Render();
                            GL.PopMatrix();
                        }

                        if (ship != null && ship.showShip)
                        {
                            GL.PushMatrix();
                            GL.Translate(ship.Position);
                            GL.Rotate(ship.Rotation, Vector3d.UnitZ);
                            ship.Render();
                            GL.PopMatrix();
                        }

                        if (projectiles != null)
                        {
                            foreach (var projectile in projectiles)
                            {
                                GL.PushMatrix();
                                GL.Translate(projectile.Position);
                                GL.Rotate(projectile.Rotation, Vector3d.UnitZ);
                                projectile.Render();
                                GL.PopMatrix();
                            }
                        }

                        if (destroy != null)
                        {
                            foreach (var d in destroy)
                            {
                                GL.PushMatrix();
                                GL.Translate(d.Position);
                                GL.Rotate(d.Rotation, Vector3d.UnitZ);
                                d.Render();
                                GL.PopMatrix();
                            }
                        }

                        if (explosion.showExplosion)
                        {
                            GL.PushMatrix();
                            GL.Translate(explosion.Position);
                            GL.Rotate(explosion.Rotation, Vector3d.UnitZ);
                            explosion.Render();
                            GL.PopMatrix();
                        }

                        for (int i = 0; i < lives; i++)
                        {
                            GL.PushMatrix();
                            GL.Translate(livesSprites[i].Position);
                            GL.Rotate(livesSprites[i].Rotation, Vector3d.UnitZ);
                            livesSprites[i].Render();
                            GL.PopMatrix();
                        }
                        GL.Disable(EnableCap.Texture2D);

                        GL.Enable(EnableCap.Texture2D);
                        GL.BindTexture(TextureTarget.Texture2D, renderer.Texture);
                        GL.Begin(BeginMode.Quads);

                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(100, 300);
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(400, 300);
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(400, 0);
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(100, 0);

                        GL.End();
                    }

                    if (startShowStartScreen)
                    {
                        renderer.DrawString("Press Enter to Start", serif, Brushes.White, new PointF(0.0f, 0.0f));

                        GL.PushMatrix();
                        GL.Translate(gameLabel.Position);
                        GL.Rotate(gameLabel.Rotation, Vector3d.UnitZ);
                        gameLabel.Render();
                        GL.PopMatrix();

                        GL.Enable(EnableCap.Texture2D);
                        GL.BindTexture(TextureTarget.Texture2D, renderer.Texture);
                        GL.Begin(BeginMode.Quads);

                        GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(650, 730);
                        GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1000, 730);
                        GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1000, 530);
                        GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(650, 530);

                        GL.End();
                    }
                    game.SwapBuffers();
                };

                game.Run(60.0);
            }
        }

        public static void SetGame(dynamic game, int lives, List<Asteroid> bigAsteroids, List<Asteroid> mediumAsteroids, List<Asteroid> smallAsteroids, List<Sprite> livesSprites, ref Ship ship)
        {
            bigAsteroids.Clear();
            mediumAsteroids.Clear();
            smallAsteroids.Clear();
            for (int i = 0; i < 4; i++)
            {
                bigAsteroids.Add(new Asteroid(game, "asteroid_big0.png"));
            }

            for (int i = 0; i < 4; i++)
            {
                mediumAsteroids.Add(new Asteroid(game, "asteroid_medium0.png"));
            }
            for (int i = 0; i < 4; i++)
            {
                smallAsteroids.Add(new Asteroid(game, "asteroid_small0.png"));
            }
            for (int i = 0; i < lives; i++)
            {
                livesSprites.Add(new Sprite(100 + 40 * i, 75, 1, 1, "player.png", -90));
            }
            ship = null;
            ship = new Ship(game);
        }
    }
}
