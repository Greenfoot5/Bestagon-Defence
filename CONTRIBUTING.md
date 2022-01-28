# Contributing

This is for code contributions.

## Basic Guidelines

### Use an IDE.
Any IDE that can understand C# is ok, but preferably one with Unity support. [You can view a full list here](https://docs.unity3d.com/Manual/ScriptingToolsIDEs.html)

### Always test your changes.
Do not submit something without at least running the game to see if it compiles.  

### Do not make large changes before discussing them first.
If you are interested in adding a large mechanic/feature or changing large amounts of code, first contact me (Greenfoot5) via [Discord](https://discord.gg/zeDey9v) (preferred method) or send an email to `bestagon-defence@protonmail.com`
For smaller changes, this isn't required, but it's still useful if we have a chat.

### Do not make formatting PRs.
Yes, there are occurrences of trailing spaces, extra newlines, empty indents, and other tiny errors. No, I don't want to merge, view, or get notified by your 1-line PR fixing it. If you're implementing a PR with modification of *actual code*, feel free to fix formatting in the general vicinity of your changes, but please don't waste everyone's time with pointless changes.

## Style Guidelines

### Follow the formatting guidelines.
[// TODO - Finish these/write some](https://chambray-comb-aa7.notion.site/Setup-the-project-properly-for-open-source-f090e030aab243deb331e0206e1c4d53)

### Avoid bloated code and unnecessary getters/setters.
This is situational, but in essence what it means is to avoid using any sort of getters and setters unless absolutely necessary. Public or protected fields should suffice for most things.

### Do not create methods unless necessary.
Unless a block of code is very large or used in more than 1-2 places, don't split it up into a separate method. Making unnecessary methods only creates confusion, and may slightly decrease performance.

## Other Notes
[I need to add a contributors list somewhere](https://chambray-comb-aa7.notion.site/Setup-the-project-properly-for-open-source-f090e030aab243deb331e0206e1c4d53)
