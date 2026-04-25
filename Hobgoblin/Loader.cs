using Dawnsbury.Core.CharacterBuilder.Feats.Features;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;
using Dawnsbury.Mods.Ancestries.Hobgoblin.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dawnsbury.Mods.Ancestries.Hobgoblin;

public class Loader
{
    public static Trait SotTTrait;
    public static string SotTPrepend = "SotT";
    

    [DawnsburyDaysModMainMethod]
    public static void LoadMod()
    {
        SotTTrait = ModManager.RegisterTrait("Shadow of the Tyrant",
            new TraitProperties("Shadow of the Tyrant", true));

        HobgoblinAncestry.Hobgoblin.Load();

        BreachingPike.Load();
        CapturingSpetum.Load();
        PhalanxPiercer.Load();
    }
}
