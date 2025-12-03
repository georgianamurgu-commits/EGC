using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ReTester
{
    public static class ContentLoader
    {

        public static TextureFromBMP LoadTexture(string filename)
        {
            try
            {
                int textureId = GL.GenTexture();

                Bitmap bmp = new Bitmap(filename);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                                        System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                              bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                              PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);

                return new TextureFromBMP(textureId, bmp.Width, bmp.Height);
            } catch (Exception)
            {
                Console.WriteLine("ERR - CRASH - BURN: Cannot load texture from file <" + filename + "> (LOADTEXTURE function)!");

                return null;
            }
        }

    }
}
