using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    public enum CardType
    {
        None = 0,
        Hero = 5,
        Character = 10,
        Spell = 20,
        Artifact = 30,
        Secret = 40,
        Equipment = 50,
    }

    /// <summary>
    /// Defines all card data 
    /// </summary>
    [CreateAssetMenu(fileName = "card", menuName = "TcgEngine/CardData", order = 5)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    public class CardData : ScriptableObject
    {
        [HorizontalGroup("Info", Width = 325)] [VerticalGroup("Info/Side1", Order = 2), LabelWidth(50)]
        public string id;

        [VerticalGroup("Info/Side1"), LabelWidth(50)]
        public string title;

        [FormerlySerializedAs("art_full")] [VerticalGroup("Info/Side2"), PreviewField(100), HideLabel,]
        public Sprite artFull;

        [FormerlySerializedAs("art_board")] [VerticalGroup("Info/Side2"), PreviewField(100), HideLabel,]
        public Sprite artBoard;

        [VerticalGroup("Info/Side1"), LabelWidth(50)]
        public CardType type;

        [VerticalGroup("Info/Side1"), LabelWidth(50)]
        [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
        public TeamData team;

        [VerticalGroup("Info/Side1"), LabelWidth(50)]
        [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
        public RarityData rarity;

        [HorizontalGroup("Info/Side1/Stats"), LabelWidth(30), LabelText(SdfIconType.DiamondFill, Text = "")]
        public int mana;

        [HorizontalGroup("Info/Side1/Stats"), LabelWidth(30), LabelText(SdfIconType.Hammer, Text = "")]
        public int attack;

        [HorizontalGroup("Info/Side1/Stats"), LabelWidth(30), LabelText(SdfIconType.DropletFill, Text = "")]
        public int hp;

        [TabGroup("Traits", Icon = SdfIconType.Speedometer), ListDrawerSettings]
        public TraitData[] traits;

        [TabGroup("Traits", Icon = SdfIconType.Speedometer)]
        public TraitStat[] stats;

        [TabGroup("Abilities", Icon = SdfIconType.Magic), ListDrawerSettings]
        public AbilityData[] abilities;

        [VerticalGroup("Info/Side1"), LabelWidth(50)] [TextArea(3, 5)]
        public string text;

        [VerticalGroup("Info/Side1"), LabelWidth(50)] [TextArea(5, 10)]
        public string desc;

        [FormerlySerializedAs("spawn_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject spawnFX;

        [FormerlySerializedAs("death_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject deathFX;

        [FormerlySerializedAs("attack_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject attackFX;

        [FormerlySerializedAs("damage_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject damageFX;

        [FormerlySerializedAs("idle_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject idleFX;

        [FormerlySerializedAs("spawn_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip spawnAudio;

        [FormerlySerializedAs("death_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip deathAudio;

        [FormerlySerializedAs("attack_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip attackAudio;

        [FormerlySerializedAs("damage_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip damageAudio;

        [TabGroup("FX", Icon = SdfIconType.Stars)] [TabGroup("Availability", Icon = SdfIconType.Basket3Fill)]
        public bool deckbuilding = false;

        [TabGroup("Availability", Icon = SdfIconType.Basket3Fill)]
        public int cost = 100;

        [TabGroup("Availability", Icon = SdfIconType.Basket3Fill)]
        public PackData[] packs;


        public static List<CardData> CardsListAvailable = new(); //Faster access in loops

        public static Dictionary<string, CardData> CardsAvailable = new(); //Faster access in Get(id)

        public static void Load(string folder = "")
        {
            if (CardsListAvailable.Count != 0) return;
            CardsListAvailable.AddRange(Resources.LoadAll<CardData>(folder));

            foreach (var card in CardsListAvailable)
                CardsAvailable.Add(card.id, card);
        }

        public Sprite GetBoardArt(VariantData variant) => artBoard;

        public Sprite GetFullArt(VariantData variant) => artFull;

        public string GetTitle() => title;

        public string GetText() => text;

        public string GetDesc() => desc;

        public string GetTypeId()
        {
            return type switch
            {
                CardType.Hero => "hero",
                CardType.Character => "character",
                CardType.Artifact => "artifact",
                CardType.Spell => "spell",
                CardType.Secret => "secret",
                CardType.Equipment => "equipment",
                _ => ""
            };
        }

        public string GetAbilitiesDesc() => abilities.Where(ability => !string.IsNullOrWhiteSpace(ability.desc))
            .Aggregate("",
                (current, ability) => current + ("<b>" + ability.GetTitle() + ":</b> " + ability.GetDesc(this) + "\n"));

        public bool IsCharacter() => type == CardType.Character;

        public bool IsSecret() => type == CardType.Secret;

        public bool IsBoardCard() => type == CardType.Character || type == CardType.Artifact;

        public bool IsRequireTarget() => type == CardType.Equipment || IsRequireTargetSpell();

        public bool IsRequireTargetSpell() =>
            type == CardType.Spell && HasAbility(AbilityTrigger.OnPlay, AbilityTarget.PlayTarget);

        public bool IsEquipment() => type == CardType.Equipment;

        public bool IsDynamicManaCost() => mana > 99;

        public bool HasTrait(string trait) => traits.Any(t => t.id == trait);

        public bool HasTrait(TraitData trait) => trait != null && HasTrait(trait.id);

        public bool HasStat(string trait) => stats != null && stats.Any(stat => stat.trait.id == trait);

        public bool HasStat(TraitData trait) => trait != null && HasStat(trait.id);

        public int GetStat(string traitID) => stats == null
            ? 0
            : (from stat in stats where stat.trait.id == traitID select stat.value).FirstOrDefault();

        public int GetStat(TraitData trait) => trait != null ? GetStat(trait.id) : 0;

        public bool HasAbility(AbilityData ability) =>
            abilities.Any(abilityData => abilityData && abilityData.id == ability.id);

        public bool HasAbility(AbilityTrigger trigger) =>
            abilities.Any(ability => ability && ability.trigger == trigger);

        public bool HasAbility(AbilityTrigger trigger, AbilityTarget target) => abilities.Any(ability =>
            ability && ability.trigger == trigger && ability.target == target);

        public AbilityData GetAbility(AbilityTrigger trigger) =>
            abilities.FirstOrDefault(ability => ability && ability.trigger == trigger);

        public bool HasPack(PackData pack) => packs.Any(apack => apack == pack);

        public static CardData Get(string id)
        {
            if (id == null)
                return null;
            var success = CardsAvailable.TryGetValue(id, out var card);
            return success ? card : null;
        }

        public static List<CardData> GetAllDeckBuilding()
        {
            if (CardsListAvailable.Count == 0)
                Load();
            return GetAll().Where(acard => acard.deckbuilding).ToList();
        }

        public static List<CardData> GetAll(PackData pack) => GetAll().Where(aCard => aCard.HasPack(pack)).ToList();

        public static List<CardData> GetAll() => CardsListAvailable;
    }
}