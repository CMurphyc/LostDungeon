![icon](https://l5zssw.sn.files.1drv.com/y4muBp3E03bbj97LMn2R4GzeAqQBs3UVZ0px2K1My30s3ts4SjBS59haTWlVz155Um6lwBkLyRAdK1F5kvk2zRPxRbQGId_3Y2TTaeE_blviF1zOaYLV5XHX06gOtjiZjiNbw8VHseW8aYwk-GhMQSwHDIsQVlF1PqjqtkY2d9Ck3se8pD5wIZQbldyAaGFMbHdEVlUJJPvHFbAqoq-wxCAZw?width=256&height=256&cropmode=none "icon")
# LostDungeon

## About
LostDungeon is a dungeon game inspired by "Soul Knight" and "The Binding of Isaac", supporting multiplayer play together.All resources that relevant to "Soul Knight" and "The Binding of Isaac" has been removed from this repo.

## Introduction
### Character
- Engineer(Generally have strong flexibility, and have the characteristics of strong survivability)
- Warrior(Flesh Shield belongs to the defensive heroes, which usually have the characteristics of high health)
- Wizard(Mage heroes have mage skills with high mage damage, and can drop heroes with low magic resistance)
- Ghost(Assassin heroes are mainly in attack outfits, and in team battles are mainly sneak attacks)
### Item(Passive item, only influence bullet type or Character Attribute)
#### BulletType
- Split
- Penetrate
- Sputtering
- LightningChain
- Freeze
- Poision
- Burn
- Dizziness
- Retard
- Bigger
- Smaller
- Longer
- Bounce
#### Character Attribute
- HP
- ATK
- AGI
### Gameplay
#### PvE
Explore dungeons, collect various items, defeat monsters, and go deeper.
#### PvP
Find powerful items, occupy keyrooms and defend them(Keyroom will give you gamepoint in regular time interval).
## Project status
**Implemented:**
- [X] MultiPlayer Online(base on room system)
- [X] Basic UI
- [X] Exclusive skills of four unique heros with cool effects
- [X] Three challenging Boss with different attack mode
- [X] Interesting monsters
- [X] Passive items
- [X] Dungeon random generation
- [X] Monster random generation
- [X] Items random generation(In this version all props will be used instantly when it is picked up)
- [X] Mini map with Fog of War
- [X] GamePoint settlement
- [X] Various of BulletType
- [X] Resurrection teammate
- [X] PvP(similar to Capture The Flag)
- [X] PvE

**Not implemented:**
- [ ] Tutorial
- [ ] Item bag
- [ ] Active Item
- [ ] Teammate location display

## Architectural Pattern
**Basic ECS architectural pattern**
- Components ---component of Player/NPC/BOSS

- EventHandler ---where events are accepted and handled uniformly
 
- Manager ---system module
 
- ModelList  ---initial module, player_name, player_item_info, room_info and more...
 
- Network --network module, unified management of network transceiver
 
- Resources --where to store various resources that need to be loaded
 
- Scenes --scene library
 
- UI -- events of unified interface
 
- Utils ---functions and library files that are often used, for example fixed-point number library and collide_detect_module
 
- GameMain.cs  ---entrance of game

## ScreenShots
- Login interface

![Login](https://m5zssw.sn.files.1drv.com/y4m4zb7vrV0GLqwqqP8v4yIuxQqs-p-T9V9_qB83KQOtU5v73PjDbT1l78xTWDjHRGcq36YN8MgHiYizSaoKQX4yFg1bH29ahfLs0wCDPWCNWpz5WJSwtPMLgzFfcvcG-XXk8YuamhLpB1ZPlidj-pjph0eqMMJOFKqsw-7tv5_27Rl-bngD4GepsUCGDRLeISRekJqXgacbGQ-CW7p8j8myg?width=1276&height=594&cropmode=none "Login")

- PvE Character Select

![PvE](https://raw.githubusercontent.com/CMurphyc/LostDungeon/czh/ScreenShots/pve_ch_select.gif)

- Monster & Normal Attack

![monster](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/ch_engineer.gif)

- BOSS

![boss](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/boss_show.gif)

- Exclusive skills of Heros

![engineer](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/Engineer_skill.gif)
![Warrior](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/Warrior_skill.gif)
![Wizard](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/Wizard_skill.gif)
![Ghost](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/Ghost_skill.gif)

- Pick Items

![item](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/item_pick.gif)

- Relife

![relife](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/relife.gif)

- PvE(Multiplayer)

![pve](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/pve_show.gif)

- PvP

![pvp](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/pvp_show.gif)

- PvP(get point)

![getpoint](https://github.com/CMurphyc/LostDungeon/blob/czh/ScreenShots/pvp_getpoint.gif)

- PvE GameOver(PvP is similar)

![PvEGameOver](https://lpzssw.sn.files.1drv.com/y4m0KpXlDG9X0_VJ35TcOPOhsFxr00RNFAbvHe3yikwJvo1Oia3VGjgWbskqWulxvvQ3uSYOAvHCq7DzbDN0S8zoUy_XQbzNJLtqP-tnzW2sZWTzBZ_ifGJrU-sPSuC3hemRpu_616elLVn2GqVWpqanPDzulAAV_DqkGoWLRgpYGqw82R4vdbRZI9ViNjanK8PPA8XahvxyAbq0Ex2HBAMxg?width=1276&height=602&cropmode=none)

## Download
### Client(Support Android and PC)
[Android Client](https://1drv.ms/u/s!AvEPc_S4bEKgiGZvFaI_LXbIOxIY?e=86exep)

[PC Client](https://1drv.ms/u/s!AvEPc_S4bEKgiGVjcEvBjvS9BSBi?e=M0mNKQ)
### Server(Run in Linux recommended)
[Server](https://github.com/CMurphyc/LostDungeon_Server)

## References
- [**Soul Knight**](http://www.chillyroom.com/zh)
- [**The Binding of Isaac**](https://bindingofisaac.com/)
- [**Darkest Dungeon**](http://www.darkestdungeon.com/)
