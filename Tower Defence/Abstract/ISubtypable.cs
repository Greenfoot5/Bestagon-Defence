using System;

namespace BestagonDefense.Abstract;

/// <summary>
/// Allows an object to have a subtype, getting its "true" type
/// </summary>
public interface ISubtypable
{
    /// <summary>
    /// Gets the subtype Type of the object
    /// </summary>
    /// <returns>The sub-Type of the object</returns>
    public Type GetSubtype();
}