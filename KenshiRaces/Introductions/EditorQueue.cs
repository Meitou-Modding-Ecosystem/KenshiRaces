using Meitou.Modding;
using Meitou.Simulation;

namespace KenshiRaces.Introductions;

/// <summary>
/// The characters the character editor opens on next: the tick that made them offers them, and the next frame that can open the editor does
/// (it refuses while the characters are not in the frame's snapshot yet, or another editor or modal screen is open).
/// </summary>
sealed class EditorQueue
{
    /// <summary>
    /// Guards <see cref="_pending"/>: the tick and the frame run on different threads.
    /// </summary>
    readonly Lock _gate = new();

    /// <summary>
    /// The characters waiting, or null.
    /// </summary>
    IReadOnlyList<CharacterId>? _pending;

    /// <summary>
    /// Waits for the editor to open on <paramref name="characters"/> (replacing any still waiting).
    /// </summary>
    public void Offer(IReadOnlyList<CharacterId> characters)
    {
        lock (_gate) _pending = characters;
    }

    /// <summary>
    /// Opens the editor on the waiting characters through the interface (the frame's callback, on the host's thread).
    /// </summary>
    public void Open(ModInterface ui) => Open(characters => ui.OpenScreen(ModScreen.CharacterEditor, new ScreenTarget(characters)));

    /// <summary>
    /// Hands the waiting characters, if any, to <paramref name="open"/>; they stop waiting once it says the editor opened.
    /// </summary>
    public void Open(Func<IReadOnlyList<CharacterId>, bool> open)
    {
        IReadOnlyList<CharacterId>? pending;
        lock (_gate) pending = _pending;
        if (pending is null || !open(pending)) return;

        lock (_gate)
        {
            if (ReferenceEquals(_pending, pending)) _pending = null;
        }
    }
}
