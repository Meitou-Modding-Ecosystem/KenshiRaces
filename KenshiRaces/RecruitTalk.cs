using Meitou.Data.Fcs;
using Meitou.Modding;

namespace KenshiRaces;

/// <summary>
/// The talk the races' characters have (docs/kenshi-races.md "Recruiting"): they greet the player, who answers "Join us." and they join the
/// player's squad (dialogue action 18, join squad). Records 1 to 4 of the mod's ids.
/// </summary>
public static class RecruitTalk
{
    /// <summary>
    /// The lines and the action, after the DIALOGUE (<see cref="Races.Recruit"/>).
    /// </summary>
    const string Greeting = "2-" + Races.FileName, Join = "3-" + Races.FileName, JoinAction = "4-" + Races.FileName;

    /// <summary>
    /// The dialogue action that makes the speaker join the player's squad.
    /// </summary>
    const int JoinSquad = 18;

    /// <summary>
    /// Speakers: the character, then the player.
    /// </summary>
    const int Character = 0, Player = 1;

    /// <summary>
    /// Adds the DIALOGUE, its two lines and the join action.
    /// </summary>
    public static void Add(ModDataContext c)
    {
        var talk = c.Add(Races.Recruit, FcsRecordType.Dialogue, "kenshi races join");
        talk.Bools["for enemies"] = false;
        talk.Bools["locked"] = false;
        talk.Bools["monologue"] = false;
        talk.Bools["one at a time"] = false;
        talk.Strings["tag"] = "";
        Chances(talk);
        talk.References["lines"] = [new FcsReference(Greeting, 0, 0, 0)];

        Line(c, Greeting, Character, "You look like you could use another pair of hands.").References["lines"] = [new FcsReference(Join, 0, 0, 0)];
        Line(c, Join, Player, "Join us.").References["effects"] = [new FcsReference(JoinAction, 0, 0, 0)];
        c.Add(JoinAction, FcsRecordType.DialogAction, "DIALOG_ACTION").Ints["action name"] = JoinSquad;
    }

    /// <summary>
    /// A DIALOGUE_LINE said by <paramref name="speaker"/>.
    /// </summary>
    static FcsRecord Line(ModDataContext c, string id, int speaker, string text)
    {
        var line = c.Add(id, FcsRecordType.DialogueLine, "DIALOGUE_LINE");
        Chances(line);
        line.Ints["speaker"] = speaker;
        line.Strings["text0"] = text;
        return line;
    }

    /// <summary>
    /// Always offered, without limits.
    /// </summary>
    static void Chances(FcsRecord r)
    {
        r.Floats["chance permanent"] = 100;
        r.Floats["chance temporary"] = 100;
        r.Ints["repetition limit"] = 0;
        r.Ints["score bonus"] = 0;
        r.Ints["target is type"] = 0;
    }
}
