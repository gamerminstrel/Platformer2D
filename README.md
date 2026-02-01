
# Platformer2D

A small throwaway project to get familiar with using **GitHub + Godot (.NET)** in a real workflow.

---

## Prerequisites

This section covers everything needed to build and work on this project from scratch.

### Required Software

The project is currently developed using:

* **Godot (.NET version)**
  [https://godotengine.org/download/linux/](https://godotengine.org/download/linux/)

* **Visual Studio Code**
  [https://code.visualstudio.com/](https://code.visualstudio.com/)

* **.NET SDK**
  [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)

---

## First-Time Setup

This mirrors my current development workflow.

### 1. Git Setup

If you’ve never used Git on this machine:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
```

Clone the project:

```bash
git clone https://github.com/gamerminstrel/Platformer2D.git
cd Platformer2D
code ./
```

---

### 2. VS Code Extensions
# Platformer2D

A small throwaway project to get familiar with using **GitHub + Godot (.NET)** in a real workflow.

---

## Prerequisites

This section covers everything needed to build and work on this project from scratch.

### Required Software

The project is currently developed using:

* **Godot (.NET version)**
  [https://godotengine.org/download/linux/](https://godotengine.org/download/linux/)

* **Visual Studio Code**
  [https://code.visualstudio.com/](https://code.visualstudio.com/)

* **.NET SDK**
  [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)

---

## First-Time Setup

This mirrors my current development workflow.

### 1. Git Setup

If you’ve never used Git on this machine:

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
```

Clone the project:

```bash
git clone https://github.com/gamerminstrel/Platformer2D.git
cd Platformer2D
code ./
```

---


Install the following extensions:

* C#
* C# Dev Kit
* C# Tools for Godot
* Godot Docs for C#
* godot-tools
* Godot .NET Tools

Follow the setup instructions on each extension’s page (especially for Godot integration).

---

### 3. Godot Setup

1. **Import the project**

   * Open Godot and import `project.godot` from the `Platformer2D` directory.

2. **Download export templates**

   * `Editor → Manage Export Templates`
   * Click **Download**
     (You may need to switch to online mode.)

3. **Android support (optional)**

   * `Editor → Install Android Build Template`
     Only needed if exporting to Android.

4. **Set up external editor (VS Code)**

   * `Editor → Editor Settings → Text Editor → External`
   * Enable **Use External Editor**
   * Set **Exec Path** to the path used by the `code` command.

---

## Notes

* This project assumes a **Linux + VS Code + Godot .NET** workflow.
* Other setups may work, but aren’t officially tested.
* Expect things to change — this is primarily a learning project.
