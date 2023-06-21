namespace CompiladorFinal
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
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
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Polishlbl = new System.Windows.Forms.Label();
            this.Polishdgv = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.sintactico_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvErrores = new System.Windows.Forms.DataGridView();
            this.dgvLexico = new System.Windows.Forms.DataGridView();
            this.ButtonSeparar = new System.Windows.Forms.Button();
            this.txtCodigoFuente = new System.Windows.Forms.TextBox();
            this.crearEnsambladorbtn = new System.Windows.Forms.Button();
            this.mensaje_lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Polishdgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexico)).BeginInit();
            this.SuspendLayout();
            // 
            // Polishlbl
            // 
            this.Polishlbl.AutoSize = true;
            this.Polishlbl.Location = new System.Drawing.Point(832, 7);
            this.Polishlbl.Name = "Polishlbl";
            this.Polishlbl.Size = new System.Drawing.Size(35, 13);
            this.Polishlbl.TabIndex = 37;
            this.Polishlbl.Text = "Polish";
            // 
            // Polishdgv
            // 
            this.Polishdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Polishdgv.Location = new System.Drawing.Point(835, 23);
            this.Polishdgv.Name = "Polishdgv";
            this.Polishdgv.Size = new System.Drawing.Size(432, 593);
            this.Polishdgv.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(504, 441);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Variables";
            // 
            // dgvVariables
            // 
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Location = new System.Drawing.Point(499, 457);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.Size = new System.Drawing.Size(313, 159);
            this.dgvVariables.TabIndex = 34;
            // 
            // sintactico_btn
            // 
            this.sintactico_btn.Enabled = false;
            this.sintactico_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sintactico_btn.Location = new System.Drawing.Point(1012, 621);
            this.sintactico_btn.Name = "sintactico_btn";
            this.sintactico_btn.Size = new System.Drawing.Size(83, 27);
            this.sintactico_btn.TabIndex = 31;
            this.sintactico_btn.Text = "Sintactico";
            this.sintactico_btn.UseVisualStyleBackColor = true;
            this.sintactico_btn.Click += new System.EventHandler(this.sintactico_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Codigo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 441);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Errores";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(504, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Tokens";
            // 
            // dgvErrores
            // 
            this.dgvErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrores.Location = new System.Drawing.Point(18, 457);
            this.dgvErrores.Name = "dgvErrores";
            this.dgvErrores.Size = new System.Drawing.Size(462, 159);
            this.dgvErrores.TabIndex = 25;
            // 
            // dgvLexico
            // 
            this.dgvLexico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLexico.Location = new System.Drawing.Point(499, 23);
            this.dgvLexico.Name = "dgvLexico";
            this.dgvLexico.Size = new System.Drawing.Size(313, 415);
            this.dgvLexico.TabIndex = 24;
            // 
            // ButtonSeparar
            // 
            this.ButtonSeparar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonSeparar.Location = new System.Drawing.Point(923, 621);
            this.ButtonSeparar.Name = "ButtonSeparar";
            this.ButtonSeparar.Size = new System.Drawing.Size(83, 27);
            this.ButtonSeparar.TabIndex = 23;
            this.ButtonSeparar.Text = "Lexico";
            this.ButtonSeparar.UseVisualStyleBackColor = true;
            this.ButtonSeparar.Click += new System.EventHandler(this.ButtonSeparar_Click);
            // 
            // txtCodigoFuente
            // 
            this.txtCodigoFuente.Location = new System.Drawing.Point(15, 23);
            this.txtCodigoFuente.Multiline = true;
            this.txtCodigoFuente.Name = "txtCodigoFuente";
            this.txtCodigoFuente.Size = new System.Drawing.Size(465, 415);
            this.txtCodigoFuente.TabIndex = 22;
            this.txtCodigoFuente.TextChanged += new System.EventHandler(this.txtCodigoFuente_TextChanged);
            // 
            // crearEnsambladorbtn
            // 
            this.crearEnsambladorbtn.Enabled = false;
            this.crearEnsambladorbtn.Location = new System.Drawing.Point(1101, 623);
            this.crearEnsambladorbtn.Name = "crearEnsambladorbtn";
            this.crearEnsambladorbtn.Size = new System.Drawing.Size(166, 23);
            this.crearEnsambladorbtn.TabIndex = 38;
            this.crearEnsambladorbtn.Text = "Crear Codigo Ensamblador";
            this.crearEnsambladorbtn.UseVisualStyleBackColor = true;
            this.crearEnsambladorbtn.Click += new System.EventHandler(this.CrearCodigoEnsamblador_Click);
            // 
            // mensaje_lbl
            // 
            this.mensaje_lbl.AutoSize = true;
            this.mensaje_lbl.ForeColor = System.Drawing.Color.Teal;
            this.mensaje_lbl.Location = new System.Drawing.Point(18, 623);
            this.mensaje_lbl.Name = "mensaje_lbl";
            this.mensaje_lbl.Size = new System.Drawing.Size(35, 13);
            this.mensaje_lbl.TabIndex = 39;
            this.mensaje_lbl.Text = "label4";
            this.mensaje_lbl.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1279, 660);
            this.Controls.Add(this.mensaje_lbl);
            this.Controls.Add(this.crearEnsambladorbtn);
            this.Controls.Add(this.Polishlbl);
            this.Controls.Add(this.Polishdgv);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.sintactico_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvErrores);
            this.Controls.Add(this.dgvLexico);
            this.Controls.Add(this.ButtonSeparar);
            this.Controls.Add(this.txtCodigoFuente);
            this.Name = "Form1";
            this.Text = "Compilador Barraza";
            ((System.ComponentModel.ISupportInitialize)(this.Polishdgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Polishlbl;
        private System.Windows.Forms.DataGridView Polishdgv;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.Button sintactico_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvErrores;
        private System.Windows.Forms.DataGridView dgvLexico;
        private System.Windows.Forms.Button ButtonSeparar;
        private System.Windows.Forms.TextBox txtCodigoFuente;
        private System.Windows.Forms.Button crearEnsambladorbtn;
        private System.Windows.Forms.Label mensaje_lbl;
    }
}

