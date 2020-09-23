# Auto-AutoRun

Auto-AutoRun is an auto-run for those who want to distribute apps using CDs/DVDs and have no programming knowledge.

It uses a tree view to show apps, which is populated depending on the contents of the disk.

To use this auto-run you (distributor) need to be able to:

- Use [GitHub](https://github.com)
- Work with file explorer
- Write markdown files (markdown is an easy to learn markup language)

The window contains:

- A tree view on the left showing the apps, which content is populated depending on the contents of the disk
- A tabbed documentation in the middle, which changes according to the selected app
- An optional version selector next to the tree view and above the documentation, which changes according to the selected app
- A list of button for setup, activation, etc. which changes according to the selected app and version
- An icon and a title showing the name and icon of the selected app

## How to make a folder tree for use with Auto-AutoRun

### Nodes

#### Description

Nodes can be either folder or apps. They appear as items in the apps tree.

#### Contains

[Actions](#actions), [versions](#versions) and [documentation](#documentation)

#### Creation

1. make a folder in the apps directory. A node's name is the same as it's folder's name

   Any name is accepted, except those staring with either an underscore ( `_` ), or the letter `v`. These letters are reserved for [actions](#actions) and versions

#### Folder tree example

- `Auto-AutoRun.exe`
- `Apps` -> Root Node
  - `Browsers` -> Node
    - `Chrome` -> Node
      - ...
    - `FireFox` -> Node
      - ...
    - `Edge` -> Node
      - ...

#### Appearance in AutoRun interface

They appear as items in the apps tree.

### Actions

#### Description

Actions are files that can be opened by clicking a button on the Auto-AutoRun window. Each action has it's own button. The buttons appear on the top-right of the window.

#### Creation

1. Inside a [node](#nodes) folder, create a new folder starting with an underscore ( `_` ). The action's name is the name of it's folder's name excluding the underscore.

   **Note: Do not use the name `_docs` because it is reserved.**
2. Inside the action folder, create or paste the file to be opened when the action is invoked (e.g. setup.exe) and make sure it's name (excluding extension) is the same as the action name.

   Acceptable file types are:

   1. Executable files (.exe)
   2. Windows installer files (.msi)
   3. Zip files (.zip)
   4. RAR files (.rar)
   5. Text file (.txt):

      Text files are not opened. They are considered as pointers to the actual file, which can be of any type. these files should contain a **relative** path to the actual file. An example is the Visual Studio setup, which doesn't work if it gets renamed. (e.g. It should be always `vs_community.exe`)

#### Folder tree example

- `Google Chrome` -> Node
  - `_Install` -> Action
    - `Install.exe` -> Opens when 'Install' button is clicked
- `Visual Studio 2015` -> Node
  - `_Community` -> Action
    - `Community.txt` -> content: `vs_community.exe`
    - `vs_community.exe` -> Opens when 'Community' button is clicked
    - `packages`
      - ...
  - `_Professional` -> Action
    - `Professional.txt` -> content: `vs_professional.exe`
    - `vs_professional.exe` -> Opens when 'Professional' button is clicked
    - `packages`
      - ...
  - `_Enterprise` -> Action
    - `Enterprise.txt` -> content: `vs_enterprise.exe`
    - `vs_enterprise.exe` -> Opens when 'Enterprise' button is clicked
    - `packages`
      - ...

#### Appearance in AutoRun interface

Actions appear as buttons in the Auto-AutoRun interface in the top-right position.

Clicking them opens the exe/msi/zip/rar file

Clicking it with **middle mouse button** shows the exe/msi/zip/rar file in file explorer.

### Documentation

#### Description

A documentation can contain one or more markdown files to be displayed, an icon, and a list of screen-shots.

#### Creation

1. Create a folder inside of the node folder, named `_docs`

##### Markdown pages

1. Create/paste any markdown file to the `_docs` folder. Make sure it has the extension `.md`

##### Icon

1. Create/paste the icon file and rename it to `icon`. Supported formats are: `.png`, `.ico`, `.jpg`, `.jpeg`, `.jfif`, `.bmp`

   Example: `icon.ico`, `icon.png`

##### Screen-shots

1. Create a folder named `_screenshots` inside the `_docs` directory
2. Inside the folder, insert image files. Most popular formats except `.webp` and `.svg` are supported.

#### Folder tree example

- `Visual Studio` -> Node
  - ...
  - `_docs` -> Documentation
    - `icon.png` -> <img src="https://visualstudio.microsoft.com/wp-content/uploads/2019/06/BrandVisualStudioWin2019-3.svg" alt="Visual Studio icon" class="" style="vertical-align: middle;" height="20">
    - `description.md` -> markdown file
    - `_screenshots` -> screen-shots folder
      - `code editor.png`
      - `UWP designer.png`
      - `image editor.png`
      - `debugger.png`
      - `start page.png`

#### Appearance in AutoRun interface

##### Markdown pages

Displayed in the center of the app, each markdown file has it's own tab, with the file name (without extension) shown as tab name

##### Icon

Shown on the left of the [node](#nodes) name, with 32px size

##### Screen-shots

Shown in a separate tab named 'Screenshots', Each picture is 600px wide, 400px high

### Versions

#### Description

Versions are used to have different versions of a software in a single [node](#nodes). They are a bit similar to [nodes](#nodes) in folder structure as they contain [actions](#actions), but are completely different.

#### Contains

[Actions](#actions), a markdown file

#### Creation

1. Inside the node folder, create a new folder starting `v`. The rest of the name is the version's name. For example a version with folder name `v5.4.3` has the name `5.4.3`. (Any name can be used as long as it doesn't contain illegal characters for paths (`\/:*?"<>|`))
2. Create the [actions](#actions) inside the version folder, as you did when creating [nodes](#nodes).
3. ***[Optional]*** Create a markdown file named `info.md` to show it as a separate tab named "version: < version name >". You can insert a changelog or breaking changes warning there.

#### Folder tree example

- `Visual Studio` -> Node
  - `_docs` -> Documentation
    - ...
  - `v2015`
    - `info.md` -> Contains new features in vs2015
    - `_Install`
      - ...
  - `v2017`
    - `info.md` -> Contains new features in vs2017
    - `_Install`
      - ...
  - `v2019`
    - `info.md` -> Contains new features in vs2019
    - `_Install`
      - ...

#### Appearance in AutoRun interface

A drop-down (combo-box) appears below the title of the node, with which you can select the version you want to install.

Version actions are shown as node actions, but with a little difference:

- Only the actions of the selected version can be seen
- They appear below the node actions (a bit lower in position)
- Middle-click does not work with it (although it is implemented)

## How to use it inside a CD or DVD

NOTE: Step 3 does not have any effect if you're not using a CD/DVD.

1. Download Auto-AutoRun executable from the [releases](https://github.com/Mammad900/Auto-AutoRun/releases) section and copy it to the root of the CD/DVD with the name `autorun.exe`
2. ***[Optional]*** Copy the icon with `.ico` format to the root folder with the name `Icon.ico`
3. Create a file named `autorun.inf`, open it with notepad (which is usually default) and copy-paste the following into it:

   ```properties
   [AutoRun]
   OPEN=autorun.exe
   ICON=Icon.ico
   ```

4. Change the label of the CD/DVD because it 
5. Copy the contents of the root node into the root folder (not the folder itself)
6. Now your app CD/DVD is ready! (Test it before publishing)
