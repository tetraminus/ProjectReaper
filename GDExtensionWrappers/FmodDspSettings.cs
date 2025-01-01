using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodDspSettings : Resource
{
    public static readonly StringName GDExtensionName = "FmodDspSettings";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Resource), please use the Instantiate() method instead.")]
    protected FmodDspSettings() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodDspSettings"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodDspSettings Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodDspSettings>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodDspSettings"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodDspSettings"/> wrapper type,
    /// a new instance of the <see cref="FmodDspSettings"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodDspSettings"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodDspSettings Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodDspSettings>(godotObject);
    }
#region Properties

    public int DspBufferSize
    {
        get => (int)Get("dsp_buffer_size");
        set => Set("dsp_buffer_size", Variant.From(value));
    }

    public int DspBufferCount
    {
        get => (int)Get("dsp_buffer_count");
        set => Set("dsp_buffer_count", Variant.From(value));
    }

#endregion

#region Methods

    public void SetDspBufferSize(int pDspBufferSize) => Call("set_dsp_buffer_size", pDspBufferSize);

    public int GetDspBufferSize() => Call("get_dsp_buffer_size").As<int>();

    public void SetDspBufferCount(int pDspBufferCount) => Call("set_dsp_buffer_count", pDspBufferCount);

    public int GetDspBufferCount() => Call("get_dsp_buffer_count").As<int>();

#endregion

}