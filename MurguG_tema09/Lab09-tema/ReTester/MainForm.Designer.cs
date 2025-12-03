namespace ReTester
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainViewport = new OpenTK.GLControl();
            this.BtnForceRefresh = new System.Windows.Forms.Button();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.BtnSquareNormal = new System.Windows.Forms.Button();
            this.BtnSquareTextured = new System.Windows.Forms.Button();
            this.BtnSquareVbo = new System.Windows.Forms.Button();
            this.BtnSquaresReset = new System.Windows.Forms.Button();
            this.BtnCubeTextured = new System.Windows.Forms.Button();
            this.BtnMeshTextured = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MainViewport
            // 
            this.MainViewport.BackColor = System.Drawing.Color.Gray;
            this.MainViewport.Location = new System.Drawing.Point(12, 12);
            this.MainViewport.Name = "MainViewport";
            this.MainViewport.Size = new System.Drawing.Size(881, 624);
            this.MainViewport.TabIndex = 0;
            this.MainViewport.VSync = false;
            this.MainViewport.Paint += new System.Windows.Forms.PaintEventHandler(this.MainViewport_Paint);
            // 
            // BtnForceRefresh
            // 
            this.BtnForceRefresh.Location = new System.Drawing.Point(899, 612);
            this.BtnForceRefresh.Name = "BtnForceRefresh";
            this.BtnForceRefresh.Size = new System.Drawing.Size(155, 23);
            this.BtnForceRefresh.TabIndex = 1;
            this.BtnForceRefresh.Text = "Force viewport refresh";
            this.BtnForceRefresh.UseVisualStyleBackColor = true;
            this.BtnForceRefresh.Click += new System.EventHandler(this.BtnForceRefresh_Click);
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 500;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // BtnSquareNormal
            // 
            this.BtnSquareNormal.Location = new System.Drawing.Point(899, 12);
            this.BtnSquareNormal.Name = "BtnSquareNormal";
            this.BtnSquareNormal.Size = new System.Drawing.Size(155, 32);
            this.BtnSquareNormal.TabIndex = 3;
            this.BtnSquareNormal.Text = "Load square (normal)";
            this.BtnSquareNormal.UseVisualStyleBackColor = true;
            this.BtnSquareNormal.Click += new System.EventHandler(this.BtnSquareNormal_Click);
            // 
            // BtnSquareTextured
            // 
            this.BtnSquareTextured.Location = new System.Drawing.Point(899, 50);
            this.BtnSquareTextured.Name = "BtnSquareTextured";
            this.BtnSquareTextured.Size = new System.Drawing.Size(155, 32);
            this.BtnSquareTextured.TabIndex = 4;
            this.BtnSquareTextured.Text = "Load square (textured)";
            this.BtnSquareTextured.UseVisualStyleBackColor = true;
            this.BtnSquareTextured.Click += new System.EventHandler(this.BtnSquareTextured_Click);
            // 
            // BtnSquareVbo
            // 
            this.BtnSquareVbo.Location = new System.Drawing.Point(899, 88);
            this.BtnSquareVbo.Name = "BtnSquareVbo";
            this.BtnSquareVbo.Size = new System.Drawing.Size(155, 32);
            this.BtnSquareVbo.TabIndex = 5;
            this.BtnSquareVbo.Text = "Load square (VBO)";
            this.BtnSquareVbo.UseVisualStyleBackColor = true;
            this.BtnSquareVbo.Click += new System.EventHandler(this.BtnSquareVbo_Click);
            // 
            // BtnSquaresReset
            // 
            this.BtnSquaresReset.Location = new System.Drawing.Point(899, 126);
            this.BtnSquaresReset.Name = "BtnSquaresReset";
            this.BtnSquaresReset.Size = new System.Drawing.Size(155, 32);
            this.BtnSquaresReset.TabIndex = 6;
            this.BtnSquaresReset.Text = "Reset ALL squares";
            this.BtnSquaresReset.UseVisualStyleBackColor = true;
            this.BtnSquaresReset.Click += new System.EventHandler(this.BtnSquaresReset_Click);
            // 
            // BtnCubeTextured
            // 
            this.BtnCubeTextured.Location = new System.Drawing.Point(899, 164);
            this.BtnCubeTextured.Name = "BtnCubeTextured";
            this.BtnCubeTextured.Size = new System.Drawing.Size(155, 32);
            this.BtnCubeTextured.TabIndex = 7;
            this.BtnCubeTextured.Text = "Load textured cube";
            this.BtnCubeTextured.UseVisualStyleBackColor = true;
            this.BtnCubeTextured.Click += new System.EventHandler(this.BtnCubeTextured_Click);
            // 
            // BtnMeshTextured
            // 
            this.BtnMeshTextured.Location = new System.Drawing.Point(899, 202);
            this.BtnMeshTextured.Name = "BtnMeshTextured";
            this.BtnMeshTextured.Size = new System.Drawing.Size(155, 32);
            this.BtnMeshTextured.TabIndex = 8;
            this.BtnMeshTextured.Text = "Load textured mesh";
            this.BtnMeshTextured.UseVisualStyleBackColor = true;
            this.BtnMeshTextured.Click += new System.EventHandler(this.BtnMeshTextured_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 648);
            this.Controls.Add(this.BtnMeshTextured);
            this.Controls.Add(this.BtnCubeTextured);
            this.Controls.Add(this.BtnSquaresReset);
            this.Controls.Add(this.BtnSquareVbo);
            this.Controls.Add(this.BtnSquareTextured);
            this.Controls.Add(this.BtnSquareNormal);
            this.Controls.Add(this.BtnForceRefresh);
            this.Controls.Add(this.MainViewport);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl MainViewport;
        private System.Windows.Forms.Button BtnForceRefresh;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button BtnSquareNormal;
        private System.Windows.Forms.Button BtnSquareTextured;
        private System.Windows.Forms.Button BtnSquareVbo;
        private System.Windows.Forms.Button BtnSquaresReset;
        private System.Windows.Forms.Button BtnCubeTextured;
        private System.Windows.Forms.Button BtnMeshTextured;
    }
}

