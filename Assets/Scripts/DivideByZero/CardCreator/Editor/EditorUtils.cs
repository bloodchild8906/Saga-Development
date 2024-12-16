using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Demos.RPGEditor;
using TcgEngine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace DivideByZero.CardCreator.Editor
{
    public static class EditorUtils
    {
        private static void UpdateForType<T>(Type type, T source, T destination)
        {
            var myObjectFields = type.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var fi in myObjectFields)
            {
                if (fi.FieldType.IsArray)
                {
                    if (fi.FieldType.GetElementType()!.IsSubclassOf(typeof(ScriptableObject)) || fi.FieldType.GetElementType()!.IsEnum)
                    {
                        fi.SetValue(destination, (fi.GetValue(source) as Array)?.Clone());
                    }
                    else
                    {
                        var sourceArray = fi.GetValue(source) as Array;
                        if (sourceArray == null)
                            continue;

                        var clonedArray = Array.CreateInstance(fi.FieldType.GetElementType()!, sourceArray.Length);
                        for (var i = 0; i < sourceArray.Length; i++)
                        {
                            if (typeof(ICloneable).IsAssignableFrom(sourceArray.GetValue(i)))
                            {
                                clonedArray.SetValue(((ICloneable)sourceArray.GetValue(i)).Clone(), i);
                            }
                        }

                        fi.SetValue(destination, clonedArray);
                    }
                } else
                {
                    fi.SetValue(destination, fi.GetValue(source));
                }  
            }
        }

        public static string GetFolder(Type selectedType)
        {
            var scriptableObjectData = selectedType.ToString();

            if (selectedType.IsSubclassOf(typeof(ConditionData)))
            {
                scriptableObjectData = "TcgEngine.ConditionData";
            }
            else if (selectedType.IsSubclassOf(typeof(FilterData)))
            {
                scriptableObjectData = "TcgEngine.FilterData";
            }
            else if (selectedType.IsSubclassOf(typeof(EffectData)))
            {
                scriptableObjectData = "TcgEngine.EffectData";
            }

            var folders = new Dictionary<string, string>
            {
                { "TcgEngine.CardData", "Cards" },
                { "TcgEngine.AbilityData", "Abilities" },
                { "TcgEngine.EffectData", "Effects" },
                { "TcgEngine.ConditionData", "Conditions" },
                { "TcgEngine.FilterData", "Filters" },
                { "TcgEngine.StatusData", "Status" },
                { "TcgEngine.TraitData", "Traits" },
                { "TcgEngine.TeamData", "Teams" },
                { "TcgEngine.RarityData", "Rarities" },
            };

            return folders.TryGetValue(scriptableObjectData, out var folder) ? $"Assets/Resources/{folder}" : "Assets/Resources/";
        }


        public static T Clone<T>(Type selectedType, object selected, bool copyValues = true) where T : ScriptableObject
        {
            T clone = null;
            var folder = "";
            if (selected == null)
            {
                folder = GetFolder(selectedType);
            }
            else
            {
                var parts = AssetDatabase.GetAssetPath(selected as UnityEngine.Object).Split("/");
                Array.Resize(ref parts, parts.Length - 1);

                folder = string.Join("/", parts);
            }

            var showDialogMethod = typeof(ScriptableObjectCreator).GetMethod("ShowDialog");
            var genericShowDialogMethod = showDialogMethod?.MakeGenericMethod(selectedType);
            object[] parameters = { $"{folder}", new Action<T>(obj =>
            {
                if (copyValues)
                    UpdateForType(selectedType, selected as T, obj);

                var propertyInfo = obj.GetType().GetField("id");

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(obj, obj.name.ToLower().Replace(" ", "_"));
                }

                clone = obj;
            })};

            genericShowDialogMethod?.Invoke(null, parameters);

            return clone;
        }


        public static object Clone(Type selectedType, object selected, bool copyValues = true)
        {
            return Clone<ScriptableObject>(selectedType, selected, copyValues);
        }
    }
}