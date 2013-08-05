using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    public struct Pair<T1, T2> {
        private T1 _first;
        private T2 _second;

        public T1 First {
            get { return _first; }
            set { _first = value; }
        }
        public T2 Second {
            get { return _second; }
            set { _second = value; }
        }

        public Pair(T1 first, T2 second) {
            _first = first;
            _second = second;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (!(obj is Pair<T1, T2>)) return false;
            var other = (Pair<T1, T2>)obj;
            return this.First.Equals(other.First) && this.Second.Equals(other.Second);
        }

        public override int GetHashCode() {
            return First.GetHashCode() ^ Second.GetHashCode();
        }

        public override string ToString() {
            return String.Format("<{0}, {1}>", First, Second);
        }
    }
}
