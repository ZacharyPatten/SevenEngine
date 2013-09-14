using OpenTK;

namespace Seven
{
  partial class Seven
  {
    /*
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
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Text = "Form1";
    }

    #endregion
     * */

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
      this.glControl = new GLControl();
      this.SuspendLayout();
      // 
      // simpleOpenGlControl1
      // 
      //this.glControl.AccumBits = ((byte)(0));
      //this.glControl.AutoCheckErrors = false;
      //this.glControl.AutoFinish = false;
      //this.glControl.AutoMakeCurrent = true;
      //this.glControl.AutoSwapBuffers = true;
      this.glControl.BackColor = System.Drawing.Color.Black;
      //this.glControl.ColorBits = ((byte)(32));
      //this.glControl.DepthBits = ((byte)(16));
      this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.glControl.Location = new System.Drawing.Point(0, 0);
      this.glControl.Name = "glControl";
      this.glControl.Size = new System.Drawing.Size(284, 264);
      //this.glControl.StencilBits = ((byte)(0));
      this.glControl.TabIndex = 0;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 264);
      this.Controls.Add(this.glControl);
      this.Name = "Seven";
      this.Text = "Seven";
      this.ResumeLayout(false);

    }

    #endregion

    private GLControl glControl;
  }
}

