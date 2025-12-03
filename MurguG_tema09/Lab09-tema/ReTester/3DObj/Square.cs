using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace ReTester._3DObj
{

    public class Square
    {
        public Vector3[] vertices = new Vector3[4];
        Color Colour;
        bool Visible;
        int side = 25;

        public Square()
        {
            Visible = false;
            Colour = Color.Red;

            SetCoordinates(side, 1.0f, 0, 0, 0);
        }
        public Square(float scale)
        {
            Visible = false;
            Colour = Color.Red;

            SetCoordinates(side, scale, 0, 0, 0);
        }

        public Square(float scale, int moveX, int moveY, int moveZ)
        {
            Visible = false;
            Colour = Color.Red;

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
                GL.Color3(Colour);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex3(vertices[0]);
                GL.Vertex3(vertices[1]);
                GL.Vertex3(vertices[2]);
                GL.Vertex3(vertices[3]);
                GL.End();
            }
        }

    }


}
