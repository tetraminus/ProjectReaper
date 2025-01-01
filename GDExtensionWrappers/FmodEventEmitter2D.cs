using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodEventEmitter2D : Node2D
{
    public static readonly StringName GDExtensionName = "FmodEventEmitter2D";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Node2D), please use the Instantiate() method instead.")]
    protected FmodEventEmitter2D() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodEventEmitter2D"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodEventEmitter2D Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodEventEmitter2D>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodEventEmitter2D"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodEventEmitter2D"/> wrapper type,
    /// a new instance of the <see cref="FmodEventEmitter2D"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodEventEmitter2D"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodEventEmitter2D Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodEventEmitter2D>(godotObject);
    }
#region Properties

    public string EventName
    {
        get => (string)Get("event_name");
        set => Set("event_name", Variant.From(value));
    }

    public string EventGuid
    {
        get => (string)Get("event_guid");
        set => Set("event_guid", Variant.From(value));
    }

    public bool Attached
    {
        get => (bool)Get("attached");
        set => Set("attached", Variant.From(value));
    }

    public bool Autoplay
    {
        get => (bool)Get("autoplay");
        set => Set("autoplay", Variant.From(value));
    }

    public bool AutoRelease
    {
        get => (bool)Get("auto_release");
        set => Set("auto_release", Variant.From(value));
    }

    public bool AllowFadeout
    {
        get => (bool)Get("allow_fadeout");
        set => Set("allow_fadeout", Variant.From(value));
    }

    public bool PreloadEvent
    {
        get => (bool)Get("preload_event");
        set => Set("preload_event", Variant.From(value));
    }

    public float Volume
    {
        get => (float)Get("volume");
        set => Set("volume", Variant.From(value));
    }

    public float Paused
    {
        get => (float)Get("paused");
        set => Set("paused", Variant.From(value));
    }

#endregion

#region Signals

    public delegate void TimelineBeatHandler(Godot.Collections.Dictionary @params);

    private TimelineBeatHandler _timelineBeat_backing;
    private Callable _timelineBeat_backing_callable;
    public event TimelineBeatHandler TimelineBeat
    {
        add
        {
            if(_timelineBeat_backing == null)
            {
                _timelineBeat_backing_callable = Callable.From<Variant>(
                    (arg0_variant) =>
                    {
                        var arg0 = arg0_variant.As<Godot.Collections.Dictionary>();
                        _timelineBeat_backing?.Invoke(arg0);
                    }
                );
                Connect("timeline_beat", _timelineBeat_backing_callable);
            }
            _timelineBeat_backing += value;
        }
        remove
        {
            _timelineBeat_backing -= value;
            
            if(_timelineBeat_backing == null)
            {
                Disconnect("timeline_beat", _timelineBeat_backing_callable);
                _timelineBeat_backing_callable = default;
            }
        }
    }

    public delegate void TimelineMarkerHandler(Godot.Collections.Dictionary @params);

    private TimelineMarkerHandler _timelineMarker_backing;
    private Callable _timelineMarker_backing_callable;
    public event TimelineMarkerHandler TimelineMarker
    {
        add
        {
            if(_timelineMarker_backing == null)
            {
                _timelineMarker_backing_callable = Callable.From<Variant>(
                    (arg0_variant) =>
                    {
                        var arg0 = arg0_variant.As<Godot.Collections.Dictionary>();
                        _timelineMarker_backing?.Invoke(arg0);
                    }
                );
                Connect("timeline_marker", _timelineMarker_backing_callable);
            }
            _timelineMarker_backing += value;
        }
        remove
        {
            _timelineMarker_backing -= value;
            
            if(_timelineMarker_backing == null)
            {
                Disconnect("timeline_marker", _timelineMarker_backing_callable);
                _timelineMarker_backing_callable = default;
            }
        }
    }

    public delegate void StartFailedHandler();

    private StartFailedHandler _startFailed_backing;
    private Callable _startFailed_backing_callable;
    public event StartFailedHandler StartFailed
    {
        add
        {
            if(_startFailed_backing == null)
            {
                _startFailed_backing_callable = Callable.From(
                    () =>
                    {
                        _startFailed_backing?.Invoke();
                    }
                );
                Connect("start_failed", _startFailed_backing_callable);
            }
            _startFailed_backing += value;
        }
        remove
        {
            _startFailed_backing -= value;
            
            if(_startFailed_backing == null)
            {
                Disconnect("start_failed", _startFailed_backing_callable);
                _startFailed_backing_callable = default;
            }
        }
    }

    public delegate void StartedHandler();

    private StartedHandler _started_backing;
    private Callable _started_backing_callable;
    public event StartedHandler Started
    {
        add
        {
            if(_started_backing == null)
            {
                _started_backing_callable = Callable.From(
                    () =>
                    {
                        _started_backing?.Invoke();
                    }
                );
                Connect("started", _started_backing_callable);
            }
            _started_backing += value;
        }
        remove
        {
            _started_backing -= value;
            
            if(_started_backing == null)
            {
                Disconnect("started", _started_backing_callable);
                _started_backing_callable = default;
            }
        }
    }

    public delegate void RestartedHandler();

    private RestartedHandler _restarted_backing;
    private Callable _restarted_backing_callable;
    public event RestartedHandler Restarted
    {
        add
        {
            if(_restarted_backing == null)
            {
                _restarted_backing_callable = Callable.From(
                    () =>
                    {
                        _restarted_backing?.Invoke();
                    }
                );
                Connect("restarted", _restarted_backing_callable);
            }
            _restarted_backing += value;
        }
        remove
        {
            _restarted_backing -= value;
            
            if(_restarted_backing == null)
            {
                Disconnect("restarted", _restarted_backing_callable);
                _restarted_backing_callable = default;
            }
        }
    }

    public delegate void StoppedHandler();

    private StoppedHandler _stopped_backing;
    private Callable _stopped_backing_callable;
    public event StoppedHandler Stopped
    {
        add
        {
            if(_stopped_backing == null)
            {
                _stopped_backing_callable = Callable.From(
                    () =>
                    {
                        _stopped_backing?.Invoke();
                    }
                );
                Connect("stopped", _stopped_backing_callable);
            }
            _stopped_backing += value;
        }
        remove
        {
            _stopped_backing -= value;
            
            if(_stopped_backing == null)
            {
                Disconnect("stopped", _stopped_backing_callable);
                _stopped_backing_callable = default;
            }
        }
    }

