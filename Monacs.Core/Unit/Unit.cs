using System;

namespace Monacs.Core.Unit
{
    ///<summary>
    /// Type that has only one value.
    /// Used to replace void whenever some value is needed, e.g. you can return Task{Unit}.
    ///</summary>
    public readonly struct Unit : IEquatable<Unit>
    {
        private static Unit _default = default;

        ///<summary>
        /// The only value of <see cref="Unit" />.
        ///</summary>
        public static ref readonly Unit Default => ref _default;

        ///<summary>
        /// <see cref="Unit" /> is always equal to itself. There is only one possible value of <see cref="Unit" />.
        ///</summary>
        public bool Equals(Unit other) => true;

        ///<summary>
        /// <see cref="Unit" /> is only equal to itself.
        ///</summary>
        public override bool Equals(object obj) => obj is Unit;

        ///<summary>
        /// Hash Code of <see cref="Unit" /> is always 0.
        ///</summary>
        public override int GetHashCode() => 0;

        ///<summary>
        /// String representation of <see cref="Unit" /> is ().
        ///</summary>
        public override string ToString() => "()";

        ///<summary>
        /// <see cref="Unit" /> is always equal to itself.
        ///</summary>
        public static bool operator ==(in Unit first, in Unit second) => true;

        ///<summary>
        /// <see cref="Unit" /> is always equal to itself.
        ///</summary>
        public static bool operator !=(in Unit first, in Unit second) => false;
    }
}