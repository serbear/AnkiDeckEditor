using System.Collections.ObjectModel;
using AnkiDeckEditor.Libs;

namespace AnkiDeckEditor.Services.FieldsCopy;

// Контекст определяет интерфейс, представляющий интерес для клиентов.
public class Context
{
    // Контекст хранит ссылку на один из объектов Стратегии. Контекст не
    // знает конкретного класса стратегии. Он должен работать со всеми
    // стратегиями через интерфейс Стратегии.
    private ICopyStrategy _strategy;

    public Context()
    {
    }

    // Обычно Контекст принимает стратегию через конструктор, а также
    // предоставляет сеттер для её изменения во время выполнения.
    public Context(ICopyStrategy strategy)
    {
        _strategy = strategy;
    }

    // Обычно Контекст позволяет заменить объект Стратегии во время
    // выполнения.
    public void SetStrategy(ICopyStrategy strategy)
    {
        _strategy = strategy;
    }

    // Вместо того чтобы самостоятельно реализовывать множественные версии
    // алгоритма, Контекст делегирует некоторую работу объекту Стратегии.
    public void DoCopyLogic<T>(ObservableCollection<T> translationData)
    {
        var textToCopy = _strategy.DoCopy(translationData);
        Clipboard.Set(textToCopy);
    }

    public void DoCopyLogic(string? data)
    {
        Clipboard.Set(data);
    }
}