#endregion

#region Methods

    public void Play() => Call("play");

    public void Stop() => Call("stop");

    public void SetParameter(string name, Variant? value) => Call("set_parameter", name, value ?? new Variant());

    public void GetParameter(string name) => Call("get_parameter", name);

    public bool IsPaused() => Call("is_paused").As<bool>();

    public void SetPaused(bool pIsPaused) => Call("set_paused", pIsPaused);

    public void SetEventName(string eventName) => Call("set_event_name", eventName);

    public string GetEventName() => Call("get_event_name").As<string>();

    public void SetEventGuid(string eventGuid) => Call("set_event_guid", eventGuid);

    public string GetEventGuid() => Call("get_event_guid").As<string>();

    public void SetAttached(bool attached) => Call("set_attached", attached);

    public bool IsAttached() => Call("is_attached").As<bool>();

    public void SetAutoplay(bool autoplay) => Call("set_autoplay", autoplay);

    public bool IsAutoplay() => Call("is_autoplay").As<bool>();

    public void SetAutoRelease(bool autoplay) => Call("set_auto_release", autoplay);

    public bool IsAutoRelease() => Call("is_auto_release").As<bool>();

    public bool IsOneShot() => Call("is_one_shot").As<bool>();

    public void SetAllowFadeout(bool allowFadeout) => Call("set_allow_fadeout", allowFadeout);

    public bool IsAllowFadeout() => Call("is_allow_fadeout").As<bool>();

    public void SetPreloadEvent(bool preloadEvent) => Call("set_preload_event", preloadEvent);

    public bool IsPreloadEvent() => Call("is_preload_event").As<bool>();

    public float GetVolume() => Call("get_volume").As<float>();

    public void SetVolume(float pVolume) => Call("set_volume", pVolume);

    public void SetProgrammerCallback(string pProgrammersCallbackSoundKey) => Call("set_programmer_callback", pProgrammersCallbackSoundKey);

    public void EmitCallbacks(Godot.Collections.Dictionary dict, int type) => Call("_emit_callbacks", dict, type);

    public void ToolRemoveAllParameters() => Call("tool_remove_all_parameters");

    public void ToolRemoveParameter(int parameterId) => Call("tool_remove_parameter", parameterId);

#endregion

}