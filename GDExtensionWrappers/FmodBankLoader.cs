using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class FmodBankLoader : Node
{
    public static readonly StringName GDExtensionName = "FmodBankLoader";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Node), please use the Instantiate() method instead.")]
    protected FmodBankLoader() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="FmodBankLoader"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static FmodBankLoader Instantiate()
    {
        return GDExtensionHelper.Instantiate<FmodBankLoader>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="FmodBankLoader"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="FmodBankLoader"/> wrapper type,
    /// a new instance of the <see cref="FmodBankLoader"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="FmodBankLoader"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static FmodBankLoader Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<FmodBankLoader>(godotObject);
    }
#region Properties

    public Godot.Collections.Array BankPaths
    {
        get => (Godot.Collections.Array)Get("bank_paths");
        set => Set("bank_paths", Variant.From(value));
    }

#endregion

#region Methods

    public void SetBankPaths(Godot.Collections.Array pPaths) => Call("set_bank_paths", pPaths);

    public Godot.Collections.Array GetBankPaths() => Call("get_bank_paths").As<Godot.Collections.Array>();

#endregion

}