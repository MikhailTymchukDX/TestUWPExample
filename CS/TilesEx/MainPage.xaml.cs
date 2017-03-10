using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TilesEx {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();

        }
    }

    [XmlRoot("Employees")]
    public class Employees : List<Employee> {
    }
    [XmlRoot("Employee")]
    public class Employee : INotifyPropertyChanged {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryRegionName { get; set; }
        public string GroupName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Title { get; set; }
        public byte[] ImageData { get; set; }
        BitmapImage imageSource;
        [XmlIgnore]
        public BitmapImage ImageSource
        {
            get
            {
                if (imageSource == null && ImageData != null) {
                    SetImageSource();
                }
                return imageSource;
            }
        }
        async void SetImageSource() {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(ImageData.AsBuffer());
            stream.Seek(0);

            imageSource = new BitmapImage();
            imageSource.SetSource(stream);
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs("ImageSource"));
        }

        #region INotifyPropertyChanged Members
        PropertyChangedEventHandler propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
        #endregion
    }
    public class ViewModel : DevExpress.Mvvm.BindableBase {
        public Employees Employees { get { return DataStorage.Employees; } }
    }
    public static class DataStorage {
        static Employees employees;
        public static Employees Employees
        {
            get
            {
                if (employees != null)
                    return employees;
                try {
                    StorageFile file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/Employees.xml")).AsTask().Result;
                    Stream stream = file.OpenStreamForReadAsync().Result;
                    XmlSerializer serializier = new XmlSerializer(typeof(Employees));
                    employees = serializier.Deserialize(stream) as Employees;
                }
                catch {
                    employees = new Employees();
                    employees.Add(new Employee() {
                        Id = 109,
                        FirstName = "Bruce",
                        LastName = "Cambell",
                        JobTitle = "Chief Executive Officer",
                        Phone = "(417) 166-3268",
                        EmailAddress = "Bruce_Cambell@example.com",
                        AddressLine1 = "4228 S National Ave",
                        City = "Tokyo",
                        PostalCode = "65809",
                        CountryRegionName = "Japan",
                        GroupName = "Executive General and Administration",
                        Gender = "M",
                        Title = "Mr."
                    });
                }
                return employees;
            }
        }
    }
}
