using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Define reward to upload easily on the api
    /// </summary>

    [CreateAssetMenu(fileName = "RewardData", menuName = "TcgEngine/RewardData", order = 5)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    
    public class RewardData : ScriptableObject
    {
        public string id;
        public string group;
        public int coins;
        public int xp;

        public PackData[] packs;
        public CardData[] cards;
        public DeckData[] decks;

        public bool repeat = true;

        public static List<RewardData> RewardList = new();

        public static void Load(string folder = "")
        {
            if (RewardList.Count == 0)
                RewardList.AddRange(Resources.LoadAll<RewardData>(folder));
        }

        public static RewardData Get(string id)
        {
            foreach (var reward in GetAll())
            {
                if (reward.id == id)
                    return reward;
            }
            return null;
        }

        public static List<RewardData> GetAll()
        {
            return RewardList;
        }
    }

}