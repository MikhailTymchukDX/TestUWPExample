Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Xml.Serialization
Imports Windows.Foundation
Imports Windows.Foundation.Collections
Imports Windows.Storage
Imports Windows.Storage.Streams
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Controls.Primitives
Imports Windows.UI.Xaml.Data
Imports Windows.UI.Xaml.Input
Imports Windows.UI.Xaml.Media
Imports Windows.UI.Xaml.Media.Imaging
Imports Windows.UI.Xaml.Navigation

' The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

Namespace TilesEx
	''' <summary>
	''' An empty page that can be used on its own or navigated to within a Frame.
	''' </summary>
	Public NotInheritable Partial Class MainPage
		Inherits Page

		Public Sub New()
			Me.InitializeComponent()

		End Sub
	End Class

	<XmlRoot("Employees")>
	Public Class Employees
		Inherits List(Of Employee)

	End Class
	<XmlRoot("Employee")>
	Public Class Employee
		Implements INotifyPropertyChanged

		Public Property Id() As Integer
		Public Property ParentId() As Integer
		Public Property FirstName() As String
		Public Property LastName() As String
		Public Property JobTitle() As String
		Public Property Phone() As String
		Public Property EmailAddress() As String
		Public Property AddressLine1() As String
		Public Property City() As String
		Public Property PostalCode() As String
		Public Property CountryRegionName() As String
		Public Property GroupName() As String
		Public Property BirthDate() As Date
		Public Property HireDate() As Date
		Public Property Gender() As String
		Public Property MaritalStatus() As String
		Public Property Title() As String
		Public Property ImageData() As Byte()
'INSTANT VB NOTE: The variable imageSource was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private imageSource_Renamed As BitmapImage
		<XmlIgnore>
		Public ReadOnly Property ImageSource() As BitmapImage
			Get
				If imageSource_Renamed Is Nothing AndAlso ImageData IsNot Nothing Then
					SetImageSource()
				End If
				Return imageSource_Renamed
			End Get
		End Property
		Private Async Sub SetImageSource()
			Dim stream As New InMemoryRandomAccessStream()
			Await stream.WriteAsync(ImageData.AsBuffer())
			stream.Seek(0)

			imageSource_Renamed = New BitmapImage()
			imageSource_Renamed.SetSource(stream)
			If propertyChanged IsNot Nothing Then
				propertyChanged(Me, New PropertyChangedEventArgs("ImageSource"))
			End If
		End Sub

		#Region "INotifyPropertyChanged Members"
		Private propertyChanged As PropertyChangedEventHandler

		Private Custom Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
			AddHandler(ByVal value As PropertyChangedEventHandler)
				propertyChanged = DirectCast(System.Delegate.Combine(propertyChanged, value), PropertyChangedEventHandler)
			End AddHandler
			RemoveHandler(ByVal value As PropertyChangedEventHandler)
				propertyChanged = DirectCast(System.Delegate.Remove(propertyChanged, value), PropertyChangedEventHandler)
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
				If propertyChanged IsNot Nothing Then
					For Each d As PropertyChangedEventHandler In propertyChanged.GetInvocationList()
						d.Invoke(sender, e)
					Next d
				End If
			End RaiseEvent
		End Event
		#End Region
	End Class
	Public Class ViewModel
		Inherits DevExpress.Mvvm.BindableBase

		Public ReadOnly Property Employees() As Employees
			Get
				Return DataStorage.Employees
			End Get
		End Property
	End Class
	Public NotInheritable Class DataStorage

		Private Sub New()
		End Sub

'INSTANT VB NOTE: The variable employees was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private Shared employees_Renamed As Employees
		Public Shared ReadOnly Property Employees() As Employees
			Get
				If employees_Renamed IsNot Nothing Then
					Return employees_Renamed
				End If
				Try
					Dim file As StorageFile = StorageFile.GetFileFromApplicationUriAsync(New Uri("ms-appx:///Data/Employees.xml")).AsTask().Result
					Dim stream As Stream = file.OpenStreamForReadAsync().Result
					Dim serializier As New XmlSerializer(GetType(Employees))
					employees_Renamed = TryCast(serializier.Deserialize(stream), Employees)
				Catch
					employees_Renamed = New Employees()
					employees_Renamed.Add(New Employee() With {.Id = 109, .FirstName = "Bruce", .LastName = "Cambell", .JobTitle = "Chief Executive Officer", .Phone = "(417) 166-3268", .EmailAddress = "Bruce_Cambell@example.com", .AddressLine1 = "4228 S National Ave", .City = "Tokyo", .PostalCode = "65809", .CountryRegionName = "Japan", .GroupName = "Executive General and Administration", .Gender = "M", .Title = "Mr."})
				End Try
				Return employees_Renamed
			End Get
		End Property
	End Class
End Namespace
