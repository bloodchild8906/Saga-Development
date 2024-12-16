using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Deck with more fields for having specific cards or starting board cards
    /// </summary>

    [System.Serializable]
    public class DeckCardSlot
    {
        public CardData card;
        public SlotXY slot;
    }

    [CreateAssetMenu(fileName = "DeckPuzzleData", menuName = "TcgEngine/DeckPuzzleData", order = 7)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    public class DeckPuzzleData : DeckData
    {
        [FormerlySerializedAs("board_cards")] public DeckCardSlot[] boardCards;
        [FormerlySerializedAs("start_cards")] public int startCards = 5;
        [FormerlySerializedAs("start_mana")] public int startMana = 2;
        [FormerlySerializedAs("start_hp")] public int startHp = 20;
        [FormerlySerializedAs("dont_shuffle_deck")] public bool dontShuffleDeck;

        public static new DeckPuzzleData Get(string id)
        {
            foreach (var deck in GetAll())
            {
                if (deck.id == id && deck is DeckPuzzleData)
                    return (DeckPuzzleData) deck;
            }
            return null;
        }
    }
}
