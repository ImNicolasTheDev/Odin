namespace Odin;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        //BackgroundColor = (Application.Current as App).color; // Does not work
    }
}
