using System;
using System.Collections.Generic;
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
    public void DoCopyLogic<T>(T data)
    {
        var textToCopy = data switch
        {
            string => data.ToString()!,
            _ when IsObservableCollection(data!) => DoCopyCollectionGeneric((dynamic)data!),
            List<object> => _strategy.DoCopyList((dynamic)data),
            _ => throw new NotSupportedException($"There is no copy logic for type {data?.GetType().Name}")
        };
        Clipboard.Set(textToCopy);
    }

    private static bool IsObservableCollection(object obj)
    {
        return obj.GetType().IsGenericType &&
               obj.GetType().GetGenericTypeDefinition() == typeof(ObservableCollection<>);
    }

    private string DoCopyCollectionGeneric<T>(ObservableCollection<T> items)
    {
        return _strategy.DoCopyCollection(items);
    }
}