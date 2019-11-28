<h1>Mouse Trap</h1>

![Last commit][commit]
![Issues][issues]
![Language][language]
![License][license]

Mouse Trap restricts the mouse cursor to the area of a specific window.

Full-screen games typically capture the mouse, preventing it from leaving the game area. Sometimes they don't, and clicking or using the mouse wheel can result in the window losing focus, or scrolling in a different window.

Some game examples are; Morrowind, Oblivion, Skyrim, Fallout 3, Fallout: New Vegas and Fallout 4. It can also benefit windowed games and programs where leaving the area is undesirable.

Particularly useful for dual-monitor setups, but can be used for any number of displays greater than zero.

<p align="center"><br><img src="img/TransAppIcon.png" height="128"></p>

## Download

Mouse Trap can be downloaded from the [releases page](https://github.com/Temetra/MouseTrap/releases).

## Details

* This is a Windows WPF application.
* Designed and tested on Windows 10.
* Compatibility with Window 7 (currently untested).
* Requires [.NET Framework 4.7.2](https://docs.microsoft.com/en-us/dotnet/framework/install/on-windows-10).

## Instructions

### Choosing a window to lock

A window can be chosen from a list of currently open applications. Enabling the lock will restrict the mouse to the boundaries of the window, whenever that window has focus. Switching to another program will free the mouse (<kbd>Alt</kbd>+<kbd>Tab</kbd>), allowing you to unlock the window.

<p align="center"><img src="img/choose-window.png"></p>

### Specifying a program

Alternatively, a specific executable path can be entered. When the lock is enabled, Mouse Trap waits until the first window for this executable gains focus.

<p align="center"><img src="img/find-program.png"></p>

### Enabling the mouse lock

Once the lock is enabled, the mouse will be limited to the boundaries of the window whenever it has focus. The boundary can be adjusted using the "padding" values, to increase or decrease the restricted area.

<p align="center"><img src="img/lock-screen-background.png"></p><br>

### Once the lock is active

An indicator appears when the window has focus. The mouse is now locked to the window. To cancel the lock, switch to another window (<kbd>Alt</kbd>+<kbd>Tab</kbd>); or the window can be closed.

If the window is closed, the application returns to the list or file tab.

<p align="center"><img src="img/lock-screen-foreground.png"></p>

### Administrator notification

If the target program is run as administrator, Mouse Trap will also need to be run as administrator for the mouse hook to work. The lock screen will warn you if you need to take action.

<p align="center"><img src="img/lock-screen-warning.png"></p>

### Hotkeys

* <kbd>F1</kbd> - Choose Window
* <kbd>F2</kbd> - Find Program
* <kbd>F3</kbd> - Lock/Unlock Mouse
* <kbd>F5</kbd> - Refresh list in Choose Window mode
* <kbd>Alt</kbd>+<kbd>F4</kbd> - Quits program

## Built With

* [Windows Presentation Foundation (WPF)](https://docs.microsoft.com/en-us/visualstudio/designers/introduction-to-wpf?view=vs-2017) - Desktop client framework

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* [denizimo01 on Pixabay](https://pixabay.com/en/mouse-mouse-icon-symbol-pc-cursor-2487884/) for the logo
* [Patterns - WPF Apps With The Model-View-ViewModel Design Pattern](https://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030) for RelayCommand class
* [Twitter Emoji glyphs](https://github.com/twitter/twemoji) (modified) licensed under CC-BY 4.0: https://creativecommons.org/licenses/by/4.0/

[commit]: https://img.shields.io/github/last-commit/Temetra/MouseTrap.svg?style=flat
[issues]: https://img.shields.io/github/issues/Temetra/MouseTrap.svg?style=flat
[language]: https://img.shields.io/github/languages/top/Temetra/MouseTrap.svg?style=flat
[license]: https://img.shields.io/badge/license-MIT-blue.svg
