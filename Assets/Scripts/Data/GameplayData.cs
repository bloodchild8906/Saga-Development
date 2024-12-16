using UnityEngine;
using TcgEngine.AI;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Generic gameplay settings, such as starting stats, decks limit, scenes, and ai level
    /// </summary>

    [CreateAssetMenu(fileName = "GameplayData", menuName = "TcgEngine/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [FormerlySerializedAs("hp_start")] [Header("Gameplay")]
        public int hpStart = 20;
        [FormerlySerializedAs("mana_start")] public int manaStart = 1;
        [FormerlySerializedAs("mana_per_turn")] public int manaPerTurn = 1;
        [FormerlySerializedAs("mana_max")] public int manaMax = 10;
        [FormerlySerializedAs("cards_start")] public int cardsStart = 5;
        [FormerlySerializedAs("cards_per_turn")] public int cardsPerTurn = 1;
        [FormerlySerializedAs("cards_max")] public int cardsMax = 10;
        [FormerlySerializedAs("turn_duration")] public float turnDuration = 30f;
        [FormerlySerializedAs("second_bonus")] public CardData secondBonus;

        [FormerlySerializedAs("deck_size")] [Header("Deckbuilding")]
        public int deckSize = 30;
        [FormerlySerializedAs("deck_duplicate_max")] public int deckDuplicateMax = 2;

        [FormerlySerializedAs("sell_ratio")] [Header("Buy/Sell")]
        public float sellRatio = 0.8f;

        [FormerlySerializedAs("ai_type")] [Header("AI")]
        public AIType aiType;              //AI algorythm
        [FormerlySerializedAs("ai_level")] public int aiLevel = 10;           //AI level, 10=best, 1=weakest

        [FormerlySerializedAs("free_decks")] [Header("Decks")]
        public DeckData[] freeDecks;       //These decks are always available in menu, useful for tests
        [FormerlySerializedAs("starter_decks")] public DeckData[] starterDecks;    //When API is enabled, each player can select ONE of those
        [FormerlySerializedAs("ai_decks")] public DeckData[] aiDecks;         //When player solo, AI will pick one of these at random

        [FormerlySerializedAs("arena_list")] [Header("Scenes")]
        public string[] arenaList;         //List of game scenes

        [FormerlySerializedAs("test_deck")] [Header("Test")]
        public DeckData testDeck;          //For when starting the game directly from Unity game scene
        [FormerlySerializedAs("test_deck_ai")] public DeckData testDeckAI;       //For when starting the game directly from Unity game scene
        [FormerlySerializedAs("ai_vs_ai")] public bool aiVSAI;

        public int GetPlayerLevel(int xp)
        {
            return Mathf.FloorToInt(xp / 1000f) + 1;
        }

        public string GetRandomArena()
        {
            if (arenaList.Length > 0)
                return arenaList[Random.Range(0, arenaList.Length)];
            return "Game";
        }

        public string GetRandomAIDeck()
        {
            if (aiDecks.Length > 0)
                return aiDecks[Random.Range(0, aiDecks.Length)].id;
            return "";
        }

        public static GameplayData Get()
        {
            return DataLoader.Get().data;
        }
    }
}