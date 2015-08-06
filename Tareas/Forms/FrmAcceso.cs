using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SimpleDataMapper;
using SimpleDataMapper.Connection;
using SimpleDataMapper.Data;
using Tareas.ComunClass;
using Tareas.ControlData;
using Tareas.ClsTablas;
using System.Collections;

namespace Tareas
{
    public partial class FrmAcceso : Form
    {
        public FrmAcceso()
        {
            InitializeComponent();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Font tipoLetra = new Font("Arial", 14, FontStyle.Bold);
            Font sbTipoLetra = new Font("Arial", 14, FontStyle.Bold);
            e.Graphics.DrawString("Control Tareas", sbTipoLetra, Brushes.Silver, 38, 7);
            e.Graphics.DrawString("Control Tareas", tipoLetra, Brushes.Black, 39, 8);

            Font tipoLetra2 = new Font("Arial", 8, FontStyle.Bold );
            Font sbTipoLetra2 = new Font("Arial", 8, FontStyle.Bold);
            e.Graphics.DrawString("BETA", sbTipoLetra2, Brushes.Silver, 185, 7);
            e.Graphics.DrawString("BETA", tipoLetra2, Brushes.Black, 186, 8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAcceso_Load_1(object sender, EventArgs e)
        {
            /*Leer de registro los valores de la Base Datos.
             * Si los valores están vacios.
             * Se debe ir a la pestaña de conexión, para introducir los valores.
             * Si los valores están bien debe aparecer en la pestaña de usuario.
             * Leer del registro si el usuario tiene marcado la opción de recordar contraseña.
             * Si el usuario tiene marcado la opción accede directamente a la aplicación.
             * En caso contrarió se queda en la pestaña del usuario.
             */

//            this.txtUserId.Text= ClsRegedit.GetSetting("connection","userid");
  ///          this.txtPassword.Text=ClsRegedit.GetSetting("connection","password");
     //       this.txtHost.Text=ClsRegedit.GetSetting("connection","host");
       //     this.txtDataBase.Text=ClsRegedit.GetSetting("connection","database");
            /*
            if(String.IsNullOrEmpty(this.txtUserId.Text) | 
               String.IsNullOrEmpty(this.txtPassword.Text) |
               String.IsNullOrEmpty(this.txtHost.Text) |
               String.IsNullOrEmpty(this.txtDataBase.Text) )
                this.tbCtrlAccesos.SelectedIndex = 1;
            else
            {
                this.chckRemenber.Checked = bool.Parse(ClsRegedit.GetSetting("user", "remenber").ToString());
                this.txtUser.Text= ClsRegedit.GetSetting("user","user");
                this.txtPass.Text=ClsRegedit.GetSetting("user","pass");

                if (this.chckRemenber.Checked)
                {
                    if (String.IsNullOrEmpty(this.txtUser.Text) |
                        String.IsNullOrEmpty(this.txtPass.Text))
                        this.tbCtrlAccesos.SelectedIndex = 0;
                    else
                    {
                        //Comprobamos que los datos de la conexión son correctos.
                        if (this.Connection())
                        {
                            if (this.ConnectionUser())
                                this.RegeditSave();
                        }                                              
                    }   
                }
                else
                    this.tbCtrlAccesos.SelectedIndex = 0;
            }
            */
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnection_Click(object sender, EventArgs e)
        {
            this.Connection();
        }
    
        /// <summary>
        /// Control que inicia la sesión del usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSession_Click(object sender, EventArgs e)
        {
             //Valida los datos de acceso.
          /*  if (String.IsNullOrEmpty(this.txtHost.Text)) 
            {
                MessageBox.Show("Debe introducir el Servidor de Datos al que se debe conectar la Aplicación","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.tbCtrlAccesos.SelectedIndex = 1;
                this.txtHost.Focus();
                return;
            }
                //* Comprueba que se haya introducido la Base de Datos
            else if (String.IsNullOrEmpty(this.txtDataBase.Text))
            {
                MessageBox.Show("Debe introducir La Base de Datos a la que se debe conectar la Aplicación", "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.tbCtrlAccesos.SelectedIndex = 1;
                this.txtDataBase.Focus();
                return;
            }
                ////* Comprueba que se haya introducido el usuario de la BB.DD
            else if (String.IsNullOrEmpty(this.txtUserId.Text) )
            {    
            MessageBox.Show("Debe introducir el Usuario de la Base de Datos de la Aplicación", "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            this.tbCtrlAccesos.SelectedIndex = 1;
                this.txtUserId.Focus();
                return;
            }
                //* Comprueba que se haya introducido la Contraseña del usuario de la BB.DD
            else if (String.IsNullOrEmpty(this.txtPassword.Text) )
            {
                MessageBox.Show("Debe introducir la clave del usuario de la Base de Datos de la Aplicación", "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.tbCtrlAccesos.SelectedIndex = 1;
                this.txtPassword.Focus();
                return;
            }
                //*Comprueba que esté lleno el Usuario
            else if (String.IsNullOrEmpty(this.txtUser.Text) )
            {
                MessageBox.Show("Debe introducir el Código de Usuario de la Aplicación", "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.tbCtrlAccesos.SelectedIndex = 0;
                this.txtUser.Focus();
                return;
            }
                //*Comprueba que esté llena la Contraseña del Usuario
            else if (String.IsNullOrEmpty(this.txtPass.Text))
            {
                MessageBox.Show("Debe introducir la Contraseña del Usuario " + this.txtUser.Text, "Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.tbCtrlAccesos.SelectedIndex = 0;
                this.txtPass.Focus();
                return;
            }
            */
            if (this.Connection())
            {
                if (this.ConnectionUser())
                    this.RegeditSave();
            }

            
        }

        /// <summary>
        /// Compruba la conexión al servidor.
        /// </summary>
        /// <returns></returns>
        private bool Connection()
        {
            bool bExit = false;
            try
            {
                Program.oCon = new ClsConnection(this.txtHost.Text, this.txtDataBase.Text,
                                                          this.txtUserId.Text, this.txtPassword.Text);
            
                Program.oCon.DbOpen();
                Program.clsStruct = new ClsStructure(Program.oCon);
                bExit = true;
            }
            catch (Exception ex)
            {
                ClsTraccer.MessageError(ex, "No se ha podido establecer la conexión, revise los parámetros establecidos.", "btnConnection_Click");
            }
            finally
            {
                Program.oCon.DbClose();
            }
            return bExit;
        }

        /// <summary>
        /// Verifica la existencia del usuario.
        /// </summary>
        private bool ConnectionUser()
        {
            ClsDataQuery oQuery = new ClsDataQuery();
            try
            {
                oQuery.SQLQuery(Program.oCon, "SELECT * FROM usuarios " +
                                          "WHERE usuario = '" + this.txtUser.Text + "' " +
                                           "AND contrasena = '" + this.txtPass.Text + "'", true);
                //Comprobamos que existe el usuario.
                if (oQuery.Count > 0)
                {
                    //Entra en la aplicación
                    MessageBox.Show("Ha entrado.");
                    return true;
                }
                else
                {
                    ClsTraccer.MessageError("No existe el usuario.", "FrmAcceso_Load_1");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ClsTraccer.MessageError("No existe el usuario.", "FrmAcceso_Load_1");
                return false;
            }

        }
        
        /// <summary>
        /// Almacena la información del registro.
        /// </summary>
        private void RegeditSave()
        {
            ClsRegedit.SaveSetting("connection", "userid", this.txtUserId.Text);
            ClsRegedit.SaveSetting("connection", "password", this.txtPassword.Text);
            ClsRegedit.SaveSetting("connection", "host", this.txtHost.Text);
            ClsRegedit.SaveSetting("connection", "database", this.txtDataBase.Text);

            ClsRegedit.SaveSetting("user", "remenber", this.chckRemenber.Checked.ToString());
            ClsRegedit.SaveSetting("user", "user", this.txtUser.Text);
            ClsRegedit.SaveSetting("user", "pass", this.txtPass.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClsIdiomas oUser = new ClsIdiomas();
            oUser.DesIdioma = "español";
            oUser.Idiso = "es-es";
            Program.clsStruct.GetFields(oUser);
            Program.clsStruct.Update(oUser);
            oUser = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Object> listPrueba = new List<Object>();
            for (int i = 81000; i <= 90000; i++)
            {
                ClsPrueba p = new ClsPrueba();
                p.P1 = i;
                p.P2 = i;
                p.Pruebabool = false;
                listPrueba.Add(p);
            }

            Program.oExe = new ExecuteQuery(Program.oCon );
            Program.oExe.Insert();
            //Program.oExe.Insert();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.label1.Text = DateTime.Now.ToString();

            ClsPrueba p = new ClsPrueba();
            p.Pruebabool = true;
         /*   ClsPrueba p2 = new ClsPrueba();
            ClsPrueba p3 = new ClsPrueba();
            ClsPrueba p4 = new ClsPrueba();
            ClsPrueba p5 = new ClsPrueba();
                
                ClsPrueba listPrueba = new ClsPrueba();*/
                //ClsPrueba listPrueba = new ClsPrueba();
                ArrayList listPrueba1 = new ArrayList ();
             /*   listPrueba1.Add(p);
                listPrueba1.Add(p2);
                listPrueba1.Add(p3);
                listPrueba1.Add(p4);
                listPrueba1.Add(p5);*/
                /**/
                Program.oExe = new ExecuteQuery(Program.oCon);
            listPrueba1 = Program.oExe.SelectDataSet(p);
            //listPrueba1 = Program.oExe.Select(p);

            /*this.label1.Text = DateTime.Now.ToString();
           // this.dataGridView1.DataSource = null
            this.dataGridView1.DataSource = null;
            DataGridViewTextBoxColumn oTipoColumn = new DataGridViewTextBoxColumn();

            //oTipoColumn.ReadOnly = true;
            oTipoColumn.Name = "P1";
            oTipoColumn.DataPropertyName = "P1";
            
            oTipoColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            oTipoColumn.HeaderCell.Value = "P1";
            dataGridView1.Columns.Add(oTipoColumn);

            DataGridViewTextBoxColumn oTipoColumn2 = new DataGridViewTextBoxColumn();
            //oTipoColumn2.ReadOnly = true;
            oTipoColumn2.DataPropertyName = "P2";
            oTipoColumn2.Name = "P2";
            oTipoColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            oTipoColumn2.HeaderCell.Value = "P2";
            dataGridView1.Columns.Add(oTipoColumn2);


            DataGridViewTextBoxColumn oTipoColumn3 = new DataGridViewTextBoxColumn();
            //oTipoColumn3.ReadOnly = true;
            oTipoColumn3.DataPropertyName = "Pruebabool";
            oTipoColumn3.Name = "Pruebabool";
            oTipoColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            oTipoColumn3.HeaderCell.Value = "Pruebabool";
            dataGridView1.Columns.Add(oTipoColumn3);
            try
            {
                this.dataGridView1.DataSource = listPrueba1;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            */
            this.dataGridView1.DataSource = listPrueba1;
            this.dataGridView1.Refresh();

            this.label7.Text = DateTime.Now.ToString();
        }

    }
}  