namespace Odin;

public partial class Parameters : ContentPage
{
    public string id;
    public string psswd;

    public Parameters()
	{
		InitializeComponent();
        checkTheDefaultLink();
    }

    private void checkTheDefaultLink()
    {
        int defaultLinkId = (App.Current as App).DefaultLink.Id;
        switch (defaultLinkId)
        {
            case 1:
                btnEdt.IsChecked = true;
                break;
            case 2:
                btnNotes.IsChecked = true;
                break;
            case 3:
                btnAbsences.IsChecked = true;
                break;
        }
    }

    private async void saveUserInfo()
    {
        
        if (!string.IsNullOrWhiteSpace((Application.Current as App).Username) && !string.IsNullOrWhiteSpace((Application.Current as App).Password))
        {
            await SecureStorage.Default.SetAsync("username", (Application.Current as App).Username);
            await SecureStorage.Default.SetAsync("password", (Application.Current as App).Password);
        }
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(usernameEntry.Text) && !string.IsNullOrWhiteSpace(passwordEntry.Text))
        {
            errorLabelInfo.IsVisible = false;
            (App.Current as App).Username = usernameEntry.Text;
            (App.Current as App).Password = passwordEntry.Text;
            (sender as Button).IsEnabled = false;
            saveUserInfo();
            userInfoLayout.IsVisible = false;
            return;
        }
        errorLabelInfo.IsVisible = true;
    }

    private void ShowInfos_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace((Application.Current as App).Username) && !string.IsNullOrWhiteSpace((Application.Current as App).Password))
        {
            errorLabel.IsVisible = false;
            showUsernameLabel.Text = (Application.Current as App).Username;
            showPasswordLabel.Text = (Application.Current as App).Password;
        }
        else
        {
            errorLabel.IsVisible = true;
        }
    }

    private void AddOrChangeInfos_Clicked(object sender, EventArgs e)
    {
        usernameEntry.Text = (Application.Current as App).Username;
        passwordEntry.Text = (Application.Current as App).Password;
        userInfoLayout.IsVisible = true;
        saveInfoButton.IsEnabled = true;
    }

    private void updateDefaultLinkInPreferenceStorage(Link l)
    {
        Preferences.Default.Set("default_link", l.Text);
        (Application.Current as App).DefaultLink = l;
    }

    private void btnEdt_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            updateDefaultLinkInPreferenceStorage((Application.Current as App).EdtLink);
        }
    }

    private void btnNotes_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            updateDefaultLinkInPreferenceStorage((Application.Current as App).NotesLink);
        }
    }

    private void btnAbsences_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            updateDefaultLinkInPreferenceStorage((Application.Current as App).AbsencesLink);
        }
    }

    private void DeleteEverything_Clicked(object sender, EventArgs e)
    {
        SecureStorage.Default.RemoveAll();
        Preferences.Default.Clear();
        (Application.Current as App).InitializeDefaultLink();
        (Application.Current as App).InitializeUserInfo();
    }
}