﻿namespace Odin;

using HtmlAgilityPack;
using Microsoft.Maui.Controls;
using System.Web;

public partial class MainPage : ContentPage
{
    public string execValue;
    public string id;
    public string psswd;
    private bool willToQuit = false;
    public bool isInternetAvailable = false;
    public MainPage()
    {
        InitializeComponent();
        loadingImage.IsVisible = true;
        gridContainingButtons.BindingContext = (Application.Current as App);
        updateInternetAvailability();
    }

    private void updateInternetAvailability()
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            isInternetAvailable = true;
            noInternetLabel.IsVisible = false;
        }
        else
        {
            isInternetAvailable = false;
            noInternetLabel.IsVisible = true;
        }
    }

    private void TheWebview_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (willToQuit)
        {
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
            Application.Current.Quit();
        }
        //loadingImage.IsVisible = false;
        //TheWebview.IsVisible = true;
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
        updateInternetAvailability();
        if (isInternetAvailable)
        {
            TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).EdtLink.Text);
            TheWebview.IsVisible = true;
            loadingImage.IsAnimationPlaying = false;
            loadingImage.IsVisible = false;
        }
        else
        {
            TheWebview.IsVisible = false;
            loadingImage.IsVisible = true;
            loadingImage.IsAnimationPlaying = true;
        }
    }

    private void Notes_Clicked(object sender, EventArgs e)
    {
        updateInternetAvailability();
        if (isInternetAvailable)
        {
            TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).NotesLink.Text);
            TheWebview.IsVisible = true;
            loadingImage.IsAnimationPlaying = false;
            loadingImage.IsVisible = false;
        }
        else
        {
            TheWebview.IsVisible = false;
            loadingImage.IsVisible = true;
            loadingImage.IsAnimationPlaying = true;
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        loadingImage.IsAnimationPlaying = true;
        bool result = await (Application.Current as App).InitializeUserInfo();
        if (isInternetAvailable)
        {
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
            TheWebview.IsVisible = true;
            loadingImage.IsAnimationPlaying = false;
            loadingImage.IsVisible = false;
        }
    }

    private void Quit_Clicked(object sender, EventArgs e)
    {
        TheWebview.Source = "https://odin.iut.uca.fr/etudiants/?p=logout";
        willToQuit = true;
        TheWebview.IsVisible = false;
        loadingImage.IsAnimationPlaying = false;
        loadingImage.IsVisible = true;
        loadingImage.IsAnimationPlaying = true;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        updateInternetAvailability();
        gridContainingButtons.BackgroundColor = (Application.Current as App).color;
        stackLayoutContainingGrid.IsVisible = (Application.Current as App).displayFastAccessMenu;
    }
}

