[![Previous: Flames.st](https://img.shields.io/badge/←_PREV_(Flames.st)-gray.svg?style=for-the-badge)](https://github.com/redgreenshift/Flames.st/blob/main/README.md)
[![Previous: Greenshift](https://img.shields.io/badge/←_PREV_(Greenshift)-gray.svg?style=for-the-badge)](https://github.com/redgreenshift/Greenshift/blob/main/README.md)

![FireDemo](assets/firedemo-logo.svg)

[![Raspberry Pi](https://img.shields.io/badge/Raspberry%20Pi-A22846.svg?logo=raspberrypi&logoColor=white)](https://www.raspberrypi.com/)
[![Linux](https://img.shields.io/badge/Linux-FCC624.svg?logo=linux&logoColor=black)](https://www.linux.org/)
[![Windows](https://img.shields.io/badge/Windows-0078D4.svg?logo=data:image/svg%2bxml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIE9yaWdpbmFsIGZyb206IFNWRyBSZXBvLCB3d3cuc3ZncmVwby5jb20sIEdlbmVyYXRvcjogU1ZHIFJlcG8gTWl4ZXIgVG9vbHM7IGhhbmQgbW9kaWZpZWQgdG8gd2hpdGUgbW9ub2Nocm9tZSAtLT4KPHN2ZyBmaWxsPSIjRkZGRkZGIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCA1MTIgNTEyIiBpZD0iaWNvbnMiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHBhdGggZD0iTTMxLjg3LDMwLjU4SDI0NC43VjI0My4zOUgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDMwLjU4SDQ3OS43VjI0My4zOUgyNjYuODlaIi8+PHBhdGggZD0iTTMxLjg3LDI2NS42MUgyNDQuN3YyMTIuOEgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDI2NS42MUg0NzkuN3YyMTIuOEgyNjYuODlaIi8+PC9zdmc+)](https://www.microsoft.com/windows)
[![C#](https://img.shields.io/badge/C%23-512BD4.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET Framework 4.8](https://img.shields.io/badge/4.8-512BD4.svg?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/platform/support/policy/dotnet-framework)
[![License: GPL-2.0-only](https://img.shields.io/badge/License-GPL--2.0--only-F58220.svg)](LICENSE)

A real-time flame animation and plasma effect playground built with C# and WinForms (.NET Framework 4.8).
Originally inspired by the challenge of generating realistic fire in Smalltalk — and proven to work
just as well in C# over two decades later.

While the engine serves as a tool for parameter exploration for dynamic lighting effects,
it was later redesigned as a standalone status indicator, used on desktop devices
like a Raspberry Pi to communicate my availability at a glance.

## What it does

FireDemo renders animated flame effects using a 2D grid of intensity values with pixel averaging,
producing realistic-looking fire, lightning, and plasma patterns. The core idea is simple: each frame,
the brightness of a pixel is the average of neighboring pixels with some cooling applied — but with
well-tuned palettes and cooling maps, the result looks surprisingly lifelike.

## Effects

| Effect | Description |
|--------|-------------|
| **Realistic Flame** | Warm, natural-looking fire with multi-point color palettes and cooling maps |
| **Candle** | A small, gentle candle flame |
| **Bonfire** | A wide, raging fire |
| **Dumpster Fire** | Playful combination of a static dumpster and bonfire |
| **Batman Logo** | Bat-symbol outline rendered in fire, with options for single and multithreaded rendering |
| **Lightning** | Branching lightning bolts that cascade down the flame grid |
| **Borg Plasma** | A Star Trek inspired Borg Alcove regeneration disc with inner plasma and outer ring |
| **Sauron** | The Eye of Sauron, built from layers of flame and lightning |
| **Status Text** | "Available", "Busy", "Away", "In a Meeting", "DND" status screens combining dynamic lighting effects with pixel art and text explaining status in more detail |
| **RainBORG** | Demonstration of multiple plasma discs in various colors displayed simultaneously |
| **Rainbow Fire** | Demonstration of multiple independent flames with a gradient palette |

## Modes

The application has two main views:

1. **Advanced UI FireDemo (Form1)** — ORIGINAL Full parameter control: adjust fire dimensions, color palettes (4-point linear, realistic curve, flat), cooling strategies (constant, map-based, with shifting/rotation), seed coal values, and interpolation mode.
A parameter exploration tool used to fine-tune the fire engine's behavior. It allows for deep adjustment of fire geometry, palettes, and cooling strategies.

In **Form1** (Advanced UI), explore a wide set of options and palettes. Useful for determining parameters to achieve a particular desired effect without needing to recompile.

You can adjust:
- **Fire dimensions** — Width and height of the flame grid (affects performance and look)
- **Magnification** — How much the flame is scaled up on screen
- **Interpolation mode** — NearestNeighbor (pixelated), Bilinear (smooth), or Bicubic (extra smooth)
- **Cooldown strategy** — Constant vs Map, with tuning for density, smoothing, shifting, and rotation


2. **Friendly Availability Neighborhood Status Indicator (Form2)** — The DEFAULT view with a sequence of preset animated effects. Press any preset status like "Available", "Busy", "Away", "In a Meeting", or "DND" to switch to the corresponding status display.
The primary application mode, designed as a desktop status indicator for a Raspberry Pi device. It features easy-to-use buttons to toggle between "Away", "Busy", "Available", and "DND" status displays.
Designed for use as a desktop status display (e.g. on a Raspberry Pi). Includes preset buttons for "Away", "Busy", "Available", and "DND" status screens.

In **Form2** (Status Indicator), you can select from a set of predefined presets.

Using Form1, you can determine values you would like to manually create a preset in the code for Form2.

3. **Fast Render (Form3)** — An optimized render path that bypasses `OnPaint` for higher framerates. EXPERIMENTAL, and did not work the way I had hoped.

## How it works

The fire algorithm works by maintaining a 2D grid where each cell holds an intensity value (0–255). Each frame:

1. **Averaging** — Each pixel's new intensity is the average of its neighbors (configurable which neighbors — any combination of the 9 pixels in a 3x3 block)
2. **Cooling** — Each pixel loses some energy based on a cooling strategy (constant decay or a cooling map with density/smoothing/rotation)
3. **Seeding** — New heat is added along the bottom or at specific shapes (Batman logo, candle, lightning bolts)
4. **Color mapping** — Intensity values are mapped through a palette (8-bit indexed color)

### Palette styles

- **Flat** — Single-color palette where each intensity level scales RGB channels uniformly.
- **FourPointLinear** — Four-color gradient interpolated at 0%, 33%, 66%, and 100% intensity.
- **RealisticFlame** — Tuned seven-stop gradient ordered from hottest region outward: blue → white → yellow → orange → dark fringe. Suggests heat dissipating at flame edges for a natural-looking flame.
- **RealisticFlameColorized** — Corresponding gradient derived from user-specified base color, allowing flames to render in arbitrary colors.
- **DarkFlame** — Exploration of what a “black” flame might look like, generalized to support any base color.
- **Lightning** — High-contrast palette optimized for realistic lightning-bolt effects. Derived from `RealisticFlame`, with default palette generated by mixing pinks and blues.
- **Plasma** — Optimized for plasma-disc-style effects, derived from `FourPointLinear`.

### Cooling strategies

- **Off** — The only "decay" is due to integer truncation when averaging pixel values.
- **Constant** — Uniform decay rate across the entire grid.
- **Map** — Spatially varying cooling with configurable density, min/max, smoothing, shifting, and rotation (more realistic, produces the characteristic flame movement patterns)

## Building

Requirements:
- Visual Studio 2019+ (or the .NET Framework 4.8 SDK)
- The project uses the standard `.csproj` format

Open `FireDemo.sln` and build. The default entry point launches **Form2** (Status Indicator).

## Architecture highlights

- **`ILightShape`** — Interface for flame seed patterns (candle, lightning, Batman logo, Borg ring, Sauron eye)
- **`ILightPen`** — Controls how new heat is seeded (fill density, intensity range, full vs binary range)
- **`ICoolingStrategy`** — Strategy pattern for cooling behavior
- **`SimpleSprite`** / **`DynamicSprite`** / **`LayeredSprite`** — Rendering hierarchy for compositing multiple flame layers
- **`RealtimeLightEffect`** — The core render loop with double-buffered flame computation
- Supports both Windows and Linux (with DPI-aware scaling adjustments)
- Experimental multithreaded rendering with `RealtimeFireBatLogoOptimizedMT_ThreadPool`

## Project History

The project dates back to February 2000, inspired by someone claiming Smalltalk was too slow for real-time fire generation.
I proved them wrong with a prototype, and have been building on that foundation ever since — adding palettes, cooling maps,
flame shapes, lightning, the Borg plasma disc, the Eye of Sauron, static sprites and text.

### Related Projects

#### Predecessor Projects

FireDemo grew directly from [`Flames.st`](https://github.com/redgreenshift/Flames.st), a Smalltalk-80 project created to
demonstrate that real-time fire graphics were possible in the language. It
expanded that idea into a broader real-time lighting-effects playground
including fire, lightning, and plasma.

[`Greenshift`](https://github.com/redgreenshift/Greenshift) is a looser ancestor to FireDemo.
It carried forward ideas including `BitCanvas`, frame-to-frame decay values, and configurable color palettes,
while exploring a broader range of real-time lighting effects.


## AI Policy

<p align="center">
  <a href="https://en.wikipedia.org/wiki/Vibe_coding">
  <img
    src="assets/no-vibe-coding.jpg"
    alt="A humorous image summarizing the project's policy against unreviewed vibe coding: “Vibe coding? We don't do that here.”"
  />
  </a>
</p>

Contributions from AI agents are welcome, provided they are reviewed by a
human before being committed. Every change MUST be approved by a real person;
approval by an automated process or another AI agent alone is insufficient.

AI tools may be used to suggest code ideas or help draft comments, but all
code is reviewed by the project author before committing. Code that the
author does not fully understand is not committed.

## License

FireDemo is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; version 2 only.

FireDemo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; see [`LICENSE`](LICENSE).

---

## Contact & Links

* [GitHub Repository](https://github.com/redgreenshift/firedemo)
* [Website](http://greenshift.net)
* **Author:** [Jared Ivey](mailto:jared.ivey+greenshift@outlook.com)
