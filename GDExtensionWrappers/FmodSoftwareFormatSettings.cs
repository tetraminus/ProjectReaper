using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodSoftwareFormatSettings : Resource
{
    public enum SPEAKERMODE : int
    {
        DEFAULT,
        RAW,
        MONO,
        STEREO,
        QUAD,
        SURROUND,
        _5POINT1,
        _7POINT1,
        _7POINT1POINT4,
        MAX,
    }
    public static readonly StringName GDExtensionName = "FmodSoftwareFormatSettings";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Resource), please use the Instantiate() method instead.")]
    protected FmodSoftwareFormatSettings() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodSoftwareFormatSettings"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodSoftwareFormatSettings Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodSoftwareFormatSettings>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodSoftwareFormatSettings"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodSoftwareFormatSettings"/> wrapper type,
    /// a new instance of the <see cref="FmodSoftwareFormatSettings"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodSoftwareFormatSettings"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodSoftwareFormatSettings Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodSoftwareFormatSettings>(godotObject);
    }
#region Properties

    public int SampleRate
    {
        get => (int)Get("sample_rate");
        set => Set("sample_rate", Variant.From(value));
    }

    public SPEAKERMODE SpeakerMode
    {
        get => (SPEAKERMODE)Get("speaker_mode").As<Int64>();
        set => Set("speaker_mode", Variant.From(value));
    }

    public int RawSpeakersCount
    {
        get => (int)Get("raw_speakers_count");
        set => Set("raw_speakers_count", Variant.From(value));
    }

#endregion

#region Methods

    public void SetSampleRate(int pSampleRate) => Call("set_sample_rate", pSampleRate);

    public int GetSampleRate() => Call("get_sample_rate").As<int>();

    public void SetSpeakerMode(int pSpeakerMode) => Call("set_speaker_mode", pSpeakerMode);

    public int GetSpeakerMode() => Call("get_speaker_mode").As<int>();

    public void SetRawSpeakersCount(int pRawSpeakersCount) => Call("set_raw_speakers_count", pRawSpeakersCount);

    public int GetRawSpeakersCount() => Call("get_raw_speakers_count").As<int>();

#endregion

}