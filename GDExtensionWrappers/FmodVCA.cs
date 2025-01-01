using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodVCA : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodVCA";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodVCA() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodVCA"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodVCA Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodVCA>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodVCA"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodVCA"/> wrapper type,
    /// a new instance of the <see cref="FmodVCA"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodVCA"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodVCA Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodVCA>(godotObject);
    }
#region Properties

    public float Volume
    {
        get => (float)Get("volume");
        set => Set("volume", Variant.From(value));
    }

#endregion

#region Methods

    public float GetVolume() => Call("get_volume").As<float>();

    public void SetVolume(float volume) => Call("set_volume", volume);

    public bool IsValid() => Call("is_valid").As<bool>();

    public string GetPath() => Call("get_path").As<string>();

    public string GetGuid() => Call("get_guid").As<string>();

#endregion

}