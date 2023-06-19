using ColorPicker.Maui;

namespace Odin;

public partial class ColorPickerPage : ContentPage
{
    private Color tempColor;

    public ColorPickerPage()
    {
        InitializeComponent();
        Color clr = (Application.Current as App).color;
        Frame1.Background = clr;
        Frame2.Background = clr;
        ColorPicker.PickedColor = clr;
    }

    private void ColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        if (Frame1 != null && Frame2 != null)
        {
            tempColor = e.NewPickedColorValue;
            Frame1.Background = e.NewPickedColorValue;
            Frame2.Background = e.NewPickedColorValue;
        }
    }

    private void saveColor_Clicked(object sender, EventArgs e)
    {
        (Application.Current as App).color = tempColor;
        string colorInHex = tempColor.ToArgbHex();
        Preferences.Default.Set("color", colorInHex);
        Navigation.PopAsync();
    }
}