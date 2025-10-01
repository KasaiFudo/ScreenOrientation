# ✅ Features Recap

📱 Responsive portrait / landscape UI.

🛠 Built-in update service prefab.

⚡ Debounce for rapid orientation changes.

🔄 Runtime detection of new components.

🎥 Smooth animated transitions.

🖥 Powerful editor tools for testing & setup.

# 🔧 Installation (via UPM)

You can install this package directly in Unity using the Unity Package Manager.

**Open Unity → Window → Package Manager.**

**Click + → Add package from git URL…**

Paste your repo URL:

```bash
https://github.com/KasaiFudo/ScreenOrientation.git
```

Unity will fetch and install it.

Alternatively, you can add it directly to your manifest.json:

```json
"com.kasaifudo.screenorientation": "https://github.com/KasaiFudo/ScreenOrientation.git"
```

# 🚀 Quick Start

## Prefab Setup

**Right-click in the Hierarchy → GameObject → Screen Orientation → Add update service.**

This will spawn the built-in prefab `OrientationUpdateService.prefab` to the scene from the package.

It automatically loads the config and starts orientation updates.

## Configuration

The default OrientationConfig is already included in the package as default, of course. 
But if you want to make your oun to manage default settings - create it in:
```
Assets/**YourPath**/Resources/Configs/
```

To create use:

**Click empty space → Create → Scriptable Object → OrientationConfig**

After creating - configure it as you wish.

Adjust settings in the Inspector:

`Ignore Rapid Changes` – debounce quick rotations.

`Rapid Change Threshold` – minimum time between updates.

`Detect new components in runtime` – scan scene dynamically(should enable it for test. Requires some resources.

`Global animation delay` – applies to all components.

## Add Components

Attach orientation-aware components to your UI:

`RectTransformOrientation` → anchors, positions, size.

`LayoutElementOrientation` → layout constraints.

`HVLayoutGroupOrientation` → padding, spacing, alignment for vertical and horizontal layout groups.

`TextOrientation` → TMP font size.

You can add your oun components! Override from `OrientationAwareComponent` and his abstract methods. For animation, you should aslo override virtual method: `ApplyInterpolatedValues`.

# 🎮 Usage

## 1. Editing Portrait/Landscape Data

Set up your UI for Portrait in the Editor.

Use the context menu (⋮ on component) → Rewrite Portrait Data.

Switch Game view to Landscape, adjust values.

Save with Rewrite Landscape Data.

The component now adapts automatically at runtime.

## 2. Testing in Editor

Open: Tools → Orientation Testing → Open Orientation Test Window.

Switch between portrait/landscape with buttons.

Inspect all OrientationAwareComponents in the scene or prefab.

Quickly select, debug, and apply orientation without entering Play mode.

## 3. Runtime Behavior

On device rotation, the OrientationUpdateService notifies all registered components.

Components apply portrait/landscape values instantly or via animation (if enabled).

New components can be auto-detected at runtime if enabled in config.

# 🎬 Animations

Each component has an Animation Data block:

**Enable/disable animation.**

**Delay before transition.** — Controls by config if 0, but rewrite it if not 0.

**Transition duration.**

**Custom animation curve.**

If animations are enabled, values interpolate smoothly instead of snapping.