namespace NumberAnimation;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void ChangeValue_Clicked(object sender, EventArgs e) 
    {
        Random rnd = new Random();
        int num = rnd.Next(1_000_000);
        lbValue.Value = num;
    }
}

