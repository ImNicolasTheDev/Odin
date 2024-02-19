namespace Odin;
using CommunityToolkit.Maui.Alerts;

public partial class Credits : ContentPage
{
	public Credits()
	{
		InitializeComponent();
    }

    /// <summary>
    /// Opens the loading animation author link to the default web browser.
    /// If not available sends a toast alert (Android)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://pin.it/fapncLr2y");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception)
        {
            // No browser may be installed.
            displayToast("Cannot open the browser");
        }
    }

    /// <summary>
    /// Opens the developer github's profile in the default web browser.
    /// If not available sends a toast alert (Android)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://github.com/ImNicolasTheDev/");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception)
        {
            displayToast("Cannot open the browser");
        }
    }

    /// <summary>
    /// Little secret... Displays a toast alert the first time the user opens the app.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        if(!(Application.Current as App).curious)
        {
            displayToast("Oh, you're curious ?");
            (Application.Current as App).curious = true;
        }
    }

    private async void displayToast(string message)
    {
        var toast = Toast.Make(message);
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await toast.Show(cts.Token);
    }
}