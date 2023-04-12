# Style Guide

Guidelines to generate consistency with project layout, naming and coding style.

You can view the most up to date version here: https://greenfoot5.notion.site/Style-Guide-6c0d5813c8684ee49339614d467bcd91.
If you notice any inconsistencies with this version and the one linked, please contact us.

## 1. Project Structure

The directory structure style of a project should be considered law. Asset naming conventions and content directory structure go hand in hand, and a violation of either causes unneeded chaos.

In this style, we will be using a structure that relies more on filtering and search abilities of the Project Window for those working with assets to find assets of a specific type instead of another common structure that groups asset types with folders.

> Using a prefix naming convention, using folders to contain assets of similar types such as Meshes, Textures, and Materials is a redundant practice as asset types are already both sorted by prefix as well as able to be filtered in the content browser.

```
Assets
    _WIP
        (Work in progress assets, all prefixed with `_` for quick and easy access when referencing mid-development)
    Abstract 
        Data
            (Place for `WeightedItem.cs`, `UpgradableStat.cs`, etc.)
        EnvironmentVariables
        (Random stuff that cannot be categorised. Such as `Runner.cs`)
    Editor - Anything that changes how something is displayed in the editor.
        Gizmos
            (Editor gizmos)
        PropertyDrawers
            (Custom property drawers)
        Turrets
        (Anything else that doesn’t fit into a subfolder)
    Enemies
        Pawns - Non-boss enemies.
            FastEnemy
                (Art, custom MBs, particle effects, animations, etc. for FastEnemy would go here)
        Bosses 
            BlueBoss
                (Art, custom MBs, particle effects, animations, etc. for BlueBoss would go here)
        *Generic assets would go here.*
    Gameplay
        Controls
            (Camera controller would be here)
            (Player controls would also be here)
        Waves
            (Everything wave spawning related goes here)
        (All remaining managers)
    Levels - Scene files and other level specific items.
        _Nodes
            (All MonoBehaviours and prefabs that make the levels would be here)
        Generic - Non-game levels.
            LevelSelect
                Leaderboards
                (Everything related to the level select)
            Menu
                Login
                (Everything related to the main menu)
        Maps
            Hexagon
                (The level itself, level data, level image, maybe some level specific stuff)
            Loop
            ...
    MaterialLibrary - Material and Shader files.
        Hexagons
        Range
        (Anything that doesn’t fit into a subfolder)
    Modules - Each Module has a subfolder.
        Bomb
            (Everything about the bomb module, maybe even custom particle effects for it or something)
        Damage
        …
        (Generic stuff for modules goes here)
    Plugins
        Android
        DiscordGameSDK
        (Plugins here…)
    TextMesh Pro
        (This is for TextMesh Pro)
    Turrets - Each turret has a subfolder for its files and blueprints.
        Shooter
            (Everything about the Shooter)
        Smasher
        ...
        (Generic stuff for turrets goes here)
    UI
        Glyphs
        Logo
        Transition
        ...
        (Any other files that don’t fit into a subfolder goes here)
```

The reasons for this structure are listed in the following sub-sections.

### 1.1 Folder Names

- Always use PascalCase, no spaces
- Only use alphanumeric characters
- No Empty Folders There simply shouldn’t be any empty folders. They clutter the content browser.

### 1.2 All Scene files belong in the `Assets/Level` folder.

Being able to tell someone to open a specific map without having to explain where it is is a great time saver and general ‘quality of life’ improvement.

### 1.3 Don’t name folders `Assets` or `AssetTypes`

- Creating a folder named `Assets` is redundant. All assets are assets.
- Creating a folder named `Meshes`, `Textures`, or `Materials` is redundant. All asset names are named with their asset type in mind. These folders offer only redundant information and the use of these folders can easily be replaced with the robust and easy to use filtering system the Content Browser provides.
    
    Want to view only static mesh in `Environment/Rocks/`? Simply turn on the Static Mesh filter. If all assets are named correctly, they will also be sorted in alphabetical order regardless of prefixes. Want to view both static meshes and skeletal meshes? Simply turn on both filters. this eliminates the need to potentially have to `Control-Click` select two folders in the Content Browser’s tree view.
    
    > This also extends the full path name of an asset for very little benefit. The `SM_` prefix for a static mesh is only three characters, whereas `Meshes/` is seven characters.
    
    Not doing this also prevents the inevitability of someone putting a static mesh or a texture in a `Materials` folder.
    

