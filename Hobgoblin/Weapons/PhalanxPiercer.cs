using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawnsbury.Mods.Ancestries.Hobgoblin.Weapons;
public class PhalanxPiercer
{
    //item name
    public static ItemName ItemName;

    //technical name
    public static string TechnicalName = "Phalanx_Piercer";

    //name
    public static string Name = "phalanx piercer";

    //level
    public static int Level = 1;

    //price
    public static int Price = 10;

    //weapon illustration
    public static Illustration PhalanxPiercerIllustration = new ModdedIllustration("PhalanxPiercer.png");

    //weapon traits
    public static Trait PhalanxPiercerTrait;

    public static void CreatePhalanxPiercerTrait()
    {
        //creates meteor hammer
        PhalanxPiercerTrait = ModManager.RegisterTrait("Phalanx Piercer",
            new TraitProperties("Phalanx Piercer", false)
            {
                ProficiencyName = "Phalanx Piercer"
            });
    }

    public static void Load()
    {
        CreatePhalanxPiercerTrait();

        ItemName = ModManager.RegisterNewItemIntoTheShop(TechnicalName, (itemName) =>
        {
            return new Item(itemName, PhalanxPiercerIllustration, Name, Level, Price,
                [
                    Loader.SotTTrait,
                    HobgoblinAncestry.Hobgoblin.HobgoblinTrait,
                    Trait.Bow,
                    Trait.Propulsive,
                    Trait.Razing,
                    Trait.Ranged,
                    Trait.Volley30Feet,
                    Trait.OneHandPlus,
                    Trait.Reload1,
                    Trait.Advanced
                ])
            .WithMainTrait(PhalanxPiercerTrait)
            .WithWeaponProperties(new WeaponProperties("1d10", DamageKind.Piercing)
            .WithRangeIncrement(16)
            .WithMaximumRange(1000)
            );
        });
    }
}
