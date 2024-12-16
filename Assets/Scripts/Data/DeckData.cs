using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all fixed deck data (for user custom decks, check UserData.cs)
    /// </summary>
    
    [CreateAssetMenu(fileName = "DeckData", menuName = "TcgEngine/DeckData", order = 7)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    public class DeckData : ScriptableObject
    {
        public string id;

        [Header("Display")]
        public string title;

        [Header("Cards")]
        public CardData hero;
        public CardData[] cards;

        public static List<DeckData> DeckList = new();

        public static void Load(string folder = "")
        {
            if(DeckList.Count == 0)
                DeckList.AddRange(Resources.LoadAll<DeckData>(folder));
        }

        public int GetQuantity()
        {
            return cards.Length;
        }

        public bool IsValid()
        {
            return cards.Length >= GameplayData.Get().deckSize;
        }

        public static DeckData Get(string id)
        {
            foreach (var deck in GetAll())
            {
                if (deck.id == id)
                    return deck;
            }
            return null;
        }

        public static List<DeckData> GetAll()
        {
            return DeckList;
        }
    }
}