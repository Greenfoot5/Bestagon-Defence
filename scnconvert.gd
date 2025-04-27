extends SceneTree

# modify this variable to point to directories containing .scn files to convert
var directories = ["res://"]

var recursive = true # search through subdirectories for .scn files
var keep_originals = true # keep the original .scn files

func _init():
	for arg in OS.get_cmdline_args():
		if (arg == "-rm"):
			keep_originals = false
	
	for dir_path in directories:
		var dir = Directory.new()
		if (dir.open(dir_path) == OK):
			dir.list_dir_begin()
			var filename = dir.get_next()
			while (filename != ""):
				if (!dir.current_is_dir() && filename.split(".")[-1] == "scn"):
					var scene = load(dir.get_current_dir() + "/" + filename)
					ResourceSaver.save(dir.get_current_dir() + "/" + filename.left(filename.length() - 4) + ".tscn", scene)
					print("saved " + dir.get_current_dir() + "/" + filename.left(filename.length() - 4) + ".tscn")
					if (!keep_originals):
						dir.remove(dir.get_current_dir() + "/" + filename)
						print("Deleted: " + dir.get_current_dir() + "/" + filename)
				elif (dir.current_is_dir() && recursive && !(filename == "." || filename == "..")):
					directories.append(dir.get_current_dir() + "/" +filename)
				filename = dir.get_next()
	
	quit()
