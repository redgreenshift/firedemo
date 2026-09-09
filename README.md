[![Previous: Greenshift](https://img.shields.io/badge/←_PREV-gray.svg?style=for-the-badge)](https://github.com/redgreenshift/Greenshift/blob/main/README.md)

![FireDemo](assets/firedemo-logo.svg)

[![Raspberry Pi](https://img.shields.io/badge/Raspberry%20Pi-A22846.svg?logo=raspberrypi&logoColor=white)](https://www.raspberrypi.com/)
[![Linux](https://img.shields.io/badge/Linux-FCC624.svg?logo=linux&logoColor=black)](https://www.linux.org/)
[![Windows](https://img.shields.io/badge/Windows-0078D4.svg?logo=data:image/svg%2bxml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48IS0tIE9yaWdpbmFsIGZyb206IFNWRyBSZXBvLCB3d3cuc3ZncmVwby5jb20sIEdlbmVyYXRvcjogU1ZHIFJlcG8gTWl4ZXIgVG9vbHM7IGhhbmQgbW9kaWZpZWQgdG8gd2hpdGUgbW9ub2Nocm9tZSAtLT4KPHN2ZyBmaWxsPSIjRkZGRkZGIiB3aWR0aD0iODAwcHgiIGhlaWdodD0iODAwcHgiIHZpZXdCb3g9IjAgMCA1MTIgNTEyIiBpZD0iaWNvbnMiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyI+PHBhdGggZD0iTTMxLjg3LDMwLjU4SDI0NC43VjI0My4zOUgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDMwLjU4SDQ3OS43VjI0My4zOUgyNjYuODlaIi8+PHBhdGggZD0iTTMxLjg3LDI2NS42MUgyNDQuN3YyMTIuOEgzMS44N1oiLz48cGF0aCBkPSJNMjY2Ljg5LDI2NS42MUg0NzkuN3YyMTIuOEgyNjYuODlaIi8+PC9zdmc+)](https://www.microsoft.com/windows)
[![C#](https://img.shields.io/badge/C%23-512BD4.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET Framework 4.8](https://img.shields.io/badge/4.8-512BD4.svg?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/platform/support/policy/dotnet-framework)
[![License: GPL-2.0-only](https://img.shields.io/badge/License-GPL--2.0--only-F58220.svg)](LICENSE)

A real-time flame and plasma effect playground built with C# and WinForms (.NET Framework 4.8). Originally inspired by the challenge of generating realistic fire in Smalltalk — and proven to work just as well in C# over two decades later.

## What it does

FireDemo renders animated flame effects using a 2D grid of intensity values with pixel averaging, producing realistic-looking fire, lightning, and plasma patterns. The core idea is simple: each frame, the brightness of a pixel is the average of neighboring pixels with some cooling applied — but with well-tuned palettes and cooling maps, the result looks surprisingly lifelike.

## Effects

| Effect | Description |
|--------|-------------|
| **Realistic Flame** | Warm, natural-looking fire with multi-point linear color palettes and cooling maps |
| **Batman Logo** | The Bat-symbol rendered in fire, with options for single and multithreaded rendering |
| **Lightning** | Branching lightning bolts that cascade down the flame grid |
| **Borg Plasma** | A Star Trek Borg Alcove regeneration disc with inner plasma and outer ring |
| **RainBORG** | Multiple plasma discs in various colors displayed simultaneously |
| **Sauron** | The Eye of Sauron, built from layers of flame and lightning (v3.5 & v3.6) |
| **Candle** | A small, gentle candle flame |
| **Rainbow Fire** | Multiple independent flames with a gradient palette |
| **Status Displays** | "Away", "Busy", "Available", "DND" status screens combining fire effects with pixel art |

## Modes

The application has two main views:

1. **Demo Mode (Form2)** — The default view with a sequence of animated effects. Click anywhere on the window or press "Start!" to begin the animated demo. Use the "DND", "Busy", "Available", and "Away" buttons to switch to the corresponding status display.

2. **Advanced UI (Form1)** — Full parameter control: adjust fire dimensions, color palettes (4-point linear, realistic curve, flat), cooling strategies (constant, map-based, with shifting/rotation), seed coal values, and interpolation mode.

3. **Fast Render (Form3)** — An optimized render path that bypasses `OnPaint` for higher framerates. EXPERIMENTAL, and did not work the way I had hoped.

## How it works

The fire algorithm works by maintaining a 2D grid where each cell holds an intensity value (0–255). Each frame:

1. **Averaging** — Each pixel's new intensity is the average of its neighbors (configurable which neighbors — any combination of the 9 pixels in a 3x3 block)
2. **Cooling** — Each pixel loses some energy based on a cooling strategy (constant decay or a cooling map with density/smoothing/rotation)
3. **Seeding** — New heat is added along the bottom or at specific shapes (Batman logo, candle, lightning bolts)
4. **Color mapping** — Intensity values are mapped through a palette (8-bit indexed or 32-bit direct color)

### Palette styles

- **Flat** — Single-color palette where each intensity level scales the RGB channels uniformly
- **4-Point Linear** — Four user-chosen colors interpolated at 0%, 33%, 66%, 100% of intensity
- **Realistic** — A tuned curve of 7 color stops that mimics real flame behavior (dark core -> orange -> yellow -> white -> blue)
- **Lightning** — High-contrast palette optimized for bolt effects
- **Plasma** — Rich blues and teals for Borg-style discs

### Cooling strategies

- **Constant** — Uniform decay rate across the entire grid
- **Map** — Spatially varying cooling with configurable density, min/max, smoothing, shifting, and rotation (more realistic, produces the characteristic flame movement patterns)

## Building

Requirements:
- Visual Studio 2019+ (or the .NET Framework 4.8 SDK)
- The project uses the standard `.csproj` format

Open `FireDemo.sln` and build. The default entry point launches **Form2** (Demo Mode).

## Configuration

In **Form2**, you can adjust:
- **Fire dimensions** — Width and height of the flame grid (affects performance and look)
- **Magnification** — How much the flame is scaled up on screen
- **Interpolation mode** — NearestNeighbor (pixelated), Bilinear (smooth), or Bicubic (extra smooth)
- **Cooldown strategy** — Constant vs Map, with tuning for density, smoothing, shifting, and rotation

In **Form1** (Advanced UI), explore a wider set of options and palettes. Useful for determining parameters to achieve a particular desired effect without needing to recompile.

## Architecture highlights

- **`ILightShape`** — Interface for flame seed patterns (candle, lightning, Batman logo, Borg ring, Sauron eye)
- **`ILightPen`** — Controls how new heat is seeded (fill density, intensity range, full vs binary range)
- **`ICoolingStrategy`** — Strategy pattern for cooling behavior
- **`SimpleSprite`** / **`DynamicSprite`** / **`LayeredSprite`** — Rendering hierarchy for compositing multiple flame layers
- **`RealtimeLightEffect`** — The core render loop with double-buffered flame computation
- Supports both Windows and Linux (with DPI-aware scaling adjustments)
- Experimental multithreaded rendering with `RealtimeFireBatLogoOptimizedMT_ThreadPool`

## Project History

The project dates back to ~2001, inspired by someone claiming Smalltalk was too slow for real-time fire generation. The creator proved them wrong with a prototype, and has been building on that foundation ever since — adding palettes, cooling maps, flame shapes, lightning, the Borg plasma disc, and the Eye of Sauron.

### Related Projects

#### Predecessor Projects

FireDemo grew directly from [Flames.st](LINK), a Smalltalk-80 project created to
demonstrate that real-time fire graphics were possible in the language. It
expanded that idea into a broader real-time lighting-effects playground
including fire, lightning, and plasma.

<a href="https://github.com/redgreenshift/Greenshift">`Greenshift`</a> is a looser ancestor to FireDemo. It carried forward ideas including `BitCanvas`, frame-to-frame decay values, and configurable color palettes, while exploring a broader range of real-time lighting effects.


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

You should have received a copy of the GNU General Public License along with this program; see <a href="LICENSE">`LICENSE`</a>.

---

## Contact & Links

* [GitHub Repository](https://github.com/redgreenshift/firedemo)
* [Website](http://greenshift.net)
* **Author:** [Jared Ivey](mailto:jared.ivey+greenshift@outlook.com)
