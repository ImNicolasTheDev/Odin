namespace Odin;

public partial class Parameters : ContentPage
{
    public string id;
    public string psswd;

    public Parameters()
    {
        InitializeComponent();
        checkTheDefaultLink();
        displayMenuCheckbox.IsChecked = (Application.Current as App).displayFastAccessMenu;
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
            showUsernameLabel.IsVisible = true;
            showPasswordLabel.IsVisible = true;
            showUsernameLabel.Text = string.Concat("Username : ", (Application.Current as App).Username);
            showPasswordLabel.Text = string.Concat("Password : ", (Application.Current as App).Password);
        }
        else
        {
            showUsernameLabel.IsVisible = false;
            showPasswordLabel.IsVisible = false;
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

    private void updateDisplayMenu(bool value)
    {
        Preferences.Default.Set("displayMenu", value);
        (Application.Current as App).displayFastAccessMenu = value;
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

    private async void DeleteEverything_Clicked(object sender, EventArgs e)
    {
        SecureStorage.RemoveAll();
        Preferences.Clear();
        SecureStorage.Default.RemoveAll();
        Preferences.Default.Clear();
        (Application.Current as App).InitializeDefaultLink();
        await (Application.Current as App).InitializeUserInfo();
        (Application.Current as App).InitializeColor();
    }

    private void ColorPickerButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ColorPickerPage());
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        errorLabel.IsVisible = false;
        showUsernameLabel.IsVisible = false;
        showPasswordLabel.IsVisible = false;
        showUsernameLabel.Text = string.Empty;
        showPasswordLabel.Text = string.Empty;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        updateDisplayMenu(displayMenuCheckbox.IsChecked);
    }
}