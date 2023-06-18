namespace Odin;

using HtmlAgilityPack;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Net;
using System.Web;

public partial class MainPage : ContentPage
{
    public string execValue;
    public string id;
    public string psswd;
    public MainPage()
    {
        InitializeComponent();
        loadingImage.IsVisible = true;
    }




    private void TheWebview_Navigated(object sender, WebNavigatedEventArgs e)
    {
        loadingImage.IsVisible = false;
        TheWebview.IsVisible = true;
    }

    private string getExecutionValue(HtmlDocument doc)
    {
        HtmlNode value = doc.DocumentNode.SelectSingleNode("//input[@name='execution']");
        return value?.GetAttributeValue("value", "").Trim();
    }

    private void loginAutomatically()
    {
        TheWebview.Source = string.Concat(
            "https://ent.uca.fr/cas/login?service=",
            (App.Current as App).DefaultLink.Text,
            "&username=",
            (App.Current as App).Username,
            "&password=",
            (App.Current as App).Password,
            "&execution=",
            execValue,
            "&_eventId=submit&geolocation=");
    }

    private void EDT_Clicked(object sender, EventArgs e)
    {
        TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).EdtLink.Text);
    }

    private void Notes_Clicked(object sender, EventArgs e)
    {
        TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).NotesLink.Text);
    }

    private void Param_Clicked(object sender, EventArgs e)
    {
        // Idk if I'll keep this button or not... Yet, it exists, but does nothing.
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await Task.Delay(100);
        loadingImage.IsAnimationPlaying = true;

        bool result = await (Application.Current as App).InitializeUserInfo();

        if (result) // If there's already a username and a password saved in-app
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(string.Concat("https://ent.uca.fr/cas/login?service=", (App.Current as App).DefaultLink.Text));
            execValue = getExecutionValue(htmlDoc);

            // Set the Webview's Source to the defaultLink + the GET request
            // with the username, the password and the execution value to connect automatically !
            loginAutomatically();
        }
        else // Else set the Webview's Source
             // to the defaultLink without auto login
        {
            TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).DefaultLink.Text);
        }
    }

    private void Quit_Clicked(object sender, EventArgs e)
    {
        TheWebview.Source = "https://odin.iut.uca.fr/etudiants/?p=logout";
        Application.Current?.CloseWindow(Application.Current.MainPage.Window);
    }
}

