using OpenTK;
using OpenTK.Graphics.OpenGL;
using ReTester._3DObj;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReTester
{
    public partial class MainForm : Form
    {
        float camDepth;

        int eyePosX, eyePosY, eyePosZ;

        Axes mainAx;
        Square sq1;
        SquareTextured sq2;
        SquareVBO sq3;
        TextureFromBMP textureSquare2, textureSquare3;

        // New textured volumetric / mesh objects
        TexturedCube texturedCube;
        TexturedMesh texturedMesh;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupDefaultValues();
            SetupWindowGUI();
            SetupSceneObjects();

            MainTimer.Start();
        }

        private void SetupDefaultValues()
        {
            camDepth = 1.04f;
            eyePosX = 100;
            eyePosY = 100;
            eyePosZ = 50;
        }

        private void SetupWindowGUI()
        {

        }

        private void SetupSceneObjects()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.Texture2D);

            textureSquare2 = ContentLoader.LoadTexture("Content/Textures/pebbles.jpg");
            textureSquare3 = ContentLoader.LoadTexture("Content/Textures/map.png");

            mainAx = new Axes();
            sq1 = new Square();
            sq2 = new SquareTextured(textureSquare2, 1.0f, 15, 35, 15);
            sq3 = new SquareVBO(textureSquare3, 1.75f, 15, 72, 15);

            // Create volumetric textured objects (reuse existing textures)
            texturedCube = new TexturedCube(textureSquare3);
            texturedMesh = TexturedMesh.CreateSamplePentagon(textureSquare2);
            texturedMesh.SetInvisible();

            Console.WriteLine("=============-----------------======================");
            Console.WriteLine("" + textureSquare2.ToString());
            Console.WriteLine("=============-----------------======================");
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            MainViewport.Invalidate();
            MainTimer.Stop();
        }

        private void BtnForceRefresh_Click(object sender, EventArgs e)
        {
            MainViewport.Invalidate();
        }

        private void BtnSquareNormal_Click(object sender, EventArgs e)
        {
            sq1.ToggleVisibility();

            MainViewport.Invalidate();
        }

        private void BtnSquareTextured_Click(object sender, EventArgs e)
        {
            sq2.ToggleVisibility();

            MainViewport.Invalidate();
        }

        private void BtnSquareVbo_Click(object sender, EventArgs e)
        {
            sq3.ToggleVisibility();

            MainViewport.Invalidate();
        }

        private void BtnSquaresReset_Click(object sender, EventArgs e)
        {
            sq1.SetInvisible();
            sq2.SetInvisible();
            sq3.SetInvisible();

            MainViewport.Invalidate();
        }

        private void MainViewport_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Gray);

            SetView();

            // GRAPHICS PAYLOAD
            mainAx.DrawMe();
            sq1.DrawMe();
            sq2.DrawMe();
            sq3.DrawMe();

            // Draw volumetric textured objects and non-rectangular mesh
            if (texturedCube != null)
            {
                // Use a public method to check visibility, e.g. IsVisible()
                // If not available, call DrawMe() only if you want to draw when visible
                texturedCube.DrawMe();
            }
            texturedMesh.DrawMe();

            MainViewport.SwapBuffers();
        }

        private void SetView() {
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(camDepth, 4 / 3, 1, 256);    
            Matrix4 lookat = Matrix4.LookAt(eyePosX, eyePosY, eyePosZ, 0, 0, 0, 0, 1, 0);                   
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);                                                             
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, MainViewport.Width, MainViewport.Height);                                        

        }

        private void BtnCubeTextured_Click(object sender, EventArgs e)
        {
            // Toggle cube visibility and refresh viewport
            if (texturedCube != null) texturedCube.ToggleVisibility();
            MainViewport.Invalidate();
        }

        private void BtnMeshTextured_Click(object sender, EventArgs e)
        {
            // Toggle mesh visibility and refresh viewport
            // if (texturedMesh != null) texturedMesh.ToggleVisibility();
            // Instead, use a method that exists, e.g. SetInvisible(), or add a ToggleVisibility method to TexturedMesh.
            // For now, comment out or remove the call to ToggleVisibility to fix the error:
            if (texturedMesh != null)
                texturedMesh.ToggleVisibility();   // 🔹 Acum îl poți afișa/ascunde

            MainViewport.Invalidate();
        }

    }
}
