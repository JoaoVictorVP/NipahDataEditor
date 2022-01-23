namespace NipahDataEditor
{
    partial class main_window
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripMenuItem load_menu;
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.file_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.new_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.save_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAs_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exit_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.errorBox = new System.Windows.Forms.RichTextBox();
            this.textbox = new NipahScriptEditor.NRichTextBox();
            load_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // load_menu
            // 
            load_menu.Name = "load_menu";
            load_menu.Size = new System.Drawing.Size(114, 22);
            load_menu.Text = "Load";
            load_menu.Click += new System.EventHandler(this.load_menu_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // file_menu
            // 
            this.file_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_menu,
            load_menu,
            this.toolStripSeparator1,
            this.save_menu,
            this.saveAs_menu,
            this.toolStripSeparator2,
            this.exit_menu});
            this.file_menu.Name = "file_menu";
            this.file_menu.Size = new System.Drawing.Size(37, 20);
            this.file_menu.Text = "File";
            // 
            // new_menu
            // 
            this.new_menu.Name = "new_menu";
            this.new_menu.Size = new System.Drawing.Size(114, 22);
            this.new_menu.Text = "New";
            this.new_menu.Click += new System.EventHandler(this.new_menu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // save_menu
            // 
            this.save_menu.Name = "save_menu";
            this.save_menu.Size = new System.Drawing.Size(114, 22);
            this.save_menu.Text = "Save";
            this.save_menu.Click += new System.EventHandler(this.save_menu_Click);
            // 
            // saveAs_menu
            // 
            this.saveAs_menu.Name = "saveAs_menu";
            this.saveAs_menu.Size = new System.Drawing.Size(114, 22);
            this.saveAs_menu.Text = "Save As";
            this.saveAs_menu.Click += new System.EventHandler(this.saveAs_menu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(111, 6);
            // 
            // exit_menu
            // 
            this.exit_menu.Name = "exit_menu";
            this.exit_menu.Size = new System.Drawing.Size(114, 22);
            this.exit_menu.Text = "Exit";
            this.exit_menu.Click += new System.EventHandler(this.exit_menu_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // errorBox
            // 
            this.errorBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorBox.Location = new System.Drawing.Point(0, 424);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(800, 39);
            this.errorBox.TabIndex = 2;
            this.errorBox.Text = "";
            // 
            // textbox
            // 
            this.textbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox.Location = new System.Drawing.Point(0, 24);
            this.textbox.Name = "textbox";
            this.textbox.Size = new System.Drawing.Size(800, 403);
            this.textbox.TabIndex = 1;
            this.textbox.Text = "";
            this.textbox.TextChanged += new System.EventHandler(this.textbox_TextChanged);
            // 
            // main_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 463);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.textbox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "main_window";
            this.Text = "Nipah Data";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem file_menu;
        private System.Windows.Forms.ToolStripMenuItem new_menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem save_menu;
        private System.Windows.Forms.ToolStripMenuItem saveAs_menu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exit_menu;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private NipahScriptEditor.NRichTextBox textbox;
        private System.Windows.Forms.RichTextBox errorBox;
    }
}

