ReTester\3DObj\TexturedCube.cs
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ReTester._3DObj
{
    // Simple immediate-mode textured cube. Each face maps the full texture.
    public class TexturedCube
    {
        public TextureFromBMP Texture;
        public Vector3 Position;
        public float Size;
        bool Visible = false;

        public TexturedCube(TextureFromBMP texture, float size = 20f, float posX = 0, float posY = 0, float posZ = 0)
        {
            Texture = texture;
            Size = size;
            Position = new Vector3(posX, posY, posZ);
        }

        public void SetVisible() => Visible = true;
        public void SetInvisible() => Visible = false;
        public void ToggleVisibility() => Visible = !Visible;
        public bool IsVisible() => Visible;

        public void DrawMe()
        {
            if (!Visible || Texture == null) return;

            GL.BindTexture(TextureTarget.Texture2D, Texture.id);
            GL.Color3(1.0f, 1.0f, 1.0f);

            float hs = Size * 0.5f;

            Vector3 p = Position;

            // Front face
            GL.Begin(PrimitiveType.Quads);
            // Front
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z + hs);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z + hs);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z + hs);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z + hs);

            // Back
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z - hs);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z - hs);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z - hs);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z - hs);

            // Left
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z - hs);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z + hs);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z + hs);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z - hs);

            // Right
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z - hs);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z - hs);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z + hs);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z + hs);

            // Top
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z - hs);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y + hs, p.Z + hs);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z + hs);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y + hs, p.Z - hs);

            // Bottom
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z - hs);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z - hs);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(p.X + hs, p.Y - hs, p.Z + hs);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(p.X - hs, p.Y - hs, p.Z + hs);

            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}