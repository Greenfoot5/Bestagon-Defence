using Abstract.Attributes;
using Godot;

namespace BestagonDefense.addons.AttributesEditor;

[Tool]
public partial class AttributeModifierEditor : VBoxContainer
{
    public AttributeModifier Modifier;
    [Export]
    private LineEdit _uidLineEdit;
    [Export]
    private LineEdit _valueLineEdit;
    [Export]
    private OptionButton _opOptionButton;
    [Export]
    public Button DeleteButton;
    
    [Signal]
    public delegate void AttributeUpdatedEventHandler(float newValue);

    public void OnUidEdit(string newValue)
    {
        Modifier.Uid = newValue;
    }
    
    public void OnValueEdit(string newValue)
    {
        Modifier.Value = int.Parse(newValue);
        _valueLineEdit.Text = Modifier.Value.ToString();
    }

    public void OnOperationSelected(int index)
    {
        Modifier.Op = (Operation)index;
    }

    public void LoadModifier(AttributeModifier modifier)
    {
        Modifier = modifier;
        _uidLineEdit.Text = Modifier.Uid.ToString();
        _valueLineEdit.Text = Modifier.Value.ToString();
        _opOptionButton.Selected = (int)Modifier.Op;
    }
}