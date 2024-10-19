using AnkiDeckEditor.Libs.HanumanInstitute;
using AnkiDeckEditor.ViewModels;
using AnkiDeckEditor.Views;
using AnkiDeckEditor.Views.Screens;
using EstonianScreenViewModel = AnkiDeckEditor.ViewModels.EstonianScreen.EstonianScreenViewModel;

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