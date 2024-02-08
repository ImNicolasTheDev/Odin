﻿namespace Odin;

public partial class App : Application
{
    public Link DefaultLink = null;
    public Link EdtLink = new(1, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dedt%26nonclose%3D7");
    public Link NotesLink = new(2, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dnotes%26nonclose%3D7");
    public Link AbsencesLink = new(3, "https%3A%2F%2Fodin.iut.uca.fr%2Fetudiants%2F%3Fp%3Dabsences%26nonclose%3D7");
    public string Username;
    public string Password;
    public bool displayFastAccessMenu { get; set; }
    public Color color { get; set; }

    public App()
    {
        InitializeComponent();
        InitializeDefaultLink();
        InitializeColor();
        InitializeDisplayMenu();
        MainPage = new AppShell();
    }

    public void InitializeDisplayMenu()
    {
        displayFastAccessMenu = Preferences.Default.Get<bool>("displayMenu", true);
    }

    public void InitializeColor()
    {
        string colorInString = Preferences.Default.Get<string>("color", "cc99ff");
        color = Color.FromArgb(colorInString);
    }

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
