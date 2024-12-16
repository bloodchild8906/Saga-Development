using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    //Default fx and audio, some can be overrided on each individual card

    [CreateAssetMenu(fileName = "AssetData", menuName = "TcgEngine/AssetData", order = 0)]
    public class AssetData : ScriptableObject
    {
        [FormerlySerializedAs("card_spawn_fx")] [Header("FX")]
        public GameObject cardSpawnFX;
        [FormerlySerializedAs("card_destroy_fx")] public GameObject cardDestroyFX;
        [FormerlySerializedAs("card_attack_fx")] public GameObject cardAttackFX;
        [FormerlySerializedAs("card_damage_fx")] public GameObject cardDamageFX;
        [FormerlySerializedAs("card_exhausted_fx")] public GameObject cardExhaustedFX;
        [FormerlySerializedAs("player_damage_fx")] public GameObject playerDamageFX;
        [FormerlySerializedAs("damage_fx")] public GameObject damageFX;
        [FormerlySerializedAs("play_card_fx")] public GameObject playCardFX;
        [FormerlySerializedAs("play_card_other_fx")] public GameObject playCardOtherFX;
        [FormerlySerializedAs("play_secret_fx")] public GameObject playSecretFX;
        [FormerlySerializedAs("play_secret_other_fx")] public GameObject playSecretOtherFX;
        [FormerlySerializedAs("dice_roll_fx")] public GameObject diceRollFX;
        [FormerlySerializedAs("hover_text_box")] public GameObject hoverTextBox;
        [FormerlySerializedAs("new_turn_fx")] public GameObject newTurnFX;
        [FormerlySerializedAs("win_fx")] public GameObject winFX;
        [FormerlySerializedAs("lose_fx")] public GameObject loseFX;
        [FormerlySerializedAs("tied_fx")] public GameObject tiedFX;

        [FormerlySerializedAs("card_spawn_audio")] [Header("Audio")]
        public AudioClip cardSpawnAudio;
        [FormerlySerializedAs("card_destroy_audio")] public AudioClip cardDestroyAudio;
        [FormerlySerializedAs("card_attack_audio")] public AudioClip cardAttackAudio;
        [FormerlySerializedAs("card_move_audio")] public AudioClip cardMoveAudio;
        [FormerlySerializedAs("card_damage_audio")] public AudioClip cardDamageAudio;
        [FormerlySerializedAs("player_damage_audio")] public AudioClip playerDamageAudio;
        [FormerlySerializedAs("hand_card_click_audio")] public AudioClip handCardClickAudio;
        [FormerlySerializedAs("new_turn_audio")] public AudioClip newTurnAudio;
        [FormerlySerializedAs("win_audio")] public AudioClip winAudio;
        [FormerlySerializedAs("defeat_audio")] public AudioClip defeatAudio;
        [FormerlySerializedAs("win_music")] public AudioClip winMusic;
        [FormerlySerializedAs("defeat_music")] public AudioClip defeatMusic;

        public static AssetData Get()
        {
            return DataLoader.Get().assets;
        }
    }
}
