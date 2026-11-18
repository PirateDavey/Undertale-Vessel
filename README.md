# Undertale Vessel

A fan mod for Undertale that adds a Deltarune-style "Create Your Vessel" screen at the start of the game. Pick a name, a soul color and a soul shape. Your choice is saved to `vessel.ini`, and your soul color is used in battles.

> Unofficial fan project. Not affiliated with Toby Fox. This repo contains **no game files**, only a patch script. You need your own legal copy of Undertale.

## Features

- Vessel creation screen with name, soul color (8 colors) and soul shape (3 shapes)
- Live preview of your soul while you edit
- Choice is saved, so the screen only appears the first time
- Soul color is applied in battles

## Requirements

- A legal copy of Undertale (Steam version v1.08)
- [UndertaleModTool](https://github.com/UnderminersTeam/UndertaleModTool/releases) 0.6 or newer

## Install

1. Back up your Undertale `data.win`.
2. Open UndertaleModTool and load your `data.win`.
3. Click `Scripts` > `Run other script...` and choose `VesselCreator.csx`.
4. Click `File` > `Save`, then launch the game.

To uninstall, restore your backup of `data.win`.

## Controls

| Key | Action |
| --- | --- |
| Up / Down | Choose a row |
| Left / Right | Change soul color or shape |
| Letter keys | Type your name (max 6) |
| Space or Tab | Confirm |

To see the screen again, delete `vessel.ini` from the game's save folder.

## Troubleshooting

- **Room not found:** change `TARGET_ROOM` at the top of `VesselCreator.csx` to the title room name shown under Rooms in UndertaleModTool.
- **Script errors:** update UndertaleModTool.
- **Screen doesn't appear:** make sure you saved `data.win` after running the script.

## Known limitations

- Early version with limited testing.
- Name and shape are saved but not yet used in the game itself.

## License

MIT. Undertale and all its assets belong to their creators.
