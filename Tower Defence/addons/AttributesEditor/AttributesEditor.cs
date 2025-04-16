using System;
using Abstract.Attributes;
using Godot;
using Godot.Collections;

namespace BestagonDefense.addons.AttributesEditor;

[Tool]
public partial class AttributesEditor : VBoxContainer
{
    [Export]
    private Container _attributesContainer;
    [Export]
    public PackedScene AttributesItem;
    [Export]
    public PackedScene AttributeModifierItem;
    
    public override void _Ready()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        Array<Dictionary> propertyList;
        try
        {
            propertyList =
                EditorInterface.Singleton.GetInspector().GetEditedObject().GetPropertyList();
        }
        catch (NullReferenceException)
        {
            return;
        }
        
        foreach (Dictionary property in propertyList)
        {
            if (property["name"].AsString().Equals("attributes", System.StringComparison.CurrentCultureIgnoreCase))
            {
                var attributes = (Attributes)EditorInterface.Singleton.GetInspector().GetEditedObject()
                    .Get(property["name"].AsStringName()).AsGodotObject();
                
                GD.Print(attributes);

                foreach (AttributeType key in attributes.Keys)
                {
                    var item = AttributesItem.Instantiate<AttributeEditor>();
                    item.LoadAttribute(attributes[key]);
                    _attributesContainer.AddChild(item);
                }
            }
        }
    }

    public override void _EnterTree()
    {
        EditorInterface.Singleton.GetInspector().EditedObjectChanged += UpdateUI;
    }

    public override void _ExitTree()
    {
        EditorInterface.Singleton.GetInspector().EditedObjectChanged -= UpdateUI;
    }

    public void AddNewAttribute()
    {
        Node item = AttributesItem.Instantiate();
        _attributesContainer.AddChild(item);
    }
}