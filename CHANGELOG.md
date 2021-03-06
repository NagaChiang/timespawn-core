# Changelog

## [Unreleased]

### Added

- Generic `Singleton` base class
- `CommonUtils.GetEnumCount()`
- `CommonUtils.GenerateRandomSeed()`
- `DotsUtils.CreateParallelWriter()`

### Changed

- Remove suffix from all ECS ComponentData class names
- Rename `EnumUtils` to `CommonUtils`
- Upgrade to Unity 2020.1.0f1
- Rename `DotsUtils.CreateECBFromSystem()` to `DotsUtils.CreateCommandBuffer()`
- Rename `VectorExtensions` to `MathExtension`
- Rename `ArrayExtensions` to `ArrayExtension`

### Removed

- All tween functionalities
- All grid and cell functionalities
- All `DotsUtils` about default world

### Fixed

- Compatibility to DOTS runtime

## [0.2.0] - 2020-05-26

### DOTS Tween

#### Added

- Translation, rotation and scale
- Loop, pingpong
- Pause, resume and stop
- Unit tests
  - Update and complete (loop, pingpong) systems
  - Pause

### DOTS Grid

#### Added

- `GridData` and `CellData` components and related utilities
- Grid debug draw system

### Math

#### Added

- Ease functions
  - `SmoothStart()`
  - `SmoothStop()`
  - `SmoothStep()`
  - `CrossFade()`

### Collections

#### Added

- Generic grid container `GridMap`

### System

#### Changed

- Rename `Singleton` to `SingletonBehaviour`

### Tests

#### Added

- `ECSTestFixture` as the base class for unit tests of DOTS
- `TestUtils`
  - `AreApproximatelyEqual()` for `double` and `quaternion`
  - `AreApproximatelyEqualFloat3()` for `float3`

### Misc

#### Added

- Vector conversions, such as `float3` to `Vector3`
- `enum Direction2D`
- `InputUtils`
  - `bool TryTranslateInputToDirection(Vector2 delta, out Direction2D outDirection, float deadZone = 1.0f)`
- `DotsUtils`
  - `T GetSystemFromDefaultWorld<T>() where T : ComponentSystemBase`
  - `EntityCommandBuffer CreateECBFromSystem<T>() where T : EntityCommandBufferSystem`

## [0.1.0] - 2019-12-29

### Collections

#### Added

- `bool Array.IsValidIndex(int index)`

### System

#### Added

- Generic singleton base class `Singleton`
