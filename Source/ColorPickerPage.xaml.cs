using ColorPicker.Maui;

namespace Odin;

public partial class ColorPickerPage : ContentPage
{
    // Temporary color
    private Color tempColor;

    public ColorPickerPage()
    {
        InitializeComponent();
        Color clr = (Application.Current as App).color; // Gets the saved color
        Frame1.Background = clr;
        Frame2.Background = clr;
        ColorPicker.PickedColor = clr;
    }

    /// <summary>
    /// Sets the color of the temp color and the 2 demos (light and dark) to the selected color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        if (Frame1 != null && Frame2 != null)
        {
            tempColor = e.NewPickedColorValue;
            Frame1.Background = e.NewPickedColorValue;
            Frame2.Background = e.NewPickedColorValue;
        }
    }

    /// <summary>
    /// Saves the temp color to the preferences and sets the current color to the temp color.
    /// Also goes back to the settings
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void saveColor_Clicked(object sender, EventArgs e)
    {
        (Application.Current as App).color = tempColor;
        string colorInHex = tempColor.ToArgbHex();
        Preferences.Default.Set("color", colorInHex);
        Navigation.PopAsync();
    }
}