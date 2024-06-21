# Unity Shooting Range-like VR Game

This is the final project for the AI3618 Virtual Reality course.

![Demo](Demo.gif)

## Overview
This project is a VR shooting range game inspired by the Resident Evil 4 remastered edition. It uses Unity and integrates Joycon for an immersive VR experience. The game features realistic gun handling, interactive targets, and a scoring system. You can watch our demo video on Bilibili: [https://www.bilibili.com/video/BV1eCg1eqE8E/?spm_id_from=333.999.0.0&vd_source=c45f62c7b45d132c96cc1bf9cf10fa87](https://www.bilibili.com/video/BV1eCg1eqE8E/?spm_id_from=333.999.0.0&vd_source=c45f62c7b45d132c96cc1bf9cf10fa87).

## Features
- **Realistic Gun Handling**: The game uses XR Grab interactable with activate events for firing. The angle of the character's arms is bound to the angle of the Joycon.
- **Interactive Targets**: Targets have head and body colliders to detect collisions and score accordingly.
- **Scoring System**: Includes global static variables for score tracking and high scores.

## Components
### Joyconlib + XR Simulator
- **Angular Calculation**: Uses Kalman filtering to calculate orientation.
- **Reset**: Press Dpad_down on the Joycon to reset the position and orientation. The angle of the character's arms is bound to the angle of the Joycon.
- **Vibration**: Provides haptic feedback while firing.
- **Grab**: The ZR and ZL trigger buttons are used for holding the gun. 

### 3D Models
- **Characters**: Models include Ashley and Leon with animations and constraints applied via Mixamo. The left joystick controls character movement, while the right joystick controls the view.
- **Guns**: Equipped with XR Grab interactable for realistic handling and firing mechanisms. Press Dpad_up to fire. 
- **Hands**: Bindings for XR controller to "Grab" actions.

### Gameplay
- **Targets**: Implemented with head and body colliders to detect collisions and update scores.
- **Canvas**: Displays current score, high score, and other game statistics.

### Technical Details
- **IK Foot Solver**: Ensures realistic foot placement and movement.
- **Multi-Parent Constraint**: Bind the angle of Leon's head to the angle of the camera.

## Known Issues
- **Angle Calculation Errors**: Large elevation angles may cause calculation errors.
- **Bluetooth Compatibility**: Occasional crashes due to mismatched Bluetooth versions.
- **Collision Detection**: Possible to cheat by colliding with targets at close range.

## Future Enhancements
- **Reload and Recoil Mechanisms**: Implementing more realistic gun handling.
- **Difficulty Settings**: Adding various levels of difficulty.
- **Translational Targets**: Moving targets to increase challenge.
- **Weapon Variety**: Introducing different types of weapons.
- **Penalty Targets**: Adding targets that deduct points when hit.
- **Bonus and Hit Rate Detection**: Implementing continuous hit bonuses and tracking hit rates.

## Installation
1. Clone the repository.
   ```sh
   git clone https://github.com/your-repository-url
2. Open the project in Unity and ensure **your Joycon has already connected to your device before running**.

## Acknowledgments
**Special thanks to our teacher, assistant and seniors who paved the way for us :D**
