using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

using Avalonia.Input;
using Avalonia.Interactivity;

namespace mytlp.Behaviors;

public class NumericInputBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        
        AssociatedObject?.AddHandler(InputElement.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
        
        AssociatedObject?.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
       
   
       
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject?.RemoveHandler(InputElement.TextInputEvent, OnTextInput);
        AssociatedObject?.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
    }

    private void OnTextInput(object? sender, TextInputEventArgs e)
    {
        // Blockiere alles, was keine Ziffer ist
        if (!int.TryParse(e.Text, out _))
        {
            e.Handled = true;
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        // Sondertasten erlauben
        if (e.Key is Key.Back or Key.Delete or Key.Left or Key.Right or Key.Tab)
            return;

        // Verhindert Copy/Paste mit Text
        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            return;

        // Nichts anderes als Zahlen zulassen
        if (!IsDigitKey(e.Key))
        {
            e.Handled = true;
        }
    }

    private bool IsDigitKey(Key key) =>
        key >= Key.D0 && key <= Key.D9 ||
        key >= Key.NumPad0 && key <= Key.NumPad9;
}


