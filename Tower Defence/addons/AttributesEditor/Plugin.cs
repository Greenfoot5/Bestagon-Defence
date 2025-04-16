#if TOOLS
using Godot;

namespace BestagonDefense.addons.AttributesEditor;

[Tool]
public partial class Plugin : EditorPlugin
{
    private Control _attributesDock;
    private PackedScene _attributeItem;
    private PackedScene _attributeModItem;
    
    public override void _EnterTree()
    {
        _attributesDock = GD.Load<PackedScene>("res://addons/AttributesEditor/AttributesEditor.tscn").Instantiate<Control>();
        
        // _attributesDock.
        
        _attributeItem = GD.Load<PackedScene>("res://addons/AttributesEditor/AttributeEditor.tscn");
        _attributeModItem = GD.Load<PackedScene>("res://addons/AttributesEditor/AttributeModifierEditor.tscn");
        AddControlToDock(DockSlot.RightUl, _attributesDock);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        // Remove the dock.
        RemoveControlFromDocks(_attributesDock);
        // Erase the control from the memory.
        _attributesDock.Free();
    }
}
#endif
