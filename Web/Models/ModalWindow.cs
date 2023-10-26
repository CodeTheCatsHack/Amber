namespace Web.Models;

public class ModalWindow
{
    public ModalWindow(string title, string message, bool showButton = false,
        List<ButtonModalWindow>? listButtonModalWindows = null)
    {
        Title = title;
        Message = message;
        ShowButton = showButton;

        if (showButton)
            ButtonModalWindows.AddRange(listButtonModalWindows ??
                                        FillListButtonModalWindows(DefaultButtonTemplate.AuthYesNo));
    }

    public ModalWindow(string title, string message, bool showButton = false,
        DefaultButtonTemplate defaultButton = DefaultButtonTemplate.AuthYesNo)
    {
        Title = title;
        Message = message;
        ShowButton = showButton;

        if (showButton)
            ButtonModalWindows.AddRange(FillListButtonModalWindows(defaultButton));
    }

    public string Title { get; set; }
    public string Message { get; set; }
    public List<ButtonModalWindow> ButtonModalWindows { get; set; } = new();

    public bool Show { get; set; } = true;
    public bool ShowButton { get; set; }

    private List<ButtonModalWindow> FillListButtonModalWindows(DefaultButtonTemplate defaultButton)
    {
        return defaultButton switch
        {
            DefaultButtonTemplate.AuthYesNo => new List<ButtonModalWindow>
            {
                new("Да", "btn-success", "button-logout"),
                new("Нет", "btn-danger", "button-no-logout")
            },
            _ => throw new ArgumentOutOfRangeException(nameof(defaultButton), defaultButton, null)
        };
    }
}

public enum DefaultButtonTemplate
{
    AuthYesNo
}

public class ButtonModalWindow
{
    public ButtonModalWindow(string title, string color, string can)
    {
        Title = title;
        Color = color;
        Can = can;
    }

    public string Title { get; set; }
    public string Color { get; set; }
    public string Can { get; set; }
}