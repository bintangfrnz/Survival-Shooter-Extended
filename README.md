<!--
*** Bintang Fajarianto
*** 22 April 2022
-->

<p align="center">
    <img align=center src="https://visitor-badge.laobi.icu/badge?page_id=bintangfrnz.Survival-Shooter-Extended" alt="Visitors">                     
</p>

## Survival-Shooter-Extended

<p align="center">
· <a href="https://github.com/bintangfrnz/Survival-Shooter-Extended/issues">Report Bug</a> ·
</p>

> note: This project is a copy version of "Tubes 2 IF3210 PBD: Unity3D Game"

<!-- Contents -->
<details open="open">
    <summary>Contents</summary>
    <ol>
        <li><a href="#description">Description</a></li>
        <li><a href="#installation">Installations</a></li>
        <li><a href="#features">Features</a></li>
        <li><a href="#specifications">Specifications</a></li>
        <li><a href="#contact">Contact</a></li>
    </ol>
</details>

## Description

**Survival Shooter** is a game where players have to survive the zombies' attacks. This game is available on the [Unity Learn platform](https://learn.unity.com/project/survival-shooter-tutorial) for anyone to learn. In this assignment, I continue my work on Unity Exercise based on the [Survival Shooter Tutorial - 1 of 10](https://www.youtube.com/watch?v=_lP6epjupJs&list=PL871udVFq7OF9w5RBjyp_lcyzFuViLe8x&index=2) video and continue the game built by [@PrateekAdhikaree](https://github.com/PrateekAdhikaree) with his permission. You can check the repository here:
- [Exercise Unity3D Survival Shooter](https://github.com/bintangfrnz/Exercise-Unity3D-Survival-Shooter)
- [Nightmares](https://github.com/PrateekAdhikaree/Nightmares)

And here comes this **Survival Shooter: Extended**.

## Installation
> Only available on Mac (idk how to build on Windows🥲)

1. Clone repository (or download as ZIP)
  ```sh
  git clone https://github.com/bintangfrnz/Survival-Shooter-Extended.git
  ```
  
2. Navigate to /WishMeLuck/Builds
  ```sh
  cd WishMeLuck/Builds
  ```
  
3. run _.app_ file to install

## Features
### (1) Player Attributes
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_HUD.png?raw=true" alt="HUD Canvas" width="600">

There are 3 main player attributes in this game. Here's the stats:

|         | Power | Speed | Health |
| :-----: | :---: | :---: | :----: |
| Initial | 20    | 5     | 80     |
| Max     | 50    | 10    | 100    |

### (2) Orbs and Pickups
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_orbs.gif?raw=true" alt="Orbs" width="600">

In this game, we have 3 orbs to **increase the value of each player attribute** and 3 pickups to **enhance weapon within a certain time** that the player can pick up and will disappear if not taken in 10 seconds.

- **Player Attribute Orbs**

Orbs will appear randomly every 3 seconds with 40% chance of spawning.<br>
`50%` -> health orb<br>
`25%` -> speed orb<br>
`25%` -> power orb<br>

|       | Power | Speed  | Health |
| :---: | :---: | :----: | :----: |
| Color | blue  | yellow | red    |
| Add   | 2     | 0.25   | 15     |

- **Enhance Weapon Pickups**

Pickups will appear on the enemy dead body with 25% chance of spawning.<br>
`40%` -> floating pickup<br>
`35%` -> bouncing pickup<br>
`25%` -> piercing pickup<br>

|             | Bounce | Pierce  | Float |
| :---------: | :----: | :-----: | :---: |
| Color       | green  | magenta | white |
| time effect | 20s    | 20s     | 20s   |

  - **bounce**: bullets can bounce while they hit the environment, but not the enemies<br>
  - **pierce**: bullets can pierce through the enemies, but not the environment<br>
  - **float**: bullets can float further (bullet floating time becomes longer)<br>

### (3) Additional Mobs

- **Zombies**
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_zom.gif?raw=true" alt="Zombies" width="500">

|             | ZomBearRage | ZomBunnyRage |
| :---------: | :---------: | :----------: |
| Calm speed  | 2           | 3            |
| Rage speed  | 4           | 5            |
| Health      | 160         | 120          |
| Damage      | 8           | 10           |
| Score point | 10          | 15           |
 
- **Skeletons**
<p float="left">
  <img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_skelebear.gif?raw=true" alt="Skelebear" width="500">
  <img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_skelebunny.gif?raw=true" alt="Skelebunny" width="500">
</p>

> skeleton mobs can't move

|                   | SkeleBear   | SkeleBunny |
| :---------------: | :---------: | :--------: |
| Shoot type        | consecutive | spread     |
| Bullet            | 3           | 3          |
| Damage per bullet | 4           | 5          |
| Health            | 180         | 200        |
| Score point       | 20          | 25         |

- **Bombers**
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_bomb.gif?raw=true" alt="Bombers" width="500">

> bomber mobs will explode while hit player

|             | BomBear | BomBunny |
| :---------: | :-----: | :------: |
| Calm speed  | 3       | 4        |
| Rage speed  | 5       | 6        |
| Health      | 100     | 75       |
| Damage      | 1       | 18       |
| Score point | 15      | 20       |
  
- **Boss**
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Gif_boss.gif?raw=true" alt="Boss" width="500">

> boss mobs can fly

|      | Move Speed | Health | Damage per Bullet | Score Point |
| :--: | :--------: | :----: | :---------------: | :---------: |
| Boss | 3          | 1000   | 15                | 200         |

|                   | Basic Attack | Special Attack |
| :---------------: | :----------: | :------------: |
| Shoot type        | consecutive  | spread         |
| Number of bullets | 10           | 36             |
| n Atk per loop    | 2            | 1              |

### (4) Game Mode

A player can choose one of this game modes:
- **Zen Mode**
  - enemies will appear randomly on nav mesh every 5 seconds + enemies will appear randomly on existing spawn points 8-30 seconds (each mob have their own spawning time)
  - every 120 seconds, boss will appear at least once
> players have to survive the zombies' attacks as long as possible

- **Wave Mode**
  - enemies will appear with the fixed number of each wave
  - boss will appears on each wave multiples of three
  - next wave triggered if all enemies killed or none killed in the last 30 seconds
  - there are 9 waves in this game, and the difficulty will increase after wave 9
> players have to survive a bunch of zombies on each wave

### (5) Weapon Upgrade
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_UpgradeWeapon.png?raw=true" alt="Weapon Upgrade Canvas" width="600">

There are two types of weapon upgrades, but player can choose only one:
- Additional Bullet **\[+2]** (diagonal)
- Speed Up Shoot **\[+10]**


> In Wave Mode, weapon upgrade options appear every time the player defeat a boss wave<br>
> In Zen Mode, weapon upgrade options appear every 90 seconds

### (6) Local Scoreboard
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_Leaderboard.png?raw=true" alt="Leaderboard Canvas" width="600">

The value of each attribute on leaderboard are saved in PlayerPrefs with base key:
- **"wave_leaderboard"** for Wave Mode
attributes: `player name`, `total score`, and `last wave`

- **"zen_leaderboard"** for Zen Wave Mode
attributes: `player name`, `total score`, and `survival time`

> Each scoreboard is sorted by total score

### (7) Main Menu
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_MainMenu.png?raw=true" alt="Main Menu Canvas" width="600">

In this menu, players can write their names and choose one of the existing game modes. By clicking the button on the side, a player can navigate to another panel.

### (8) Game Over
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_GameOver.png?raw=true" alt="Game Over Canvas" width="600">

When a player dies, the game over panel will show, and the player can see their game statistic on the screen. The player can choose to start a new game or go back to Main Menu.

### (9) Others
- **Minimap**

While the game is started, you can click `m` on the keyboard to show the minimap

- **Pause**
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_Pause.png?raw=true" alt="Paused Canvas" width="600">

While the game is started, you can click `ESC` on the keyboard to pause the game

- **Setting Panel**
<img src="https://github.com/bintangfrnz/Survival-Shooter-Extended/blob/main/Pictures/Canvas_Setting.png?raw=true" alt="Setting Canvas" width="600">

In the Main Menu, you can navigate to Setting Panel by clicking the button on the left. In this panel, you can adjust the music, sfx, and brightness

## Specifications
1. **Player Attributes**
   - [x] Add power attribute
   - [x] Add speed attribute
   - [x] Add health attribute
   - [x] Show all attributes on the screen
   - [x] Set the initial value and max value for each attribute
2. **Orbs**
   - [x] Add power orb
   - [x] Add speed orb
   - [x] Add health orb
   - [x] Spawn orbs randomly and periodically
   - [x] Eliminate orbs if not taken for a certain period of time
3. **Additional Mobs**
   - [x] Add skeleton mobs
   - [x] Add bomber mobs
   - [x] Add boss mobs
4. **Game Modes**
   1. **Zen Mode**
      - [x] Add a timer to count the game time
   2. **Wave Mode**
      - [x] Set the max number of waves (min. 3)
      - [x] Calculate the weight of each wave
      - [x] Set the pool where the enemy may come out in each wave
      - [x] Set the score point of each enemy
      - [x] Increase the weight capacity of each wave
      - [x] Boss appears on each wave multiples of three
5. **Weapon Upgrades**
   - [x] Apply Diagonal Weapon
   - [x] Apply Faster Weapon
   - [x] Weapon upgrades can be applied more than once
   - [x] In _Zen Mode_, weapon upgrade options appear at certain times
   - [x] In _Wave Mode_, weapon upgrade options appear every time you defeat a boss wave
6. **Local Scoreboards**
   - [x] Dapat melihat dua jenis halaman _scoreboard_, yakni _Scoreboard Zen Mode_ dan _Scoreboard Wave Mode_
   - [x] In _Zen Mode Scoreboard_, show player name, total score, and survival time
   - [x] In _Wave Mode Scoreboard_, show player name, total score, and last wave
   - [x] Each scoreboard is sorted by total score
7. **Main Menus**
   - [x] Set the Player Name
   - [x] Choose one of the existing Game Modes
   - [x] Able to open the Local Scoreboard
8. **Game Over**
   - [x] In _Zen Mode_, show total score and survival time
   - [x] In _Wave Mode_, show total score and last wave
   - [ ] A player can choose to start a new game (BUG!)
   - [x] A player can choose to back to Main Menu
9. **Bonuses**
   - [ ] Add more _Game Mode_
   - [x] Add more _Weapon Upgrade_
   - [x] Add _Map_
   - [ ] Add _First Person Mode_
10. **Not in specs**
    - [x] Enemy's health bar
    - [x] 2 type of skeleton: Spread Shoot & Consecutive Shoot
    - [x] Blinking bomber
    - [x] Shoot range attribute (bullet floating time)
    - [x] Setting Panel to adjust music, sfx, and brightness
    - [x] Pause Panel with `esc` shortcut

## Contact

### Group 61
| NIM      | Name               |
| :------: | :----------------: |
| 13519138 | Bintang Fajarianto |

[![Instagram](https://img.shields.io/badge/-@bintangfrnz__-E1306C?style=flat&logo=instagram&logoColor=EEEEEE&link=https://instagram.com/bintangfrnz_/)](https://instagram.com/bintangfrnz_)

<a href="#survival-shooter-extended">Back to Top</a>
