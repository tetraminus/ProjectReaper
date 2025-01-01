using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodServer : GodotObject
{
    public static readonly StringName GDExtensionName = "FmodServer";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying GodotObject), please use the Instantiate() method instead.")]
    protected FmodServer() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodServer"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodServer Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodServer>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodServer"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodServer"/> wrapper type,
    /// a new instance of the <see cref="FmodServer"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodServer"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodServer Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodServer>(godotObject);
    }
#region Methods

    public void Init(FmodGeneralSettings pSettings) => Call("init", (Resource)pSettings);

    public void Update() => Call("update");

    public void Shutdown() => Call("shutdown");

    public void SetSoftwareFormat(FmodSoftwareFormatSettings pSettings) => Call("set_software_format", (Resource)pSettings);

    public void SetSound3dSettings(FmodSound3DSettings pSettings) => Call("set_sound_3D_settings", (Resource)pSettings);

    public void SetSystemDspBufferSize(FmodDspSettings dspSettings) => Call("set_system_dsp_buffer_size", (Resource)dspSettings);

    public FmodDspSettings GetSystemDspBufferSettings() => GDExtensionHelper.Bind<FmodDspSettings>(Call("get_system_dsp_buffer_settings").As<GodotObject>());

    public int GetSystemDspBufferLength() => Call("get_system_dsp_buffer_length").As<int>();

    public int GetSystemDspNumBuffers() => Call("get_system_dsp_num_buffers").As<int>();

    public bool CheckVcaGuid(string guid) => Call("check_vca_guid", guid).As<bool>();

    public bool CheckVcaPath(string cvaPath) => Call("check_vca_path", cvaPath).As<bool>();

    public bool CheckBusGuid(string guid) => Call("check_bus_guid", guid).As<bool>();

    public bool CheckBusPath(string busPath) => Call("check_bus_path", busPath).As<bool>();

    public bool CheckEventGuid(string guid) => Call("check_event_guid", guid).As<bool>();

    public bool CheckEventPath(string eventPath) => Call("check_event_path", eventPath).As<bool>();

    public FmodVCA GetVcaFromGuid(string guid) => GDExtensionHelper.Bind<FmodVCA>(Call("get_vca_from_guid", guid).As<GodotObject>());

    public FmodVCA GetVca(string cvaPath) => GDExtensionHelper.Bind<FmodVCA>(Call("get_vca", cvaPath).As<GodotObject>());

    public FmodBus GetBusFromGuid(string guid) => GDExtensionHelper.Bind<FmodBus>(Call("get_bus_from_guid", guid).As<GodotObject>());

    public FmodBus GetBus(string busPath) => GDExtensionHelper.Bind<FmodBus>(Call("get_bus", busPath).As<GodotObject>());

    public FmodEventDescription GetEventFromGuid(string guid) => GDExtensionHelper.Bind<FmodEventDescription>(Call("get_event_from_guid", guid).As<GodotObject>());

    public FmodEventDescription GetEvent(string eventPath) => GDExtensionHelper.Bind<FmodEventDescription>(Call("get_event", eventPath).As<GodotObject>());

    public Godot.Collections.Array GetAllVca() => Call("get_all_vca").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetAllBuses() => Call("get_all_buses").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetAllEventDescriptions() => Call("get_all_event_descriptions").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetAllBanks() => Call("get_all_banks").As<Godot.Collections.Array>();

    public Godot.Collections.Array GetAvailableDrivers() => Call("get_available_drivers").As<Godot.Collections.Array>();

    public int GetDriver() => Call("get_driver").As<int>();

    public void SetDriver(int id) => Call("set_driver", id);

    public FmodPerformanceData GetPerformanceData() => GDExtensionHelper.Bind<FmodPerformanceData>(Call("get_performance_data").As<GodotObject>());

    public void SetGlobalParameterByName(string parameterName, float value) => Call("set_global_parameter_by_name", parameterName, value);

    public void SetGlobalParameterByNameWithLabel(string parameterName, string label) => Call("set_global_parameter_by_name_with_label", parameterName, label);

    public float GetGlobalParameterByName(string parameterName) => Call("get_global_parameter_by_name", parameterName).As<float>();

    public void SetGlobalParameterById(int parameterId, float value) => Call("set_global_parameter_by_id", parameterId, value);

    public void SetGlobalParameterByIdWithLabel(int parameterId, string label) => Call("set_global_parameter_by_id_with_label", parameterId, label);

    public float GetGlobalParameterById(int parameterId) => Call("get_global_parameter_by_id", parameterId).As<float>();

    public Godot.Collections.Dictionary GetGlobalParameterDescByName(string parameterName) => Call("get_global_parameter_desc_by_name", parameterName).As<Godot.Collections.Dictionary>();

    public Godot.Collections.Dictionary GetGlobalParameterDescById(int parameterId) => Call("get_global_parameter_desc_by_id", parameterId).As<Godot.Collections.Dictionary>();

    public int GetGlobalParameterDescCount() => Call("get_global_parameter_desc_count").As<int>();

    public Godot.Collections.Array GetGlobalParameterDescList() => Call("get_global_parameter_desc_list").As<Godot.Collections.Array>();

    public void AddListener(int index, GodotObject gameObj) => Call("add_listener", index, gameObj);

    public void RemoveListener(int index) => Call("remove_listener", index);

    public void SetListenerNumber(int listenerNumber) => Call("set_listener_number", listenerNumber);

    public int GetListenerNumber() => Call("get_listener_number").As<int>();

    public float GetListenerWeight(int index) => Call("get_listener_weight", index).As<float>();

    public void SetListenerWeight(int index, float weight) => Call("set_listener_weight", index, weight);

    public Transform3D GetListenerTransform3d(int index) => Call("get_listener_transform3d", index).As<Transform3D>();

    public Transform2D GetListenerTransform2d(int index) => Call("get_listener_transform2d", index).As<Transform2D>();

    public Vector3 GetListener3dVelocity(int index) => Call("get_listener_3d_velocity", index).As<Vector3>();

    public Vector2 GetListener2dVelocity(int index) => Call("get_listener_2d_velocity", index).As<Vector2>();

    public void SetListenerTransform3d(int index, Transform3D transform) => Call("set_listener_transform3d", index, transform);

    public void SetListenerTransform2d(int index, Transform2D transform) => Call("set_listener_transform2d", index, transform);

    public void SetListenerLock(int index, bool isLocked) => Call("set_listener_lock", index, isLocked);

    public bool GetListenerLock(int index) => Call("get_listener_lock", index).As<bool>();

    public Object GetObjectAttachedToListener(int index) => Call("get_object_attached_to_listener", index).As<GodotObject>();

    public FmodBank LoadBank(string pathToBank, int flag) => GDExtensionHelper.Bind<FmodBank>(Call("load_bank", pathToBank, flag).As<GodotObject>());

    public void WaitForAllLoads() => Call("wait_for_all_loads");

    public bool BanksStillLoading() => Call("banks_still_loading").As<bool>();

    public void UnloadBank(string pathToBank) => Call("unload_bank", pathToBank);

    public FmodFile LoadFileAsSound(string path) => GDExtensionHelper.Bind<FmodFile>(Call("load_file_as_sound", path).As<GodotObject>());

    public FmodFile LoadFileAsMusic(string path) => GDExtensionHelper.Bind<FmodFile>(Call("load_file_as_music", path).As<GodotObject>());

    public void UnloadFile(string path) => Call("unload_file", path);

    public FmodEvent CreateEventInstanceWithGuid(string guid) => GDExtensionHelper.Bind<FmodEvent>(Call("create_event_instance_with_guid", guid).As<GodotObject>());

    public FmodEvent CreateEventInstance(string eventPath) => GDExtensionHelper.Bind<FmodEvent>(Call("create_event_instance", eventPath).As<GodotObject>());

    public FmodEvent CreateEventInstanceFromDescription(FmodEventDescription eventPath) => GDExtensionHelper.Bind<FmodEvent>(Call("create_event_instance_from_description", (RefCounted)eventPath).As<GodotObject>());

    public void PlayOneShotUsingGuid(string guid) => Call("play_one_shot_using_guid", guid);

    public void PlayOneShot(string eventName) => Call("play_one_shot", eventName);

    public void PlayOneShotUsingEventDescription(FmodEventDescription eventDescription) => Call("play_one_shot_using_event_description", (RefCounted)eventDescription);

    public void PlayOneShotUsingGuidWithParams(string guid, Godot.Collections.Dictionary parameters) => Call("play_one_shot_using_guid_with_params", guid, parameters);

    public void PlayOneShotWithParams(string eventName, Godot.Collections.Dictionary parameters) => Call("play_one_shot_with_params", eventName, parameters);

    public void PlayOneShotUsingEventDescriptionWithParams(FmodEventDescription eventDescription, Godot.Collections.Dictionary parameters) => Call("play_one_shot_using_event_description_with_params", (RefCounted)eventDescription, parameters);

    public void PlayOneShotUsingGuidAttached(string guid, Node gameObj) => Call("play_one_shot_using_guid_attached", guid, gameObj);

    public void PlayOneShotAttached(string eventName, Node gameObj) => Call("play_one_shot_attached", eventName, gameObj);

    public void PlayOneShotUsingEventDescriptionAttached(FmodEventDescription eventDescription, Node gameObj) => Call("play_one_shot_using_event_description_attached", (RefCounted)eventDescription, gameObj);

    public void PlayOneShotUsingGuidAttachedWithParams(string guid, Node gameObj, Godot.Collections.Dictionary parameters) => Call("play_one_shot_using_guid_attached_with_params", guid, gameObj, parameters);

    public void PlayOneShotAttachedWithParams(string eventName, Node gameObj, Godot.Collections.Dictionary parameters) => Call("play_one_shot_attached_with_params", eventName, gameObj, parameters);

    public void PlayOneShotUsingEventDescriptionAttachedWithParams(FmodEventDescription eventDescription, Node gameObj, Godot.Collections.Dictionary parameters) => Call("play_one_shot_using_event_description_attached_with_params", (RefCounted)eventDescription, gameObj, parameters);

    public void PauseAllEvents() => Call("pause_all_events");

    public void UnpauseAllEvents() => Call("unpause_all_events");

    public void MuteAllEvents() => Call("mute_all_events");

    public void UnmuteAllEvents() => Call("unmute_all_events");

    public FmodSound CreateSoundInstance(string path) => GDExtensionHelper.Bind<FmodSound>(Call("create_sound_instance", path).As<GodotObject>());

#endregion

}