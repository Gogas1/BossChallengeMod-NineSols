# Nine Sols BossChallengeMod

A mod to add some kind of boss survival mode. Additionally, it allows you to make the fight more difficult with customizable boss speed up and various modifiers.

For normal game mode, allows you to apply the mod's functionality to regular enemies, but limiting the number of "revivals" of enemies, making enemies and bosses more dangerous, giving them extra lives and modifiers

# Installation using ThunderStore/R2ModMan

1. Install your mod manager and create profile for Nine Sols
2. Search for this or other mods and install them
3. Install dependencies

# Features

## Primary functionality

The main functionality of the mod is to restore bosses and enemies to their original state after death, preserving the state of the player's resources. Thus, the mod adds a boss survival mode, where the player's goal is to kill the boss as many times as possible in one run.

To make the fight more challenging, there is the ability to speed up the boss with each death and various modifiers that affect the fight in favor of the boss.

### Modifiers

#### Modifiers are disabled by default, use the configurator (F1 key by default) to enable them

Modifiers, when active, affect combat in various ways. For example, there is a modifier that prevents a boss from being interrupted by a stun, a modifier that randomizes the active talisman or arrow after each use, a timer modifier, a modifier that turns on Yanlao's cannon while active, and others. 

By default, the modifiers that are activated are randomly selected from a pool with each death. This means that only a few modifiers are active out of all possible ones. The configuration allows you to change the number of modifiers that are selected and to selectively choose which modifiers can be rolled.

Some modifiers are incompatible with each other, mainly for difficulty balancing purposes.

Some modifiers are marked as incompatible to ensure that only one modifier from a group drops (for example, shield and bomb modifiers, so that you can only get one bomb/shield modifier from all bomb/shield modifiers at a time). Also, some internal damage modifiers are incompatible with each other, to avoid unbearable situations when choosing them all at once.

Also, some bosses cannot receive some modifiers. This is often caused by technical implementation issues.

Lady Ethereal cannot gain the following modifiers:
- Any shield modifier(Clones have a problem with shields)
- Endurance modifier(Not applicable for balance reasons)

Sky Rending Claw cannot gain the following modifiers:
- Any shield modifier(Shield cannot be propely attached)

General: Yingzhao cannot gain the following modifiers:
- Any shield modifier(Shield have no effect)

The full list of modifiers is:

- Cooldown Shield
- Distance Shield
- Qi Shield
- Cooldown Shield
- Damage Buildup
- Endurance
- Knockback
- Precise Parries Only
- Qi Bomb
- Qi Depletion Bomb
- Qi Overload Bomb
- Cooldown Bomb
- Shield Break Bomb
- Qi Overload
- Random Arrow
- Random Talisman
- Regeneration
- Speed Up
- Timer
- Yanlao's Assistance

## Secondary functionality

Secondary functionality is the ability to customize combat: limit the number of enemy respawns, customize boss acceleration, the number of selectable modifiers, available modifiers. Limiting the number of respawns, for example, allows you to turn off boss survival mode and just make enemies more difficult.

This also includes functionality in normal game mode. Enemies will receive additional lives and modifiers (the impact of such an increase in the difficulty is noticeable in arena battles with many enemies and minibosses battles)

The mod is incompatible with the wrong versions of the game. On versions of the mod before 0.3.0, this crashes the game. On versions starting with this part of the mod's functionality is disabled, and the player will receive notifications

# Configuration

## For configuration, the BepInEx configurator is used (called by pressing the F1 key)

# Modifier API

# Custom Modifiers API Documentation

This document explains how to create and configure custom modifiers using the mod's API. It covers the creation of a custom modifier class, the base members inherited from `ModifierBase`, and the use of the `ModifierConfigBuilder` for configuring modifiers.

## Table of Contents

