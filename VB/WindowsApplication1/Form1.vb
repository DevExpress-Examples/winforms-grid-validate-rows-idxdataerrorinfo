Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.DXErrorProvider
Imports DevExpress.XtraEditors.Controls

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()

		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' Specify the type of records stored in the BindingSource.
			bindingSource1.DataSource = GetType(MyRecord)
			' Create an empty MyRecord and add it to the BindingSource.

			For i As Integer = 0 To 4
				Dim rec As New MyRecord()
				rec.FirstName = ""
				rec.LastName = ""
				rec.BirthDay = Convert.ToDateTime("01/01/2020")
				bindingSource1.Add(rec)
			Next i

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

		Private Sub gridView1_ValidateRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs) Handles gridView1.ValidateRow
            Dim info As New ErrorInfo()
            TryCast(e.Row, MyRecord).GetError(info)
			e.Valid = info.ErrorText = ""
		End Sub

		Private Sub gridView1_InvalidRowException(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs) Handles gridView1.InvalidRowException
			e.ExceptionMode = ExceptionMode.NoAction
		End Sub
	End Class

	' A custom record class.
	Public Class MyRecord
		Implements IDXDataErrorInfo
		Private firstName_Renamed As String
		Private lastName_Renamed As String
		Private birthDay_Renamed As DateTime
		Public Sub New()
		End Sub
		Public Property FirstName() As String
			Get
				Return firstName_Renamed
			End Get
			Set(ByVal value As String)
				firstName_Renamed = value
			End Set
		End Property
		Public Property LastName() As String
			Get
				Return lastName_Renamed
			End Get
			Set(ByVal value As String)
				lastName_Renamed = value
			End Set
		End Property
		Public Property BirthDay() As DateTime
			Get
				Return birthDay_Renamed
			End Get
			Set(ByVal value As DateTime)
				birthDay_Renamed = value
			End Set
		End Property

		#Region "IDXDataErrorInfo Members"
		' Implements the IDXDataErrorInfo.GetPropertyError method.
		Public Sub GetPropertyError(ByVal propertyName As String, ByVal info As ErrorInfo) Implements IDXDataErrorInfo.GetPropertyError
			If propertyName = "FirstName" AndAlso String.IsNullOrEmpty(FirstName) Then
				info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName)
				info.ErrorType = ErrorType.Critical
			End If
			If propertyName = "LastName" AndAlso String.IsNullOrEmpty(LastName) Then
				info.ErrorText = String.Format("The '{0}' field cannot be empty", propertyName)
				info.ErrorType = ErrorType.Warning
			End If
			If propertyName = "BirthDay" AndAlso birthDay_Renamed > DateTime.Now Then
				info.ErrorText = String.Format("The BirthDay should be less than '{0}'", DateTime.Now.ToString())
				info.ErrorType = ErrorType.Critical
			End If
		End Sub

		' IDXDataErrorInfo.GetError method
		Public Sub GetError(ByVal info As ErrorInfo) Implements IDXDataErrorInfo.GetError
			Dim propertyInfo As New ErrorInfo()
			GetPropertyError("FirstName", propertyInfo)
			If propertyInfo.ErrorText = "" Then
				GetPropertyError("BirthDay", propertyInfo)
			End If
			If propertyInfo.ErrorText <> "" Then
				info.ErrorText = "This object has errors"
			End If
		End Sub
		#End Region
	End Class
End Namespace