### 1.4 Very Large Asset Sets Get Their Own Folder Layout

This can be seen as a pseudo-exception to [1.3](https://www.notion.so/Style-Guide-6b4ece8ba14a496e9bbefde32b745c1e).

There are certain asset types that have a huge volume of related files where each asset has a unique purpose. The two most common are Animation and Audio assets. If you find yourself having 15+ of these assets that belong together, they should be together.

For example, animations that are shared across multiple characters should lay in `Characters/Common/Animations` and may have sub-folders such as `Locomotion` or `Cinematic`.

> This does not apply to assets like textures and materials. It is common for a Rocks folder to have a large amount of textures if there are a large amount of rocks, however these textures are generally only related to a few specific rocks and should be named appropriately. Even if these textures are part of a Material Library.

### 1.5 `MaterialLibrary`

If your project makes use of master materials, layered materials, or any form of reusable materials or textures that do not belong to any subset of assets, these assets should be located in `Assets/MaterialLibrary`.

This way all ‘global’ materials have a place to live and are easily located.

> This also makes it incredibly easy to enforce a ‘use material instances only’ policy within a project. If all artists and assets should be using material instances, then the only regular material assets that should exist are within this folder. You can easily verify this by searching for base materials in any folder that isn’t the MaterialLibrary.

The `MaterialLibrary` doesn’t have to consist of purely materials. Shared utility textures, material functions, and other things of this nature should be stored here as well within folders that designate their intended purpose. For example, generic noise textures should be located in `MaterialLibrary/Utility`.

Any testing or debug materials should be within `MaterialLibrary/Debug`. This allows debug materials to be easily stripped from a project before shipping and makes it incredibly apparent if production assets are using them if reference errors are shown.

### 1.6 Scene Structure

Next to the project’s hierarchy, there’s also scene hierarchy. As before, we’ll present you a template. You can adjust it to your needs. Use named empty game objects as scene folders.

```
@System - Game essentials, shared between scenes
@Management - game logic, local the the scene (nothing to render in there)
@UI
    Screen
    World
Camera
Map - (Only in levels)
    Nodes
    Path
    Waypoints
```

- All empty objects should be located at `0, 0, 0` with default rotation and scale.
- For empty objects that are only containers for scripts, use `@` as prefix – e.g. `@Cheats`
- When you’re instantiating an object in runtime, make sure to prefix it with `_` – do not pollute the root of your hierarchy or you will find it difficult to navigate through it.

## 2. Scripts

This section will focus on C# classes and their internals. When possible, style rules conform to Microsoft’s C# standard.

### 2.1 Class Organisation Source files should contain only one public type, although multiple internal classes are allowed.

Source files should be given the name of the public class in the file.

Organise namespaces with a clearly defined structure,

Class members should be ordered logically, and grouped into sections, with spaces between each section

```csharp
namespace Scripts.Character
{
    /// <summary>
    /// Brief summary of what the class does
    /// </summary>
    public class Account
    {
        // Fields
        public static decimal Reserves;

        [Tooltip("Public variables set in the Inspector, should have a Tooltip")]
        public string BankName;
        public const string ShippingType = "DropShip";

        private float _timeToDie;
        
        // Properties
        public string Number {get; set;}
        public DateTime DateOpened {get; set;}
        public DateTime DateClosed {get; set;}
        public decimal Balance {get; set;}

        // Life Cycle
        public void Awake()
        {
            // ...
        }

        // Public methods
        public void AddObjectToBank()
        {
            // ...
        }
    }
}
```

#### 2.1.1 Namespace

Use a namespace to ensure your scoping of classes/enum/interface/etc won’t conflict with existing ones from other namespaces or the global namespace. The project should at minimum use the projects name for the Namespace to prevent conflicts with any imported Third Party assets.

#### 2.1.2 All functions should have a summary

It should describe what the function does, but not how (as the how isn’t relevant). Parameters and return types (except `IEnumerator`) should all be commented.

```csharp
/// <summary>
/// Fire a gun
/// </summary>
/// <param name="trigger">The game object that shot the gun</param>
/// <returns>The bullet shot from the gun</returns>
public GameObject Fire(GameObject trigger)
{
    // Fire the gun.
}
```

#### 2.1.3 Headers

If a class has only a small number of variables, Foldout Groups are not required.

If a class has a moderate amount of variables (5-10), all Serializable variables should have a non-default Header assigned. A common category is `Config`.

To create a header, use the `[Header("<name>")]` attribute.

#### 2.1.4 Commenting

Comments should be used to describe intention, algorithmic overview, and/or logical flow. It would be ideal if from reading the comments alone someone other than the author could understand a function’s intended behaviour and general operation.

#### 2.1.5 Comment Style

- Place the comment on a separate line, not at the end of a line of code.
- Begin comment text with an uppercase letter.
- Insert one space between the comment delimiter `//` and the comment text, as shown in the following example.

The `//` (two slashes) style of comment tags should be used in most situations. Where ever possible, place comments above the code instead of beside it. Here is an example:

```csharp
// Sample comment above a variable.
private int _myInt = 5;
```

#### 2.1.6 Regions

While not explicitly required, they may be useful to help break up larger files into manageable chunks.

```csharp
#region "This is the code to be collapsed"
    Private components As System.ComponentModel.Container
#endregion
```

#### 2.1.7 Spacing

Do use a single space after a comma between function arguments.

Example: `Console.In.Read(myChar, 0, 1);`

- Do not use a space after the parenthesis and function arguments.
- Do not use spaces between a function name and parenthesis.
- Do not use spaces inside brackets.

### 2.2 Compiling

All scripts should compile with zero warnings and zero errors. You should fix script warnings and errors immediately as they can quickly cascade into very scary unexpected behaviour.

Do *not* submit broken scripts to source control. If you must store them on source control, shelve them instead.

### 2.3 Variables

The words `variable` and `property` may be used interchangeably.

#### 2.3.1 Variable Naming

**Nouns**

All non-boolean variable names must be clear, unambiguous, and descriptive nouns.

**Case**

All variables use PascalCase unless marked as private which use camelCase.

Use PascalCase for abbreviations of 4 characters or more (3 chars are all uppercase).

**Considered Context**

All variable names must not be redundant with their context as all variable references in the class will always have context.

**Considered Context Examples:**

Consider a Class called `PlayerCharacter`.

**Bad**

- `PlayerScore`
- `PlayerKills`
- `MyTargetPlayer`
- `MyCharacterName`
- `CharacterSkills`
- `ChosenCharacterSkin`

All of these variables are named redundantly. It is implied that the variable is representative of the `PlayerCharacter` it belongs to because it is `PlayerCharacter` that is defining these variables.

**Good**

- `Score`
- `Kills`
- `TargetPlayer`
- `Name`
- `Skills`
- `Skin`

#### 2.3.2 Variable Access Level

In C#, variables have a concept of access level. Public means any code outside the class can access the variable. Protected means only the class and any child classes can access this variable internally. Private means only this class and no child classes can access this variable. Variables should only be made public if necessary.

Prefer to use the attribute `[SerializeField]` instead of making a variable public.

#### 2.3.4 Local Variables

Local variables should use camelCase.

Implicitly Typed Local Variables

Use implicit typing for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important.

```csharp
var var1 = "This is clearly a string.";
var var2 = 27;
var var3 = Convert.ToInt32(Console.ReadLine());
// Also used in for loops
for (var i = 0; i < bountyHunterFleets.Length; ++i) {};
```

Do not use var when the type is not apparent from the right side of the assignment. Example

```csharp
int var4 = ExampleClass.ResultSoFar();
```

#### 2.3.4 Private Variables

Private variables should have a prefix with am underscore `_myVariable` and use camelCase.

#### 2.3.5 Tooltips

All Serializable variables should have a description in their `[Tooltip]` fields that explains how changing this value affects the behaviour of the script.

#### 2.3.6 Variable Slider And Value Ranges

All Serializable variables should make use of slider and value ranges if there is ever a value that a variable should *not* be set to.

Example: A script that generates fence posts might have an editable variable named `PostsCount` and a value of -1 would not make any sense. Use the range fields `[Range(min, max)]` to mark 0 as a minimum.

If an editable variable is used in a Construction Script, it should have a reasonable Slider Range defined so that someone can not accidentally assign it a large value that could crash the editor.

A Value Range only needs to be defined if the bounds of a value are known. While a Slider Range prevents accidental large number inputs, an undefined Value Range allows a user to specify a value outside the Slider Range that may be considered ‘dangerous’ but still valid.

#### 2.3.7 Booleans

- All booleans should be named in PascalCase but prefixed with a verb.
- All booleans should be named as descriptive adjectives when possible if representing general information.
- Do not use booleans to represent complex and/or dependent states. This makes state adding and removing complex and no longer easily readable. Use an enumeration instead.

#### 2.3.8 Enums

Enums use PascalCase and use singular names for enums and their values. Exception: bit field enums should be plural. Enums can be placed outside the class space to provide global access.

Example:

```csharp
public enum WeaponType
{
    Knife,
    Gun
}

// Enum can have multiple values
[Flags]
public enum Dockings
{
    None = 0,
    Top = 1,
    Bottom = 2,
    Side = 4
}

public WeaponType Weapon
```

#### 2.3.9 Arrays

Arrays follow the same naming rules as above, but should be named as a plural noun.

#### 2.3.10 Interfaces

Interfaces are led with a capital `I` then followed with PascalCase.

### 2.4 Functions, Events, and Event Dispatchers

This section describes how you should author functions, events, and event dispatchers. Everything that applies to functions also applies to events, unless otherwise noted.

#### 2.4.1 Function Naming

The naming of functions, events, and event dispatchers is critically important. Based on the name alone, certain assumptions can be made about functions. For example:

- Is it a pure function?
- Is it fetching state information?
- Is it a handler?
- What is its purpose?

These questions and more can all be answered when functions are named appropriately.

#### 2.4.2 All Functions Should Be Verbs

All functions and events perform some form of action, whether it’s getting info, calculating data, or causing something to explode. Therefore, all functions should start with verbs. They should be worded in the present tense whenever possible. They should also have some context as to what they are doing.

Good examples:

- `Fire` - Good example if in a Character / Weapon class, as it has context. Bad if in a Barrel / Grass / any ambiguous class.
- `Jump` - Good example if in a Character class, otherwise, needs context.
- `Explode`
- `ReceiveMessage`
- `SortPlayerArray`
- `GetArmOffset`
- `GetCoordinates`
- `UpdateTransforms`
- `EnableBigHeadMode`
- `IsEnemy` - [“Is” is a verb.](http://writingexplained.org/is-is-a-verb)

Bad examples:

- `Dead` - Is Dead? Will deaden?
- `Rock`
- `ProcessData` - Ambiguous, these words mean nothing.
- `PlayerState` - Nouns are ambiguous.
- `Color` - Verb with no context, or ambiguous noun.

#### 2.4.3 Functions Returning Bool Should Ask Questions

When writing a function that does not change the state of or modify any object and is purely for getting information, state, or computing a yes/no value, it should ask a question. This should also follow [the verb rule](about:blank#function-verbrule).

This is extremely important as if a question is not asked, it may be assumed that the function performs an action and is returning whether that action succeeded.

Good examples:

- `IsDead`
- `IsOnFire`
- `IsAlive`
- `IsSpeaking`
- `IsHavingAnExistentialCrisis`
- `IsVisible`
- `HasWeapon` - [“Has” is a verb.](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html)
- `WasCharging` - [“Was” is past-tense of “be”.](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html) Use “was” when referring to ‘previous frame’ or ‘previous state’.
- `CanReload` - [“Can” is a verb.](http://grammar.yourdictionary.com/parts-of-speech/verbs/Helping-Verbs.html)

Bad examples:

- `Fire` - Is on fire? Will fire? Do fire?
- `OnFire` - Can be confused with event dispatcher for firing.
- `Dead` - Is dead? Will deaden?
- `Visibility` - Is visible? Set visibility? A description of flying conditions?

#### 2.4.4 Event Handlers and Dispatchers Should Start With `On`

Any function that handles an event or dispatches an event should start with `On` and continue to follow [the verb rule](about:blank#function-verbrule).

Good examples:

- `OnDeath` - Common collocation in games
- `OnPickup`
- `OnReceiveMessage`
- `OnMessageRecieved`
- `OnTargetChanged`
- `OnClick`
- `OnLeave`

Bad examples:

- `OnData`
- `OnTarget`

## 3. Asset Naming Conventions

Naming conventions should be treated as law. A project that conforms to a naming convention is able to have its assets managed, searched, parsed, and maintained with incredible ease.

Most things are prefixed with the prefix generally being an acronym of the asset type followed by an underscore.

**Assets use [PascalCase](about:blank#cases)**

### 3.1 Base Asset Name - `Prefix_BaseAssetName_Variant_Suffix`

All assets should have a *Base Asset Name*. A Base Asset Name represents a logical grouping of related assets. Any asset that is part of this logical group should follow the the standard of `Prefix_BaseAssetName_Variant_Suffix`.

Keeping the pattern `Prefix_BaseAssetName_Variant_Suffix` in mind and using common sense is generally enough to warrant good asset names. Here are some detailed rules regarding each element.

`Prefix` and `Suffix` are to be determined by the asset type through the following [Asset Name Modifier](about:blank#asset-name-modifiers) table.

`BaseAssetName` should be determined by short and easily recognisable name related to the context of this group of assets. For example, if you had a character named Bob, all of Bob’s assets would have the `BaseAssetName` of `Bob`.

For unique and specific variations of assets, `Variant` is either a short and easily recognisable name that represents logical grouping of assets that are a subset of an asset’s base name. For example, if Bob had multiple skins these skins should still use `Bob` as the `BaseAssetName` but include a recognisable `Variant`. An ‘Evil’ skin would be referred to as `Bob_Evil` and a ‘Retro’ skin would be referred to as `Bob_Retro`.

For unique but generic variations of assets, `Variant` is a two digit number starting at `01`. For example, if you have an environment artist generating nondescript rocks, they would be named `Rock_01`, `Rock_02`, `Rock_03`, etc. Except for rare exceptions, you should never require a three digit variant number. If you have more than 100 assets, you should consider organising them with different base names or using multiple variant names.

Depending on how your asset variants are made, you can chain together variant names. For example, if you are creating flooring assets for an Arch Viz project you should use the base name `Flooring` with chained variants such as `Flooring_Marble_01`, `Flooring_Maple_01`, `Flooring_Tile_Squares_01`.

Character Example

| Asset Type               | Asset Name   |
|--------------------------|--------------|
| Skeletal Mesh            | SK_Bom       |
| Material                 | M_Bom        |
| Texture (Diffuse/Albedo) | T_Bob_D      |
| Texture (Normal)         | T_Bob_N      |
| Texture (Evil Diffuse)   | T_Bob_Evil_D |

Prop Examples

| Asset Type               | Asset Name   |
|--------------------------|--------------|
| Static Mesh (01)         | SM_Rock_01   |
| Static Mesh (02)         | SM_Rock_02   |
| Static Mesh (03)         | SM_Rock_03   |
| Material                 | M_Rock       |
| Material Instance (Snow) | MI_Rock_Snow |

### 3.2 Asset Name Modifiers

When naming an asset use these tables to determine the prefix and suffix to use with an asset’s [Base Asset Name](about:blank#base-asset-name).

| Asset Type                           | Prefix | Suffix        | Notes |
|--------------------------------------|--------|---------------|-------|
| Texture                              | T_     |               |       |
| Shader                               | S_     |               |       |
| Material                             | M_     |               |       |
| Font                                 | Font_  |               |       |
| Animation Clip                       | A_     |               |       |
| Animation Controller                 | AC_    |               |       |
| Particle System                      | PS_    |               |       |
| Level Data                           | LD_    |               |       |
| Curved Variable (used in level data) | LDV_   | _\<levelname> |       |

## 4. Asset Workflows

This section describes best practices for creating and importing assets usable in Unity.

### 4.1 Textures

- They are a power of two (For example, 512 x 512 or 256 x 1024).
- Use Texture Atlases wherever possible.

### 4.2 Audio

Only import uncompressed audio files in to Unity using WAV or AIFF formats.

Great guide on [Unity Audio Import Optimization](https://www.gamasutra.com/blogs/ZanderHulme/20190107/333794/Unity_Audio_Import_Optimisation__getting_more_BAM_for_your_RAM.php)