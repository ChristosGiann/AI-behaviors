# NPC Behaviour Prototypes

A Unity academic project exploring several rule-based behaviours for enemy and ally non-player characters.

The project was developed for **GDEV108 – Advanced Game Development** as part of master's-level coursework. Its purpose was to implement distinct NPC behaviours using Unity navigation, perception checks, simple state transitions and player interaction.

> **Status:** Completed academic prototype

## Overview

The project contains a small first-person environment with one ally behaviour and two different enemy behaviours:

- an interactable ally that can follow the player,
- a patrolling enemy that detects and chases the player through a field of view,
- a sleeping enemy that periodically wakes up and reacts when it detects player movement.

The focus of the assignment is the behaviour logic rather than a complete game loop or production-ready AI architecture.

## Implemented Behaviours

### Ally Follow Behaviour

The ally uses a `NavMeshAgent` and can be instructed to start or stop following the player.

- The player must be within the configured interaction range.
- Pressing `E` toggles the follow state.
- While active, the ally follows a target position behind the player.
- The desired follow distance and interaction range can be configured from the Unity Inspector.

```text
Player enters interaction range
              ↓
          Presses E
              ↓
      Toggle follow state
              ↓
NavMeshAgent follows behind player
```

### Patrol and Chase Enemy

The first enemy demonstrates waypoint patrol and field-of-view detection.

- Patrols through an ordered collection of waypoints.
- Waits briefly when reaching each waypoint.
- Uses configurable vision range and vision angle values.
- Switches from patrol to chase after detecting the player.
- Uses a `NavMeshAgent` to pursue the player's current position.
- Visualizes its field of view with a `LineRenderer`.
- Restarts the gameplay scene when it catches the player.

```text
Patrol waypoints
      ↓
Check distance and viewing angle
      ↓
Player detected?
  ├── No → Continue patrol
  └── Yes → Chase player
                    ↓
             Player caught
                    ↓
              Restart scene
```

### Sleep and Wake Enemy

The second enemy uses a simple state-based behaviour with `Sleep` and `Wake` states.

- Alternates automatically between sleeping and waking at a configurable interval.
- Plays an audio warning when waking up.
- Gives the player a short reaction period before perception begins.
- Checks whether the player is within detection range.
- Uses a raycast and obstacle layer mask to test line of sight.
- Detects player movement by comparing the current and previous positions.
- Starts chasing after a short delay when movement is detected.
- Stops chasing when the player leaves its range, becomes obstructed or the enemy returns to sleep.
- Restarts the current scene when it catches the player.

```text
Sleep
  ↓ timed transition
Wake and play warning sound
  ↓ reaction delay
Check range, line of sight and movement
  ↓
Player moving and visible?
  ├── No → Keep observing
  └── Yes → Delay → Chase
  ↓ timed transition
Sleep and stop chasing
```

## Player Controls

| Action | Input |
|---|---|
| Move | `WASD` / Unity horizontal and vertical axes |
| Look around | Mouse |
| Toggle ally follow | `E` while near the ally |
| Return to menu | `F` |

## Technical Concepts

- Unity and C# gameplay scripting
- `NavMeshAgent` pathfinding
- Waypoint-based patrol
- Range and angle perception checks
- Field-of-view visualization with `LineRenderer`
- Raycast-based obstacle detection
- Simple enum-based NPC states
- Coroutines for timed transitions and reaction delays
- Interaction based on player proximity
- Scene management and restart conditions

## Technology

- **Unity:** 2022.3.26f1
- **Language:** C#
- **AI Navigation:** 1.1.5
- **Navigation:** Unity NavMesh and `NavMeshAgent`

## Project Structure

```text
Assets/
├── Scenes/
│   ├── Menu.unity
│   └── Gameplay.unity
└── Scripts/
    ├── AllyMovement.cs
    ├── EnemyMovement.cs
    ├── Enemy2Movement.cs
    ├── Menu.cs
    └── PlayerMovement.cs
```

### Main Scripts

| Script | Responsibility |
|---|---|
| `AllyMovement.cs` | Toggles and controls the ally's NavMesh follow behaviour |
| `EnemyMovement.cs` | Controls waypoint patrol, field-of-view detection and chasing |
| `Enemy2Movement.cs` | Controls sleep/wake states, movement detection and delayed chasing |
| `PlayerMovement.cs` | Provides first-person movement, mouse look and menu navigation |
| `Menu.cs` | Starts the gameplay scene or exits the application |

## Running the Project

1. Install Unity Hub.
2. Install **Unity 2022.3.26f1**.
3. Clone the repository:

```bash
git clone https://github.com/ChristosGiann/AI-behaviors.git
```

4. Add the cloned directory through Unity Hub.
5. Open `Assets/Scenes/Menu.unity`.
6. Press **Play**.

The required scenes are already included in the project's build settings.

## Academic Scope

This repository is a focused coursework prototype. It demonstrates several independent NPC behaviour patterns rather than a reusable production AI framework.

Possible extensions would include:

- explicit finite-state machines for all NPCs,
- returning enemies from chase to patrol,
- shared perception components,
- more robust line-of-sight checks,
- animation-driven state feedback,
- configurable behaviour data through ScriptableObjects,
- automated tests for state transitions.
