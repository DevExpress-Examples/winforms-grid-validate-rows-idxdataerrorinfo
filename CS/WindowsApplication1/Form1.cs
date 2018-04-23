using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Controls;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Specify the type of records stored in the BindingSource.
            bindingSource1.DataSource = typeof(MyRecord);
            // Create an empty MyRecord and add it to the BindingSource.

            for (int i = 0; i < 5; i++)
            {
                MyRecord rec = new MyRecord();
                rec.FirstName = "";
                rec.LastName = "";
                rec.BirthDay = Convert.ToDateTime("01/01/2020");
                bindingSource1.Add(rec);
            }

            // Bind the text editors to the MyRecord.FirstName and MyRecord.LastName properties.
            textEdit1.DataBindings.Add(new Binding("EditValue", bindingSource1,
              "FirstName", true));
            textEdit2.DataBindings.Add(new Binding("EditValue", bindingSource1,
               "LastName", true));
            gridControl1.DataSource = bindingSource1;
            // Bind the DXErrorProvider to the data source.
            dxErrorProvider1.DataSource = bindingSource1;
            // Specify the container of controls (textEdit1 and textEdit2) 
            // which are monitored for errors.
            dxErrorProvider1.ContainerControl = this;

        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            ErrorInfo info = new ErrorInfo();
            (e.Row as MyRecord).GetError(info);
            e.Valid = info.ErrorText == "";
        }

        private void gridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
    }

    // A custom record class.
    public class MyRecord : IDXDataErrorInfo
    {
        private string firstName;
        private string lastName;
        private DateTime birthDay;
        public MyRecord() { }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public DateTime BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        #region IDXDataErrorInfo Members
        // Implements the IDXDataErrorInfo.GetPropertyError method.
        public void GetPropertyError(string propertyName, ErrorInfo info)
        {
            if (propertyName == "FirstName" && string.IsNullOrEmpty(FirstName))
            {
                info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName);
                info.ErrorType = ErrorType.Critical;
            }
            if (propertyName == "LastName" && string.IsNullOrEmpty(LastName))
            {
                info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName);
                info.ErrorType = ErrorType.Warning;
            }
            if (propertyName == "BirthDay" && birthDay > DateTime.Now)
            {
                info.ErrorText = String.Format("The BirthDay should be less than '{0}'", DateTime.Now.ToString());
                info.ErrorType = ErrorType.Critical;
            }
        }

        // IDXDataErrorInfo.GetError method
        public void GetError(ErrorInfo info)
        {
            ErrorInfo propertyInfo = new ErrorInfo();
            GetPropertyError("FirstName", propertyInfo);
            if (propertyInfo.ErrorText == "")
                GetPropertyError("BirthDay", propertyInfo);
            if (propertyInfo.ErrorText != "")
                info.ErrorText = "This object has errors";
        }
        #endregion
    }
}

