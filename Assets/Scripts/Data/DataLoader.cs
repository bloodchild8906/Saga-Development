using System.Collections.Generic;
using UnityEngine;

namespace TcgEngine
{

    /// <summary>
    /// This script initiates loading all the game data
    /// </summary>

    public class DataLoader : MonoBehaviour
    {
        public GameplayData data;
        public AssetData assets;

        private HashSet<string> _cardIds = new();
        private HashSet<string> _abilityIds = new();
        private HashSet<string> _deckIds = new();

        private static DataLoader _instance;

        void Awake()
        {
            _instance = this;
            LoadData();
        }

        public void LoadData()
        {
            //To make loading faster, add a path inside each Load() function, relative to Resources folder
            //For example CardData.Load("Cards");  to only load data inside the Resources/Cards folder
            CardData.Load();
            TeamData.Load();
            RarityData.Load();
            TraitData.Load();
            VariantData.Load();
            PackData.Load();
            LevelData.Load();
            DeckData.Load();
            AbilityData.Load();
            StatusData.Load();
            AvatarData.Load();
            CardbackData.Load();
            RewardData.Load();

            CheckCardData();
            CheckAbilityData();
            CheckDeckData();
            CheckVariantData();
        }

        //Make sure the data is valid
        private void CheckCardData()
        {
            _cardIds.Clear();
            foreach (var card in CardData.GetAll())
            {
                if (string.IsNullOrEmpty(card.id))
                    Debug.LogError(card.name + " id is empty");
                if (_cardIds.Contains(card.id))
                    Debug.LogError("Dupplicate Card ID: " + card.id);

                if (card.team == null)
                    Debug.LogError(card.id + " team is null");
                if (card.rarity == null)
                    Debug.LogError(card.id + " rarity is null");

                foreach (var trait in card.traits)
                {
                    if (trait == null)
                        Debug.LogError(card.id + " has null trait");
                }

                if (card.stats != null)
                {
                    foreach (var stat in card.stats)
                    {
                        if (stat.trait == null)
                            Debug.LogError(card.id + " has null stat trait");
                    }
                }

                foreach (var ability in card.abilities)
                {
                    if(ability == null)
                        Debug.LogError(card.id + " has null ability");
                }

                _cardIds.Add(card.id);
            }
        }

        //Make sure the data is valid
        private void CheckAbilityData()
        {
            _abilityIds.Clear();
            foreach (var ability in AbilityData.GetAll())
            {
                if (string.IsNullOrEmpty(ability.id))
                    Debug.LogError(ability.name + " id is empty");
                if (_abilityIds.Contains(ability.id))
                    Debug.LogError("Dupplicate Ability ID: " + ability.id);

                foreach (var chain in ability.chainAbilities)
                {
                    if (chain == null)
                        Debug.LogError(ability.id + " has null chain ability");
                }

                _abilityIds.Add(ability.id);
            }
        }

        //Make sure the data is valid
        private void CheckDeckData()
        {
            var gdata = GameplayData.Get();
            CheckDeckArray(gdata.aiDecks);
            CheckDeckArray(gdata.freeDecks);
            CheckDeckArray(gdata.starterDecks);

            if(gdata.testDeck == null || gdata.testDeckAI == null)
                Debug.Log("Deck is null in Resources/GameplayData");

            _deckIds.Clear();
            foreach (var deck in DeckData.GetAll())
            {
                if (string.IsNullOrEmpty(deck.id))
                    Debug.LogError(deck.name + " id is empty");
                if (_deckIds.Contains(deck.id))
                    Debug.LogError("Dupplicate Deck ID: " + deck.id);

                foreach (var card in deck.cards)
                {
                    if (card == null)
                        Debug.LogError(deck.id + " has null card");
                }

                _deckIds.Add(deck.id);
            }
        }

        private void CheckDeckArray(DeckData[] decks)
        {
            foreach (var deck in decks)
            {
                if (deck == null)
                    Debug.Log("Deck is null in Resources/GameplayData");
            }
        }

        private void CheckVariantData()
        {
            var dvariant = VariantData.GetDefault();
            if(dvariant == null)
                Debug.LogError("No default variant data found, make sure you have a default VariantData");
        }

        public static DataLoader Get()
        {
            return _instance;
        }
    }
}