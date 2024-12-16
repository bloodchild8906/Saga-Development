using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TcgEngine;
using UnityEditor;
using UnityEngine;

namespace DivideByZero.CardCreator.Editor
{
    public class CardEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Divide By Zero/Card Manager")]
        private static void OpenEditor() => GetWindow<CardEditor>();

        public static bool ShowNotNull(UnityEngine.Object value)
        {
            return value != null;
        }

        public static void CloneAction<T>(object root, InspectorProperty property, T value)
        {
            if (value == null) return;

            string fieldName;
            if (property.Parent.Name.Contains("#") || property.Parent.Name.Contains("$"))
            {
                fieldName = property.Name;
            }
            else
            {
                fieldName = property.Parent.Name;
            }

            var field = root.GetType().GetField(fieldName);
            if (field != null && field.FieldType.IsArray)
            {
                CreateNewActionButton(root, property);
                var array = field.GetValue(root) as T[];
                var item = (T)EditorUtils.Clone(value.GetType(), value);

                if (item != null)
                {
                    Array.Resize(ref array, array.Length + 1);
                    array[^1] = item;
                    field.SetValue(root, array);
                }
            }
            else if (field != null)
            {
                var item = (T)EditorUtils.Clone(value.GetType(), value);

                if (item != null)
                {
                    field.SetValue(root, item);

                }
            }
        }

        public static void CreateNewAction(object root, InspectorProperty property)
        {
            string fieldName;

            if (property.Parent.Name.Contains("#") || property.Parent.Name.Contains("$"))
            {
                fieldName = property.Name;
            }
            else
            {
                fieldName = property.Parent.Name;
            }

            var field = root.GetType().GetField(fieldName);

            if (field != null && field.FieldType.IsArray)
            {
                var array = field.GetValue(root) as object[];
                var item = EditorUtils.Clone(field.FieldType.GetElementType(), null, false);

                if (item == null) return;
                Array.Resize(ref array, array!.Length + 1);
                array[^1] = item;
                field.SetValue(root, array);
            }
            else if (field != null)
            {
                var item = EditorUtils.Clone(field.FieldType, null, false);

                if (item != null)
                {
                    field.SetValue(root, item);
                }
            }
        }

        public static void CreateNewActionButton(object root, InspectorProperty property)
        {
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.PlusCircleDotted))
            {
                CreateNewAction(root, property);
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree
            {
                Config =
                {
                    DrawSearchToolbar = true
                }
            };

            tree.AddAllAssetsAtPath("Cards", "Assets/Resources/Cards", typeof(CardData), true, true).SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Abilities", "Assets/Resources/Abilities", typeof(AbilityData), true, true).SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Effects", "Assets/Resources/Effects", typeof(EffectData), true, true).SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Conditions", "Assets/Resources/Conditions", typeof(ConditionData), true, true).SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Filters", "Assets/Resources/Conditions", typeof(FilterData), true, true).SortMenuItemsByName();
            tree.AddAllAssetsAtPath("Status", "Assets/Resources/Status", typeof(StatusData), true, true).SortMenuItemsByName();

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null) return;

            SirenixEditorGUI.BeginHorizontalToolbar(MenuTree.Config.SearchToolbarHeight);
            {

                GUILayout.Label("Card Editor");
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Card")))
                {
                    var selected = MenuTree.Selection.SelectedValue as UnityEngine.Object;

                    if (selected == null) return;

                    ScriptableObjectCreator.ShowDialog<CardData>("Assets/Resources/Cards", TrySelectMenuItemWithObject);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clone")))
                {
                    var selected = MenuTree.Selection.SelectedValue as UnityEngine.Object;

                    if (selected == null) return;

                    var selectedType = selected.GetType();

                    var objToSelect = EditorUtils.Clone(selectedType, selected);

                    if (objToSelect != null)
                    {
                        TrySelectMenuItemWithObject(objToSelect);
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}