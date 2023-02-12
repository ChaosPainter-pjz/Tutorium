// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2019 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace Steamworks
{
    [System.Serializable]
    public struct PublishedFileUpdateHandle_t : System.IEquatable<PublishedFileUpdateHandle_t>, System.IComparable<PublishedFileUpdateHandle_t>
    {
        public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(0xffffffffffffffff);
        public ulong m_PublishedFileUpdateHandle;

        public PublishedFileUpdateHandle_t(ulong value)
        {
            m_PublishedFileUpdateHandle = value;
        }

        public override string ToString()
        {
            return m_PublishedFileUpdateHandle.ToString();
        }

        public override bool Equals(object other)
        {
            return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t) other;
        }

        public override int GetHashCode()
        {
            return m_PublishedFileUpdateHandle.GetHashCode();
        }

        public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
        {
            return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
        }

        public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
        {
            return !(x == y);
        }

        public static explicit operator PublishedFileUpdateHandle_t(ulong value)
        {
            return new PublishedFileUpdateHandle_t(value);
        }

        public static explicit operator ulong(PublishedFileUpdateHandle_t that)
        {
            return that.m_PublishedFileUpdateHandle;
        }

        public bool Equals(PublishedFileUpdateHandle_t other)
        {
            return m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
        }

        public int CompareTo(PublishedFileUpdateHandle_t other)
        {
            return m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
        }
    }
}

#endif // !DISABLESTEAMWORKS