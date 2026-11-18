# Undertale: Vessel Creator

A fan mod that adds a Deltarune-style **"Create Your Vessel"** screen to the start of Undertale. Pick a name, soul color and soul shape; your choice is saved to `vessel.ini` and your soul color is applied in battles.

> Unofficial fan project. Not affiliated with Toby Fox. This repo contains **no game files**, only a patch script. You need your own legal copy of Undertale.

## Install

1. Back up your Undertale `data.win`.
2. Install [UndertaleModTool](https://github.com/UnderminersTeam/UndertaleModTool/releases).
3. Open your `data.win` in it.
4. `Scripts` > `Run other script...` > choose `VesselCreator.csx`.
5. `File` > `Save`, then launch the game.

## Controls

- Up/Down: choose a row
- Left/Right: change color or shape
- Type letters on the NAME row (max 6, Backspace to delete)
- Space or Tab on CONFIRM: save and continue

Delete `vessel.ini` (in the game's save folder) to see the screen again.

## Notes

- Written for the Steam v1.08 build. If `room_start` isn't right for you, change `TARGET_ROOM` at the top of the script.
- The script needs a recent UndertaleModTool (0.6 or newer) for `EventHandlerFor` / `ReplaceGML`.
- Currently only the soul color is applied in-game; the name and shape are saved for future use.

## License

MIT, covering this script only. Undertale belongs to its creators.
