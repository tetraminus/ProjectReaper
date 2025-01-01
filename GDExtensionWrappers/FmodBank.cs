using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodBank : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodBank";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodBank() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodBank"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodBank Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodBank>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodBank"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodBank"/> wrapper type,
    /// a new instance of the <see cref="FmodBank"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodBank"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodBank Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodBank>(godotObject);
    }
#region Methods

    public int GetLoadingState() => Call("get_loading_state").As<int>();

    public int GetBusCount() => Call("get_bus_count").As<int>();

    public int GetEventDescriptionCount() => Call("get_event_description_count").As<int>();

    public int GetStringCount() => Call("get_string_count").As<int>();

    public int GetVcaCount() => Call("get_VCA_count").As<int>();

    public Godot.Collections.Array GetDescriptionList() => Call("get_description_list").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetBusList() => Call("get_bus_list").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetVcaList() => Call("get_vca_list").As<Godot.Collections.Array>();

    public bool IsValid() => Call("is_valid").As<bool>();

    public string GetGodotResPath() => Call("get_godot_res_path").As<string>();

    public string GetPath() => Call("get_path").As<string>();

    public string GetGuid() => Call("get_guid").As<string>();

#endregion

}