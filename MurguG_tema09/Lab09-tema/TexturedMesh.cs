ReTester\3DObj\TexturedMesh.cs
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace ReTester._3DObj
{
    // Lightweight textured triangle-mesh renderer that accepts arbitrary vertices, UVs and indices.
    public class TexturedMesh
    {
        public TextureFromBMP Texture;
        public List<Vector3> Vertices = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();
        public List<int> Indices = new List<int>();
        bool Visible = false;

        public TexturedMesh(TextureFromBMP tex)
        {
            Texture = tex;
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

            // Draw indexed triangles
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i + 2 < Indices.Count; i += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    int idx = Indices[i + k];
                    if (idx < 0 || idx >= Vertices.Count) continue;
                    Vector2 uv = (idx < UVs.Count) ? UVs[idx] : new Vector2(0, 0);
                    GL.TexCoord2(uv.X, uv.Y);
                    Vector3 v = Vertices[idx];
                    GL.Vertex3(v);
                }
            }
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Factory: create a simple irregular non-rectangular surface (a textured pentagon triangulated).
        public static TexturedMesh CreateSamplePentagon(TextureFromBMP tex, float scale = 25f, float moveX = 15f, float moveY = 20f, float moveZ = 15f)
        {
            var mesh = new TexturedMesh(tex);
            // pentagon vertices (in XZ plane, Y up)
            mesh.Vertices.Add(new Vector3(0 + moveX, 0 + moveY, 0 + moveZ));
            mesh.Vertices.Add(new Vector3(scale + moveX, 0 + moveY, scale * 0.2f + moveZ));
            mesh.Vertices.Add(new Vector3(scale * 0.6f + moveX, 0 + moveY, scale + moveZ));
            mesh.Vertices.Add(new Vector3(-scale * 0.4f + moveX, 0 + moveY, scale + moveZ));
            mesh.Vertices.Add(new Vector3(-scale + moveX, 0 + moveY, scale * 0.2f + moveZ));

            // UVs (map the texture across the pentagon)
            mesh.UVs.Add(new Vector2(0.5f, 0.0f));
            mesh.UVs.Add(new Vector2(1.0f, 0.25f));
            mesh.UVs.Add(new Vector2(0.8f, 1.0f));
            mesh.UVs.Add(new Vector2(0.2f, 1.0f));
            mesh.UVs.Add(new Vector2(0.0f, 0.25f));

            // Triangulate (fan around vertex 0)
            mesh.Indices.Add(0); mesh.Indices.Add(1); mesh.Indices.Add(2);
            mesh.Indices.Add(0); mesh.Indices.Add(2); mesh.Indices.Add(3);
            mesh.Indices.Add(0); mesh.Indices.Add(3); mesh.Indices.Add(4);

            return mesh;
        }
    }
}