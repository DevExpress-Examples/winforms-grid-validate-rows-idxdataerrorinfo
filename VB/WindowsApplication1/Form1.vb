Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.DXErrorProvider
Imports DevExpress.XtraEditors.Controls

Namespace WindowsApplication1

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            ' Specify the type of records stored in the BindingSource.
            bindingSource1.DataSource = GetType(MyRecord)
            ' Create an empty MyRecord and add it to the BindingSource.
            For i As Integer = 0 To 5 - 1
                Dim rec As MyRecord = New MyRecord()
                rec.FirstName = ""
                rec.LastName = ""
                rec.BirthDay = Convert.ToDateTime("01/01/2020")
                bindingSource1.Add(rec)
            Next

            ' Bind the text editors to the MyRecord.FirstName and MyRecord.LastName properties.
            textEdit1.DataBindings.Add(New Binding("EditValue", bindingSource1, "FirstName", True))
            textEdit2.DataBindings.Add(New Binding("EditValue", bindingSource1, "LastName", True))
            gridControl1.DataSource = bindingSource1
            ' Bind the DXErrorProvider to the data source.
            dxErrorProvider1.DataSource = bindingSource1
            ' Specify the container of controls (textEdit1 and textEdit2) 
            ' which are monitored for errors.
            dxErrorProvider1.ContainerControl = Me
        End Sub

        Private Sub gridView1_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs)
            Dim info As ErrorInfo = New ErrorInfo()
            TryCast(e.Row, MyRecord).GetError(info)
            e.Valid = Equals(info.ErrorText, "")
        End Sub

        Private Sub gridView1_InvalidRowException(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs)
            e.ExceptionMode = ExceptionMode.NoAction
        End Sub
    End Class

    ' A custom record class.
    Public Class MyRecord
        Implements IDXDataErrorInfo

        Private firstNameField As String

        Private lastNameField As String

        Private birthDayField As Date

        Public Sub New()
        End Sub

        Public Property FirstName As String
            Get
                Return firstNameField
            End Get

            Set(ByVal value As String)
                firstNameField = value
            End Set
        End Property

        Public Property LastName As String
            Get
                Return lastNameField
            End Get

            Set(ByVal value As String)
                lastNameField = value
            End Set
        End Property

        Public Property BirthDay As Date
            Get
                Return birthDayField
            End Get

            Set(ByVal value As Date)
                birthDayField = value
            End Set
        End Property

#Region "IDXDataErrorInfo Members"
        ' Implements the IDXDataErrorInfo.GetPropertyError method.
        Public Sub GetPropertyError(ByVal propertyName As String, ByVal info As ErrorInfo) Implements IDXDataErrorInfo.GetPropertyError
            If Equals(propertyName, "FirstName") AndAlso String.IsNullOrEmpty(FirstName) Then
                info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName)
                info.ErrorType = ErrorType.Critical
            End If

            If Equals(propertyName, "LastName") AndAlso String.IsNullOrEmpty(LastName) Then
                info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName)
                info.ErrorType = ErrorType.Warning
            End If

            If Equals(propertyName, "BirthDay") AndAlso birthDayField > Date.Now Then
                info.ErrorText = String.Format("The BirthDay should be less than '{0}'", Date.Now.ToString())
                info.ErrorType = ErrorType.Critical
            End If
        End Sub

        ' IDXDataErrorInfo.GetError method
        Public Sub GetError(ByVal info As ErrorInfo) Implements IDXDataErrorInfo.GetError
            Dim propertyInfo As ErrorInfo = New ErrorInfo()
            GetPropertyError("FirstName", propertyInfo)
            If Equals(propertyInfo.ErrorText, "") Then GetPropertyError("BirthDay", propertyInfo)
            If Not Equals(propertyInfo.ErrorText, "") Then info.ErrorText = "This object has errors"
        End Sub
#End Region
    End Class
End Namespace
