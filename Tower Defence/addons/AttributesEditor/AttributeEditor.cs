using Abstract.Attributes;
using Godot;

namespace BestagonDefense.addons.AttributesEditor;

[Tool]
public partial class AttributeEditor : VBoxContainer
{
    public Attribute Attribute;
    [Export]
    private OptionButton _nameOptionButton;
    [Export]
    private Label _valueLabel;
    [Export]
    private LineEdit _baseLineEdit;
    [Export]
    private LineEdit _minLineEdit;
    [Export]
    private LineEdit _maxLineEdit;

    [ExportSubgroup("Modifiers")]
    [Export]
    private Node _modifiersParent;
    [Export] 
    private PackedScene _modifierItem;

    public override void _Ready()
    {
        if (Attribute != null)
            LoadAttribute(Attribute);
    }

    public void RemoveModifier(AttributeModifierEditor modifier)
    {
        Attribute.Remove(modifier.Modifier.Uid);
        modifier.QueueFree();
    }

    public void LoadAttribute(Attribute attribute)
    {
        GD.Print("Loading Attribute: " + attribute);
        Attribute = attribute;
        if (Attribute == null)
            return;

        _nameOptionButton.Selected = (int)Attribute.Name;
        _valueLabel.Text = Attribute.Value.ToString();
        
        foreach (Variant key in Attribute.Keys)
        {
            var modifier = _modifierItem.Instantiate<AttributeModifierEditor>();
            modifier.LoadModifier(Attribute[key]);
            _modifiersParent.AddChild(modifier);
            modifier.DeleteButton.Pressed += () => RemoveModifier(modifier);
        }
    }
    
    public void OnNameSelected(int index)
    {
        Attribute.Name = (AttributeType)index;
    }
    
    public void OnBaseUpdated(string newValue)
    {
        Attribute.Base = int.Parse(newValue);
        _baseLineEdit.Text = Attribute.Base.ToString();
        _valueLabel.Text = Attribute.Value.ToString();
    }
    
    public void OnMinUpdated(string newValue)
    {
        Attribute.Min = int.Parse(newValue);
        _baseLineEdit.Text = Attribute.Min.ToString();
        _valueLabel.Text = Attribute.Value.ToString();
    }
    
    public void OnMaxUpdated(string newValue)
    {
        Attribute.Max = int.Parse(newValue);
        _baseLineEdit.Text = Attribute.Max.ToString();
        _valueLabel.Text = Attribute.Value.ToString();
    }
}