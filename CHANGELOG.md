# Changelog

## 0.1.0

Initial release.

### Added

- **Skopka.Abstraction.OperationResult**
    - `OperationResult` / `OperationResult<T>` primitives
    - Structured `Error` model
    - Functional extensions: `Map`, `Bind`, `Match`, `Tap`, `TapError`, `Ensure`
    - LINQ support: `Select`, `SelectMany`, `Where`

- **Skopka.Abstraction.Serialization**
    - `Optional<T>` tri-state wrapper: Missing / Null / Value
    - System.Text.Json integration (`Skopka.Abstraction.Serialization.Json`): converter for `Optional<T>`

- **Skopka.Abstraction** (meta-package)
    - Single package that references all packages from this repository.