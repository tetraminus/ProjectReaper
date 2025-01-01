using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodEventDescription : RefCounted
{
    public static readonly StringName GDExtensionName = "FmodEventDescription";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected FmodEventDescription() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodEventDescription"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodEventDescription Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodEventDescription>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodEventDescription"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodEventDescription"/> wrapper type,
    /// a new instance of the <see cref="FmodEventDescription"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodEventDescription"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodEventDescription Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodEventDescription>(godotObject);
    }
#region Methods

    public int GetLength() => Call("get_length").As<int>();

    public Godot.Collections.Array GetInstanceList() => Call("get_instance_list").As<Godot.Collections.Array>();

    public int GetInstanceCount() => Call("get_instance_count").As<int>();

    public void ReleaseAllInstances() => Call("release_all_instances");

    public void LoadSampleData() => Call("load_sample_data");

    public void UnloadSampleData() => Call("unload_sample_data");

    public int GetSampleLoadingState() => Call("get_sample_loading_state").As<int>();

    public bool Is3d() => Call("is_3d").As<bool>();

    public bool IsOneShot() => Call("is_one_shot").As<bool>();

    public bool IsSnapshot() => Call("is_snapshot").As<bool>();

    public bool IsStream() => Call("is_stream").As<bool>();

    public bool HasSustainPoint() => Call("has_sustain_point").As<bool>();

    public Godot.Collections.Array GetMinMaxDistance() => Call("get_min_max_distance").As<Godot.Collections.Array>();

    public float GetSoundSize() => Call("get_sound_size").As<float>();

    public FmodParameterDescription GetParameterByName(string name) => GDExtensionHelper.Bind<FmodParameterDescription>(Call("get_parameter_by_name", name).As<GodotObject>());

    public FmodParameterDescription GetParameterById(int eventPathidPair) => GDExtensionHelper.Bind<FmodParameterDescription>(Call("get_parameter_by_id", eventPathidPair).As<GodotObject>());

    public int GetParameterCount() => Call("get_parameter_count").As<int>();

    public FmodParameterDescription GetParameterByIndex(int index) => GDExtensionHelper.Bind<FmodParameterDescription>(Call("get_parameter_by_index", index).As<GodotObject>());

    public Godot.Collections.Array GetParameters() => Call("get_parameters").As<Godot.Collections.Array>();

    public string GetParameterLabelById(int unnamedArg0, int unnamedArg1) => Call("get_parameter_label_by_id", unnamedArg0, unnamedArg1).As<string>();

    public string GetParameterLabelByName(string unnamedArg0, int unnamedArg1) => Call("get_parameter_label_by_name", unnamedArg0, unnamedArg1).As<string>();

    public string GetParameterLabelByIndex(int unnamedArg0, int unnamedArg1) => Call("get_parameter_label_by_index", unnamedArg0, unnamedArg1).As<string>();

    public string[] GetParameterLabelsById(int unnamedArg0) => Call("get_parameter_labels_by_id", unnamedArg0).As<string[]>();

    public string[] GetParameterLabelsByName(string unnamedArg0) => Call("get_parameter_labels_by_name", unnamedArg0).As<string[]>();

    public string[] GetParameterLabelsByIndex(int unnamedArg0) => Call("get_parameter_labels_by_index", unnamedArg0).As<string[]>();

    public Godot.Collections.Dictionary GetUserProperty(string name) => Call("get_user_property", name).As<Godot.Collections.Dictionary>();

    public int GetUserPropertyCount() => Call("get_user_property_count").As<int>();

    public Godot.Collections.Dictionary UserPropertyByIndex(int index) => Call("user_property_by_index", index).As<Godot.Collections.Dictionary>();

    public bool IsValid() => Call("is_valid").As<bool>();

    public string GetPath() => Call("get_path").As<string>();

    public string GetGuid() => Call("get_guid").As<string>();

#endregion

}