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

## Most configuration changes are applied at the moment of enemy initialization, so it is better to configure in the main menu or before entering the enemy location. Settings changed in the middle of a battle or at a location should be applied by re-entering the target location or restarting the battle.

## Main configuraion for the Memories of Battle game mode

1.1 Enable mod - Enables the mod in the Memories of Battle game mode
1.2 Record regardless of configuration - By default, fight results records are created for each unique configuration state, so each configured challenge difficulty has its own records. Enabling this setting will record and use a single result regardless of the challenge settings.
1.3 Boss deaths number - Boss will die after this number of deaths(including this one). With a default value of -1 the boss will not die.

## Scaling configuraion for the Memories of Battle game mode

2.1 Enable Speed Scaling - Bosses will gain linear speed scaling with each death. Stacked multiplicatively with the random speed scaling and modifiers.
2.1.2 Scaling: Initial Speed - The starting value for speed scaling.
2.1.3 Scaling: Maximum Speed - The maximum value for speed scaling.

2.2 Enable Modifier Scaling - Bosses will receive additional modifiers, with the total number being the sum of the scaling and a random scaling value.
2.2.1 Scaling: Maximum Modifiers Number - The maximum increase in the number of modifiers.
2.2.2 Maximum Modifiers Number Scaling After Deaths - The increase in modifiers will reach its maximum after this amount of deaths.

2.3 Enable Random Speed Scaling - Bosses will receive a random speed boost with each death. Stacks multiplicatively with the linear speed scaling.
2.3.1 Random Speed Scaling After Deaths - The boss will begin receiving a random speed boost after this death.
2.3.2 Random Scaling: Minimal Speed - The minimum value for the random speed boost.
2.3.3 Random Scaling: Maximum Speed - The maximum value for the random speed boost.

2.4 Enable Random Modifiers Scaling - Bosses will receive a random number of modifiers with each death, added to the linear modifier increase.
2.4.1 Random Modifiers Scaling After Deaths - The boss will begin receiving a random number of modifiers after this death.
2.4.2 Random Scaling: Min Modifiers Number - The minimum value for the random modifiers count.
2.4.3 Random Scaling: Max Modifiers Number - The maximum value for the random modifiers count.

## Modifiers configuraion for the Memories of Battle game mode

3.1 Enable Modifiers - Enable or disable modifiers that affect the boss fight.
3.2 Modifiers Start Death - Modifiers will start to apply after this amount of boss deaths. If set to 0, modifiers will be applied at the start of the fight.
3.3 Enable Modifiers Repeating - Modifiers can be repeated multiple times in a row.
3.M These settings are enabling specific modifiers

## UI configuration

4.1.1 Kill counter and modifiers list UI enabled(right panel) - Enable display of kill counter and modifier list
4.1.2 Use custom rigth panel position - Enable custom placement of kill counter and modifier list
4.1.3 Custom right panel X position - X position of kill counter and modifier list
4.1.4 Custom right panel Y position - Y position of kill counter and modifier list
4.1.5 Right panel UI scale - Scale of kill counter and modifier list

4.2.1 Timer UI enabled - Enable display of the timer
4.2.2 Use custom timer position - Enable custom placement of the timer
4.2.3 Custom timer X position - X position of the timer
4.2.4 Custom timer Y position - Y position of the  timer
4.2.5 Timer UI scale - Scale of the timer

4.3.1 Talisman mode UI enabled - Enable display of the current talisman indicator
4.3.2 Use custom talisman mode position - Enable custom placement of the current talisman indicator
4.3.3 Custom talisman mode X position - X position of the current talisman indicator
4.3.4 Custom talisman mode Y position - Y position of the current talisman indicator
4.3.5 Talisman mode UI scale - Scale of the current talisman indicator


## Main configuraion for the normal game mode

5.1 Enable mod - Enables the mod in the normal game mode

5.2.1 Boss deaths number - Boss will die after this number of deaths(including this one).
5.2.2 Miniboss deaths number - The miniboss will die after the specified number of deaths (including this one).
5.2.2 Regular enemy deaths number - The regular enemy will die after the specified number of deaths (including this one).

5.3.1 Affect bosses - Bosses will revive, gain modifiers and boosts.
5.3.2 Affect minibosses - Minibosses will reviwe, gain modifiers and boosts.
5.3.3 Affect regular enemies - Regular enemies will revive, gain modifiers and boosts.

5.4 Randomize boss death number - The maximum number of boss deaths will be randomly determined at the start of the battle.
5.4.1 Min boss deaths random number - The lowest possible value for the randomized boss death count.
5.4.1 Max boss deaths random number - The highest possible value for the randomized boss death count.

5.5 Randomize miniboss death number - The maximum number of miniboss deaths will be randomly determined at the start of the battle.
5.5.1 Min miniboss deaths random number - The lowest possible value for the randomized miniboss death count.
5.5.1 Max miniboss deaths random number - The highest possible value for the randomized miniboss death count.

5.6 Randomize regular enemy death number - The maximum number of regular enemy deaths will be randomly determined at the start of the battle.
5.6.1 Min regular enemy deaths random number - The lowest possible value for the randomized regular enemy death count.
5.6.1 Max regular enemy deaths random number - The highest possible value for the randomized regular enemy death count.


## Scaling configuraion for the normal game mode

Reflects the settings for the Memories of Battle game mode, but individually for each enemy type.

## Modifiers configuraion for the normal game mode

Reflects the settings for the Memories of Battle game mode.

# Modifier API

Custom modifiers can be added using mod's api.

More details on the mod repository page in the same readme section.
https://github.com/Gogas1/BossChallengeMod-NineSols

# Contacts

<p>I can answer in the thread on the modding server: https://ninesols.wiki.gg/wiki/Community:Modding</p>
<p>Or in the Discord DMs: literature__</p>