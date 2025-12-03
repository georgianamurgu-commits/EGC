using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ReTester
{
    public class SquareTextured
    {
        public Vector3[] vertices = new Vector3[4];
        public TextureFromBMP Texture;
        Color Colour;
        bool Visible;
        int side = 25;        

        public SquareTextured(TextureFromBMP tex)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, 1.0f, 0, 0, 0);
        }

        public SquareTextured(TextureFromBMP tex, float scale)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, scale, 0, 0, 0);
        }

        public SquareTextured(TextureFromBMP tex, float scale, int moveX, int moveY, int moveZ)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, scale, moveX, moveY, moveZ);
        }

        private void SetCoordinates(int sideVal, float scaleVal, int _moveX, int _moveY, int _moveZ)
        {
            vertices[0] = new Vector3(0 + _moveX, 0 + _moveY, 0 + _moveZ);
            vertices[1] = new Vector3((int)(sideVal * scaleVal) + _moveX, 0 + _moveY, 0 + _moveZ);
            vertices[2] = new Vector3((int)(sideVal * scaleVal) + _moveX, 0 + _moveY, (int)(sideVal * scaleVal) + _moveZ);
            vertices[3] = new Vector3(0 + _moveX, 0 + _moveY, (int)(sideVal * scaleVal) + _moveZ);
        }

        public void SetVisible()
        {
            Visible = true;
        }

        public void SetInvisible()
        {
            Visible = false;
        }

        public void ToggleVisibility()
        {
            if (Visible)
            {
                SetInvisible();
            }
            else
            {
                SetVisible();
            }
        }
        public bool IsVisible()
        {
            return Visible;
        }

        public void SetColour(Color col)
        {
            Colour = col;
        }

        public Color GetColour()
        {
            return Colour;
        }

        public void DrawMe()
        {
            if (Visible)
            {
                GL.BindTexture(TextureTarget.Texture2D, this.Texture.id);

                GL.Color3(this.Colour);

                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 0);
                GL.Vertex3(this.vertices[0]);
                GL.TexCoord2(0, 1);
                GL.Vertex3(this.vertices[1]);
                GL.TexCoord2(1, 1);
                GL.Vertex3(this.vertices[2]);
                GL.TexCoord2(1, 0);
                GL.Vertex3(this.vertices[3]);
                GL.End();
            }
        }

    }
}
