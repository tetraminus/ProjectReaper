using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodEditorPlugin : EditorPlugin
{
    public static readonly StringName GDExtensionName = "FmodEditorPlugin";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying EditorPlugin), please use the Instantiate() method instead.")]
    protected FmodEditorPlugin() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodEditorPlugin"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodEditorPlugin Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodEditorPlugin>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodEditorPlugin"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodEditorPlugin"/> wrapper type,
    /// a new instance of the <see cref="FmodEditorPlugin"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodEditorPlugin"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodEditorPlugin Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodEditorPlugin>(godotObject);
    }
}