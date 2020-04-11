using QMS_System.Data.BLL;
using QMS_System.Data.Enum;
using QMS_System.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMS_System
{
    public partial class frmCOMSetting : Form
    {
        public frmCOMSetting()
        {
            InitializeComponent();
        }

        private void frmCOMSetting_Load(object sender, EventArgs e)
        {
            cbCOMKeypad.DisplayMember = "Name";
            cbCOMKeypad.ValueMember = "Code";
            cbCOMPrint1.DisplayMember = "Name";
            cbCOMPrint1.ValueMember = "Code";
            cbCOMPrint2.DisplayMember = "Name";
            cbCOMPrint2.ValueMember = "Code";
            loadCOM();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            BLLConfig.Instance.UpdateConfigValueFromInterface(QMSAppInfo.ConnectString, new Data.Q_Config() { Code = eConfigCode.ComportName, Value = cbCOMKeypad.Text });
            BLLConfig.Instance.UpdateConfigValueFromInterface(QMSAppInfo.ConnectString, new Data.Q_Config() { Code = eConfigCode.ComName_Printer, Value = cbCOMPrint1.Text });
            BLLConfig.Instance.UpdateConfigValueFromInterface(QMSAppInfo.ConnectString, new Data.Q_Config() { Code = eConfigCode.COM_Print, Value = cbCOMPrint2.Text });
            btCancel.PerformClick();
        }

        private void loadCOM()
        { 
            cbCOMKeypad.Items.Clear();
            cbCOMPrint1.Items.Clear();
            cbCOMPrint2.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                cbCOMKeypad.Items.Add(new ModelSelectItem() { Name = s, Code = s });
                cbCOMPrint1.Items.Add(new ModelSelectItem() { Name = s, Code = s });
                cbCOMPrint2.Items.Add(new ModelSelectItem() { Name = s, Code = s });
            }

            cbCOMKeypad.Text = BLLConfig.Instance.GetConfigByCode(QMSAppInfo.ConnectString, eConfigCode.ComportName);
            cbCOMPrint1.Text = BLLConfig.Instance.GetConfigByCode(QMSAppInfo.ConnectString, eConfigCode.ComName_Printer);
            cbCOMPrint2.Text = BLLConfig.Instance.GetConfigByCode(QMSAppInfo.ConnectString, eConfigCode.COM_Print);
        }

        private void btRefesh_Click(object sender, EventArgs e)
        {
            loadCOM();
        }
    }
}
