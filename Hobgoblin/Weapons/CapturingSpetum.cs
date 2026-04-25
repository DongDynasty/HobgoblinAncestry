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
public class CapturingSpetum
{
    //item name
    public static ItemName ItemName;

    //technical name
    public static string TechnicalName = "Capturing_Spetum";

    //name
    public static string Name = "capturing spetum";

    //level
    public static int Level = 0;

    //price
    public static int Price = 9;

    //weapon illustration
    public static Illustration CapturingSpetumIllustration = new ModdedIllustration("CapturingSpetum.png");

    //weapon traits
    public static Trait CapturingSpetumTrait;

    public static void CreateCapturingSpetumTrait()
    {
        //creates meteor hammer
        CapturingSpetumTrait = ModManager.RegisterTrait("Capturing Spetum",
            new TraitProperties("Capturing Spetum", false)
            {
                ProficiencyName = "Capturing Spetum"
            });
    }

    public static void Load()
    {
        CreateCapturingSpetumTrait();

        ItemName = ModManager.RegisterNewItemIntoTheShop(TechnicalName, (itemName) =>
        {
            return new Item(itemName, CapturingSpetumIllustration, Name, Level, Price,
                [
                    Loader.SotTTrait,
                    HobgoblinAncestry.Hobgoblin.HobgoblinTrait,
                    Trait.TwoHanded,
                    Trait.Polearm,
                    Trait.Reach,
                    Trait.Trip,
                    Trait.Uncommon,
                    Trait.Advanced
                ])
            .WithMainTrait(CapturingSpetumTrait)
            .WithWeaponProperties(new WeaponProperties("1d10", DamageKind.Piercing));
        });
    }
}
