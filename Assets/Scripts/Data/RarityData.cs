using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all rarities data (common, uncommon, rare, mythic)
    /// </summary>

    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [CreateAssetMenu(fileName = "RarityData", menuName = "TcgEngine/RarityData", order = 1)]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    
    public class RarityData : ScriptableObject
    {
        public string id;
        public string title;
        public Sprite icon;
        public int rank;        //Index of the rarity, should start at 1 (common) and increase sequentially

        public static List<RarityData> RarityList = new();

        public static void Load(string folder = "")
        {
            if (RarityList.Count == 0)
                RarityList.AddRange(Resources.LoadAll<RarityData>(folder));
        }

        public static RarityData GetFirst()
        {
            var lowest = 99999;
            RarityData first = null;
            foreach (var rarity in GetAll())
            {
                if (rarity.rank < lowest)
                {
                    first = rarity;
                    lowest = rarity.rank;
                }
            }
            return first;
        }

        public static RarityData Get(string id)
        {
            foreach (var rarity in GetAll())
            {
                if (rarity.id == id)
                    return rarity;
            }
            return null;
        }

        public static List<RarityData> GetAll()
        {
            return RarityList;
        }
    }
}