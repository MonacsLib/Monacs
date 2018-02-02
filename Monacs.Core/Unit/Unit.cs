using System;

namespace Monacs.Core.Unit
{
    public struct Unit : IEquatable<Unit>
    {
        public static Unit Default { get; } = default;

        public bool Equals(Unit other) => true;

        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode() => 0;

        public override string ToString() => "()";

        public static bool operator ==(Unit first, Unit second) => true;

        public static bool operator !=(Unit first, Unit second) => false;
    }
}