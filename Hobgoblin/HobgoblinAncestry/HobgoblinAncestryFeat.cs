using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawnsbury.Mods.Ancestries.Hobgoblin.HobgoblinAncestry;

internal class HobgoblinAncestryFeat : TrueFeat
{
    public HobgoblinAncestryFeat(string name, string flavorText, string rulesText, int level)
        : base(ModManager.RegisterFeatName(name), level, flavorText, rulesText, [Hobgoblin.HobgoblinTrait])
    {
        
    }

}
