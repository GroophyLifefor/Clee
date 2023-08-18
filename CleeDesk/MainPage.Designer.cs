using System.ComponentModel;

namespace CleeDesk
{
    partial class MainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.EditorTab = new System.Windows.Forms.TabPage();
            this.Result = new ScintillaNET.Scintilla();
            this.Editor = new ScintillaNET.Scintilla();
            this.LogsTab = new System.Windows.Forms.TabPage();
            this.Logs = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.EditorTab.SuspendLayout();
            this.LogsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.EditorTab);
            this.tabControl1.Controls.Add(this.LogsTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // EditorTab
            // 
            this.EditorTab.Controls.Add(this.Result);
            this.EditorTab.Controls.Add(this.Editor);
            this.EditorTab.Location = new System.Drawing.Point(4, 22);
            this.EditorTab.Name = "EditorTab";
            this.EditorTab.Padding = new System.Windows.Forms.Padding(3);
            this.EditorTab.Size = new System.Drawing.Size(792, 424);
            this.EditorTab.TabIndex = 0;
            this.EditorTab.Text = "Editor";
            this.EditorTab.UseVisualStyleBackColor = true;
            // 
            // Result
            // 
            this.Result.Location = new System.Drawing.Point(382, 0);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(394, 410);
            this.Result.TabIndex = 1;
            // 
            // Editor
            // 
            this.Editor.Location = new System.Drawing.Point(0, 0);
            this.Editor.Name = "Editor";
            this.Editor.Size = new System.Drawing.Size(339, 410);
            this.Editor.TabIndex = 0;
            // 
            // LogsTab
            // 
            this.LogsTab.Controls.Add(this.Logs);
            this.LogsTab.Location = new System.Drawing.Point(4, 22);
            this.LogsTab.Name = "LogsTab";
            this.LogsTab.Padding = new System.Windows.Forms.Padding(3);
            this.LogsTab.Size = new System.Drawing.Size(792, 424);
            this.LogsTab.TabIndex = 1;
            this.LogsTab.Text = "Logs";
            this.LogsTab.UseVisualStyleBackColor = true;
            // 
            // Logs
            // 
            this.Logs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logs.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Logs.Location = new System.Drawing.Point(3, 3);
            this.Logs.Name = "Logs";
            this.Logs.Size = new System.Drawing.Size(786, 418);
            this.Logs.TabIndex = 0;
            this.Logs.Text = "";
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainPage";
            this.Text = "CleeDesk | Groophy Lifefor | Batch-Man";
            this.SizeChanged += new System.EventHandler(this.MainPage_SizeChanged);
            this.tabControl1.ResumeLayout(false);
            this.EditorTab.ResumeLayout(false);
            this.LogsTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.RichTextBox Logs;

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage EditorTab;
        private System.Windows.Forms.TabPage LogsTab;
        private ScintillaNET.Scintilla Result;
        private ScintillaNET.Scintilla Editor;
    }
}