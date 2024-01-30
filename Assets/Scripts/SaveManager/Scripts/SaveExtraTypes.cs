using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveManager.Scripts
{
    /// <summary>
    /// Holds the values required to save a Vector2
    /// </summary>
    [Serializable]
    public class SaveVector2
    {
        [FormerlySerializedAs("_x")] public float x;
        [FormerlySerializedAs("_y")] public float y;

        /// <summary>
        /// Get & Set the Vector 2 that is saved
        /// </summary>
        public Vector2 Vector2
        {
            get => new(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }
    }

    /// <summary>
    /// Holds the values required to save a Vector3
    /// </summary>
    [Serializable]
    public class SaveVector3
    {
        [FormerlySerializedAs("_x")] public float x;
        [FormerlySerializedAs("_y")] public float y;
        [FormerlySerializedAs("_z")] public float z;

        /// <summary>
        /// Get & Set the Vector 3 that is saved
        /// </summary>
        public Vector3 Vector3
        {
            get => new(x, y, z);
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }
    }

    /// <summary>
    /// Holds the values required to save a Vector4
    /// </summary>
    [Serializable]
    public class SaveVector4
    {
        [FormerlySerializedAs("_x")] public float x;
        [FormerlySerializedAs("_y")] public float y;
        [FormerlySerializedAs("_z")] public float z;
        [FormerlySerializedAs("_w")] public float w;

        /// <summary>
        /// Get & Set the Vector 4 that is saved
        /// </summary>
        public Vector4 Vector4
        {
            get => new(x, y, z, w);
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
                w = value.w;
            }
        }
    }

    /// <summary>
    /// Holds the values required to save a Color
    /// </summary>
    [Serializable]
    public class SaveColor
    {
        [FormerlySerializedAs("_r")] public float r;
        [FormerlySerializedAs("_g")] public float g;
        [FormerlySerializedAs("_b")] public float b;
        [FormerlySerializedAs("_a")] public float a;

        /// <summary>
        /// Get & Set the Color that is saved
        /// </summary>
        public Color Color
        {
            get => new(r, g, b, a);
            set
            {
                r = value.r;
                g = value.g;
                b = value.b;
                a = value.a;
            }
        }
    }
}