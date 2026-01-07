namespace BlazorHybrid;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage())
        {
            Title = "Dynamics Service Endpoint Discovery Tool",
            Width = 800,
            Height = 600,
        };
    }
}
