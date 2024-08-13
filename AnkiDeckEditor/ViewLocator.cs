using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views;
using AnkiDeckEditor.Views.Screens;
using HanumanInstitute.MvvmDialogs.Avalonia;

namespace AnkiDeckEditor;

public class ViewLocator : StrongViewLocator
{
    public ViewLocator()
    {
        Register<MainWindowViewModel, MainWindow>();
        Register<EstonianScreenViewModel, EstonianScreen>();
        Register<EnglishScreenViewModel, EnglishScreen>();
    }
}
// public class ViewLocator : IDataTemplate
// {
// public Control? Build(object? data)
// {
// if (data is null)
// return null;

// var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
// var type = Type.GetType(name);

// if (type == null) return new TextBlock { Text = "Not Found: " + name };

// var control = (Control)Activator.CreateInstance(type)!;
// control.DataContext = data;
// return control;
// }

// public bool Match(object? data)
// {
// return data is ViewModelBase;
// }
// }