using Dawnsbury.Auxiliary;
using Dawnsbury.Core;
using Dawnsbury.Core.CharacterBuilder;
using Dawnsbury.Core.CharacterBuilder.AbilityScores;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Champion;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Spellbook;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb.Archetypes.Agnostic;
using Dawnsbury.Core.CharacterBuilder.Selections.Options;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Coroutines.Options;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Creatures.Parts;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Rules;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Targeting.TargetingRequirements;
using Dawnsbury.Core.Mechanics.Targeting.Targets;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core.StatBlocks.Monsters.L5;
using Dawnsbury.Core.Tiles;
using Dawnsbury.Display;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Modding;
using Dawnsbury.Phases.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Dawnsbury.Core.CharacterBuilder.FeatsDb.TrueFeatDb.Archetypes.Agnostic.Beastmaster;

namespace Dawnsbury.Mods.Ancestries.Hobgoblin.HobgoblinAncestry;
public class Hobgoblin
{
    //traits
    public static Trait HobgoblinTrait;
    public static List<Trait> traits = [];

    public static List<Trait> AncestryWeaponTraits =
    [
        HobgoblinTrait,
        Trait.CompositeLongbow,
        Trait.CompositeShortbow,
        Trait.Glaive,
        Trait.Longbow,
        Trait.Shortbow,
        Trait.Longsword
    ];

    //base stats
    public static int hp = 8;
    public static int speed = 5;

    //prepend
    public static string HobgoblinAncestry = "HobgoblinAncestryFeat";

    //feat names
    public static FeatName HobgoblinOnly_ContorianReinforcement = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_ContorianReinforcement");
    public static FeatName HobgoblinOnly_HobgoblinLore = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_HobgoblinLore");
    public static FeatName HobgoblinOnly_HobgoblinWeaponFamiliarity = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_HobgoblinWeaponFamiliarity");
    public static FeatName HobgoblinOnly_LeechClip = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_Leech-Clip");
    public static FeatName HobgoblinOnly_RemorselessLash = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_RemoreselessLash");
    public static FeatName HobgoblinOnly_StoneFace = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_StoneFace");
    public static FeatName HobgoblinOnly_VigorousHealth = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_VigorousHealth");
    public static FeatName HobgoblinOnly_AgonizingRebuke = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_AgonizingRebuke");
    public static FeatName HobgoblinOnly_FormationTraining = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_FormationTraining");
    public static FeatName HobgoblinOnly_HobgoblinWeaponDiscipline = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_HobgoblinWeaponDiscipline");
    public static FeatName HobgoblinOnly_Runtsage = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_Runtsage");
    public static FeatName HobgoblinOnly_PrideInArms = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_PrideInArms");
    public static FeatName HobgoblinOnly_SquadTactics = ModManager.RegisterFeatName(Loader.SotTPrepend + "_" + HobgoblinAncestry + "_SquadTactics");


    //Main Loader
    //-------------------------------------------------------------

    public static void Load()
    {
        //registers the HobgoblinTrait as an ancestry trait
        HobgoblinTrait = ModManager.RegisterTrait("Hobgoblin",
            new TraitProperties("Hobgoblin", true, "Hobgoblins have bald, wide heads and beady eyes, as well as " +
            "gray skin that becomes steely blue when tanned.")
        {
            //is an ancestry
            IsAncestryTrait = true
        });

        //parses the trait
        if (ModManager.TryParse("Breaching Pike", out Trait BreachingPikeTrait))
            AncestryWeaponTraits.Add(BreachingPikeTrait);
        if (ModManager.TryParse("Capturing Spetum", out Trait CapturingSpetumTrait))
            AncestryWeaponTraits.Add(CapturingSpetumTrait);
        if (ModManager.TryParse("Phalanx Piercer", out Trait PhalanxPiercerTrait))
            AncestryWeaponTraits.Add(CapturingSpetumTrait);

        //creates the traits list
        traits = [Loader.SotTTrait, HobgoblinTrait, Trait.Humanoid, Trait.Uncommon];

        //adds the ancestry feats
        AddAncestryFeats(CreateHobgoblinAncestryFeats());

        //registers the new ancestry feat into the ModManager
        ModManager.AddFeat(new AncestrySelectionFeat(ModManager.RegisterFeatName("Hobgoblin_Ancestry", "Hobgoblin"),
            "Taller and stronger than their goblin kin, hobgoblins are equals in strength and size to humans, " +
            "with broad shoulders and long, powerful arms. Hobgoblins structure their society after military " +
            "hierarchies. Even civilian groups such as farming collectives or trading houses organize into " +
            "regiments, companies, and divisions.", traits, hp, speed,
                [
                    //forced constitution increase
                    new EnforcedAbilityBoost(Ability.Constitution),

                    //forced intelligence increase
                    new EnforcedAbilityBoost(Ability.Intelligence),

                    //free ability boost
                    new FreeAbilityBoost()

                   //hobgoblin heritages
                ], CreateHobgoblinHeritages().ToList())

            //forced wisdom flaw
            .WithAbilityFlaw(Ability.Wisdom));
    }

