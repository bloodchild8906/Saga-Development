using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all factions data
    /// </summary>
    
    [CreateAssetMenu(fileName = "TeamData", menuName = "TcgEngine/TeamData", order = 1)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    
    public class TeamData : ScriptableObject
    {
        public string id;
        public string title;
        public Sprite icon;
        public Color color;

        public static List<TeamData> TeamList = new();

        public static void Load(string folder = "")
        {
            if (TeamList.Count == 0)
                TeamList.AddRange(Resources.LoadAll<TeamData>(folder));
        }

        public static TeamData Get(string id)
        {
            foreach (var team in GetAll())
            {
                if (team.id == id)
                    return team;
            }
            return null;
        }

        public static List<TeamData> GetAll()
        {
            return TeamList;
        }
    }
}