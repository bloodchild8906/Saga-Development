using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TcgEngine
{
    /// <summary>
    /// Defines card variants
    /// </summary>

    [CreateAssetMenu(fileName = "VariantData", menuName = "TcgEngine/VariantData", order = 5)]
    [InlineButton("@CardEditor.CloneAction($root, $property, $value)", SdfIconType.FileEarmarkPlusFill, "", ShowIf = "@CardEditor.ShowNotNull($value)")]
    [InlineButton("@CardEditor.CreateNewAction($root, $property)", SdfIconType.PlusCircleDotted, "")]
    public class VariantData : ScriptableObject
    {
        public string id;
        public string title;
        public Sprite frame;
        [FormerlySerializedAs("frame_board")] public Sprite frameBoard;
        public Color color = Color.white;
        [FormerlySerializedAs("cost_factor")] public int costFactor = 1;
        [FormerlySerializedAs("is_default")] public bool isDefault;

        public static List<VariantData> VariantList = new();

        public string GetSuffix()
        {
            return "_" + id;
        }

        public static void Load(string folder = "")
        {
            if (VariantList.Count == 0)
                VariantList.AddRange(Resources.LoadAll<VariantData>(folder));
        }

        public static VariantData GetDefault()
        {
            foreach (var variant in GetAll())
            {
                if (variant.isDefault)
                    return variant;
            }
            return null;
        }

        public static VariantData GetSpecial()
        {
            foreach (var variant in GetAll())
            {
                if (!variant.isDefault)
                    return variant;
            }
            return null;
        }

        public static VariantData Get(string id)
        {
            foreach (var variant in GetAll())
            {
                if (variant.id == id)
                    return variant;
            }
            return GetDefault();
        }

        public static List<VariantData> GetAll()
        {
            return VariantList;
        }
    }
}