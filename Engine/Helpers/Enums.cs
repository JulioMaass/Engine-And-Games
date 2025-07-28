using System;

namespace Engine.Helpers;

public enum Dir { Up, Down, Left, Right, Forward, Backward } // Named Dir to avoid conflict with Direction component
public enum Switch { On, Off }
public enum Axis { X, Y } // Only one axis can be active at a time

// Uses enums for bitwise operations
[Flags] // This attribute allows to print the enum values as a combination of flags
public enum Axes // Multiple axes can be active at the same time
{
    None = 0,
    X = 1,
    Y = 2,
    Both = X | Y
}