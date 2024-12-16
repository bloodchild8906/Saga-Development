using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all traits and stats data
    /// </summary>

    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [CreateAssetMenu(fileName = "TraitData", menuName = "TcgEngine/TraitData", order = 1)]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    public class TraitData : ScriptableObject
    {
        public string id;
        public string title;
        public Sprite icon;

        public static List<TraitData> TraitList = new();

        public string GetTitle()
        {
            return title;
        }

        public static void Load(string folder = "")
        {
            if (TraitList.Count == 0)
                TraitList.AddRange(Resources.LoadAll<TraitData>(folder));
        }

        public static TraitData Get(string id)
        {
            foreach (var team in GetAll())
            {
                if (team.id == id)
                    return team;
            }
            return null;
        }

        public static List<TraitData> GetAll()
        {
            return TraitList;
        }
    }

    [System.Serializable]
    public struct TraitStat
    {
        public TraitData trait;
        public int value;
    }
}