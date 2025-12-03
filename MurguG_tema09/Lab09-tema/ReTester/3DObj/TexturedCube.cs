using OpenTK.Graphics.OpenGL;
namespace ReTester._3DObj
{
    public class TexturedCube
    {
        private TextureFromBMP texture;
        private bool visible = true;

        public TexturedCube(TextureFromBMP texture)
        {
            this.texture = texture;
        }

        public void DrawMe()
        {
            if (!visible) return;

            // Bind texture
            GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture.TextureID);

            // Draw a simple cube (6 faces)
            GL.Begin(PrimitiveType.Quads);

            // Front face
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, 1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, 1);

            // Back face
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, -1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, -1);

            // Left face
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(-1, -1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(-1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, -1);

            // Right face
            GL.TexCoord2(0, 0); GL.Vertex3(1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, 1, -1);

            // Top face
            GL.TexCoord2(0, 0); GL.Vertex3(-1, 1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, 1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, 1);

            // Bottom face
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, -1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, -1, 1);

            GL.End();
        }

        public void ToggleVisibility()
        {
            visible = !visible;
        }

        public void SetInvisible()
        {
            visible = false;
        }
    }
}