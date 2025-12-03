using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace ReTester
{
    public class TextureFromBMP
    {
        public int id;
        public int TextureID => id; // ← adaugă asta
        public int texWidth;
        public int texHeight;

        public TextureFromBMP(int _id, int _texWidth, int _texHeight)
        {
            id = _id;
            texWidth = _texWidth;
            texHeight = _texHeight;
        }

    }

   

}
