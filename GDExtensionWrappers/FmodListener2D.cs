using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodListener2D : Node2D
{
    public static readonly StringName GDExtensionName = "FmodListener2D";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Node2D), please use the Instantiate() method instead.")]
    protected FmodListener2D() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodListener2D"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodListener2D Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodListener2D>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodListener2D"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodListener2D"/> wrapper type,
    /// a new instance of the <see cref="FmodListener2D"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodListener2D"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodListener2D Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodListener2D>(godotObject);
    }
#region Properties

    public int ListenerIndex
    {
        get => (int)Get("listener_index");
        set => Set("listener_index", Variant.From(value));
    }

    public bool IsLocked
    {
        get => (bool)Get("is_locked");
        set => Set("is_locked", Variant.From(value));
    }

    public float Weight
    {
        get => (float)Get("weight");
        set => Set("weight", Variant.From(value));
    }

#endregion

#region Methods

    public void SetListenerIndex(int index) => Call("set_listener_index", index);

    public int GetListenerIndex() => Call("get_listener_index").As<int>();

    public void SetLocked(bool locked) => Call("set_locked", locked);

    public bool GetLocked() => Call("get_locked").As<bool>();

    public void SetListenerWeight(float pWeight) => Call("set_listener_weight", pWeight);

    public float GetListenerWeight() => Call("get_listener_weight").As<float>();

#endregion

}