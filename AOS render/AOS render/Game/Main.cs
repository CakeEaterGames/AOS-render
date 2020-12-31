using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCake;
using MonoCake.Objects;
using MonoCake.Rendering;

namespace AOS_render
{
    public class Main : BasicObject
    {
        public AOS<Bullet> Bullets;
        Random rng = new Random();
        Texture2D Img;
        public Main()
        {
            Img = GlobalContent.LoadImg("x8", true);
            Bullets = new AOS<Bullet>(200000);
           
        }
        public override void Update()
        {
            for (int i = 0; i <= Bullets.Last; i++)
            {
                ref Bullet p = ref Bullets.Array[i];
                p.X += p.SX;
                p.Y += p.SY;
                Bullets.Array[i].lifetime++;
                if (Bullets.Array[i].lifetime == 30)
                {
                    var gx = KEY.MouseX;
                    var gy = KEY.MouseY;
                    var distX = gx - p.X;
                    var distY = gy - p.Y;
                    double speed = 10.0;
                    double length = Math.Sqrt(distX * distX + distY * distY);
                    p.SX = speed / length*distX; 
                    p.SY = speed / length * distY;
                }
          /*      if (Bullets.Array[i].lifetime == 60)
                {
                    var gx = KEY.MouseX;
                    var gy = KEY.MouseY;
                    var distX = gx - p.X;
                    var distY = gy - p.Y;
                    double speed = 10.0;
                    double length = Math.Sqrt(distX * distX + distY * distY);
                    p.SX = speed / length * distX;
                    p.SY = speed / length * distY;
                }
                */

                if (p.Y > 720 || p.Y < 0 || p.X > 1280 || p.X < 0)
                {
                    Bullets.FreeStruct(i);
                    i--;
                }
             

            }

            while (Bullets.FreeCount > 0)
            {
                int i = Bullets.ReserveStruct();
                ref Bullet p = ref Bullets.Array[i];
                p.X = 1280 / 2;
                p.Y = 720 / 2;
                p.SX = Tools.Rand(-1.0, 1.0) * 10;
                p.SY = Tools.Rand(-1.0, 1.0) * 10;
                p.lifetime = 0;
                while (p.SY == 0) p.SY = Tools.Rand(-1.0, 1.0) * 10;
            }

        }



        public override void Render()
        {

            for (int i = 0; i <= Bullets.Last; i++)
            {
              /*  if ( Bullets.Array[i].lifetime>60)
                {
                    CakeEngine.spriteBatch.Draw(Img, new Vector2((float)Bullets.Array[i].X, (float)Bullets.Array[i].Y), Color.Red);
                }
                else*/ if (Bullets.Array[i].lifetime > 30)
                {
                    CakeEngine.spriteBatch.Draw(Img, new Vector2((float)Bullets.Array[i].X, (float)Bullets.Array[i].Y), Color.Lime);
                }
                else
                {
                    CakeEngine.spriteBatch.Draw(Img, new Vector2((float)Bullets.Array[i].X, (float)Bullets.Array[i].Y), Color.White);
                }
            }
        }


    }


    public struct Bullet
    {
        public double X, Y, SY, SX;
        public int lifetime;
        public Bullet(double x, double y, double sx, double sy)
        {
            X = x;
            Y = y;
            SY = sy;
            SX = sx;
            lifetime = 0;
        }
    }

    public class AOS<T> where T : struct
    {
        public T[] Array;

        public int Length;
        public int Last;
        public int FreeCount;

        public AOS(int l)
        {
            Length = l;
            Last = -1;
            Array = new T[l];
            FreeCount = l;
        }

        public int ReserveStruct()
        {
            if (FreeCount == 0) return -1;
            Last++;
            FreeCount--;
            return Last;
        }
        public void FreeStruct(int i)
        {
            T t = Array[i];
            Array[i] = Array[Last];
            Array[Last] = t;

            Last--;
            FreeCount++;
        }

    }

}
