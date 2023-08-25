using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Unipro_MallManagement
{
    public partial class UploadToMall : Form
    {
        string sCon;
        string sHBDMALL, sSFTP, sUsername, sPassword,  sFilePath , sSourceFilePat;
        int sPortNo;
        DateTime fromDate, toDate;
        public UploadToMall()
        {
            InitializeComponent();
            sCon = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            sHBDMALL = ConfigurationManager.AppSettings["HBD_MALL"];
            sSFTP = ConfigurationManager.AppSettings["SFTPID"];
            sUsername = ConfigurationManager.AppSettings["Username"];
            sPassword = ConfigurationManager.AppSettings["Password"];
            sPortNo = Convert.ToInt32(ConfigurationManager.AppSettings["PortNo"]);
            sFilePath = ConfigurationManager.AppSettings["SftpFilePath"];
            sSourceFilePat = ConfigurationManager.AppSettings["SourceFilePath"];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void scriptExecuteFile(string connectionString, string fileName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string data = System.IO.File.ReadAllText(fileName);
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = data;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void batchFileExecute()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            try
            {
                string targetDir = string.Format(Application.StartupPath + "\\MALL_SCRIPT\\HBD_SCRIPT.bat");//this is where mybatch.bat lies
                proc.StartInfo.FileName = targetDir;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                string errorMessage = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                string outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
        }
        
        private void UploadtoSftp()
        {
            try
            {
                using (SftpClient client = new SftpClient(sSFTP, sPortNo, sUsername, sPassword))
                {
                    client.Connect();
                    client.ChangeDirectory(sFilePath);
                    string[] files = Directory.GetFiles(Application.StartupPath + sSourceFilePat);
                    foreach (string file in files)
                    {
                        using (FileStream fs = new FileStream(file, FileMode.Open))
                        {
                            client.BufferSize = 4 * 1024;
                            client.UploadFile(fs, Path.GetFileName(file));
                        }
                    }                    
                }
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message.ToString());
                Application.Exit();
            }            
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                fromDate = dateTimePicker1.Value.Date;
                toDate = dateTimePicker2.Value.Date;
                for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
                {
                    try
                    {
                        SqlConnection sqlCon = null;
                        using (sqlCon = new SqlConnection(sCon))
                        {
                            sqlCon.Open();
                            SqlCommand sql_cmnd = new SqlCommand("Sp_ManagementFilebyDate", sqlCon);
                            sql_cmnd.CommandType = CommandType.StoredProcedure;
                            sql_cmnd.Parameters.AddWithValue("@curDate", SqlDbType.NVarChar).Value = day;
                            sql_cmnd.ExecuteNonQuery();
                            sqlCon.Close();
                        }
                        UploadtoSftp();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                        if (ex.Message.ToString() == "Could not find stored procedure 'Sp_ManagementFilebyDate'.")
                        {
                            string sFileName;
                            sFileName = Application.StartupPath + "\\MALL_SCRIPT\\MAIN_SP.sql";
                            scriptExecuteFile(sCon, sFileName);
                            sFileName = Application.StartupPath + "\\MALL_SCRIPT\\INTERFACE_SP.sql";
                            scriptExecuteFile(sCon, sFileName);
                            sFileName = Application.StartupPath + "\\MALL_SCRIPT\\HBD_SCRIPT.sql";
                            scriptExecuteFile(sCon, sFileName);
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show("File uploaded to mall!...");
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
        }
    }
}
