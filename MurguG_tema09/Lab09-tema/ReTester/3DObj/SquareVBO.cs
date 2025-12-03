using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace ReTester
{
    public class SquareVBO
    {
        public Vector3[] vertices = new Vector3[8];
        public TextureFromBMP Texture;
        Color Colour;
        bool Visible;
        int side = 25;

        int VBO;

        public SquareVBO(TextureFromBMP tex)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, 1.0f, 0, 0, 0);
        }

        public SquareVBO(TextureFromBMP tex, float scale)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, scale, 0, 0, 0);
        }

        public SquareVBO(TextureFromBMP tex, float scale, int moveX, int moveY, int moveZ)
        {
            Visible = false;
            Colour = Color.White;
            Texture = tex;

            SetCoordinates(side, scale, moveX, moveY, moveZ);
        }

        private void SetCoordinates(int sideVal, float scaleVal, int _moveX, int _moveY, int _moveZ)
        {
            vertices[0] = new Vector3(0 + _moveX, 0 + _moveY, 0 + _moveZ);
            vertices[1] = new Vector3(0, 0, 0);
            vertices[2] = new Vector3((int)(sideVal * scaleVal) + _moveX, 0 + _moveY, 0 + _moveZ);
            vertices[3] = new Vector3(1, 0, 0);
            vertices[4] = new Vector3((int)(sideVal * scaleVal) + _moveX, 0 + _moveY, (int)(sideVal * scaleVal) + _moveZ);
            vertices[5] = new Vector3(1, 1, 0);
            vertices[6] = new Vector3(0 + _moveX, 0 + _moveY, (int)(sideVal * scaleVal) + _moveZ);
            vertices[7] = new Vector3(0, 1, 0);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(Vector3.SizeInBytes * vertices.Length), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.VertexPointer(3, VertexPointerType.Float, Vector3.SizeInBytes * 2, 0);
                GL.TexCoordPointer(3, TexCoordPointerType.Float, Vector3.SizeInBytes * 2, Vector3.SizeInBytes);
                
                GL.BindTexture(TextureTarget.Texture2D, this.Texture.id);

                GL.Color3(this.Colour);

                GL.DrawArrays(PrimitiveType.Quads, 0, vertices.Length / 2);

            }
        }

    }
}
