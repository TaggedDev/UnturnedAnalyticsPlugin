# Player Analytics unturned plugin
Currently implemented analytics features:
1. Position tracker (Each 1 second logs all player's positions)
2. Combat tracker (Each time unturned user get's damaged, this data is logged)
3. Player death tracker

Logs are stored in Steam\steamapps\common\U3DS\Unturned_Data\Plugins\Logs in `player_position.csv`, `player_combat.csv` and `player_kill.csv`

Developed using [OpenMod](https://github.com/openmod/openmod)