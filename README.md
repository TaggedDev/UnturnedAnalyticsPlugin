# Player Analytics unturned plugin
![NuGet Version](https://img.shields.io/nuget/v/Scitalis.Analytics)

Currently implemented analytics features:
1. Position tracker (Each 1 second logs all player's positions)
2. Combat tracker (Each time unturned user get's damaged, this data is logged)
3. Player death tracker

Logs are stored in Steam\steamapps\common\U3DS\Unturned_Data\Plugins\Logs in `player_position.csv`, `player_combat.csv` and `player_kill.csv`

Developed using [OpenMod](https://github.com/openmod/openmod)

# Installation
1. [Install](https://store.steampowered.com/app/304930/Unturned/) Unturned Dedicated Server from Steam (look for Tools app)
2. [Install](https://openmod.github.io/openmod-docs/userdoc/installation/unturned.html) OpenMod Core
3. Run Unturned Dedicated Server, first launch will install OpenMod dependencies
4. Execute `om install Scitalis.Analytics` right in the console
5. Execute `om reload` to reload plugin
6. After first player connects to the server, `Steam/steamapps/common/U3DS/Unturned_Data/Plugins/Logs/` folder will contain the logs

Player combat and kill feed logs are created after the first damage is received or first player death occurs (from zombie, high ground, bleeding, etc)


