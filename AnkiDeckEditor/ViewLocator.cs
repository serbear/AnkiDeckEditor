using AnkiDeckEditor.Libs.HanumanInstitute;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views;
using AnkiDeckEditor.Views.Screens;

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