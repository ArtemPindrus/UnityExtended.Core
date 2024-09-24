# UnityExtended (Core)
Package that provides extensions and utilities I find useful when developing 3D games in Unity.
Made for personal use and expanded to public, so don't expect any support, but don't hesitate to contact me.

Use it for anything. Commercial use as well. No credits required.

The code is thoroughly documented. If you wonder what any method or utility does - just read the summary on the definition.

# Dependencies
- [Tri-Inspector](https://github.com/codewriter-packages/Tri-Inspector)

# Supported versions:
- Unity 2022.3
- Unity 6+

I haven't tested versions prior to 2022.3.

Unity 6 presented some good API changes, so I try to accommodate to both APIs with conditional compilation, but if you encounter compilation errors on 2022.3, let me know.

# Modules
Some parts of package are optional and are installed separately.
- [UnityExtended.AI](https://github.com/ArtemPindrus/UnityExtended.AI/tree/main)

# Installation
- Install all the [Dependencies](#dependencies).
- Clone repository into desired folder inside Assets folder of your project. (the same applies for modules)

# Features
## Selective Kinematics with OnContactImpulseModifier
OnContactImpulseModifier can be used to modify the inverse mass and inverse inertia scales during collisions of Rigidbodies (effectively modifying impulses applied to bodies).
One of the use cases for it is Selective Kinematics, which is Rigidbody's ability to ignore action forces from SOME object, while applying action & reaction forces to them AND receiving action forces from OTHER Rigidbodies.
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/UmZfIb84_9U/0.jpg)](https://www.youtube.com/watch?v=UmZfIb84_9U)
