using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{

    [CreateAssetMenu(fileName = "LevelData", menuName = "TcgEngine/LevelData", order = 7)]
    public class LevelData : ScriptableObject
    {
        public string id;
        public int level;

        [Header("Display")]
        public string title;

        [Header("Gameplay")]
        public string scene;
        [FormerlySerializedAs("player_deck")] public DeckData playerDeck;
        [FormerlySerializedAs("ai_deck")] public DeckData aiDeck;
        [FormerlySerializedAs("ai_level")] public int aiLevel = 10; //From 1 to 10
        [FormerlySerializedAs("first_player")] public LevelFirst firstPlayer;

        [FormerlySerializedAs("reward_xp")] [Header("Rewards")]
        public int rewardXp = 100;
        [FormerlySerializedAs("reward_coins")] public int rewardCoins = 100;
        [FormerlySerializedAs("reward_packs")] public PackData[] rewardPacks;
        [FormerlySerializedAs("reward_cards")] public CardData[] rewardCards;
        [FormerlySerializedAs("reward_decks")] public DeckData[] rewardDecks;

        public static List<LevelData> LevelList = new();

        public static void Load(string folder = "")
        {
            if (LevelList.Count == 0)
            {
                LevelList.AddRange(Resources.LoadAll<LevelData>(folder));
                LevelList.Sort((LevelData a, LevelData b) => { return a.level.CompareTo(b.level); });
            }
        }

        public string GetTitle()
        {
            return title;
        }

        public static LevelData Get(string id)
        {
            foreach (var level in GetAll())
            {
                if (level.id == id)
                    return level;
            }
            return null;
        }

        public static List<LevelData> GetAll()
        {
            return LevelList;
        }
    }

    public enum LevelFirst
    {
        Random = 0,
        Player = 10,
        AI = 20,
    }
}