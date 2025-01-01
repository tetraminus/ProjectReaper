using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodEditorExportPlugin : EditorExportPlugin
{
    public static readonly StringName GDExtensionName = "FmodEditorExportPlugin";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying EditorExportPlugin), please use the Instantiate() method instead.")]
    protected FmodEditorExportPlugin() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodEditorExportPlugin"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodEditorExportPlugin Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodEditorExportPlugin>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodEditorExportPlugin"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodEditorExportPlugin"/> wrapper type,
    /// a new instance of the <see cref="FmodEditorExportPlugin"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodEditorExportPlugin"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodEditorExportPlugin Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodEditorExportPlugin>(godotObject);
    }
}