using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Linq;


namespace mytlp.CustomControls;


public class NumericTextBox : TextBox
{
    public NumericTextBox()
    {
        AddHandler(InputElement.TextInputEvent,
            (EventHandler<TextInputEventArgs>)OnPreviewTextInput,
            RoutingStrategies.Tunnel);
    }

    private void OnPreviewTextInput(object? sender, TextInputEventArgs e)
    {
        if (!e.Text.All(char.IsDigit))
            e.Handled = true;
    }
}

