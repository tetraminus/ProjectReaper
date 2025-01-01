using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodPerformanceData : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodPerformanceData";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodPerformanceData() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodPerformanceData"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodPerformanceData Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodPerformanceData>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodPerformanceData"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodPerformanceData"/> wrapper type,
    /// a new instance of the <see cref="FmodPerformanceData"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodPerformanceData"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodPerformanceData Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodPerformanceData>(godotObject);
    }
#region Properties

    public float Dsp
    {
        get => (float)Get("dsp");
        set => Set("dsp", Variant.From(value));
    }

    public float Geometry
    {
        get => (float)Get("geometry");
        set => Set("geometry", Variant.From(value));
    }

    public float Stream
    {
        get => (float)Get("stream");
        set => Set("stream", Variant.From(value));
    }

    public float Update
    {
        get => (float)Get("update");
        set => Set("update", Variant.From(value));
    }

    public float Convolution1
    {
        get => (float)Get("convolution1");
        set => Set("convolution1", Variant.From(value));
    }

    public float Convolution2
    {
        get => (float)Get("convolution2");
        set => Set("convolution2", Variant.From(value));
    }

    public float Studio
    {
        get => (float)Get("studio");
        set => Set("studio", Variant.From(value));
    }

    public int CurrentlyAllocated
    {
        get => (int)Get("currently_allocated");
        set => Set("currently_allocated", Variant.From(value));
    }

    public int MaxAllocated
    {
        get => (int)Get("max_allocated");
        set => Set("max_allocated", Variant.From(value));
    }

    public int SampleBytesRead
    {
        get => (int)Get("sample_bytes_read");
        set => Set("sample_bytes_read", Variant.From(value));
    }

    public int StreamBytesRead
    {
        get => (int)Get("stream_bytes_read");
        set => Set("stream_bytes_read", Variant.From(value));
    }

    public int OtherBytesRead
    {
        get => (int)Get("other_bytes_read");
        set => Set("other_bytes_read", Variant.From(value));
    }

#endregion

#region Methods

    public float GetDsp() => Call("get_dsp").As<float>();

    public float GetGeometry() => Call("get_geometry").As<float>();

    public float GetStream() => Call("get_stream").As<float>();

    public float GetUpdate() => Call("get_update").As<float>();

    public float GetConvolution1() => Call("get_convolution1").As<float>();

    public float GetConvolution2() => Call("get_convolution2").As<float>();

    public float GetStudio() => Call("get_studio").As<float>();

    public int GetCurrentlyAllocated() => Call("get_currently_allocated").As<int>();

    public int GetMaxAllocated() => Call("get_max_allocated").As<int>();

    public int GetSampleBytesRead() => Call("get_sample_bytes_read").As<int>();

    public int GetStreamBytesRead() => Call("get_stream_bytes_read").As<int>();

    public int GetOtherBytesRead() => Call("get_other_bytes_read").As<int>();

#endregion

}