using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodBus : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodBus";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodBus() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodBus"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodBus Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodBus>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodBus"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodBus"/> wrapper type,
    /// a new instance of the <see cref="FmodBus"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodBus"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodBus Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodBus>(godotObject);
    }
#region Properties

    public bool Mute
    {
        get => (bool)Get("mute");
        set => Set("mute", Variant.From(value));
    }

    public bool Paused
    {
        get => (bool)Get("paused");
        set => Set("paused", Variant.From(value));
    }

    public float Volume
    {
        get => (float)Get("volume");
        set => Set("volume", Variant.From(value));
    }

#endregion

#region Methods

    public bool GetMute() => Call("get_mute").As<bool>();

    public bool GetPaused() => Call("get_paused").As<bool>();

    public float GetVolume() => Call("get_volume").As<float>();

    public void SetMute(bool mute) => Call("set_mute", mute);

    public void SetPaused(bool paused) => Call("set_paused", paused);

    public void SetVolume(float volume) => Call("set_volume", volume);

    public void StopAllEvents(int stopMode) => Call("stop_all_events", stopMode);

    public bool IsValid() => Call("is_valid").As<bool>();

    public string GetPath() => Call("get_path").As<string>();

    public string GetGuid() => Call("get_guid").As<string>();

#endregion

}