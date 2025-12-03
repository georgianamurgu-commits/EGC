using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using ReTester;   // necesar pentru TextureFromBMP

namespace ReTester._3DObj
{
    // Lightweight textured triangle mesh
    public class TexturedMesh
    {
        public TextureFromBMP Texture;
        public List<Vector3> Vertices = new List<Vector3>();
        public List<Vector2> UVs = new List<Vector2>();
        public List<int> Indices = new List<int>();

        private bool Visible = false;

        public TexturedMesh(TextureFromBMP tex)
        {
            Texture = tex;
        }
        public TexturedMesh(TextureFromBMP tex, float scale, int p1, int p2)
        {
            Texture = tex;

            // scale → control pentru cât se mărește obiectul
            if (Vertices.Count > 0)
            {
                for (int i = 0; i < Vertices.Count; i++)
                    Vertices[i] *= scale;
            }

            // p1 și p2 pot fi parametri extra (opționali)
            // dacă nu îi folosești acum, îi păstrăm pentru compatibilitate
        }

        public void SetVisible() => Visible = true;
        public void SetInvisible() => Visible = false;
        public void ToggleVisibility() => Visible = !Visible;
        public bool IsVisible() => Visible;

        public void DrawMe()
        {
            if (!Visible || Texture == null)
                return;

            // Bind texture
            GL.BindTexture(TextureTarget.Texture2D, Texture.id);
            GL.Color3(1.0f, 1.0f, 1.0f);

            // Draw triangles
            GL.Begin(PrimitiveType.Triangles);

            for (int i = 0; i + 2 < Indices.Count; i += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    int idx = Indices[i + k];

                    if (idx < 0 || idx >= Vertices.Count)
                        continue;

                    // UV safe fallback
                    Vector2 uv = (idx < UVs.Count) ? UVs[idx] : Vector2.Zero;
                    GL.TexCoord2(uv);

                    Vector3 v = Vertices[idx];
                    GL.Vertex3(v);
                }
            }

            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        // Example mesh: textured pentagon
        public static TexturedMesh CreateSamplePentagon(
            TextureFromBMP tex,
            float scale = 25f,
            float moveX = 15f,
            float moveY = 20f,
            float moveZ = 15f)
        {
            var mesh = new TexturedMesh(tex);

            mesh.Vertices.Add(new Vector3(moveX, moveY, moveZ));
            mesh.Vertices.Add(new Vector3(scale + moveX, moveY, 0.2f * scale + moveZ));
            mesh.Vertices.Add(new Vector3(0.6f * scale + moveX, moveY, scale + moveZ));
            mesh.Vertices.Add(new Vector3(-0.4f * scale + moveX, moveY, scale + moveZ));
            mesh.Vertices.Add(new Vector3(-scale + moveX, moveY, 0.2f * scale + moveZ));

            mesh.UVs.Add(new Vector2(0.5f, 0.0f));
            mesh.UVs.Add(new Vector2(1.0f, 0.25f));
            mesh.UVs.Add(new Vector2(0.8f, 1.0f));
            mesh.UVs.Add(new Vector2(0.2f, 1.0f));
            mesh.UVs.Add(new Vector2(0.0f, 0.25f));

            mesh.Indices.AddRange(new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4
            });

            return mesh;
        }
    }
}
