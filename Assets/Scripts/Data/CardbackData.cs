using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Defines all cardback data
    /// </summary>

    [CreateAssetMenu(fileName = "Cardback", menuName = "TcgEngine/Cardback", order = 10)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    public class CardbackData : ScriptableObject
    {
        public string id;
        public Sprite cardback;
        public Sprite deck;
        [FormerlySerializedAs("sort_order")] public int sortOrder;

        public static List<CardbackData> CardbackList = new();

        public static void Load(string folder = "")
        {
            if (CardbackList.Count == 0)
                CardbackList.AddRange(Resources.LoadAll<CardbackData>(folder));

            CardbackList.Sort((CardbackData a, CardbackData b) => {
                if (a.sortOrder == b.sortOrder)
                    return a.id.CompareTo(b.id);
                else
                    return a.sortOrder.CompareTo(b.sortOrder);
            });
        }

        public static CardbackData Get(string id)
        {
            foreach (var cardback in GetAll())
            {
                if (cardback.id == id)
                    return cardback;
            }
            return null;
        }

        public static List<CardbackData> GetAll()
        {
            return CardbackList;
        }
    }
}
