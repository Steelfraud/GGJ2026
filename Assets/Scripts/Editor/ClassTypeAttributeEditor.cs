using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ClassTypeAttribute))]
public class ClassTypeAttributeEditor : PropertyDrawer
{

    private ClassTypeAttribute usedAttribute;
    private List<string> TypesAsStrings = new List<string>();
    private List<string> FullTypesAsStrings = new List<string>();

    private void SetupLists()
    {
        List<string> ignoreList = new List<string>();

        if (usedAttribute.ExcludedTypes.IsEmpty() == false)
        {
            ignoreList = new List<string>(usedAttribute.ExcludedTypes);
        }

        this.TypesAsStrings = LogicUtils.GetSubClassesAsStrings(usedAttribute.TargetType, ignoredClassNames: ignoreList, includeBaseClass: this.usedAttribute.IncludeTargetType);
        this.TypesAsStrings.Add("Nothing Selected");
        this.FullTypesAsStrings = LogicUtils.GetSubClassesAsStrings(usedAttribute.TargetType, ignoredClassNames: ignoreList, includeBaseClass: this.usedAttribute.IncludeTargetType, useAssemblyQualifiedName: true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        usedAttribute = (ClassTypeAttribute)attribute;

        if (usedAttribute == null)
        {
            EditorGUI.LabelField(position, "something wrong with given attribute?");
            return;
        }

        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, "wrong property type with given attribute!");
            return;
        }

        SetupLists();

        var triggerClassString = property.stringValue;
        int selectedIndex = this.TypesAsStrings.Count - 1;

        for (int i = 0; i < this.FullTypesAsStrings.Count; i++)
        {
            if (triggerClassString == this.FullTypesAsStrings[i])
            {
                selectedIndex = i;
                //Debug.Log("this is current skill: " + this.TriggerFullTypesAsStrings[i]);
            }
        }

        selectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, this.TypesAsStrings.ToArray());
        
        if (selectedIndex < this.FullTypesAsStrings.Count)
        {
            property.stringValue = this.FullTypesAsStrings[selectedIndex];
            //Debug.Log("set to: " + triggerClassString.stringValue);
        }
        else
        {
            property.stringValue = "";
        }
    }

}
