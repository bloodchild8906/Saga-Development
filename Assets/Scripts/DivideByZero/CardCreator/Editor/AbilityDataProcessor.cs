using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using TcgEngine;

namespace DivideByZero.CardCreator.Editor
{
    public class AbilityDataProcessor : OdinAttributeProcessor<AbilityData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);

            if (member.Name != "status" && member.Name != "chain_abilities") return;
            var attribute = attributes.GetAttribute<ListDrawerSettingsAttribute>();

            attribute.OnTitleBarGUI = "@CardEditor.CreateNewActionButton($root, $property)";

        }
    }
}