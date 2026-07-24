using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace LichessAnalytics.App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnConvertClick(object? sender, RoutedEventArgs e)
    {
        var pgnInput = this.FindControl<TextBox>("PgnInput");
        var fenOutput = this.FindControl<TextBox>("FenOutput");

        if (pgnInput is null || fenOutput is null)
            return;

        var pgn = pgnInput.Text?.Trim();
        if (string.IsNullOrEmpty(pgn))
        {
            fenOutput.Text = "Please enter a PGN.";
            return;
        }

        try
        {
            List<string> fens = PgnToFenConverter.ConvertPgnToFen(pgn);
            fenOutput.Text = string.Join(Environment.NewLine, fens);
        }
        catch (Exception ex)
        {
            fenOutput.Text = $"Error: {ex.Message}";
        }
    }
}
