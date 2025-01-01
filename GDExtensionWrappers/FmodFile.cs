using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodFile : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodFile";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodFile() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodFile"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodFile Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodFile>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodFile"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodFile"/> wrapper type,
    /// a new instance of the <see cref="FmodFile"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodFile"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodFile Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodFile>(godotObject);
    }
}