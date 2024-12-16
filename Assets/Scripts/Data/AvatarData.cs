using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
namespace TcgEngine
{
    /// <summary>
    /// Defines all avatar data
    /// </summary>

    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    public class AvatarData : ScriptableObject
    {
        public string id;
        public Sprite avatar;
        [FormerlySerializedAs("sort_order")] public int sortOrder;

        public static List<AvatarData> AvatarList = new();

        public static void Load(string folder = "")
        {
            if (AvatarList.Count == 0)
                AvatarList.AddRange(Resources.LoadAll<AvatarData>(folder));

            AvatarList.Sort((AvatarData a, AvatarData b) => { 
                if (a.sortOrder == b.sortOrder) 
                    return a.id.CompareTo(b.id); 
                else 
                    return a.sortOrder.CompareTo(b.sortOrder); 
            });
        }

        public static AvatarData Get(string id)
        {
            foreach (var avatar in GetAll())
            {
                if (avatar.id == id)
                    return avatar;
            }
            return null;
        }

        public static List<AvatarData> GetAll()
        {
            return AvatarList;
        }
    }
}
