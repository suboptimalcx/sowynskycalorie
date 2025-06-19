using MySql.Data.MySqlClient;
using System.IO;
using sowynskycalorie.Stores;
using sowynskycalorie.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace sowynskycalorie;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly NavigationStore _navigationStore;
    // this solution is of course pretty bad,but for simplicitys sake im not making an extra config file to store your password to, i trust i wont accidentaly push my own credentials to github.......
    public static string ConnectionStr => "server=localhost;user id=INSERTID;password=INSERTPASSWORD;"; 
    public App()
    {
        _navigationStore = new NavigationStore();
        if (!DatabaseExists())
        {
            ImportDB();
        }
    }
    protected override void OnStartup(StartupEventArgs e)
    {

        _navigationStore.CurrentViewModel = new LoginMenuViewModel(_navigationStore);
        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_navigationStore)
        };
        MainWindow.Show();
        base.OnStartup(e);
    }
    private void ImportDB()
    {
        string scriptPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Database", "calorie_test.sql");
        if (!File.Exists(scriptPath))
        {
            Console.WriteLine("SQL file not found at: " + scriptPath);
            return;
        }
        string script = File.ReadAllText(scriptPath);

        try
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionStr))
            {
                conn.Open();
                MySqlScript mySqlScript = new MySqlScript(conn, script);
                mySqlScript.Execute();
                Console.WriteLine("Database imported successfully :).");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR IMPORTING DATABASE: " + ex.Message);
        }
    }
    private bool DatabaseExists()
    {
        string dbName = "sowynsky_calorie";
        using (MySqlConnection conn = new MySqlConnection(ConnectionStr))
        {
            conn.Open();
            string query = $"SHOW DATABASES LIKE '{dbName}'";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }
    }

}

