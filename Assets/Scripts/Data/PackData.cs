using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Defines all packs data
    /// </summary>

    [CreateAssetMenu(fileName = "PackData", menuName = "TcgEngine/PackData", order = 5)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    
    public class PackData : ScriptableObject
    {
        public string id;

        [Header("Content")]
        public PackType type;
        public int cards = 5;   //Cards per pack
        [FormerlySerializedAs("rarities_1st")] public PackRarity[] rarities1St;  //Probability of each rarity, for first card
        public PackRarity[] rarities;      //Probability of each rarity, for other cards
        public PackVariant[] variants;      //Probability of each variant, for other cards

        [Header("Display")]
        public string title;
        [FormerlySerializedAs("pack_img")] public Sprite packImg;
        [FormerlySerializedAs("cardback_img")] public Sprite cardbackImg;
        [TextArea(5, 10)]
        public string desc;
        [FormerlySerializedAs("sort_order")] public int sortOrder;

        [Header("Availability")]
        public bool available = true;
        public int cost = 100;  //Cost to buy

        public static List<PackData> PackList = new();

        public static void Load(string folder = "")
        {
            if (PackList.Count == 0)
                PackList.AddRange(Resources.LoadAll<PackData>(folder));

            PackList.Sort((PackData a, PackData b) => {
                if (a.sortOrder == b.sortOrder)
                    return a.id.CompareTo(b.id);
                else
                    return a.sortOrder.CompareTo(b.sortOrder);
            });
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetDesc()
        {
            return desc;
        }

        public static PackData Get(string id)
        {
            foreach (var pack in GetAll())
            {
                if (pack.id == id)
                    return pack;
            }
            return null;
        }

        public static List<PackData> GetAllAvailable()
        {
            var validList = new List<PackData>();
            foreach (var apack in GetAll())
            {
                if (apack.available)
                    validList.Add(apack);
            }
            return validList;
        }

        public static List<PackData> GetAll()
        {
            return PackList;
        }
    }

    public enum PackType
    {
        Random = 0,
        Fixed = 10,
    }

    [System.Serializable]
    public struct PackRarity
    {
        public RarityData rarity;
        public int probability;
    }

    [System.Serializable]
    public struct PackVariant
    {
        public VariantData variant;
        public int probability;
    }
}