using System;

namespace AnkiDeckEditor.Libs.HanumanInstitute;

/// <summary>
/// Defines a View, its type and how to create one.
/// </summary>
public readonly struct ViewDefinition
{
    /// <summary>
    /// Initializes a new instance of the ViewDefinition class.
    /// </summary>
    /// <param name="viewType">The type of the view must match the return type of <see cref="CreateFunc"/> function.
    /// </param>
    /// <param name="createFunc">A function to create a new instance of the view without using reflection.</param>
    public ViewDefinition(Type viewType, Func<object> createFunc)
    {
        CreateFunc = createFunc;
        ViewType = viewType;
    }

    /// <summary>
    /// The type of the view must match the return type of <see cref="CreateFunc"/> function.
    /// </summary>
    public Type ViewType { get; }

    /// <summary>
    /// A function to create a new instance of the view without using reflection.
    /// </summary>
    public Func<object> CreateFunc { get; }

    /// <summary>
    /// Executes CreateFunc to create a new view.
    /// </summary>
    /// <returns>The newly created view.</returns>
    public object Create()
    {
        return CreateFunc.Invoke();
    }

    /// <summary>
    /// Returns whether the view type derives from a specified class or interface. A base class instance is also
    /// accepted.
    /// </summary>
    /// <typeparam name="T">The type to verify against ViewType.</typeparam>
    /// <returns>True if ViewType derives from a specified type, otherwise false.</returns>
    public bool TypeDerivesFrom<T>()
    {
        return typeof(T).IsAssignableFrom(ViewType);
    }
}