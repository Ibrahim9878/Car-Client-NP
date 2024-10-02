using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace Task_Manager_Client_NP;


public partial class MainWindow : Window, INotifyPropertyChanged
{
    HttpClient Client = new();
    HttpRequestMessage message = new();
    private List<Car> cars;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public List<Car> Cars { get => cars; set { cars = value; NotifyPropertyChanged(); } }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Cars = new();
    }

    private async void GETButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Get;


        var response = await Client.GetAsync(message.RequestUri);
        var json = await response.Content.ReadAsStringAsync();
        Cars = JsonSerializer.Deserialize<List<Car>>(json);

    }

    private async void PUTButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Put;
        var SelectedItemId = (CarBox.SelectedItem as Car).Id;
        var EditName = ModelBox.Text;
        message.Content = new StringContent($"{SelectedItemId} {EditName}");

        var response = await Client.PutAsync(message.RequestUri, message.Content);
        var json = await response.Content.ReadAsStringAsync();
        MessageBox.Show(json);
    }

    private async void POSTButton_Click(object sender, RoutedEventArgs e)
    {
        var CarName = ModelBox.Text;
        message = new();
        Client = new();

        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Method = HttpMethod.Post;

        message.Content = new StringContent(CarName);

        var response = await Client.PostAsync(message.RequestUri, message.Content);
        var json = await response.Content.ReadAsStringAsync();
        MessageBox.Show(json);
        MessageBox.Show("Please Click the Get Button for Refresh The List Of Cars");
    }

    private async void DELETEButton_Click(object sender, RoutedEventArgs e)
    {
        message = new();
        Client = new();
        var SelectedItem = CarBox.SelectedItem as Car;
        var Id = SelectedItem.Id;

        message.Method = HttpMethod.Delete;
        message.RequestUri = new Uri(@"http://localhost:27001/");
        message.Content = new StringContent(Id.ToString());

        var response = await Client.SendAsync(message);
        var json = await response.Content.ReadAsStringAsync();

        MessageBox.Show(json);
        MessageBox.Show("Please Click the Get Button for Refresh The List Of Cars");
    }
}