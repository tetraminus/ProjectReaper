using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodParameterDescription : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodParameterDescription";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodParameterDescription() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodParameterDescription"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodParameterDescription Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodParameterDescription>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodParameterDescription"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodParameterDescription"/> wrapper type,
    /// a new instance of the <see cref="FmodParameterDescription"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodParameterDescription"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodParameterDescription Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodParameterDescription>(godotObject);
    }
#region Methods

    public string GetName() => Call("get_name").As<string>();

    public int GetId() => Call("get_id").As<int>();

    public float GetMinimum() => Call("get_minimum").As<float>();

    public float GetMaximum() => Call("get_maximum").As<float>();

    public float GetDefaultValue() => Call("get_default_value").As<float>();

    public bool IsReadOnly() => Call("is_read_only").As<bool>();

    public bool IsAutomatic() => Call("is_automatic").As<bool>();

    public bool IsGlobal() => Call("is_global").As<bool>();

    public bool IsDiscrete() => Call("is_discrete").As<bool>();

    public bool IsLabeled() => Call("is_labeled").As<bool>();

#endregion

}