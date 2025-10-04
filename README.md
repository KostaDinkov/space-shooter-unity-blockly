# Space Shooter 🚀

An innovative educational game that teaches programming fundamentals through interactive space-themed puzzles. Students learn programming concepts by controlling a space drone using visual blocks that generate real C# code.

![alt text](screenshot.png)

## 🎯 Project Overview

This R&D project explores the integration between browser-based visual programming (Google Blockly) and Unity game engine. Students drag and drop code blocks to control their spaceship, learning programming concepts like loops, conditionals, variables, and functions in an engaging game environment.

## ✨ Key Features

### Educational Gameplay

- **Visual Programming**: Drag-and-drop Blockly interface generates real C# code
- **Progressive Learning**: Each level introduces fundamental programming concepts
- **Space-Themed Puzzles**: Navigate asteroids, collect objects, and complete missions
- **Real-time Code Execution**: Generated code runs directly in Unity engine

### Technical Innovation

- **Browser-Unity Communication**: Seamless integration between web interface and game
- **Runtime Code Compilation**: Dynamic C# script execution using Roslyn
- **Save/Restore System**: Persistent block workspace and progress tracking
- **Event-Driven Architecture**: Modular system for objectives and game mechanics

## 🎮 Game Mechanics

- **Grid-Based Movement**: Structured navigation system
- **Objective System**: Dynamic challenge tracking and completion
- **Cargo Management**: Collect and manage space objects
- **Collision Detection**: Realistic space physics and interactions
- **Achievement System**: Progress tracking and rewards

## 🛠️ Technology Stack

- **Unity 2020.3.21f1** - Game engine (LTS version)
- **Google Blockly** - Visual programming interface
- **ZFBrowser** - Embedded browser component for Unity
- **Microsoft.CodeAnalysis.CSharp.Scripting** - Runtime C# compilation
- **UniTask** - Async/await support for Unity
- **Odin Inspector** - Enhanced Unity editor tools

## 📚 Learning Progression

### Level Structure

1. **Sequential Execution** - Basic command sequences
2. **Loops** - Repetitive actions and iteration
3. **Conditional Statements** - Decision-making logic
4. **Variables** - Data storage and manipulation
5. **Functions** - Code organization and reusability
6. **Advanced Concepts** - Complex programming patterns

## 🏗️ Project Structure

```
Assets/
├── Scripts/
│   ├── Systems/          # Core game systems
│   ├── Commands/         # Player command interfaces
│   ├── Objectives/       # Challenge and goal tracking
│   ├── GameEvents/       # Event management system
│   └── SpaceObject/      # Game object definitions
├── Scenes/               # Game levels and menus
├── BrowserAssets/        # Web interface files
└── Documentation/        # Design documents
```

## 🚀 Getting Started

### Prerequisites

- Unity 2020.3.21f1 or later
- Modern web browser with JavaScript support
- Visual Studio or compatible C# IDE

### Development Setup

1. Clone the repository
2. Open project in Unity Hub
3. Install required packages via Package Manager
4. Build and run to test browser-Unity communication

## 🧪 Testing

The project includes comprehensive unit tests for:

- Player movement and collision detection
- Command execution and error handling
- Objective completion tracking
- Game state management

## 📈 Current Status

**Development Stage**: Early R&D Phase

- ✅ Core browser-Unity communication working
- ✅ Basic gameplay mechanics implemented
- ✅ Visual programming interface functional
- 🔄 Content creation and level design in progress
- 🔄 UI/UX polish and educational content expansion

## 🎯 Future Development

- **Content Expansion**: More levels and programming challenges
- **UI Enhancement**: Improved visual design and user experience
- **Performance Optimization**: Code compilation and execution improvements
- **Analytics Integration**: Learning progress tracking and insights
- **Accessibility Features**: Support for diverse learning needs

## ⚠️ Development Note

This project is currently in early development and research phase. It is not intended for production use or cloning outside of the development environment without proper setup and configuration.

## 📄 License

Educational research project - see documentation for usage guidelines.

---

*Combining the power of visual programming with immersive gameplay to make learning programming fun and accessible.*