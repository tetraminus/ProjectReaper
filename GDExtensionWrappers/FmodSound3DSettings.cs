using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodSound3DSettings : Resource
{
    public static readonly StringName GDExtensionName = "FmodSound3DSettings";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Resource), please use the Instantiate() method instead.")]
    protected FmodSound3DSettings() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodSound3DSettings"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodSound3DSettings Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodSound3DSettings>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodSound3DSettings"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodSound3DSettings"/> wrapper type,
    /// a new instance of the <see cref="FmodSound3DSettings"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodSound3DSettings"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodSound3DSettings Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodSound3DSettings>(godotObject);
    }
#region Properties

    public float DopplerScale
    {
        get => (float)Get("doppler_scale");
        set => Set("doppler_scale", Variant.From(value));
    }

    public float DistanceFactor
    {
        get => (float)Get("distance_factor");
        set => Set("distance_factor", Variant.From(value));
    }

    public float RolloffScale
    {
        get => (float)Get("rolloff_scale");
        set => Set("rolloff_scale", Variant.From(value));
    }

#endregion

#region Methods

    public void SetDopplerScale(float pDopplerScale) => Call("set_doppler_scale", pDopplerScale);

    public float GetDopplerScale() => Call("get_doppler_scale").As<float>();

    public void SetDistanceFactor(float pDistanceFactor) => Call("set_distance_factor", pDistanceFactor);

    public float GetDistanceFactor() => Call("get_distance_factor").As<float>();

    public void SetRolloffScale(float pRolloffScale) => Call("set_rolloff_scale", pRolloffScale);

    public float GetRolloffScale() => Call("get_rolloff_scale").As<float>();

#endregion

}