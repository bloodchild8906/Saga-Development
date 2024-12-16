using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using TcgEngine.Gameplay;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Defines all ability data
    /// </summary>
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "",
        ShowIf = "@CardEditor.ShowNotNull($value)"), InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    public class AbilityData : ScriptableObject
    {
        public string id;

        [TabGroup("Trigger", Icon = SdfIconType.Alarm), EnumToggleButtons, HideLabel]
        public AbilityTrigger trigger; //WHEN does the ability trigger?

        [FormerlySerializedAs("conditions_trigger")] [TabGroup("Trigger", Icon = SdfIconType.Alarm), ListDrawerSettings]
        public ConditionData[]
            conditionsTrigger; //Condition checked on the card triggering the ability (usually the caster)

        [TabGroup("Target", Icon = SdfIconType.Capslock), EnumToggleButtons, HideLabel]
        public AbilityTarget target; //WHO is targeted?

        [FormerlySerializedAs("conditions_target")]
        [TabGroup("Target", Icon = SdfIconType.Capslock), ListDrawerSettings]
        public ConditionData[] conditionsTarget; //Condition checked on the target to know if its a valid taget

        [FormerlySerializedAs("filters_target")] [TabGroup("Target", Icon = SdfIconType.Capslock), ListDrawerSettings]
        public FilterData[] filtersTarget; //Condition checked on the target to know if its a valid taget

        [TabGroup("Effect", Icon = SdfIconType.Lightning), ListDrawerSettings]
        public EffectData[] effects; //WHAT this does?

        [TabGroup("Effect", Icon = SdfIconType.Lightning), ListDrawerSettings]
        public StatusData[] status; //Status added by this ability  

        [TabGroup("Effect", Icon = SdfIconType.Lightning)]
        public int value; //Value passed to the effect (deal X damage)

        [TabGroup("Effect", Icon = SdfIconType.Lightning)]
        public int duration; //Duration passed to the effect (usually for status, 0=permanent)

        [FormerlySerializedAs("chain_abilities")]
        [TabGroup("Chain or Choices", Icon = SdfIconType.SignpostSplit), ListDrawerSettings]
        public AbilityData[] chainAbilities; //Abilities that will be triggered after this one

        [FormerlySerializedAs("mana_cost")] [Header("Activated Ability")]
        public int manaCost; //Mana cost for  activated abilities

        public bool exhaust; //Action cost for activated abilities

        [FormerlySerializedAs("board_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject boardFX;

        [FormerlySerializedAs("caster_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject casterFX;

        [FormerlySerializedAs("target_fx")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public GameObject targetFX;

        [FormerlySerializedAs("cast_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip castAudio;

        [FormerlySerializedAs("target_audio")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public AudioClip targetAudio;

        [FormerlySerializedAs("charge_target")] [TabGroup("FX", Icon = SdfIconType.Stars)]
        public bool chargeTarget;

        [Header("Text")] public string title;
        [TextArea(5, 7)] public string desc;

        [TabGroup("FX", Icon = SdfIconType.Stars), FormerlySerializedAs("projectile_fx")]
        public GameObject projectileFX;

        public static List<AbilityData> AbilityList = new(); //Faster access in loops
        public static Dictionary<string, AbilityData> AbilityDict = new(); //Faster access in Get(id)

        public static void Load(string folder = "")
        {
            if (AbilityList.Count != 0) return;
            AbilityList.AddRange(Resources.LoadAll<AbilityData>(folder));

            foreach (var ability in AbilityList)
                AbilityDict.Add(ability.id, ability);
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetDesc()
        {
            return desc;
        }

        public string GetDesc(CardData card)
        {
            var dsc = desc;
            dsc = dsc.Replace("<name>", card.title);
            dsc = dsc.Replace("<value>", value.ToString());
            dsc = dsc.Replace("<duration>", duration.ToString());
            return dsc;
        }

        //Generic condition for the ability to trigger
        public bool AreTriggerConditionsMet(Game data, Card caster) =>
            AreTriggerConditionsMet(data, caster, caster); //Triggerer is the caster

        //Some abilities are caused by another card (PlayOther), otherwise most of the time the triggerer is the caster, check condition on triggerer
        public bool AreTriggerConditionsMet(Game data, Card caster, Card triggerCard)
        {
            foreach (var cond in conditionsTrigger)
            {
                if (cond == null) continue;
                if (!cond.IsTriggerConditionMet(data, this, caster))
                    return false;
                if (!cond.IsTargetConditionMet(data, this, caster, triggerCard))
                    return false;
            }

            return true;
        }

        //Some abilities are caused by an action on a player (OnFight when attacking the player), check condition on that player
        public bool AreTriggerConditionsMet(Game data, Card caster, Player triggerPlayer)
        {
            foreach (var cond in conditionsTrigger)
            {
                if (cond == null) continue;
                if (!cond.IsTriggerConditionMet(data, this, caster))
                    return false;
                if (!cond.IsTargetConditionMet(data, this, caster, triggerPlayer))
                    return false;
            }

            return true;
        }

        //Check if the card target is valid
        public bool AreTargetConditionsMet(Game data, Card caster, Card targetCard)
        {
            return conditionsTarget.All(cond =>
                cond == null || cond.IsTargetConditionMet(data, this, caster, targetCard));
        }

        //Check if the player target is valid
        public bool AreTargetConditionsMet(Game data, Card caster, Player targetPlayer)
        {
            return conditionsTarget.All(cond =>
                cond == null || cond.IsTargetConditionMet(data, this, caster, targetPlayer));
        }

        //Check if the slot target is valid
        public bool AreTargetConditionsMet(Game data, Card caster, Slot targetSlot)
        {
            return conditionsTarget.All(cond =>
                cond == null || cond.IsTargetConditionMet(data, this, caster, targetSlot));
        }

        //Check if the card data target is valid
        public bool AreTargetConditionsMet(Game data, Card caster, CardData targetCard)
        {
            return conditionsTarget.All(cond =>
                cond == null || cond.IsTargetConditionMet(data, this, caster, targetCard));
        }

        //CanTarget is similar to AreTargetConditionsMet but only applies to targets on the board, with extra board-only conditions
        public bool CanTarget(Game data, Card caster, Card target)
        {
            if (target.HasStatus(StatusType.Stealth))
                return false; //Hidden

            if (target.HasStatus(StatusType.SpellImmunity))
                return false; //Spell immunity

            var conditionMatch = AreTargetConditionsMet(data, caster, target);
            return conditionMatch;
        }

        //Can target check additional restrictions and is usually for SelectTarget or PlayTarget abilities
        public bool CanTarget(Game data, Card caster, Player targetSlot)
        {
            var conditionMatch = AreTargetConditionsMet(data, caster, targetSlot);
            return conditionMatch;
        }

        public bool CanTarget(Game data, Card caster, Slot targetSlot)
        {
            return AreTargetConditionsMet(data, caster, targetSlot); //No additional conditions for slots
        }

        //Check if destination array has the target after being filtered, used to support filters in CardSelector
        public bool IsCardSelectionValid(Game data, Card caster, Card targetCard, ListSwap<Card> cardArray = null)
        {
            var targets = GetCardTargets(data, caster, cardArray);
            return targets.Contains(targetCard); //Card is still in array after filtering
        }

        public void DoEffects(GameLogic logic, Card caster)
        {
            foreach (var effect in effects)
                effect?.DoEffect(logic, this, caster);
        }

        public void DoEffects(GameLogic logic, Card caster, Card target)
        {
            foreach (var effect in effects)
                effect?.DoEffect(logic, this, caster, target);
            foreach (var stat in status)
                target.AddStatus(stat, value, duration);
        }

        public void DoEffects(GameLogic logic, Card caster, Player target)
        {
            foreach (var effect in effects)
                effect?.DoEffect(logic, this, caster, target);
            foreach (var stat in status)
                target.AddStatus(stat, value, duration);
        }

        public void DoEffects(GameLogic logic, Card caster, Slot target)
        {
            foreach (var effect in effects)
                effect?.DoEffect(logic, this, caster, target);
        }

        public void DoEffects(GameLogic logic, Card caster, CardData target)
        {
            foreach (var effect in effects)
                effect?.DoEffect(logic, this, caster, target);
        }

        public void DoOngoingEffects(GameLogic logic, Card caster, Card target)
        {
            foreach (var effect in effects)
                effect?.DoOngoingEffect(logic, this, caster, target);
            foreach (var stat in status)
                target.AddOngoingStatus(stat, value);
        }

        public void DoOngoingEffects(GameLogic logic, Card caster, Player target)
        {
            foreach (var effect in effects)
                effect?.DoOngoingEffect(logic, this, caster, target);
            foreach (var stat in status)
                target.AddOngoingStatus(stat, value);
        }

        public bool HasEffect<T>() where T : EffectData
        {
            return effects.Any(eff => eff != null && eff is T);
        }

        public bool HasStatus(StatusType type)
        {
            return status.Any(sta => sta != null && sta.effect == type);
        }

        public int GetDamage()
        {
            return effects.Where(eff => eff != null && eff is EffectDamage).Sum(eff => this.value);
        }

        private void AddValidCards(Game data, Card caster, List<Card> source, List<Card> targets)
        {
            targets.AddRange(source.Where(card => AreTargetConditionsMet(data, caster, card)));
        }

        //Return cards targets,  memory_array is used for optimization and avoid allocating new memory
        public List<Card> GetCardTargets(Game data, Card caster, ListSwap<Card> memoryArray = null)
        {
            memoryArray ??= new ListSwap<Card>();

            var targets = memoryArray.Get();

            if (target == AbilityTarget.Self)
            {
                if (AreTargetConditionsMet(data, caster, caster))
                    targets.Add(caster);
            }

            if (target == AbilityTarget.AllCardsBoard || target == AbilityTarget.SelectTarget)
            {
                targets.AddRange(from player in data.players
                    from card in player.cards_board
                    where AreTargetConditionsMet(data, caster, card)
                    select card);
            }

            if (target == AbilityTarget.AllCardsHand)
            {
                targets.AddRange(from player in data.players
                    from card in player.cards_hand
                    where AreTargetConditionsMet(data, caster, card)
                    select card);
            }

            if (target == AbilityTarget.AllCardsAllPiles || target == AbilityTarget.CardSelector)
            {
                foreach (var player in data.players)
                {
                    AddValidCards(data, caster, player.cards_deck, targets);
                    AddValidCards(data, caster, player.cards_discard, targets);
                    AddValidCards(data, caster, player.cards_hand, targets);
                    AddValidCards(data, caster, player.cards_secret, targets);
                    AddValidCards(data, caster, player.cards_board, targets);
                    AddValidCards(data, caster, player.cards_equip, targets);
                    AddValidCards(data, caster, player.cards_temp, targets);
                }
            }

            if (target == AbilityTarget.LastPlayed)
            {
                var targetCard = data.GetCard(data.last_played);
                if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                    targets.Add(targetCard);
            }

            if (target == AbilityTarget.LastDestroyed)
            {
                var targetCard = data.GetCard(data.last_destroyed);
                if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                    targets.Add(targetCard);
            }

            if (target == AbilityTarget.LastTargeted)
            {
                var targetCard = data.GetCard(data.last_target);
                if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                    targets.Add(targetCard);
            }

            if (target == AbilityTarget.LastSummoned)
            {
                var targetCard = data.GetCard(data.last_summoned);
                if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                    targets.Add(targetCard);
            }

            if (target == AbilityTarget.AbilityTriggerer)
            {
                var targetCard = data.GetCard(data.ability_triggerer);
                if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                    targets.Add(targetCard);
            }

            if (target == AbilityTarget.EquippedCard)
            {
                if (caster.CardData.IsEquipment())
                {
                    //Get bearer of the equipment
                    var player = data.GetPlayer(caster.player_id);
                    var targetCard = player.GetBearerCard(caster);
                    if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                        targets.Add(targetCard);
                }
                else if (caster.equipped_uid != null)
                {
                    //Get equipped card
                    var targetCard = data.GetCard(caster.equipped_uid);
                    if (targetCard != null && AreTargetConditionsMet(data, caster, targetCard))
                        targets.Add(targetCard);
                }
            }

            //Filter targets
            if (filtersTarget == null || targets.Count <= 0) return targets;

            return filtersTarget.Where(filter => filter != null).Aggregate(targets,
                (current, filter) => filter.FilterTargets(data, this, caster, current, memoryArray.GetOther(current)));
        }

        //Return player targets,  memory_array is used for optimization and avoid allocating new memory
        public List<Player> GetPlayerTargets(Game data, Card caster, ListSwap<Player> memoryArray = null)
        {
            memoryArray ??= new ListSwap<Player>();

            var targets = memoryArray.Get();

            switch (target)
            {
                case AbilityTarget.PlayerSelf:
                {
                    var player = data.GetPlayer(caster.player_id);
                    targets.Add(player);
                    break;
                }
                case AbilityTarget.PlayerOpponent:
                {
                    targets.AddRange(data.players.Where((t, tp) => tp != caster.player_id));

                    break;
                }
                case AbilityTarget.AllPlayers:
                {
                    targets.AddRange(data.players.Where(player => AreTargetConditionsMet(data, caster, player)));

                    break;
                }
                case AbilityTarget.None:
                case AbilityTarget.Self:
                case AbilityTarget.AllCardsBoard:
                case AbilityTarget.AllCardsHand:
                case AbilityTarget.AllCardsAllPiles:
                case AbilityTarget.AllSlots:
                case AbilityTarget.AllCardData:
                case AbilityTarget.PlayTarget:
                case AbilityTarget.AbilityTriggerer:
                case AbilityTarget.EquippedCard:
                case AbilityTarget.SelectTarget:
                case AbilityTarget.CardSelector:
                case AbilityTarget.ChoiceSelector:
                case AbilityTarget.LastPlayed:
                case AbilityTarget.LastTargeted:
                case AbilityTarget.LastDestroyed:
                case AbilityTarget.LastSummoned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //Filter targets
            if (filtersTarget != null && targets.Count > 0)
            {
                targets = filtersTarget.Where(filter => filter != null).Aggregate(targets,
                    (current, filter) =>
                        filter.FilterTargets(data, this, caster, current, memoryArray.GetOther(current)));
            }

            return targets;
        }

        //Return slot targets,  memory_array is used for optimization and avoid allocating new memory
        public List<Slot> GetSlotTargets(Game data, Card caster, ListSwap<Slot> memoryArray = null)
        {
            memoryArray ??= new ListSwap<Slot>();

            var targets = memoryArray.Get();

            if (target == AbilityTarget.AllSlots)
            {
                var slots = Slot.GetAll();
                foreach (var slot in slots)
                {
                    if (AreTargetConditionsMet(data, caster, slot))
                        targets.Add(slot);
                }
            }

            //Filter targets
            if (filtersTarget != null && targets.Count > 0)
            {
                targets = filtersTarget.Where(filter => filter != null).Aggregate(targets,
                    (current, filter) =>
                        filter.FilterTargets(data, this, caster, current, memoryArray.GetOther(current)));
            }

            return targets;
        }

        public List<CardData> GetCardDataTargets(Game data, Card caster, ListSwap<CardData> memoryArray = null)
        {
            memoryArray ??= new ListSwap<CardData>();

            var targets = memoryArray.Get();

            if (target == AbilityTarget.AllCardData)
            {
                targets.AddRange(CardData.GetAll().Where(card => AreTargetConditionsMet(data, caster, card)));
            }

            //Filter targets
            if (filtersTarget == null || targets.Count <= 0) return targets;

            return filtersTarget.Where(filter => filter != null).Aggregate(targets,
                (current, filter) => filter.FilterTargets(data, this, caster, current, memoryArray.GetOther(current)));
        }

        // Check if there is any valid target, if not, AI wont try to cast activated ability
        public bool HasValidSelectTarget(Game gameData, Card caster)
        {
            switch (target)
            {
                case AbilityTarget.SelectTarget when HasValidBoardCardTarget(gameData, caster):
                case AbilityTarget.SelectTarget when HasValidPlayerTarget(gameData, caster):
                case AbilityTarget.SelectTarget when HasValidSlotTarget(gameData, caster):
                    return true;
                case AbilityTarget.SelectTarget:
                    return false;
                case AbilityTarget.CardSelector when HasValidCardTarget(gameData, caster):
                    return true;
                case AbilityTarget.CardSelector:
                    return false;
                case AbilityTarget.ChoiceSelector:
                {
                    return chainAbilities.Any(choice => choice.AreTriggerConditionsMet(gameData, caster));
                }
                case AbilityTarget.None:
                case AbilityTarget.Self:
                case AbilityTarget.PlayerSelf:
                case AbilityTarget.PlayerOpponent:
                case AbilityTarget.AllPlayers:
                case AbilityTarget.AllCardsBoard:
                case AbilityTarget.AllCardsHand:
                case AbilityTarget.AllCardsAllPiles:
                case AbilityTarget.AllSlots:
                case AbilityTarget.AllCardData:
                case AbilityTarget.PlayTarget:
                case AbilityTarget.AbilityTriggerer:
                case AbilityTarget.EquippedCard:
                case AbilityTarget.LastPlayed:
                case AbilityTarget.LastTargeted:
                case AbilityTarget.LastDestroyed:
                case AbilityTarget.LastSummoned:
                default:
                    return true; //Not selecting, valid
            }
        }

        public bool HasValidBoardCardTarget(Game gameData, Card caster)
        {
            return gameData.players.Any(player => player.cards_board.Any(card => CanTarget(gameData, caster, card)));
        }

        public bool HasValidCardTarget(Game gameData, Card caster)
        {
            return (from player in gameData.players
                let v1 = HasValidCardTarget(gameData, caster, player.cards_deck)
                let v2 = HasValidCardTarget(gameData, caster, player.cards_discard)
                let v3 = HasValidCardTarget(gameData, caster, player.cards_hand)
                let v4 = HasValidCardTarget(gameData, caster, player.cards_board)
                let v5 = HasValidCardTarget(gameData, caster, player.cards_equip)
                let v6 = HasValidCardTarget(gameData, caster, player.cards_secret)
                let v7 = HasValidCardTarget(gameData, caster, player.cards_temp)
                where v1 || v2 || v3 || v4 || v5 || v6 || v7
                select v1).Any();
        }

        public bool HasValidCardTarget(Game gameData, Card caster, List<Card> list)
        {
            for (var c = 0; c < list.Count; c++)
            {
                var card = list[c];
                if (AreTargetConditionsMet(gameData, caster, card))
                    return true;
            }

            return false;
        }

        public bool HasValidPlayerTarget(Game gameData, Card caster)
        {
            for (var p = 0; p < gameData.players.Length; p++)
            {
                var player = gameData.players[p];
                if (CanTarget(gameData, caster, player))
                    return true;
            }

            return false;
        }

        public bool HasValidSlotTarget(Game gameData, Card caster)
        {
            foreach (var slot in Slot.GetAll())
            {
                if (CanTarget(gameData, caster, slot))
                    return true;
            }

            return false;
        }

        public bool IsSelector()
        {
            return target == AbilityTarget.SelectTarget || target == AbilityTarget.CardSelector ||
                   target == AbilityTarget.ChoiceSelector;
        }

        public static AbilityData Get(string id)
        {
            if (id == null)
                return null;
            var success = AbilityDict.TryGetValue(id, out var ability);
            if (success)
                return ability;
            return null;
        }

        public static List<AbilityData> GetAll()
        {
            return AbilityList;
        }
    }


    public enum AbilityTrigger
    {
        None = 0,

        Ongoing = 2, //Always active (does not work with all effects)
        Activate = 5, //Action

        OnPlay = 10, //When playeds
        OnPlayOther = 12, //When another card played

        StartOfTurn = 20, //Every turn
        EndOfTurn = 22, //Every turn

        OnBeforeAttack = 30, //When attacking, before damage
        OnAfterAttack = 31, //When attacking, after damage if still alive
        OnBeforeDefend = 32, //When being attacked, before damage
        OnAfterDefend = 33, //When being attacked, after damage if still alive
        OnKill = 35, //When killing another card during an attack

        OnDeath = 40, //When dying
        OnDeathOther = 42, //When another dying
    }

    public enum AbilityTarget
    {
        None = 0,
        Self = 1,

        PlayerSelf = 4,
        PlayerOpponent = 5,
        AllPlayers = 7,

        AllCardsBoard = 10,
        AllCardsHand = 11,
        AllCardsAllPiles = 12,
        AllSlots = 15,
        AllCardData = 17, //For card Create effects only

        PlayTarget = 20, //The target selected at the same time the spell was played (spell only)      
        AbilityTriggerer = 25, //The card that triggered the trap
        EquippedCard = 27, //If equipment, the bearer, if character, the item equipped

        SelectTarget = 30, //Select a card, player or slot on board
        CardSelector = 40, //Card selector menu
        ChoiceSelector = 50, //Choice selector menu

        LastPlayed = 70, //Last card that was played 
        LastTargeted = 72, //Last card that was targeted with an ability
        LastDestroyed = 74, //Last card that was killed
        LastSummoned = 77, //Last card that was summoned or created
    }
}