- [Creating a Custom Modifier](#creating-a-custom-modifier)
- [The ModifierBase Class](#the-modifierbase-class)
  - [Properties](#properties)
  - [Fields](#fields)
  - [Methods](#methods)
- [Configuring Modifiers with BossChallengeMod](#configuring-modifiers-with-bosschallengemod)
  - [ModifierConfigBuilder Overview](#modifierconfigbuilder-overview)
  - [Builder Methods](#builder-methods)

## Creating a Custom Modifier

To add a custom modifier:

1. **Create a Modifier Class**  
   Your modifier class must inherit from the base class:
   ```csharp
   public class MyModifier : ModifierBase
   {
       // Your custom functionality here
   }
   ```
2. **Note on Pausing:**  
   The modifier is paused in normal game mode when the enemy is not targeting the player.

---

## The ModifierBase Class

The `ModifierBase` class extends `MonoBehaviour` and provides additional members for custom modifier functionality.

### Properties

- **`IsPaused`**  
  ```csharp
  public bool IsPaused { get; protected set; }
  ```  
  Indicates whether the modifier is currently paused.

- **`Key`**  
  ```csharp
  public string Key { get; set; } = string.Empty;
  ```  
  Key of the modifier.

- **`Monster`**  
  ```csharp
  public MonsterBase? Monster { get; protected set; }
  ```  
  References the owner monster of the modifier.

- **`challengeConfiguration`**  
  ```csharp
  public ChallengeConfiguration challengeConfiguration { get; set; }
  ```  
  Configuration settings for the current game mode.

- **`EnemyType`**  
  ```csharp
  public ChallengeEnemyType EnemyType { get; set; }
  ```  
  Specifies the enemy type (e.g., Boss, Miniboss, or Enemy).

### Fields

- **`deathNumber`**  
  ```csharp
  protected int deathNumber;
  ```  
  Tracks the number of times the associated monster has died.

### Methods

- **Constructor**  
  ```csharp
  public ModifierBase() {
      DisableComponent();
  }
  ```  
  Disables the modifier immediately after creation.

- **DisableComponent**  
  ```csharp
  public void DisableComponent() {
      enabled = false;
  }
  ```  
  Disables the modifier component.

- **NotifyActivation**  
  ```csharp
  public virtual void NotifyActivation()
  ```  
  Handles the notification for when the modifier is activated.

- **NotifyDeactivation**  
  ```csharp
  public virtual void NotifyDeactivation()
  ```  
  Handles the notification for when the modifier is deactivated.

- **NotifyEngage**  
  ```csharp
  public virtual void NotifyEngage()
  ```  
  Handles the notification for when the owner monster proceeds to engage.

- **NotifyDisengage**  
  ```csharp
  public virtual void NotifyDisengage()
  ```  
  Handles the notification for when the owner monster proceeds to disengage.

- **NotifyDeath**  
  ```csharp
  public virtual void NotifyDeath(int deathNumber = 0) {
      this.deathNumber = deathNumber;
  }
  ```  
  Updates the death counter upon monster death.

- **NotifyPause**  
  ```csharp
  public virtual void NotifyPause() {
      IsPaused = true;
  }
  ```  
  Sets the modifier to a paused state.

- **NotifyResume**  
  ```csharp
  public virtual void NotifyResume() {
      IsPaused = false;
  }
  ```  
  Resumes the modifier from a paused state.

- **CustomNotify**  
  ```csharp
  public virtual void CustomNotify(object message)
  ```  
  Handles custom notifications from the modifier manager (for instance, reacting to events like talisman explosions).

- **SetController**  
  ```csharp
  public virtual void SetController(Component controllerComponent) {
      throw new NotImplementedException();
  }
  ```  
  Should be overridden for modifiers that need a controller; obtains a reference to the controller component.

---

## Configuring Modifiers with BossChallengeMod

The `BossChallengeMod` type exposes a static property to manage modifiers:

```csharp
public static ModifiersStore Modifiers;
```

This store offers two main ways to add modifiers:

1. **Direct Addition:**  
   Use the method:
   ```csharp
   void AddModifierConfig(ModifierConfig config)
   ```
   to add a pre-configured modifier directly.

2. **Using the ModifierConfigBuilder:**  
   Create a builder with:
   ```csharp
   CreateModifierBuilder<T>(string key, string objectName)
   ```
   where `T` is the type of your custom modifier.

### ModifierConfigBuilder Overview

The `ModifierConfigBuilder` is a builder that allows you to configure a modifier before adding it to the modifiers list.

### Builder Methods

1. **AddKey**  
   ```csharp
   ModifierConfigBuilder AddKey(string key)
   ```  
   Sets the modifier's key. By default, this is taken from the `CreateModifierBuilder` parameters.

2. **AddObjectName**  
   ```csharp
   ModifierConfigBuilder AddObjectName(string name)
   ```  
   Sets the name of the modifier's game object. By default, this is taken from the `CreateModifierBuilder` parameters.

3. **AddIncompatibles**  
   ```csharp
   ModifierConfigBuilder AddIncompatibles(IEnumerable<string> incompatibles)
   ```  
   Specifies keys of modifiers that cannot be selected with this modifier. Selecting one will exclude the other.

4. **AddIgnoredMonsters**  
   ```csharp
   ModifierConfigBuilder AddIgnoredMonsters(IEnumerable<string> ignoredMonsters)
   ```  
   Lists the names of monsters (use monster names, not paths) that are not allowed to receive this modifier.

5. **AddCombinationModifiers**  
   ```csharp
   ModifierConfigBuilder AddCombinationModifiers(IEnumerable<string> configs)
   ```  
   Adds keys for modifiers that should become available when this modifier is selected. This is useful for dependent modifiers, such as a "Shield Break Bomb" that only activates when a shield modifier is already applied.  
   Mark dependent modifiers with `SetIsCombination` method.

6. **AddController**  
   ```csharp
   ModifierConfigBuilder AddController(Type controllerType, bool isShared = false)
   ```  
   Sets a controller class to be created when the modifier initializes. This allows for shared functionality among multiple modifiers or creating a helper object. The `isShared` flag determines whether a single instance is shared or each modifier gets its own controller instance. The controller is added as a component to the monster root object.

7. **SetPersistance**  
   ```csharp
   ModifierConfigBuilder SetPersistance(bool isPersistent)
   ```  
   Determines whether the modifier is selectable. This is useful for modifiers with constant effects (e.g., speed scaling) that only respond to deactivation and enemy death notifications.

8. **SetIsCombination**  
   ```csharp
   ModifierConfigBuilder SetIsCombination(bool isCombination)
   ```  
   Marks the modifier as a dependent (combination) modifier. Such modifiers are not initially added to the available pool until their dependency condition is met.

9. **AddConditionPredicate**  
   ```csharp
   ModifierConfigBuilder AddConditionPredicate(Func<ModifierConfig, bool> predicate)
   ```  
   Adds a predicate function to determine if the modifier should be created.

10. **AddCanBeRolledConditionPredicate**  
    ```csharp
    ModifierConfigBuilder AddCanBeRolledConditionPredicate(Func<ModifierConfig, int, bool> predicate)
    ```  
    Adds a predicate function that decides whether the modifier can be rolled. The integer parameter is the number of boss deaths.

11. **BuildAndAdd**  
    ```csharp
    ModifierConfig BuildAndAdd()
    ```  
    Builds the modifier configuration, adds it to the modifiers list, and returns the built configuration.

12. **Build**  
    ```csharp
    ModifierConfig Build()
    ```  
    Builds the modifier configuration and returns it without automatically adding it to the list.

---

Examples of the modifiers can be found in this repository.

Building the configuration: https://github.com/Gogas1/BossChallengeMod-NineSols/blob/master/Source/BossChallengeMod.cs

Modifiers: https://github.com/Gogas1/BossChallengeMod-NineSols/tree/master/Source/Modifiers

An example of a mod with a modifier added can be found in this repository:
https://github.com/Gogas1/ExampleModifierMod