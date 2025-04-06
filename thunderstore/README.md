# Nine Sols BossChallengeMod

A mod to add some kind of boss survival mode. Additionally, it allows you to make the fight more difficult with customizable boss speed up and various modifiers.

For normal game mode, allows you to apply the mod's functionality to regular enemies, but limiting the number of "revivals" of enemies, making enemies and bosses more dangerous, giving them extra lives and modifiers

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

# Configuration

## For configuration, the BepInEx configurator is used (called by pressing the F1 key)

# Modifier API

Custom modifiers can be added using mod's api.

More details on the mod repository page in the same readme section.
https://github.com/Gogas1/BossChallengeMod-NineSols

# Contacts

<p>I can answer in the thread on the modding server: https://ninesols.wiki.gg/wiki/Community:Modding</p>
<p>Or in the Discord DMs: literature__</p>