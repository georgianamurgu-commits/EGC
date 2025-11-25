using System;

using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_winforms_z02
{
    public partial class Form1 : Form
    {

        //Stări de control cameră.
        private int eyePosX, eyePosY, eyePosZ;


        private Point mousePos;
        private float camDepth;

        //Stări de control mouse.
        private bool statusControlMouse2D, statusControlMouse3D, statusMouseDown;

        //Stări de control axe de coordonate.
        private bool statusControlAxe;

        //Stări de control iluminare.
        private bool lightON;
        private bool lightON_0;
        private bool lightON_1; // new: state for light 1

        //Stări de control obiecte 3D.
        private string statusCube = "OFF";

        //Structuri de stocare a vertexurilor și a listelor de vertexuri.
        private int[,] arrVertex = new int[50, 3];         //Stocam matricea de vertexuri; 3 coloane vor reține informația pentru X, Y, Z. Nr. de linii definește nr. de vertexuri.
        private int nVertex;

        private int[] arrQuadsList = new int[100];        //Lista de vertexuri pentru construirea cubului prin intermediul quadurilor. Ne bazăm pe lista de vertexuri.
        private int nQuadsList;

        private int[] arrTrianglesList = new int[100];    //Lista de vertexuri pentru construirea cubului prin intermediul triunghiurilor. Ne bazăm pe lista de vertexuri.
        private int nTrianglesList;

        //Fișiere de in/out pentru manipularea vertexurilor.
        private string fileVertex = "vertexList.txt";
        private string fileQList = "quadsVertexList.txt";
        private string fileTList = "trianglesVertexList.txt";
        private bool statusFiles;



        //Dim valuesAmbientTemplate0() As Single = {255, 0, 0, 1.0}      //Valori alternative ambientale(lumină colorată)
        //# SET 1
        private float[] valuesAmbientTemplate0 = new float[] { 0.1f, 0.1f, 0.1f, 1.0f };
        private float[] valuesDiffuseTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        private float[] valuesSpecularTemplate0 = new float[] { 0.1f, 0.1f, 0.1f, 1.0f };
        private float[] valuesPositionTemplate0 = new float[] { 0.0f, 0.0f, 5.0f, 1.0f };
        //# SET 2
        //private float[] valuesAmbientTemplate0 = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
        //private float[] valuesDiffuseTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        //private float[] valuesSpecularTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        //private float[] valuesPositionTemplate0 = new float[] { 1.0f, 1.0f, 1.0f, 0.0f };

        private float[] valuesAmbient0 = new float[4];
        private float[] valuesDiffuse0 = new float[4];
        private float[] valuesSpecular0 = new float[4];
        private float[] valuesPosition0 = new float[4];

        // NEW: templates and storage for light1
        private float[] valuesAmbientTemplate1 = new float[] { 0.05f, 0.05f, 0.05f, 1.0f };
        private float[] valuesDiffuseTemplate1 = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
        private float[] valuesSpecularTemplate1 = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
        private float[] valuesPositionTemplate1 = new float[] { -5.0f, 5.0f, 5.0f, 1.0f };

        private float[] valuesAmbient1 = new float[4];
        private float[] valuesDiffuse1 = new float[4];
        private float[] valuesSpecular1 = new float[4];
        private float[] valuesPosition1 = new float[4];


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   ON_LOAD
        public Form1() {
            InitializeComponent();

            // Ensure application state and UI controls are initialized
            // before the GL control can paint. This fills light parameter arrays
            // and sets button texts so both lights can be shown and edited.
            SetupValues();
            SetupWindowGUI();

            /// TODO:
            /// În fișierul <Form1.Designer.cs>, la linia 26 înlocuiți conțînutul original cu linia de mai jos:
            ///         this.GlControl1 = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
            /// Acest mod de inițializare va activa antialiasing-ul (multisampling MSAA la 8x).
            /// ATENTȚIE!
            /// Veți pierde designerul grafic. Aplicația funcționează dar pentru a putea accesa designerul grafic va trebui să reveniți la constructorul
            /// implicit al controlului OpenTK!
        }

        private void Form1_Load(object sender, EventArgs e) {

            SetupValues();
            SetupWindowGUI();
        }

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   SETARI INIȚIALE

        private float ClampFloat(float value, float min, float max) {
            return (value < min) ? min : (value > max) ? max : value;
        }

        private void SetupValues() {
            eyePosX = 100;
            eyePosY = 100;
            eyePosZ = 50;

            // camDepth now represents field-of-view in degrees (not a "depth" factor).
            // Use a sensible default FOV (e.g., 60°).
            camDepth = 60.0f;

            setLight0Values();
            setLight1Values(); // initialize light1 values

            numericXeye.Value = eyePosX;
            numericYeye.Value = eyePosY;
            numericZeye.Value = eyePosZ;
        }


        private void SetupWindowGUI() {
 
             setControlMouse2D(false);
             setControlMouse3D(false);
 
             // Numeric control should represent degrees for FOV; set a safe range [1..179].
             numericCameraDepth.Minimum = 1;
             numericCameraDepth.Maximum = 179;
             numericCameraDepth.Value = (int)ClampFloat(camDepth, 1f, 179f);
 
             setControlAxe(true);
 
             setCubeStatus("OFF");

             // Show lighting and both sources by default so the user sees them
             // and can edit parameters for both lights immediately.
             setIlluminationStatus(true);
             setSource0Status(true);
             setSource1Status(true);
 
             setTrackLigh0Default();
             setColorAmbientLigh0Default();
             setColorDifuseLigh0Default();
             setColorSpecularLigh0Default();
 
             // NEW: initialize light1 controls defaults
             setTrackLigh1Default();
             setColorAmbientLigh1Default();
             setColorDifuseLigh1Default();
             setColorSpecularLigh1Default();
         }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   MANIPULARE VERTEXURI ȘI LISTE DE COORDONATE.
        //Încărcarea coordonatelor vertexurilor și lista de compunere a obiectelor 3D.
        private void loadVertex() {

            //Testăm dacă fișierul există
            try {
                StreamReader fileReader = new StreamReader((fileVertex));
                nVertex = Convert.ToInt32(fileReader.ReadLine().Trim());
                Console.WriteLine("Vertexuri citite: " + nVertex.ToString());

                string tmpStr = "";
                string[] str = new string[3];
                for (int i = 0; i < nVertex; i++) {
                    tmpStr = fileReader.ReadLine();
                    str = tmpStr.Trim().Split(' ');
                    arrVertex[i, 0] = Convert.ToInt32(str[0].Trim());
                    arrVertex[i, 1] = Convert.ToInt32(str[1].Trim());
                    arrVertex[i, 2] = Convert.ToInt32(str[2].Trim());
                }
                fileReader.Close();

            } catch (Exception) {
                statusFiles = false;
                Console.WriteLine("Fisierul cu informații vertex <" + fileVertex + "> nu exista!");
                MessageBox.Show("Fisierul cu informații vertex <" + fileVertex + "> nu exista!");
            }
        }

        private void loadQList() {

            //Testăm dacă fișierul există
            try {
                StreamReader fileReader = new StreamReader(fileQList);

                int tmp;
                string line;
                nQuadsList = 0;

                while ((line = fileReader.ReadLine()) != null) {
                    tmp = Convert.ToInt32(line.Trim());
                    arrQuadsList[nQuadsList] = tmp;
                    nQuadsList++;
                }

                fileReader.Close();
            } catch (Exception) {
                statusFiles = false;
                MessageBox.Show("Fisierul cu informații vertex <" + fileQList + "> nu exista!");
            }

        }

        private void loadTList() {

            //Testăm dacă fișierul există
            try {
                StreamReader fileReader = new StreamReader(fileTList);

                int tmp;
                string line;
                nTrianglesList = 0;

                while ((line = fileReader.ReadLine()) != null) {
                    tmp = Convert.ToInt32(line.Trim());
                    arrTrianglesList[nTrianglesList] = tmp;
                    nTrianglesList++;
                }

                fileReader.Close();
            } catch (Exception) {
                statusFiles = false;
                MessageBox.Show("Fisierul cu informații vertex <" + fileTList + "> nu exista!");
            }

        }

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   CONTROL CAMERĂ

        //Controlul camerei după axa X cu spinner numeric (un cadran).
        private void numericXeye_ValueChanged(object sender, EventArgs e) {
            eyePosX = (int)numericXeye.Value;
            GlControl1.Invalidate(); //Forțează redesenarea întregului control OpenGL. Modificările vor fi luate în considerare (actualizare).
        }
        //Controlul camerei după axa Y cu spinner numeric (un cadran).
        private void numericYeye_ValueChanged(object sender, EventArgs e) {
            eyePosY = (int)numericYeye.Value;
            GlControl1.Invalidate(); //Forțează redesenarea întregului control OpenGL. Modificările vor fi luate în considerare (actualizare).
        }
        //Controlul camerei după axa Z cu spinner numeric (un cadran).
        private void numericZeye_ValueChanged(object sender, EventArgs e) {
            eyePosZ = (int)numericZeye.Value;
            GlControl1.Invalidate(); //Forțează redesenarea întregului control OpenGL. Modificările vor fi luate în considerare (actualizare).
        }
        //Controlul adâncimii camerei față de (0,0,0).
        private void numericCameraDepth_ValueChanged(object sender, EventArgs e) {
            // numericCameraDepth now stores degrees. Keep camDepth in degrees for UI/logic.
            camDepth = (float)numericCameraDepth.Value;
            GlControl1.Invalidate();
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   CONTROL MOUSE
        //Setăm variabila de stare pentru rotația în 2D a mouseui.
        private void setControlMouse2D(bool status) {
            if (status == false) {
                statusControlMouse2D = false;
                btnMouseControl2D.Text = "2D mouse control OFF";
            } else {
                statusControlMouse2D = true;
                btnMouseControl2D.Text = "2D mouse control ON";
            }
        }
        //Setăm variabila de stare pentru rotația în 3D a mouseui.
        private void setControlMouse3D(bool status) {
            if (status == false) {
                statusControlMouse3D = false;
                btnMouseControl3D.Text = "3D mouse control OFF";
            } else {
                statusControlMouse3D = true;
                btnMouseControl3D.Text = "3D mouse control ON";
            }
        }

        //Controlul mișcării setului de coordonate cu ajutorul mouseului (în plan 2D/3D)
        private void btnMouseControl2D_Click(object sender, EventArgs e) {
            if (statusControlMouse2D == true) {
                setControlMouse2D(false);
            } else {
                setControlMouse3D(false);
                setControlMouse2D(true);
            }
        }
        private void btnMouseControl3D_Click(object sender, EventArgs e) {
            if (statusControlMouse3D == true) {
                setControlMouse3D(false);
            } else {
                setControlMouse2D(false);
                setControlMouse3D(true);
            }
        }



        //Mișcarea lumii 3D cu ajutorul mouselui (click'n'drag de mouse).
        private void GlControl1_MouseMove(object sender, MouseEventArgs e) {
            if (statusMouseDown == true) {
                mousePos = new Point(e.X, e.Y);
                GlControl1.Invalidate();     //Forțează redesenarea întregului control OpenGL. Modificările vor fi luate în considerare (actualizare).
            }
        }
        private void GlControl1_MouseDown(object sender, MouseEventArgs e) {
            statusMouseDown = true;
        }
        private void GlControl1_MouseUp(object sender, MouseEventArgs e) {
            statusMouseDown = false;
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   CONTROL ILUMINARE
        //Setăm variabila de stare pentru iluminare.
        private void setIlluminationStatus(bool status) {
            if (status == false) {
                lightON = false;
                btnLights.Text = "Iluminare OFF";
            } else {
                lightON = true;
                btnLights.Text = "Iluminare ON";
            }
        }

        //Activăm/dezactivăm iluminarea.
        private void btnLights_Click(object sender, EventArgs e) {
            if (lightON == false) {
                setIlluminationStatus(true);
            } else {
                setIlluminationStatus(false);
            }
            GlControl1.Invalidate();
        }

        //Identifică numărul maxim de lumini pentru implementarea curentă a OpenGL.
        private void btnLightsNo_Click(object sender, EventArgs e) {
            int nr = GL.GetInteger(GetPName.MaxLights);
            MessageBox.Show("Nr. maxim de luminii pentru aceasta implementare este <" + nr.ToString() + ">.");
        }

        //Setăm variabila de stare pentru sursa de lumină 0.
        private void setSource0Status(bool status) {
            if (status == false) {
                lightON_0 = false;
                btnLight0.Text = "Sursa 0 OFF";
            } else {
                lightON_0 = true;
                btnLight0.Text = "Sursa 0 ON";
            }
        }

        // NEW: set state for source 1
        private void setSource1Status(bool status)
        {
            if (status == false)
            {
                lightON_1 = false;
                btnLight1.Text = "Sursa 1 OFF";
            }
            else
            {
                lightON_1 = true;
                btnLight1.Text = "Sursa 1 ON";
            }
        }

        //Activăm/dezactivăm sursa 0 de iluminare (doar dacă iluminarea este activă).
        private void btnLight0_Click(object sender, EventArgs e) {
            if (lightON == true) {
                if (lightON_0 == false) {
                    setSource0Status(true);
                } else {
                    setSource0Status(false);
                }
                GlControl1.Invalidate();
            }
        }

        // NEW: handler for Light1 button
        private void btnLight1_Click(object sender, EventArgs e)
        {
            if (lightON == true)
            {
                if (lightON_1 == false)
                {
                    setSource1Status(true);
                }
                else
                {
                    setSource1Status(false);
                }
                GlControl1.Invalidate();
            }
        }

        //Schimbăm poziția sursei 0 de iluminare după axele XYZ.
        private void setTrackLigh0Default() {
            trackLight0PositionX.Value = (int)valuesPosition0[0];
            trackLight0PositionY.Value = (int)valuesPosition0[1];
            trackLight0PositionZ.Value = (int)valuesPosition0[2];
        }
        private void trackLight0PositionX_Scroll(object sender, EventArgs e) {
            valuesPosition0[0] = trackLight0PositionX.Value;
            GlControl1.Invalidate();
        }
        private void trackLight0PositionY_Scroll(object sender, EventArgs e) {
            valuesPosition0[1] = trackLight0PositionY.Value;
            GlControl1.Invalidate();
        }
        private void trackLight0PositionZ_Scroll(object sender, EventArgs e) {
            valuesPosition0[2] = trackLight0PositionZ.Value;
            GlControl1.Invalidate();
        }

        // NEW: Light1 position controls
        private void setTrackLigh1Default()
        {
            trackLight1PositionX.Value = (int)valuesPosition1[0];
            trackLight1PositionY.Value = (int)valuesPosition1[1];
            trackLight1PositionZ.Value = (int)valuesPosition1[2];
        }
        private void trackLight1PositionX_Scroll(object sender, EventArgs e)
        {
            valuesPosition1[0] = trackLight1PositionX.Value;
            GlControl1.Invalidate();
        }
        private void trackLight1PositionY_Scroll(object sender, EventArgs e)
        {
            valuesPosition1[1] = trackLight1PositionY.Value;
            GlControl1.Invalidate();
        }
        private void trackLight1PositionZ_Scroll(object sender, EventArgs e)
        {
            valuesPosition1[2] = trackLight1PositionZ.Value;
            GlControl1.Invalidate();
        }

        //Schimbăm culoarea sursei de lumină 0 (ambiental) în domeniul RGB.
        private void setColorAmbientLigh0Default() {
            // Convert float to decimal before multiplying
            numericLight0Ambient_Red.Value = (decimal)valuesAmbient0[0] * 100m;
            numericLight0Ambient_Green.Value = (decimal)valuesAmbient0[1] * 100m;
            numericLight0Ambient_Blue.Value = (decimal)valuesAmbient0[2] * 100m;
        }
        private void numericLight0Ambient_Red_ValueChanged(object sender, EventArgs e) {
            valuesAmbient0[0] = (float)numericLight0Ambient_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Ambient_Green_ValueChanged(object sender, EventArgs e) {
            valuesAmbient0[1] = (float)numericLight0Ambient_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Ambient_Blue_ValueChanged(object sender, EventArgs e) {
            valuesAmbient0[2] = (float)numericLight0Ambient_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        // NEW: Light1 ambient color controls
        private void setColorAmbientLigh1Default()
        {
            numericLight1Ambient_Red.Value = (decimal)valuesAmbient1[0] * 100m;
            numericLight1Ambient_Green.Value = (decimal)valuesAmbient1[1] * 100m;
            numericLight1Ambient_Blue.Value = (decimal)valuesAmbient1[2] * 100m;
        }
        private void numericLight1Ambient_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient1[0] = (float)numericLight1Ambient_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Ambient_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient1[1] = (float)numericLight1Ambient_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Ambient_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesAmbient1[2] = (float)numericLight1Ambient_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        //Schimbăm culoarea sursei de lumină 0 (difuză) în domeniul RGB.
        private void setColorDifuseLigh0Default() {
            numericLight0Difuse_Red.Value = (decimal)(valuesDiffuse0[0] * 100f);
            numericLight0Difuse_Green.Value = (decimal)(valuesDiffuse0[1] * 100f);
            numericLight0Difuse_Blue.Value = (decimal)(valuesDiffuse0[2] * 100f);
        }
        private void numericLight0Difuse_Red_ValueChanged(object sender, EventArgs e) {
            valuesDiffuse0[0] = (float)numericLight0Difuse_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Difuse_Green_ValueChanged(object sender, EventArgs e) {
            valuesDiffuse0[1] = (float)numericLight0Difuse_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Difuse_Blue_ValueChanged(object sender, EventArgs e) {
            valuesDiffuse0[2] = (float)numericLight0Difuse_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        // NEW: Light1 diffuse color controls
        private void setColorDifuseLigh1Default()
        {
            numericLight1Difuse_Red.Value = (decimal)(valuesDiffuse1[0] * 100f);
            numericLight1Difuse_Green.Value = (decimal)(valuesDiffuse1[1] * 100f);
            numericLight1Difuse_Blue.Value = (decimal)(valuesDiffuse1[2] * 100f);
        }
        private void numericLight1Difuse_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse1[0] = (float)numericLight1Difuse_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Difuse_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse1[1] = (float)numericLight1Difuse_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Difuse_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesDiffuse1[2] = (float)numericLight1Difuse_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        //Schimbăm culoarea sursei de lumină 0 (specular) în domeniul RGB.
        private void setColorSpecularLigh0Default() {
            numericLight0Specular_Red.Value = (decimal)(valuesSpecular0[0] * 100f);
            numericLight0Specular_Green.Value = (decimal)(valuesSpecular0[1] * 100f);
            numericLight0Specular_Blue.Value = (decimal)(valuesSpecular0[2] * 100f);
        }
        private void numericLight0Specular_Red_ValueChanged(object sender, EventArgs e) {
            valuesSpecular0[0] = (float)numericLight0Specular_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Specular_Green_ValueChanged(object sender, EventArgs e) {
            valuesSpecular0[1] = (float)numericLight0Specular_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight0Specular_Blue_ValueChanged(object sender, EventArgs e) {
            valuesSpecular0[2] = (float)numericLight0Specular_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        // NEW: Light1 specular color controls
        private void setColorSpecularLigh1Default()
        {
            numericLight1Specular_Red.Value = (decimal)(valuesSpecular1[0] * 100f);
            numericLight1Specular_Green.Value = (decimal)(valuesSpecular1[1] * 100f);
            numericLight1Specular_Blue.Value = (decimal)(valuesSpecular1[2] * 100f);
        }
        private void numericLight1Specular_Red_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular1[0] = (float)numericLight1Specular_Red.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Specular_Green_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular1[1] = (float)numericLight1Specular_Green.Value / 100;
            GlControl1.Invalidate();
        }
        private void numericLight1Specular_Blue_ValueChanged(object sender, EventArgs e)
        {
            valuesSpecular1[2] = (float)numericLight1Specular_Blue.Value / 100;
            GlControl1.Invalidate();
        }

        //Resetare stare sursă de lumină nr. 0.
        private void setLight0Values() {
            for (int i = 0; i < valuesAmbientTemplate0.Length; i++) {
                valuesAmbient0[i] = valuesAmbientTemplate0[i];
            }
            for (int i = 0; i < valuesDiffuseTemplate0.Length; i++) {
                valuesDiffuse0[i] = valuesDiffuseTemplate0[i];
            }
            // copy specular template into valuesSpecular0
            for (int i = 0; i < valuesSpecularTemplate0.Length; i++) {
                valuesSpecular0[i] = valuesSpecularTemplate0[i];
            }
            for (int i = 0; i < valuesPositionTemplate0.Length; i++) {
                valuesPosition0[i] = valuesPositionTemplate0[i];
            }
        }

        // NEW: Resetare stare sursă de lumină nr. 1.
        private void setLight1Values()
        {
            for (int i = 0; i < valuesAmbientTemplate1.Length; i++)
            {
                valuesAmbient1[i] = valuesAmbientTemplate1[i];
            }
            for (int i = 0; i < valuesDiffuseTemplate1.Length; i++)
            {
                valuesDiffuse1[i] = valuesDiffuseTemplate1[i];
            }
            for (int i = 0; i < valuesSpecularTemplate1.Length; i++)
            {
                valuesSpecular1[i] = valuesSpecularTemplate1[i];
            }
            for (int i = 0; i < valuesPositionTemplate1.Length; i++)
            {
                valuesPosition1[i] = valuesPositionTemplate1[i];
            }
        }

        private void btnLight0Reset_Click(object sender, EventArgs e) {
            setLight0Values();
            setTrackLigh0Default();
            setColorAmbientLigh0Default();
            setColorDifuseLigh0Default();
            setColorSpecularLigh0Default();
            GlControl1.Invalidate();
        }

        // Improved: btnLight1Reset will now reset values and controls for light1
        private void btnLight1Reset_Click(object sender, EventArgs e)
        {
            setLight1Values();
            setTrackLigh1Default();
            setColorAmbientLigh1Default();
            setColorDifuseLigh1Default();
            setColorSpecularLigh1Default();
            GlControl1.Invalidate();
        }

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   CONTROL OBIECTE 3D
        //Setăm variabila de stare pentru afișarea/scunderea sistemului de coordonate.
        private void setControlAxe(bool status) {
            if (status == false) {
                statusControlAxe = false;
                btnShowAxes.Text = "Axe Oxyz OFF";
            } else {
                statusControlAxe = true;
                btnShowAxes.Text = "Axe Oxyz ON";
            }
        }

        //Controlul axelor de coordonate (ON/OFF).
        private void btnShowAxes_Click(object sender, EventArgs e) {
            if (statusControlAxe == true) {
                setControlAxe(false);
            } else {
                setControlAxe(true);
            }
            GlControl1.Invalidate();
        }

        //Setăm variabila de stare pentru desenarea cubului. Valorile acceptabile sunt:
        //TRIANGLES = cubul este desenat, prin triunghiuri.
        //QUADS = cubul este desenat, prin quaduri.
        //OFF (sau orice altceva) = cubul nu este desenat.
        private void setCubeStatus(string status) {
            if (status.Trim().ToUpper().Equals("TRIANGLES")) {
                statusCube = "TRIANGLES";
            } else if (status.Trim().ToUpper().Equals("QUADS")) {
                statusCube = "QUADS";
            } else {
                statusCube = "OFF";
            }
        }
        private void btnCubeQ_Click(object sender, EventArgs e) {
            statusFiles = true;
            loadVertex();
            loadQList();
            setCubeStatus("QUADS");
            GlControl1.Invalidate();
        }
        private void btnCubeT_Click(object sender, EventArgs e) {
            statusFiles = true;
            loadVertex();
            loadTList();
            setCubeStatus("TRIANGLES");
            GlControl1.Invalidate();
        }
        private void btnResetObjects_Click(object sender, EventArgs e) {
            setCubeStatus("OFF");
            GlControl1.Invalidate();
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   ADMINISTRARE MOD 3D (METODA PRINCIPALĂ)
        private void GlControl1_Paint(object sender, PaintEventArgs e) {
    //Resetează buffer-ele la valori default.
    GL.Clear(ClearBufferMask.ColorBufferBit);
    GL.Clear(ClearBufferMask.DepthBufferBit);

    //Culoarea default a mediului.
    GL.ClearColor(Color.Black);

    const float eps = 1e-4f;
    float fovRadians = (float)(Math.PI * camDepth / 180.0);
    fovRadians = ClampFloat(fovRadians, eps, (float)Math.PI - eps);

    float aspect = 1.0f;
    if (GlControl1.Height > 0) {
        aspect = (float)GlControl1.Width / (float)GlControl1.Height;
    }

    Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(fovRadians, aspect, 1f, 10000f);
    Matrix4 lookat = Matrix4.LookAt(eyePosX, eyePosY, eyePosZ, 0, 0, 0, 0, 1, 0);
    GL.MatrixMode(MatrixMode.Projection);
    GL.LoadIdentity();
    GL.LoadMatrix(ref perspective);
    GL.MatrixMode(MatrixMode.Modelview);
    GL.LoadIdentity();
    GL.LoadMatrix(ref lookat);
    GL.Viewport(0, 0, GlControl1.Width, GlControl1.Height);
    GL.Enable(EnableCap.DepthTest);
    GL.DepthFunc(DepthFunction.Less);

    if (lightON == true) {
        GL.Enable(EnableCap.Lighting);
    } else {
        GL.Disable(EnableCap.Lighting);
    }

    GL.Enable(EnableCap.ColorMaterial);
    GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
    GL.Enable(EnableCap.Normalize);
    GL.ShadeModel(ShadingModel.Smooth);

    // Configure ambient/diffuse/specular for both lights (positions set later)
    GL.Light(LightName.Light0, LightParameter.Ambient, valuesAmbient0);
    GL.Light(LightName.Light0, LightParameter.Diffuse, valuesDiffuse0);
    GL.Light(LightName.Light0, LightParameter.Specular, valuesSpecular0);

    GL.Light(LightName.Light1, LightParameter.Ambient, valuesAmbient1);
    GL.Light(LightName.Light1, LightParameter.Diffuse, valuesDiffuse1);
    GL.Light(LightName.Light1, LightParameter.Specular, valuesSpecular1);

    // Controlul rotației cu mouse-ul (2D).
    if (statusControlMouse2D == true) {
        GL.Rotate(mousePos.X, 0, 1, 0);
    }

    // Controlul rotației cu mouse-ul (3D).
    if (statusControlMouse3D == true) {
        GL.Rotate(mousePos.X, 0, 1, 1);
    }

    // Now set light positions after camera/rotation transforms so they are in the same space
    GL.Light(LightName.Light0, LightParameter.Position, valuesPosition0);
    GL.Light(LightName.Light1, LightParameter.Position, valuesPosition1);

    if ((lightON == true) && (lightON_0 == true)) {
        GL.Enable(EnableCap.Light0);
    } else {
        GL.Disable(EnableCap.Light0);
    }

    if ((lightON == true) && (lightON_1 == true)) {
        GL.Enable(EnableCap.Light1);
    } else {
        GL.Disable(EnableCap.Light1);
    }

    if (statusControlAxe == true) {
        DeseneazaAxe();
    }

    if (string.Equals(statusCube, "QUADS", StringComparison.OrdinalIgnoreCase)) {
        DeseneazaCubQ();
    } else if (string.Equals(statusCube, "TRIANGLES", StringComparison.OrdinalIgnoreCase)) {
        DeseneazaCubT();
    }

    // Draw small visual markers for light positions (helps confirm lights exist & where they are)
    DrawLightMarkers();

    GlControl1.SwapBuffers();
}


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //   DESENARE OBIECTE 3D
        //Desenează axe XYZ.
        private void DeseneazaAxe() {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(75, 0, 0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 75, 0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 75);
            GL.End();
        }
        //Desenează cubul - quads.
        private void DeseneazaCubQ() {
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < nQuadsList; i++) {
                switch (i % 4) {
                    case 0:
                        GL.Color3(Color.Blue);
                        break;
                    case 1:
                        GL.Color3(Color.Red);
                        break;
                    case 2:
                        GL.Color3(Color.Green);
                        break;
                    case 3:
                        GL.Color3(Color.Yellow);
                        break;
                }
                int x = arrQuadsList[i];
                GL.Vertex3(arrVertex[x, 0], arrVertex[x, 1], arrVertex[x, 2]);
            }
            GL.End();
        }
        //Desenează cubul - triunghuri.
        private void DeseneazaCubT() {
            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < nTrianglesList; i++) {
                switch (i % 3) {
                    case 0:
                        GL.Color3(Color.Blue);
                        break;
                    case 1:
                        GL.Color3(Color.Red);
                        break;
                    case 2:
                        GL.Color3(Color.Green);
                        break;
                }
                int x = arrTrianglesList[i];
                GL.Vertex3(arrVertex[x, 0], arrVertex[x, 1], arrVertex[x, 2]);
            }
            GL.End();
        }

        /// <summary>
        /// Draws small visual markers at the positions of the two light sources.
        /// </summary>
        private void DrawLightMarkers()
        {
            // Light0 marker (white)
            GL.PushMatrix();
            GL.Translate(valuesPosition0[0], valuesPosition0[1], valuesPosition0[2]);
            GL.Disable(EnableCap.Lighting);
            GL.Color3(Color.White);
            DrawMarkerCube(2.0f);
            if (lightON) GL.Enable(EnableCap.Lighting);
            GL.PopMatrix();

            // Light1 marker (magenta)
            GL.PushMatrix();
            GL.Translate(valuesPosition1[0], valuesPosition1[1], valuesPosition1[2]);
            GL.Disable(EnableCap.Lighting);
            GL.Color3(Color.Magenta);
            DrawMarkerCube(2.0f);
            if (lightON) GL.Enable(EnableCap.Lighting);
            GL.PopMatrix();
        }

        /// <summary>
        /// Draws a small cube at the current position.
        /// </summary>
        private void DrawMarkerCube(float size)
        {
            float h = size / 2f;
            GL.Begin(PrimitiveType.Quads);

            // Front face
            GL.Vertex3(-h, -h, h);
            GL.Vertex3(h, -h, h);
            GL.Vertex3(h, h, h);
            GL.Vertex3(-h, h, h);

            // Back face
            GL.Vertex3(-h, -h, -h);
            GL.Vertex3(h, -h, -h);
            GL.Vertex3(h, h, -h);
            GL.Vertex3(-h, h, -h);

            // Left face
            GL.Vertex3(-h, -h, -h);
            GL.Vertex3(-h, -h, h);
            GL.Vertex3(-h, h, h);
            GL.Vertex3(-h, h, -h);

            // Right face
            GL.Vertex3(h, -h, -h);
            GL.Vertex3(h, -h, h);
            GL.Vertex3(h, h, h);
            GL.Vertex3(h, h, -h);

            // Top face
            GL.Vertex3(-h, h, -h);
            GL.Vertex3(h, h, -h);
            GL.Vertex3(h, h, h);
            GL.Vertex3(-h, h, h);

            // Bottom face
            GL.Vertex3(-h, -h, -h);
            GL.Vertex3(h, -h, -h);
            GL.Vertex3(h, -h, h);
            GL.Vertex3(-h, -h, h);

            GL.End();
        }

    }

}