    //adds the ancestry feats
    private static void AddAncestryFeats(IEnumerable<Feat> feats)
    {
        foreach (var feat in feats)
        {
            ModManager.AddFeat(feat);
        }
    }


    //Hobgoblin Heritages
    //-------------------------------------------------------------

    //creates the hobgoblin heritages
    public static IEnumerable<Feat> CreateHobgoblinHeritages()
    {
        //registers the hobgoblin heritage: "Elfbane Hobgoblin"
        yield return new HeritageSelectionFeat(ModManager.RegisterFeatName("HobgoblinHeritage_Elfbane", "Elfbane Hobgoblin"),
            "Hobgoblins were engineered long ago from the unreliable and fecund goblins to be used as an army against the " +
            "elves. Although the elves ultimately freed the hobgoblins from their bondage, some hobgoblins retain ancestral " +
            "resistance to magic, which they refer to as “elf magic.” You gain the Resist Elf Magic reaction.",

            "{b}Resist Elf Magic{/b}: \n" +
            "{b}Trigger{/b}: You attempt a saving throw against a magical effect but haven’t rolled yet;\n" +
            "\n" +
            "{b}Effect{/b}: Your ancestral resistance to magic protects you. You gain a +1 circumstance bonus " +
            "to the triggering saving throw. If the triggering effect is arcane, you gain a +2 circumstance bonus instead")
            .WithOnCreature(creature =>
            {
                //creates the new reaction
                creature.AddQEffect(new("Resist Elf Magic " + RulesBlock.GetIconTextFromNumberOfActions(-2),
                    "When you attempt a saving throw against a magical effect but haven’t rolled yet, your ancestral resistance to " +
                    "magic protects you. You gain a +1 circumstance bonus to the triggering saving throw. If the triggering effect is arcane, " +
                    "you gain a +2 circumstance bonus instead.")
                {
                    //triggers after you make a saving throw
                    BeforeYourSavingThrow = async delegate (QEffect qEffect, CombatAction action, Creature you)
                    {
                        //if the action is a magical effect,
                        if (action != null && action.HasTrait(Trait.Magical))
                        {
                            if (await you.AskToUseReaction("Use resist elf magic to gain a circumstance bonus to the triggering saving throw?"))
                            {
                                //gain a bonus to your defense
                                qEffect.BonusToDefenses = (qEffect, action, defense) =>
                                {
                                    //if the action is an arcane effect,
                                    if (action != null && action.HasTrait(Trait.Arcane) && defense != 0)
                                    {
                                        //returns a circumstance bonus of 2
                                        return new Bonus(2, BonusType.Circumstance, "Resist Elf Magic");
                                    }
                                    else
                                    {
                                        //returns a circumstance bonus of 1
                                        return new Bonus(1, BonusType.Circumstance, "Resist Elf Magic");
                                    }
                                };

                            }
                        }
                    }
                });
            });

        //registers the hobgoblin heritage: "Runtboss Hobgoblin"
        yield return new HeritageSelectionFeat(ModManager.RegisterFeatName("HobgoblinHeritage_Runtboss", "Runtboss Hobgoblin"),
            "You come from a long line of hobgoblins who commanded goblins. You are smaller than other hobgoblins, but you have no less authority than any other hobgoblin.",

            "If you roll a success on an Intimidation check, you get a critical success instead; if you roll a critical failure, you get a failure instead.")
            .WithPermanentQEffect("If you roll a success on an Intimidation check, you get a critical success instead; if you roll a critical failure, you get a failure instead.",
            qEffectFeat =>
            {
                //changes the active roll
                qEffectFeat.AdjustActiveRollCheckResult = (qf, defense, action, originalResult) =>
                {
                    //if the action has the intimidation trait,
                    if (action.HasTrait(Trait.Intimidation))
                    {

                        //increase the result by one step
                        return originalResult.ImproveByOneStep();
                    }

                    //otherwise, return the original result
                    return originalResult;
                };
            });

        //registers the hobgoblin heritage: "Smokeworker Hobgoblin"
        yield return new HeritageSelectionFeat(ModManager.RegisterFeatName("HobgoblinHeritage_Smokeworker", "Smokeworker Hobgoblin"),
            "Your family have been alchemists, engineers, and scientists for generations, laboring on projects that bring smoke and fire to the field of battle.",
            "You gain fire resistance equal to half your level (minimum 1). You automatically succeed at the DC 5 flat check to target a concealed creature if " +
            "that creature is concealed only by smoke.")
            .WithOnCreature(creature =>
            {
                //adds the fire resistance equal to half your level (minimum of 1)
                creature.AddQEffect(QEffect.DamageResistance(DamageKind.Fire, Math.Max(creature.Level / 2, 1)));

                //adds smokevision (bless you Petr for having this made already!)
                creature.AddQEffect(QEffect.SmokeVision());
            });

        //registers the hobgoblin heritage: "Warrenbred Hobgoblin"
        yield return new HeritageSelectionFeat(ModManager.RegisterFeatName("HobgoblinHeritage_Warrenbred", "Warrenbred Hobgoblin"),
            "Your ancestors lived underground. Your ears are larger than those of other hobgoblins and sensitive to echoes.",

            "When you target an opponent that is concealed from you or hidden from you, you gain a +2 status bonus to the roll. " +
            "In addition, if you roll a success on an Acrobatics check to Squeeze, you get a critical success instead.")
            .WithPermanentQEffect("When you target an opponent that is concealed from you or hidden from you, you gain a + 2 status bonus to the roll. " +
            "In addition, if you roll a success on an Acrobatics check to Squeeze, you get a critical success instead.",
            qEffectFeat =>
            {

                //stealth bonus
                qEffectFeat.BeforeYourActiveRoll = (qEffect, action, target) =>
                {
                    //gets the detection strength of the target
                    DetectionStrength DetectionStrength = HiddenRules.DetermineHidden(qEffectFeat.Owner, target);

                    //if the target is concealed or hidden, 
                    if (DetectionStrength == DetectionStrength.Concealed || DetectionStrength == DetectionStrength.Hidden)
                    {
                        //gives a bonus to all checks and DCs again the concealed/hidden target
                        qEffect.BonusToAllChecksAndDCs = (QEffect qEffect) =>
                        {
                            //increase chance to hit
                            return new Bonus(2, BonusType.Status, "Warrenbred", true);
                        };
                    }

                    //returns task completed
                    return Task.CompletedTask;
                };

                //sneak bonus
                qEffectFeat.AdjustActiveRollCheckResult = (qEffect, action, self, result) =>
                {
                    if (action.ActionId == ActionId.Squeeze && result == CheckResult.Success && self == qEffectFeat.Owner)
                    {
                        //improve by a step
                        return result.ImproveByOneStep();
                    }

                    return result;
                };
            });
    }


