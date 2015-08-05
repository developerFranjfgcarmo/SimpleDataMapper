namespace Tareas
{
    partial class FrmAcceso
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAcceso));
            this.tbCtrlAccesos = new System.Windows.Forms.TabControl();
            this.tgPgUsuario = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSession = new System.Windows.Forms.Button();
            this.chckRemenber = new System.Windows.Forms.CheckBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPgConexion = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.btnConnection = new System.Windows.Forms.Button();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtDataBase = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.Usuario = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlCarga = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCtrlAccesos.SuspendLayout();
            this.tgPgUsuario.SuspendLayout();
            this.tbPgConexion.SuspendLayout();
            this.pnlCarga.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCtrlAccesos
            // 
            this.tbCtrlAccesos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCtrlAccesos.Controls.Add(this.tgPgUsuario);
            this.tbCtrlAccesos.Controls.Add(this.tbPgConexion);
            this.tbCtrlAccesos.Controls.Add(this.tabPage1);
            this.tbCtrlAccesos.HotTrack = true;
            this.tbCtrlAccesos.ItemSize = new System.Drawing.Size(165, 20);
            this.tbCtrlAccesos.Location = new System.Drawing.Point(0, 45);
            this.tbCtrlAccesos.Margin = new System.Windows.Forms.Padding(0);
            this.tbCtrlAccesos.Name = "tbCtrlAccesos";
            this.tbCtrlAccesos.SelectedIndex = 0;
            this.tbCtrlAccesos.Size = new System.Drawing.Size(368, 247);
            this.tbCtrlAccesos.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbCtrlAccesos.TabIndex = 3;
            // 
            // tgPgUsuario
            // 
            this.tgPgUsuario.BackColor = System.Drawing.SystemColors.Control;
            this.tgPgUsuario.Controls.Add(this.button1);
            this.tgPgUsuario.Controls.Add(this.btnSession);
            this.tgPgUsuario.Controls.Add(this.chckRemenber);
            this.tgPgUsuario.Controls.Add(this.txtUser);
            this.tgPgUsuario.Controls.Add(this.txtPass);
            this.tgPgUsuario.Controls.Add(this.label5);
            this.tgPgUsuario.Controls.Add(this.label6);
            this.tgPgUsuario.Location = new System.Drawing.Point(4, 24);
            this.tgPgUsuario.Margin = new System.Windows.Forms.Padding(0);
            this.tgPgUsuario.Name = "tgPgUsuario";
            this.tgPgUsuario.Size = new System.Drawing.Size(327, 156);
            this.tgPgUsuario.TabIndex = 0;
            this.tgPgUsuario.Text = "Usuario";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 25);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSession
            // 
            this.btnSession.BackColor = System.Drawing.SystemColors.Control;
            this.btnSession.FlatAppearance.BorderSize = 3;
            this.btnSession.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSession.Location = new System.Drawing.Point(117, 116);
            this.btnSession.Name = "btnSession";
            this.btnSession.Size = new System.Drawing.Size(92, 28);
            this.btnSession.TabIndex = 14;
            this.btnSession.Text = "Iniciar Sesión";
            this.btnSession.UseVisualStyleBackColor = false;
            this.btnSession.Click += new System.EventHandler(this.btnSession_Click);
            // 
            // chckRemenber
            // 
            this.chckRemenber.AutoSize = true;
            this.chckRemenber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chckRemenber.Location = new System.Drawing.Point(66, 93);
            this.chckRemenber.Name = "chckRemenber";
            this.chckRemenber.Size = new System.Drawing.Size(126, 17);
            this.chckRemenber.TabIndex = 13;
            this.chckRemenber.Text = "Recordar contraseña.";
            this.chckRemenber.UseVisualStyleBackColor = true;
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Location = new System.Drawing.Point(134, 24);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(129, 20);
            this.txtUser.TabIndex = 1;
            // 
            // txtPass
            // 
            this.txtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPass.Location = new System.Drawing.Point(133, 56);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(129, 20);
            this.txtPass.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Usuario:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Contraseña:";
            // 
            // tbPgConexion
            // 
            this.tbPgConexion.BackColor = System.Drawing.SystemColors.Control;
            this.tbPgConexion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tbPgConexion.Controls.Add(this.button2);
            this.tbPgConexion.Controls.Add(this.btnConnection);
            this.tbPgConexion.Controls.Add(this.txtUserId);
            this.tbPgConexion.Controls.Add(this.txtHost);
            this.tbPgConexion.Controls.Add(this.txtDataBase);
            this.tbPgConexion.Controls.Add(this.txtPassword);
            this.tbPgConexion.Controls.Add(this.Usuario);
            this.tbPgConexion.Controls.Add(this.label2);
            this.tbPgConexion.Controls.Add(this.label4);
            this.tbPgConexion.Controls.Add(this.label3);
            this.tbPgConexion.Location = new System.Drawing.Point(4, 24);
            this.tbPgConexion.Margin = new System.Windows.Forms.Padding(0);
            this.tbPgConexion.Name = "tbPgConexion";
            this.tbPgConexion.Size = new System.Drawing.Size(327, 156);
            this.tbPgConexion.TabIndex = 1;
            this.tbPgConexion.Text = "Datos Conexión";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnConnection
            // 
            this.btnConnection.BackColor = System.Drawing.SystemColors.Control;
            this.btnConnection.FlatAppearance.BorderSize = 3;
            this.btnConnection.Image = ((System.Drawing.Image)(resources.GetObject("btnConnection.Image")));
            this.btnConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConnection.Location = new System.Drawing.Point(88, 116);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(164, 28);
            this.btnConnection.TabIndex = 4;
            this.btnConnection.Text = "Establecer Conexión";
            this.btnConnection.UseVisualStyleBackColor = false;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // txtUserId
            // 
            this.txtUserId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserId.Location = new System.Drawing.Point(99, 12);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(199, 20);
            this.txtUserId.TabIndex = 0;
            this.txtUserId.Text = "postgres";
            // 
            // txtHost
            // 
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Location = new System.Drawing.Point(99, 90);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(199, 20);
            this.txtHost.TabIndex = 3;
            this.txtHost.Text = "192.168.1.126";
            // 
            // txtDataBase
            // 
            this.txtDataBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDataBase.Location = new System.Drawing.Point(99, 64);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(199, 20);
            this.txtDataBase.TabIndex = 2;
            this.txtDataBase.Text = "dbtareas";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(99, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(199, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "qpmz56tg";
            // 
            // Usuario
            // 
            this.Usuario.AutoSize = true;
            this.Usuario.Location = new System.Drawing.Point(26, 19);
            this.Usuario.Name = "Usuario";
            this.Usuario.Size = new System.Drawing.Size(46, 13);
            this.Usuario.TabIndex = 7;
            this.Usuario.Text = "Usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Contraseña:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Servidor:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Base Datos:";
            // 
            // pnlCarga
            // 
            this.pnlCarga.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCarga.Controls.Add(this.panel1);
            this.pnlCarga.Controls.Add(this.tbCtrlAccesos);
            this.pnlCarga.Controls.Add(this.pictureBox1);
            this.pnlCarga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCarga.Location = new System.Drawing.Point(0, 0);
            this.pnlCarga.Name = "pnlCarga";
            this.pnlCarga.Size = new System.Drawing.Size(374, 301);
            this.pnlCarga.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(374, 40);
            this.panel1.TabIndex = 17;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Tareas.Properties.Resources.table;
            this.pictureBox2.Location = new System.Drawing.Point(12, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(152, 87);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(360, 219);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 194);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(57, 22);
            this.button3.TabIndex = 1;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(354, 172);
            this.dataGridView1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(204, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "label7";
            // 
            // FrmAcceso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(374, 301);
            this.Controls.Add(this.pnlCarga);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAcceso";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Management Tasks";
            this.Load += new System.EventHandler(this.FrmAcceso_Load_1);
            this.tbCtrlAccesos.ResumeLayout(false);
            this.tgPgUsuario.ResumeLayout(false);
            this.tgPgUsuario.PerformLayout();
            this.tbPgConexion.ResumeLayout(false);
            this.tbPgConexion.PerformLayout();
            this.pnlCarga.ResumeLayout(false);
            this.pnlCarga.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbCtrlAccesos;
        private System.Windows.Forms.TabPage tgPgUsuario;
        private System.Windows.Forms.CheckBox chckRemenber;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tbPgConexion;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtDataBase;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label Usuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlCarga;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSession;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
    }
}