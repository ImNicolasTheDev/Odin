namespace Odin;

using CommunityToolkit.Maui.Alerts;
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
    private int loadingClickCount = 0;

    public MainPage()
    {
        InitializeComponent();
        loadingImage.IsVisible = true;
        gridContainingButtons.BindingContext = (Application.Current as App);
        updateInternetAvailability();
    }


    //======== Navigation ========//

    /// <summary>
    /// When the page is appearing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        updateInternetAvailability();
        gridContainingButtons.BackgroundColor = (Application.Current as App).color;
        stackLayoutContainingGrid.IsVisible = (Application.Current as App).displayFastAccessMenu;
    }

    /// <summary>
    /// Navigates to the page given in the url. (sets the Webview's source to the url)
    /// </summary>
    /// <param name="url">Url of the page</param>
    private void switchPage(string url)
    {
        updateInternetAvailability();
        if (isInternetAvailable)
        {
            loadingFunButton.IsVisible = false;
            loadingFunButton.IsEnabled = false;
            TheWebview.Source = HttpUtility.UrlDecode(url);
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

    /// <summary>
    /// Event fired when the schedule button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EDT_Clicked(object sender, EventArgs e)
    {
        switchPage((Application.Current as App).EdtLink.Text);
    }

    /// <summary>
    /// Event fired when the notes button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Notes_Clicked(object sender, EventArgs e)
    {
        switchPage((Application.Current as App).NotesLink.Text);
    }

    /// <summary>
    /// When the exit button is clicked : it disconnects the user and sets the willToQuit as true
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Quit_Clicked(object sender, EventArgs e)
    {
        TheWebview.Source = "https://odin.iut.uca.fr/etudiants/?p=logout";
        willToQuit = true;
        TheWebview.IsVisible = false;
        loadingImage.IsAnimationPlaying = false;
        loadingImage.IsVisible = true;
        loadingImage.IsAnimationPlaying = true;
    }

    /// <summary>
    /// Exits the app if the user wants to quit.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TheWebview_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (willToQuit)
        {
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
            Application.Current.Quit();
        }
    }



    //======= Connectivity =======//

    /// <summary>
    /// Check if internet is available : if not, displays an error message
    /// </summary>
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

    /// <summary>
    /// Gets the execution value in the html file. (needed to connect automatically)
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    private string getExecutionValue(HtmlDocument doc)
    {
        HtmlNode value = doc.DocumentNode.SelectSingleNode("//input[@name='execution']");
        return value?.GetAttributeValue("value", "").Trim();
    }

    /// <summary>
    /// Function that sets the source of the webview automatically using the saved username and password (+ the exec value)
    /// </summary>
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

    /// <summary>
    /// Big function that basically connects automatically if the user id and password are stored.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        loadingImage.IsAnimationPlaying = true;
        bool result = await (Application.Current as App).InitializeUserInfo();
        if (isInternetAvailable)
        {
            loadingFunButton.IsVisible = false;
            loadingFunButton.IsEnabled = false;
            if (result) // If there's already a username and a password saved in-app
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(string.Concat("https://ent.uca.fr/cas/login?service=", (App.Current as App).DefaultLink.Text));
                execValue = getExecutionValue(htmlDoc);

                // Sets the Webview's Source to the defaultLink + the GET request
                // with the username, the password and the execution value to connect automatically !
                loginAutomatically();
            }
            else // else sets the Webview's Source
                 // to the defaultLink without auto login
            {
                TheWebview.Source = HttpUtility.UrlDecode((Application.Current as App).DefaultLink.Text);
            }
            TheWebview.IsVisible = true;
            loadingImage.IsAnimationPlaying = false;
            loadingImage.IsVisible = false;
        }
    }



    //======== Easter egg ========//

    /// <summary>
    /// Function used above to display a toast alert (Android special, didn't test how this works on other platforms)
    /// </summary>
    /// <param name="message">Message displayed in the toast alert</param>
    private async void displayLoadingMessage(string message)
    {
        var toast = Toast.Make(message);
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        await toast.Show(cts.Token);
    }

    /// <summary>
    /// Little fun function, called everytime the loading screen is clicked :)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LoadingClicked(object sender, EventArgs e)
    {
        loadingClickCount++;
        switch (loadingClickCount)
        {
            case 4:
                displayLoadingMessage("Oh hi!");
                break;
            case 8:
                displayLoadingMessage("Please wait...");
                break;
            case 14:
                displayLoadingMessage("Still loading...");
                break;
            case 22:
                displayLoadingMessage("Hey, stop clicking me!");
                break;
            case 34:
                displayLoadingMessage("That tickles!");
                break;
            case 50:
                displayLoadingMessage("Don't you have something better to do?");
                break;
            case 85:
                displayLoadingMessage("And... Nothing happens.");
                break;
            case 125:
                displayLoadingMessage("BOOM! You unlocked: nothing.");
                break;
            case 200:
                displayLoadingMessage("What a waste of time...");
                break;
            case 275:
                displayLoadingMessage("How many clicks is that? Oh I know... TOO MUCH");
                break;
            case 375:
                displayLoadingMessage("Wow, impressive.");
                break;
            case 400:
                displayLoadingMessage("... - --- .--.");
                break;
            case 500:
                displayLoadingMessage("You're a legend. Well done.");
                loadingFunButton.IsVisible = false;
                loadingFunButton.IsEnabled = false;
                break;
        }
    }
}

