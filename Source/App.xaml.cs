namespace Odin;

public partial class App : Application
{
    // Open the app using one of these links
    public Link DefaultLink = null;
    public Link EdtLink = new(1, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dedt%26nonclose%3D7");
    public Link NotesLink = new(2, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dnotes%26nonclose%3D7");
    public Link AbsencesLink = new(3, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dabsences%26nonclose%3D7");
    
    // Little secret...
    public bool curious = false;

    // Used to toggle on/off the fast access menu
    public bool displayFastAccessMenu { get; set; }

    // Color of the fast access menu
    public Color color { get; set; }

    public string Username {
        get
        {
            return this.username;
        }
        set
        {
            this.username = value;
        }
    }
    public string Password
    {
        get
        {
            return this.password;
        }
        set
        {
            this.password = value;
        }
    }
    private string username;
    private string password;


    public App()
    {
        InitializeComponent();
        InitializeDefaultLink();
        InitializeColor();
        InitializeDisplayMenu();
        MainPage = new AppShell();
    }

    /// <summary>
    /// Set the displayFastAccessMenu to the saved preference for the value "displayMenu", defaults to true
    /// </summary>
    public void InitializeDisplayMenu()
    {
        displayFastAccessMenu = Preferences.Default.Get<bool>("displayMenu", true);
    }

    /// <summary>
    /// Set the color of the fast access menu to the saved preference for the value "color", defaults to light purple
    /// </summary>
    public void InitializeColor()
    {
        string colorInString = Preferences.Default.Get<string>("color", "cc99ff");
        color = Color.FromArgb(colorInString);
    }

    /// <summary>
    /// Set the default link to the saved preference for the value "default_link", defaults to EdtLink
    /// </summary>
    public void InitializeDefaultLink()
    {
        if (!Preferences.Default.ContainsKey("default_link"))
        {
            DefaultLink = EdtLink;
            Preferences.Default.Set("default_link", EdtLink.Text);
        }
        else
        {
            string link = Preferences.Default.Get("default_link", EdtLink.Text);
            switch (link)
            {
                case "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dedt%26nonclose%3D7":
                    DefaultLink = EdtLink;
                    break;
                case "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dnotes%26nonclose%3D7":
                    DefaultLink = NotesLink;
                    break;
                case "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dabsences%26nonclose%3D7":
                    DefaultLink = AbsencesLink;
                    break;
            }
        }
    }

    /// <summary>
    /// Gets the username and password from the SecureStorage, and sets to null if nothing found
    /// </summary>
    /// <returns>Returns true if found, else false</returns>
    public async Task<bool> InitializeUserInfo()
    {
        string username = await SecureStorage.Default.GetAsync("username");
        string password = await SecureStorage.Default.GetAsync("password");
        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
        {
            Username = username;
            Password = password;
            return true;
        }
        Username = null;
        Password = null;
        return false;
    }
}
