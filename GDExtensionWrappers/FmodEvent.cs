using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodEvent : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodEvent";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodEvent() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodEvent"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodEvent Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodEvent>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodEvent"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodEvent"/> wrapper type,
    /// a new instance of the <see cref="FmodEvent"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodEvent"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodEvent Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodEvent>(godotObject);
    }
#region Properties

    public bool Paused
    {
        get => (bool)Get("paused");
        set => Set("paused", Variant.From(value));
    }

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

    public int Position
    {
        get => (int)Get("position");
        set => Set("position", Variant.From(value));
    }

    public int ListenerMask
    {
        get => (int)Get("listener_mask");
        set => Set("listener_mask", Variant.From(value));
    }

    public Transform2D Transform2d
    {
        get => (Transform2D)Get("transform_2d");
        set => Set("transform_2d", Variant.From(value));
    }

    public Transform3D Transform3d
    {
        get => (Transform3D)Get("transform_3d");
        set => Set("transform_3d", Variant.From(value));
    }

#endregion

#region Methods

    public float GetParameterByName(string parameterName) => Call("get_parameter_by_name", parameterName).As<float>();

    public void SetParameterByName(string parameterName, float value) => Call("set_parameter_by_name", parameterName, value);

    public void SetParameterByNameWithLabel(string parameterName, string label, bool ignoreseekspeed) => Call("set_parameter_by_name_with_label", parameterName, label, ignoreseekspeed);

    public float GetParameterById(int parameterId) => Call("get_parameter_by_id", parameterId).As<float>();

    public void SetParameterById(int parameterId, float value) => Call("set_parameter_by_id", parameterId, value);

    public void SetParameterByIdWithLabel(int parameterId, string label, bool ignoreseekspeed) => Call("set_parameter_by_id_with_label", parameterId, label, ignoreseekspeed);

    public void Start() => Call("start");

    public void Stop(int stopMode) => Call("stop", stopMode);

    public void EventKeyOff() => Call("event_key_off");

    public int GetPlaybackState() => Call("get_playback_state").As<int>();

    public bool GetPaused() => Call("get_paused").As<bool>();

    public void SetPaused(bool paused) => Call("set_paused", paused);

    public float GetPitch() => Call("get_pitch").As<float>();

    public void SetPitch(float pitch) => Call("set_pitch", pitch);

    public float GetVolume() => Call("get_volume").As<float>();

    public void SetVolume(float volume) => Call("set_volume", volume);

    public int GetTimelinePosition() => Call("get_timeline_position").As<int>();

    public void SetTimelinePosition(int position) => Call("set_timeline_position", position);

    public float GetReverbLevel(int index) => Call("get_reverb_level", index).As<float>();

    public void SetReverbLevel(int index, float level) => Call("set_reverb_level", index, level);

    public bool IsVirtual() => Call("is_virtual").As<bool>();

    public void SetListenerMask(int mask) => Call("set_listener_mask", mask);

    public int GetListenerMask() => Call("get_listener_mask").As<int>();

    public void Set2dAttributes(Transform2D position) => Call("set_2d_attributes", position);

    public Transform2D Get2dAttributes() => Call("get_2d_attributes").As<Transform2D>();

    public void Set3dAttributes(Transform3D transform) => Call("set_3d_attributes", transform);

    public Transform3D Get3dAttributes() => Call("get_3d_attributes").As<Transform3D>();

    public void SetNodeAttributes(Node transform) => Call("set_node_attributes", transform);

    public void SetCallback(Callable callback, int callbackMask) => Call("set_callback", callback, callbackMask);

    public void SetProgrammerCallback(string pProgrammersCallbackSoundKey) => Call("set_programmer_callback", pProgrammersCallbackSoundKey);

    public string GetProgrammerCallbackSoundKey() => Call("get_programmer_callback_sound_key").As<string>();

    public bool IsValid() => Call("is_valid").As<bool>();

    public void Release() => Call("release");

#endregion

}