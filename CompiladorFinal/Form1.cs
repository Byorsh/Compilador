using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladorFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<Error> listaErroresLexico;
        public List<Error> listaErroresSintactico;
        public List<Variable> listaVariables;
        public List<Polish> listaPolish;
        Lexico lexico = new Lexico();

        private void ButtonSeparar_Click(object sender, EventArgs e)
        {

            lexico.EjecutarLexico(txtCodigoFuente.Text);

            listaErroresLexico = lexico.listaError;
            dgvErrores.DataSource = null;
            dgvErrores.DataSource = listaErroresLexico;

            var lista = new BindingList<Token>(lexico.listaToken);
            dgvLexico.DataSource = null;
            dgvLexico.DataSource = lista;

            dgvVariables.DataSource = null;

            if (listaErroresLexico.Capacity == 0)
            {
                sintactico_btn.Enabled = true;
                mensaje_lbl.Visible = true;
                mensaje_lbl.Text = "Analisis lexito terminado";
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "docx";
            saveFileDialog1.Filter = "Microsoft Word Document (*.docx)|*.docx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtCodigoFuente.Text = saveFileDialog1.FileName;
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "Text files(*.txt)|*txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog1.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    txtCodigoFuente.Text = reader.ReadToEnd();
                    reader.Dispose();
                }
            }
        }

        private void sintactico_btn_Click(object sender, EventArgs e)
        {
            lexico.Sintaxis();
            listaErroresSintactico = lexico.listaError;
            dgvErrores.DataSource = null;
            dgvErrores.DataSource = listaErroresSintactico;

            listaVariables = lexico.listaVariables;
            dgvVariables.DataSource = null;
            dgvVariables.DataSource = listaVariables;

            listaPolish = lexico.listaPolish;
            Polishdgv.DataSource = null;
            Polishdgv.DataSource = listaPolish;

            if (listaErroresSintactico.Count == 0)
            {
                crearEnsambladorbtn.Enabled = true;
                mensaje_lbl.Visible = true;
                mensaje_lbl.Text = "Analisis sintactico y semantico terminado";

            }
        }

        private void txtCodigoFuente_TextChanged(object sender, EventArgs e)
        {
            sintactico_btn.Enabled = false;
            crearEnsambladorbtn.Enabled = false;
        }

        private void CrearCodigoEnsamblador_Click(object sender, EventArgs e)
        {
            lexico.CrearEnsamblador();
            mensaje_lbl.Visible = true;
            mensaje_lbl.Text = "Codigo ensamblador generado";
        }
    }
}
