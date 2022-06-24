using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCPArena.Database.Modals
{
    public class SerializableVector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector()
            => new Vector3(X, Y, Z);

        public static List<Vector3> ToManyVectors(List<SerializableVector> serializableVectors)
            => serializableVectors.Select(serializableVector => serializableVector.ToVector()).ToList();

        public static SerializableVector FromVector(Vector3 vector)
            => new SerializableVector(vector.x, vector.y, vector.z);

        public static List<SerializableVector> FromManyVectors(List<Vector3> vectors)
            => vectors.Select(FromVector).ToList();

        public SerializableVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SerializableVector() {}

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }
    }
}