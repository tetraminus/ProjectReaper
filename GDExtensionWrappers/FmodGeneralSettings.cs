using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodGeneralSettings : Resource
{
    public static readonly StringName GDExtensionName = "FmodGeneralSettings";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Resource), please use the Instantiate() method instead.")]
    protected FmodGeneralSettings() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodGeneralSettings"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodGeneralSettings Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodGeneralSettings>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodGeneralSettings"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodGeneralSettings"/> wrapper type,
    /// a new instance of the <see cref="FmodGeneralSettings"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodGeneralSettings"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodGeneralSettings Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodGeneralSettings>(godotObject);
    }
#region Properties

    public int ChannelCount
    {
        get => (int)Get("channel_count");
        set => Set("channel_count", Variant.From(value));
    }

    public bool IsLiveUpdateEnabled
    {
        get => (bool)Get("is_live_update_enabled");
        set => Set("is_live_update_enabled", Variant.From(value));
    }

    public bool IsMemoryTrackingEnabled
    {
        get => (bool)Get("is_memory_tracking_enabled");
        set => Set("is_memory_tracking_enabled", Variant.From(value));
    }

    public int DefaultListenerCount
    {
        get => (int)Get("default_listener_count");
        set => Set("default_listener_count", Variant.From(value));
    }

    public string BanksPath
    {
        get => (string)Get("banks_path");
        set => Set("banks_path", Variant.From(value));
    }

    public bool ShouldLoadByName
    {
        get => (bool)Get("should_load_by_name");
        set => Set("should_load_by_name", Variant.From(value));
    }

#endregion

#region Methods

    public void SetChannelCount(int pChannelCount) => Call("set_channel_count", pChannelCount);

    public int GetChannelCount() => Call("get_channel_count").As<int>();

    public void SetIsLiveUpdateEnabled(bool pEnableLiveUpdate) => Call("set_is_live_update_enabled", pEnableLiveUpdate);

    public bool GetIsLiveUpdateEnabled() => Call("get_is_live_update_enabled").As<bool>();

    public void SetIsMemoryTrackingEnabled(bool pEnableMemoryTracking) => Call("set_is_memory_tracking_enabled", pEnableMemoryTracking);

    public bool GetIsMemoryTrackingEnabled() => Call("get_is_memory_tracking_enabled").As<bool>();

    public void SetDefaultListenerCount(int pListenerCount) => Call("set_default_listener_count", pListenerCount);

    public int GetDefaultListenerCount() => Call("get_default_listener_count").As<int>();

    public void SetBanksPath(string pPaths) => Call("set_banks_path", pPaths);

    public string GetBanksPath() => Call("get_banks_path").As<string>();

    public void SetShouldLoadByName(bool pShouldLoadByName) => Call("set_should_load_by_name", pShouldLoadByName);

    public bool GetShouldLoadByName() => Call("get_should_load_by_name").As<bool>();

#endregion

}