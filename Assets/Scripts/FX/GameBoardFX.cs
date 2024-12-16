﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TcgEngine.Client;
using TcgEngine.UI;

namespace TcgEngine.FX
{
    /// <summary>
    /// FX that are not related to any card/player, and appear in the middle of the board
    /// Usually when big abilities are played
    /// </summary>

    public class GameBoardFX : MonoBehaviour
    {
        void Start()
        {
            GameClient client = GameClient.Get();
            client.onNewTurn += OnNewTurn;
            client.onCardPlayed += OnPlayCard;
            client.onAbilityStart += OnAbility;
            client.onSecretTrigger += OnSecret;
            client.onValueRolled += OnRoll;
        }

        void OnNewTurn(int player_id)
        {
            AudioTool.Get().PlaySFX("turn", AssetData.Get().newTurnAudio);
            FXTool.DoFX(AssetData.Get().newTurnFX, Vector3.zero);
        }

        void OnPlayCard(Card card, Slot slot)
        {
            int player_id = GameClient.Get().GetPlayerID();
            if (card != null)
            {
                CardData icard = CardData.Get(card.card_id);
                if (icard.type == CardType.Spell)
                {
                    GameObject prefab = player_id == card.player_id ? AssetData.Get().playCardFX : AssetData.Get().playCardOtherFX;
                    GameObject obj = FXTool.DoFX(prefab, Vector3.zero);
                    CardUI ui = obj.GetComponentInChildren<CardUI>();
                    ui.SetCard(icard, card.VariantData);

                    AudioClip spawn_audio = icard.spawnAudio != null ? icard.spawnAudio : AssetData.Get().cardSpawnAudio;
                    AudioTool.Get().PlaySFX("card_spell", spawn_audio);
                }

                if (icard.type == CardType.Secret)
                {
                    GameObject sprefab = player_id == card.player_id ? AssetData.Get().playSecretFX : AssetData.Get().playSecretOtherFX;
                    FXTool.DoFX(sprefab, Vector3.zero);

                    AudioClip spawn_audio = icard.spawnAudio != null ? icard.spawnAudio : AssetData.Get().cardSpawnAudio;
                    AudioTool.Get().PlaySFX("card_spell", spawn_audio);
                }
            }
        }

        private void OnAbility(AbilityData iability, Card caster)
        {
            if (iability != null)
            {
                FXTool.DoFX(iability.boardFX, Vector3.zero);
            }
        }

        private void OnSecret(Card secret, Card triggerer)
        {
            CardData icard = CardData.Get(secret.card_id);
            if (icard?.attackAudio != null)
                AudioTool.Get().PlaySFX("card_secret", icard.attackAudio);
        }

        private void OnRoll(int value)
        {
            GameObject fx = FXTool.DoFX(AssetData.Get().diceRollFX, Vector3.zero);
            DiceRollFX dice = fx?.GetComponent<DiceRollFX>();
            if (dice != null)
            {
                dice.value = value;
            }
        }

    }
}