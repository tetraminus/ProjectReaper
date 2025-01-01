using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodSound : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodSound";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodSound() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodSound"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodSound Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodSound>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodSound"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodSound"/> wrapper type,
    /// a new instance of the <see cref="FmodSound"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodSound"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodSound Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodSound>(godotObject);
    }
#region Properties

    public float Pitch
    {
        get => (float)Get("pitch");
        set => Set("pitch", Variant.From(value));
    }

    public float Volume
    {
        get => (float)Get("volume");
        set => Set("volume", Variant.From(value));
    }

#endregion

#region Methods

    public bool IsValid() => Call("is_valid").As<bool>();

    public void Release() => Call("release");

    public void Play() => Call("play");

    public void Stop() => Call("stop");

    public void SetPaused(bool paused) => Call("set_paused", paused);

    public bool IsPlaying() => Call("is_playing").As<bool>();

    public void SetVolume(float volume) => Call("set_volume", volume);

    public float GetVolume() => Call("get_volume").As<float>();

    public void SetPitch(float pitch) => Call("set_pitch", pitch);

    public float GetPitch() => Call("get_pitch").As<float>();

#endregion

}