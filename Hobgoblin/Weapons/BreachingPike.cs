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
public class BreachingPike
{
    //item name
    public static ItemName ItemName;

    //technical name
    public static string TechnicalName = "Breaching_Pike";

    //name
    public static string Name = "breaching pike";

    //level
    public static int Level = 0;

    //price
    public static int Price = 8;

    //weapon illustration
    public static Illustration BreachingPikeIllustration = new ModdedIllustration("Breaching_Pike.png");

    //weapon traits
    public static Trait BreachingPikeTrait;

    public static void CreateBreachingPikeTrait()
    {
        //creates meteor hammer
        BreachingPikeTrait = ModManager.RegisterTrait("Breaching Pike",
            new TraitProperties("Breaching Pike", false)
            {
                ProficiencyName = "Breaching Pike"
            });
    }

    public static void Load()
    {
        CreateBreachingPikeTrait();

        ItemName = ModManager.RegisterNewItemIntoTheShop(TechnicalName, (itemName) =>
        {
            return new Item(itemName, BreachingPikeIllustration, Name, Level, Price,
                [
                    Loader.SotTTrait,
                    HobgoblinAncestry.Hobgoblin.HobgoblinTrait,
                    Trait.Martial,
                    Trait.Spear,
                    Trait.Razing,
                    Trait.Reach,
                    Trait.Uncommon
                ])
            .WithMainTrait(BreachingPikeTrait)
            .WithWeaponProperties(new WeaponProperties("1d6", DamageKind.Piercing));
        });
    }
}
