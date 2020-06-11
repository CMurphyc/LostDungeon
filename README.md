![icon](https://l5zssw.sn.files.1drv.com/y4muBp3E03bbj97LMn2R4GzeAqQBs3UVZ0px2K1My30s3ts4SjBS59haTWlVz155Um6lwBkLyRAdK1F5kvk2zRPxRbQGId_3Y2TTaeE_blviF1zOaYLV5XHX06gOtjiZjiNbw8VHseW8aYwk-GhMQSwHDIsQVlF1PqjqtkY2d9Ck3se8pD5wIZQbldyAaGFMbHdEVlUJJPvHFbAqoq-wxCAZw?width=256&height=256&cropmode=none "icon")
# LostDungeon

## About
LostDungeon is a dungeon game inspired by "Soul Knight" and "The Binding of Isaac" and support multiplayer play together.All resources that relevant to "Soul Knight" and "The Binding of Isaac" has been removed from this repo.

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

## Project status
**Implemented:**
- [X] MultiPlayer Online(base on room system)
- [X] Basic UI
- [X] Exclusive skills of four unique heros with cool effects
- [X] Three challenging Boss with different attack mode
- [X] Interesting monsters
- [X] Dungeon random generation
- [X] Monster random generation
- [X] Props random generation
- [X] Mini map with Fog of War
- [X] GamePoint settlement
- [X] Various of BulletType
- [X] Resurrection teammate
- [X] PvP(similar to Capture The Flag)
- [X] PvE

**Not implemented:**
- [ ] Tutorial
- [ ] Props bag(In this version all props will be used instantly when it is picked up)
- [ ] Teammate location display

## ScreenShots
- Login interface

![Login](https://m5zssw.sn.files.1drv.com/y4m4zb7vrV0GLqwqqP8v4yIuxQqs-p-T9V9_qB83KQOtU5v73PjDbT1l78xTWDjHRGcq36YN8MgHiYizSaoKQX4yFg1bH29ahfLs0wCDPWCNWpz5WJSwtPMLgzFfcvcG-XXk8YuamhLpB1ZPlidj-pjph0eqMMJOFKqsw-7tv5_27Rl-bngD4GepsUCGDRLeISRekJqXgacbGQ-CW7p8j8myg?width=1276&height=594&cropmode=none "Login")

- PvE Character Select

![PvE](https://njzssw.sn.files.1drv.com/y4mhtoVWaA621Oo2YXfx2hQuoy1JG0wFJH3OsboFzfCRudrp7Qapl0vjbGCwTcbyB4hRQwD6u4k67ByKSsFfH5ddPa96FvOwW8o5dvu3KC31vlMFAfcJmh7xpuBdqTZbbIqX1qJ8M4OMvqQKj5_cUprFCu5SvMbDYdhLQoGUT5byx4lCncp6tmfMgwqjRwhH1Ex2rRu81A-BndotEb_UmOgew?width=1280&height=600&cropmode=none "PvE")

## References
- [**Soul Knight**](http://www.chillyroom.com/zh)
- [**The Binding of Isaac**](https://bindingofisaac.com/)
- [**Darkest Dungeon**](http://www.darkestdungeon.com/)
