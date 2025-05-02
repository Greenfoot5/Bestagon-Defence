@tool
extends Control

func _ready():
    item_rect_changed.connect(_on_size_change)
    
func _on_size_change():
    pivot_offset = Vector2(size.x / 2, size.y / 2)