    //Hobgoblin Ancestry Feats
    //-------------------------------------------------------------

    //creates feats
    private static IEnumerable<Feat> CreateHobgoblinAncestryFeats()
    {

        //Level 1 Ancestry Feats
        //-------------------------------------------------------------

        //adds the Hobgoblin Ancestry Feat "Contorian Reinforcement"
        yield return new HobgoblinAncestryFeat("Cantorian Reinforcement",
            "The life energy that helped create the first hobgoblins is particularly potent in you, protecting you from ongoing maladies.",

            "If you roll a success on a saving throw against a disease or poison, you get a critical success instead. If you have a " +
            "different ability that would improve the save in this way (such as the battle hardened fighter class feature), if you roll a " +
            "critical failure on the save you get a failure instead.", 1)
            .WithPermanentQEffect(qEffectFeat =>
            {
                //adds the QEffect to increase the save
                qEffectFeat.AdjustSavingThrowCheckResult = (QEffect qf, Defense defense, CombatAction action, CheckResult originalResult) =>
                {
                        if (action.HasTrait(Trait.Disease) || action.HasTrait(Trait.Poison))
                        {
                            //increase the result by one step
                            return originalResult.ImproveByOneStep();
                        }

                        //otherwise, do nothing
                        return originalResult;
                };
            });

        //adds the Hobgoblin Ancestry Feat "Hobgoblin Lore"
        yield return new HobgoblinAncestryFeat("Hobgoblin Lore",
            "You’ve studied traditional hobgoblin exercises and fieldcraft, all of which have a militaristic bent.",

            "You gain the trained proficiency rank in Athletics and Crafting. If you would automatically become " +
            "trained in one of those skills (from your background or class, for example), you instead become " +
            "trained in a skill of your choice.", 1)
            .WithOnSheet(sheet =>
            {
                //train in this or substitute it
                sheet.TrainInThisOrSubstitute(Skill.Athletics);
                sheet.TrainInThisOrSubstitute(Skill.Crafting);
            });

        //adds the Hobgoblin Ancestry Feat "Hobgoblin Weapon Familiarity"
        yield return new HobgoblinAncestryFeat("Hobgoblin Weapon Familiarity",
            "You gain greater access to weapons specific to your cultural heritage",

            "You gain access to all uncommon weapons with the hobgoblin trait. You " +
            "have familiarity with weapons with the hobgoblin trait plus the composite " +
            "longbow, composite shortbow, glaive, longbow, longsword, and shortbow—for " +
            "the purpose of proficiency, you treat any of these that are martial weapons " +
            "as simple weapons and any that are advanced weapons as martial weapons.\n\n" +
            "At 5th level, whenever you get a critical hit with one of these weapons, you " +
            "get its {tooltip:criteffect}critical specialization effect.{/}", 1)
            .WithOnSheet(sheet =>
            {
                //for every weapon trait in ancestry weapon traits
                foreach (Trait WeaponTrait in AncestryWeaponTraits)
                {
                    sheet.Proficiencies.AutoupgradeAlongBestWeaponProficiency([WeaponTrait]);
                }

                //sets the proficiencies for martial and simple weapons
                sheet.Proficiencies.AddProficiencyAdjustment(
                    traits => traits.Any(AncestryWeaponTraits.Contains) && traits.Contains(Trait.Martial), Trait.Simple);

                sheet.Proficiencies.AddProficiencyAdjustment(
                    traits => traits.Any(AncestryWeaponTraits.Contains) && traits.Contains(Trait.Advanced), Trait.Martial);
            })
            .WithPermanentQEffect(null, (qEffect) =>
            {
                qEffect.YouHaveCriticalSpecialization = (qEffectThis, item, _, _) =>
                {
                    return qEffectThis.Owner.Level >= 5 && item.Traits.Any(AncestryWeaponTraits.Contains);
                };
            });

        //adds the Hobgoblin Ancestry Feat "Leech-Clip"
        yield return new HobgoblinAncestryFeat("Leech-Clip " + RulesBlock.GetIconTextFromNumberOfActions(2),
            "You are trained to capture deserters, or 'leeches.'",

            "Make a melee Strike with a weapon from the flail group. On a hit, the target takes a –10-foot status penalty to its Speed (or a –15-foot status penalty " +
            "on a critical hit). The penalty lasts for 1 round. It applies only if the target has a land Speed and depends on legs " +
            "or other targetable appendages to use its land Speed. As with all penalties to Speed, this can’t reduce a creature’s " +
            "Speed below 5 feet.", 1)
            .WithPermanentQEffect("You are trained to capture deserters, or 'leeches.'", qEffect =>
            {
                qEffect.ProvideStrikeModifier = delegate (Item item)
                {
                    if (item.HasTrait(Trait.Flail))
                    {
                        CombatAction action = qEffect.Owner.CreateStrike(item)
                        .WithActionCost(2)
                        .WithEffectOnEachTarget((action, creature, chosenTarget, result) =>
                        {
                            if (result == CheckResult.Success)
                            {
                                chosenTarget.AddQEffect(QEffect.PenaltyToSpeed(2, BonusType.Status)
                                    .WithExpirationOneRoundOrRestOfTheEncounter(creature, false));
                            }
                            else if (result == CheckResult.CriticalSuccess)
                            {
                                chosenTarget.AddQEffect(QEffect.PenaltyToSpeed(3, BonusType.Status)
                                    .WithExpirationOneRoundOrRestOfTheEncounter(creature, false));
                            }

                            return Task.CompletedTask;
                        });
                        action.Name = "Leech-Clip";
                        action.Illustration = IllustrationName.Scimitar;
                        action.ActionCost = 2;
                        action.Traits.Add(HobgoblinTrait);
                        action.Description = "Make a melee Strike with a weapon from the flail group.\n\n" +

                        "On a hit, the target takes a –10-foot status penalty to its Speed (or a –15-foot status penalty on a critical hit).\n\n" +

                        "The penalty lasts for 1 round. It applies only if the target has a land Speed. As with all penalties to Speed, this can’t " +
                        "reduce a creature’s Speed below 5 feet.";

                        return action;
                    }

                    return (CombatAction?)null;
                };
            });

        //adds the Hobgoblin Ancestry Feat "Remorseless Lash"
        yield return new HobgoblinAncestryFeat("Remorseless Lash",
            "You’re skilled at beating an enemy when their morale is already breaking.",

            "When you succeed at a melee weapon Strike against a frightened enemy, that " +
            "enemy can’t reduce their frightened condition below 1 until the beginning of " +
            "your next turn.", 1)
            .WithPermanentQEffect("You’re skilled at beating an enemy when their morale is already breaking.",
            qEffectFeat =>
            {
                 qEffectFeat.AfterYouTakeActionAgainstTarget = (qEffectThis, action, creature, result) =>
                {
                    if (action.HasTrait(Trait.Melee) && (result == CheckResult.Success || result == CheckResult.CriticalSuccess))
                    {
                        var frightened = creature.FindQEffect(QEffectId.Frightened);

                        if (frightened != null)
                        {
                            frightened.CannotExpireThisTurn = true;
                            creature.Overhead("Remorseless Lash!", Color.Black);
                        }
                    }

                    return Task.CompletedTask;
                };
            });

        //adds the Hobgoblin Ancestry Feat "Stone Face"
        yield return new HobgoblinAncestryFeat("Stone Face",
            "You’ve mastered the art of composure, even in the face of fear.",

            "You gain a +1 circumstance bonus to saves against effects with the fear trait and a +2 circumstance " +
            "bonus to your Will DC against Intimidation skill actions, such as Demoralize.", 1)
            .WithPermanentQEffect("You’ve mastered the art of composure, even in the face of fear.",
            qEffectFeat =>
            {
                qEffectFeat.BeforeYourSavingThrow = (qEffectThis, actionThis, creature) =>
                {
                    if (actionThis.HasTrait(Trait.Intimidation))

                        qEffectThis.BonusToDefenses = (qEffectTheOther, actionTheOther, defense) =>
                        {
                            if (defense == Defense.Will)
                                return new Bonus(2, BonusType.Circumstance, "Stone Face");

                            return null;
                        };

                    if (actionThis.HasTrait(Trait.Fear))

                        qEffectThis.BonusToDefenses = (qEffectThat, actionThat, defense) =>
                        {
                            return new Bonus(1, BonusType.Circumstance, "Stone Face");
                        };

                    return Task.CompletedTask;
                };
            });

        //adds the Hobgoblin Ancestry Feat "Vigorous Health"
        yield return new HobgoblinAncestryFeat("Vigorous Health",
            "You can withstand blood loss startlingly well.",

            "Whenever you would gain the drained condition, you can attempt a DC 17 flat check. On a success, " +
            "you don’t gain the drained condition.", 1)
            .WithPermanentQEffect("You can withstand blood loss startlingly well.",
            qEffectFeat =>
            {
                qEffectFeat.YouAcquireQEffect = (qEffectThis, qEffectThat) =>
                {
                    if (qEffectThat.Id == QEffectId.Drained)
                    {
                        int VigorousHealthDC = 17;

                        (CheckResult result, string String) tuple = Checks.RollFlatCheck(VigorousHealthDC);

                        CheckResult result = tuple.result;
                        string NumberRolled = tuple.String;

                        if (result == CheckResult.Success || result == CheckResult.CriticalSuccess)
                        {
                            qEffectThat.Owner.Overhead("Vigorous Health Success!", Color.Black,
                                $"{qEffectThat.Owner} makes a vigorous health check against {qEffectThat.Name} vs. DC {VigorousHealthDC} ({NumberRolled})");

                            //doesn't gain Drained
                            return null;
                        }
                    }

                    //gains drained
                    return qEffectThat;
                };
            });

        /*
        //NOT POSSIBLE UNTIL THEY ALLOW MORE VARIABLE SNEAKING (having a variable for sneak movement rather than creature.speed / 2)
        //OR I DECIDE TO DO A WHOLE THING WHICH I DONT WANT TO DO
        adds the Hobgoblin Ancestry Feat "Sneaky"
        yield return new HobgoblinAncestryFeat("Sneaky",
            "{tooltip:stealth}Stealth{/} is an important tool in your arsenal.",

            "You can move 5 feet farther when you take the Sneak action, up to your Speed.\n\n" +
            "In addition, as long as you continue to use Sneak actions and succeed at your Stealth check, " +
            "you don’t become observed if you don’t have cover or greater cover and aren’t concealed at " +
            "the end of the Sneak action, as long as you have cover or greater cover or are concealed at " +
            "the end of your turn.", 1)
            .WithPermanentQEffect("You are extra sneaky.",
            qEffectFeat =>
            {
                qEffectFeat.AfterYouTakeAction = (qEffectThis, action) =>
                {
                    if (action.ActionId == ActionId.Sneak)
                    {
                        if ((qEffectThis.Owner.Speed + 1) % 2 == 0)

                            qEffectThis.BonusToAllSpeeds = qEffectThat =>
                            {
                                return new Bonus(2, BonusType.Untyped, "Sneaky");
                            };

                        else if ((qEffectThis.Owner.Speed + 1) % 2 == 1)
                            qEffectThis.BonusToAllSpeeds = 1;
                    }
                };
            }); */



        //Level 5 Ancestry Feats
        //-------------------------------------------------------------


        //adds the Hobgoblin Ancestry Feat "Agonizing Rebuke"
        //changes from TT:
        // - changed the effect to be persistent 1d4 mental damage
        // - removed the ending conditions (ends on a regular persistent damage save)
        yield return new HobgoblinAncestryFeat("Agonizing Rebuke",
            "When you terrorize your enemies, you also cause them painful mental distress.",

            "When you successfully Demoralize an enemy, that enemy takes a persistent 1d4 mental damage at " +
            "the start of each of its turns. \n\n" +

            "If you have master proficiency in Intimidation, the damage increases to 2d4, and if you have " +
            "legendary proficiency, the damage increases to 3d4.", 5)
            .WithPermanentQEffect("When you terrorize your enemies, you also cause them painful mental distress.",
            qEffectFeat =>
            {
                qEffectFeat.AfterYouTakeActionAgainstTarget = (qEffectThis, action, creature, result) =>
                {
                    if (action.ActionId == ActionId.Demoralize && (result == CheckResult.Success || result == CheckResult.CriticalSuccess))
                    {
                        if (qEffectThis.Owner.Proficiencies.AllProficiencies.ContainsKey(Trait.Intimidation))
                        {
                            if (qEffectThis.Owner.Proficiencies.AllProficiencies.TryGetValue(Trait.Intimidation, out Proficiency proficiency))
                            {
                                //gets the intimidation proficiency
                                Proficiency IntimidationProficiency = proficiency;

                                switch (IntimidationProficiency)
                                {
                                    case Proficiency.Trained:
                                    case Proficiency.Expert:
                                        AgonizingRebuke("1d4", creature);
                                        break;

                                    case Proficiency.Master:
                                        AgonizingRebuke("2d4", creature);
                                        break;

                                    case Proficiency.Legendary:
                                        AgonizingRebuke("3d4", creature);
                                        break;
                                }
                            }
                        }
                    }

                    return Task.CompletedTask;
                    
                    static void AgonizingRebuke(string persistentDamage, Creature creature)
                    {
                        creature.AddQEffect(QEffect.PersistentDamage(persistentDamage, DamageKind.Mental));
                    }
                };
            });

        //adds the Hobgoblin Ancestry Feat "Formation Training"
        //changes from TT:
        // - the two allies do not need to be hobgoblins
        // - removed the prerequisite to be trained in all martial weapons.
        yield return new HobgoblinAncestryFeat("Formation Training",
            "You know how to fight in formation with your brethren.",

            "When you are adjacent to at least two allies, you gain a +1 circumstance bonus to AC and saving throws. " +
            "This bonus increases to +2 on Reflex saves against area effects.", 5)
            .WithPermanentQEffect("You know how to fight in formation with your brethren.",
            qEffectFeat =>
            {
                qEffectFeat.StartOfYourEveryTurn = (qEffectThis, you) =>
                {
                    TBattle CurrentBattle = qEffectFeat.Owner.Battle;

                    foreach (Creature creature in CurrentBattle.AllCreatures)
                    {
                        if (qEffectFeat.Owner.IsAdjacentTo(creature) && creature.FriendOfAndNotSelf(qEffectFeat.Owner))
                        {
                            qEffectThis.YouAreTargeted = (qEffectThat, action) =>
                            {
                                qEffectThat.BonusToDefenses = (qEffectTheOther, action, defenses) =>
                                {
                                    if (defenses == Defense.Reflex && action != null && action.ChosenTargets.AllCreaturesInArea.Count > 1)
                                        return new Bonus(1, BonusType.Circumstance, "Formation Training");

                                    if (defenses == Defense.AC || defenses == Defense.Reflex || defenses == Defense.Will || defenses == Defense.Fortitude)
                                        return new Bonus(2, BonusType.Circumstance, "Formation Training");

                                    return null;
                                };

                                return Task.CompletedTask;
                            };

                            break;
                        }
                    };

                    return Task.CompletedTask;
                };
            });

        //adds the Hobgoblin Ancestry Feat "Hobgoblin Weapon Discipline"
        yield return new HobgoblinAncestryFeat("Hobgoblin Weapon Discipline",
            "You know how to efficiently utilize the weapons soldiers use in close quarters.",

            "Whenever you score a critical hit using a weapon of the polearm, spear, or sword group, " +
            "you apply the weapon’s critical specialization effect.", 5)
            .WithPrerequisite(HobgoblinOnly_HobgoblinWeaponFamiliarity, "Hobgoblin Weapon Familiarity")
            .WithPermanentQEffect("You know how to efficiently utilize the weapons soldiers use in close quarters.",
            qEffectFeat =>
            {
                qEffectFeat.YouHaveCriticalSpecialization = (_, item, _, _) =>
                {
                    return item.HasTrait(Trait.Polearm) || item.HasTrait(Trait.Spear) || item.HasTrait(Trait.Sword);
                };
            });

        /*
        //MIGHT REWORK THIS FEAT LATER SINCE PLAYERS WILL ALWAYS HAVE THE PROPER ITEMS IN THEIR HANDS
        //adds the Hobgoblin Ancestry Feat "Recognize Ambush"
        yield return new HobgoblinAncestryFeat("Recognize Ambush",
            "Your combat training has honed you to be ready for an attack at all times.",

            "You Interact to draw a weapon.", 5)
            .WithPermanentQEffect("You know how to efficiently utilize the weapons soldiers use in close quarters.",
            qEffectFeat =>
            {
        
            }); */

        //adds the Hobgoblin Ancestry Feat "Runtsage"
        yield return new HobgoblinAncestryFeat("Runtsage",
            "Unlike most of your kind, who dismiss goblins as embarrassments or expendable annoyances, you have studied " +
            "the methodology behind their irresponsible and incomprehensible actions.",

            "You gain the Adopted Ancestry general feat and must select goblin as the feat’s chosen ancestry. You also " +
            "gain one goblin ancestry feat.", 5)
            .WithOnSheet(sheet =>
            {
                sheet.GrantFeat(FeatName.AdoptedAncestry);
                sheet.Ancestries.Add(Trait.Goblin);
                sheet.AddSelectionOption(new SingleFeatSelectionOption("AdoptedAncestryGoblin", "Adopted Ancestry feat", 5,
                    feat =>
                    {
                        return feat.HasTrait(Trait.Goblin) && feat.HasTrait(Trait.Ancestry);
                    })
                {
                    GeneralFeatGroupName = Trait.Goblin.HumanizeTitleCase2()
                });
            });


        //Level 9 Ancestry Feats
        //-------------------------------------------------------------

        //adds the Hobgoblin Ancestry Feat "Cantorian Rejuvenation"
        yield return new HobgoblinAncestryFeat("Cantorian Rejuvenation",
            "The life-giving energy that flows in your blood revitalizes you.",

            "You recover 4d6 Hit Points and gain 10 temporary Hit Points for 1 minute. At 15th level, you instead recover 6d6 HP and gain 15 temporary HP.", 9)
            .WithActionCost(2)
            .WithPermanentQEffect("You recover 4d6 Hit Points and gain 10 temporary Hit Points for 1 minute. At 15th level, you instead recover 6d6 HP and gain 15 temporary HP.",
            qEffectFeat =>
            {
                qEffectFeat.ProvideMainAction = qEffectThis =>
                {
                    var section = new PossibilitySection("Cantorian Rejuvenation");
                    ActionPossibility ContorianRejuvenation = new(CreateCantorianRejuvenationCombatAction());
                    section.Possibilities.Add(ContorianRejuvenation);

                    return ContorianRejuvenation;

                    CombatAction CreateCantorianRejuvenationCombatAction()
                    {
                        Target target = Target.Self().WithAdditionalRestriction(creature =>
                        {
                            if (creature.Damage == 0)
                            {
                                return "Full Health!";
                            }

                            if (qEffectThis.Owner.PersistentUsedUpResources.UsedUpActions.Contains("Cantorian Rejuvenation"))
                            {
                                return "Already Used!";
                            }

                            else return null;
                        });

                        return new CombatAction(qEffectThis.Owner, IllustrationName.Heal, "Cantorian Rejuvenation",
                            [HobgoblinTrait, Trait.Ancestry, Trait.Healing],
                            "You recover 4d6 Hit Points and gain 10 temporary Hit Points for 1 minute. At 15th level, you instead recover 6d6 HP and gain 15 temporary HP.",
                            target)
                        .WithActionCost(2)
                        .WithEffectOnSelf(creature =>
                        {
                            if (creature.Level >= 15)
                            {
                                creature.GainTemporaryHP(15);
                                creature.HealAsync("6d6", CombatAction.DefaultCombatAction);
                            }
                            else
                            {
                                creature.GainTemporaryHP(10);
                                creature.HealAsync("4d6", CombatAction.DefaultCombatAction);
                            }

                            creature.PersistentUsedUpResources.UsedUpActions.Add("Cantorian Rejuvenation");
                        });
                    }
                };
            });


        /* Animal companion creation is hard coded so I can't change anything about them
        //adds the Hobgoblin Ancestry Feat "Fell Rider"
        yield return new HobgoblinAncestryFeat("Fell Rider",
            "You have trained with your animal companion to become a terrifying juggernaut on the battlefield.",

            "Your animal companion becomes trained in Intimidation. If your animal companion uses Support while " +
            "serving as your mount, it grants you the effects of the Aid reaction on your first Intimidation check " +
            "to Demoralize on the same turn, even though it can’t take reactions.", 9)
            .WithPrerequisite(values => ObtainBeastmasterData(values).HasAnyAnimalCompanion, "You must have an animal companion.")
            .WithPermanentQEffect("Your animal companion becomes trained in Intimidation",
            qEffectFeat =>
            {


            }); */

        //adds the Hobgoblin Ancestry Feat "Pride in Arms"
        yield return new HobgoblinAncestryFeat("Pride in Arms " + RulesBlock.GetIconTextFromNumberOfActions(-2),
            "With a shout of triumph, you grant inspiration to an ally fight on.",

            "{b}Trigger:{b} An ally within 30 feet brings an enemy to 0 Hit Points. " +
            "The triggering ally gains temporary Hit Points equal to their Constitution modifier.", 9)
            .WithPermanentQEffect("An ally within 30 feet that brings an enemy to 0 Hit Points gains temporary Hit Points equal to their Constitution modifier",
            qEffectFeat =>
            {
                //get hobgoblin
                Creature hobgoblin = qEffectFeat.Owner;

                //set up listers
                PrideInArmsReaction(qEffectFeat, hobgoblin, delegate (QEffect callerQEffect)
                {
                    //when lethal damage is dealt,
                    callerQEffect.AfterYouTakeActionAgainstTarget = async delegate (QEffect qfAlly, CombatAction action, Creature target, CheckResult result)
                    {
                        //get the ally
                        Creature ally = action.Owner;

                        if (action.Target != null && hobgoblin != null && ally != null && target != null && ally.FriendOf(hobgoblin) && ally.DistanceTo(hobgoblin) <= 6 && target.DeathScheduledForNextStateCheck)
                        {
                            //if the hobgoblin can't use reacations,
                            if (!hobgoblin.Actions.CanTakeReaction())
                            {
                                //return
                                return;
                            }

                            //gets ally constitution modifier
                            int allyConstitution = ally.Abilities.Constitution;

                            if (!await hobgoblin.AskToUseReaction(ally?.ToString() +
                                " just brought " + target?.ToString() + " to 0 hit points! Use your Pride In Arms reaction to give " +
                                ally?.ToString() + " " + allyConstitution + " temporary hit points?"))
                            {
                                //return
                                return;
                            }

                            //notification
                            hobgoblin.Overhead("pride in arms!", Color.Lime, hobgoblin?.ToString() + " uses pride in arms!");

                            //ally gains temp HP
                            ally?.GainTemporaryHP(allyConstitution);
                        }

                        //return
                        return;
                    };
                });

                static void PrideInArmsReaction(QEffect qEffectFeat, Creature hobgoblin, Action<QEffect> configure)
                {
                    qEffectFeat.StateCheck = delegate (QEffect qEffectReaction)
                    {
                        //if the hobgoblin can't take reactions
                        if (!hobgoblin.Actions.CanTakeReaction())
                        {
                            //return
                            return;
                        }

                        //for each creature that is allied with the hobgoblin and is within 30 feet,
                        foreach (Creature creature in hobgoblin.Battle.AllCreatures.Where((Creature cr) => cr.FriendOf(hobgoblin) && cr != hobgoblin && cr.DistanceTo(hobgoblin) <= 6))
                        {
                            //create a new qeffect with ephemeral expiration
                            QEffect listener = new(ExpirationCondition.Ephemeral);
                            configure(listener);
                            creature.AddQEffect(listener);
                        }
                    };
                };
            });

        //adds the Hobgoblin Ancestry Feat "Cantorian Rejuvenation"
        yield return new HobgoblinAncestryFeat("Squad Tactics",
            "You are adept at working with your allies to surround an enemy.",
            "If an enemy is within reach of you and at least two of your allies, that enemy is off-guard to you.", 9)
            .WithPermanentQEffect("If an enemy is within reach of you and at least two of your allies, that enemy is off-guard to you.",
            qEffectFeat =>
            {

                //get hobgoblin
                Creature hobgoblin = qEffectFeat.Owner;

                qEffectFeat.StateCheck = delegate (QEffect qEffect)
                {
                    if (hobgoblin.PrimaryWeapon == null || !hobgoblin.PrimaryWeapon.HasTrait(Trait.Melee))
                    {
                        return;
                    }

                    //each enemy
                    foreach (Tile tile in hobgoblin.FindThreatenedSquares())
                    {
                        if (tile.PrimaryOccupant == null)
                        {
                            continue;
                        }

                        Creature target = tile.PrimaryOccupant;

                        if (hobgoblin == null || !hobgoblin.EnemyOf(target))
                        {
                            continue;
                        }

                        //get current battle
                        TBattle CurrentBattle = hobgoblin.Battle;
                        int allies = 0;

                        //each ally
                        foreach (Creature ally in CurrentBattle.AllCreatures.Where(creature => !hobgoblin.EnemyOf(creature)))
                        {
                            if (ally.PrimaryWeapon == null || ally == hobgoblin)
                            {
                                continue;
                            }

                            //set variables
                            Item weapon = ally.PrimaryWeapon;

                            if (weapon.DetermineReach(ally) >= ally.DistanceTo(target))
                            {
                                //increment ally count
                                allies++;

                                if (allies >= 2)
                                {
                                    target.AddQEffect(new QEffect("Squad Tactics", "You're flat-footed to " + hobgoblin?.ToString() + ".", ExpirationCondition.Ephemeral, hobgoblin, IllustrationName.Flatfooted)
                                    {
                                        DoNotShowUpOverhead = true,
                                        IsFlatFootedTo = (_, attacker, attack) => (attacker != hobgoblin) ? null : "Squad Tactics"
                                    });

                                    break;
                                }
                            }
                        }
                    };
                };
            });
    }
}