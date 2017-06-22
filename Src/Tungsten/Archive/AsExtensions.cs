﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

// Notes
// 2.23.2017 - It would seem that using AggressiveInLining breaks W.Encryption.RSA code, so don't use it

namespace W
{
    /// <summary>
    /// Extensions which convert objects of one type to another
    /// </summary>
    public static class AsExtensions
    {
        /// <summary>
        /// Use Generic syntax for the <bold>as</bold> operator.
        /// </summary>
        /// <typeparam name="TType">The type to convert the item reference to.</typeparam>
        /// <param name="this">The item to convert to type TType</param>
        /// <returns>Null if @this cannot be referenced as TType.  Otherwise, the item as TType</returns>
        /// <example><code>expression as type</code> becomes <code>expression&lt;type&gt;()</code></example>
        [System.Diagnostics.DebuggerStepThrough]
        public static TType As<TType>(this object @this) where TType : class
        {
            return @this as TType;
        }
        /// <summary>
        /// Converts a string to Base64 encoding
        /// </summary>
        /// <param name="this">The string to convert to Base64 encoding</param>
        /// <returns>The Base64 encoded string</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsBase64(this string @this)
        {
            return Convert.ToBase64String(@this.AsBytes());
        }
        /// <summary>
        /// Converts a byte array to Base64 encoding
        /// </summary>
        /// <param name="this">The byte array to convert to Base64</param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsBase64(this byte[] @this)
        {
            return Convert.ToBase64String(@this);
        }
        /// <summary>
        /// Converts a string to an encoded byte array
        /// </summary>
        /// <param name="this">The string to convert to an encoded byte array</param>
        /// <returns>A byte array encoding of the specified string</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static byte[] AsBytes(this string @this)
        {
            return System.Text.Encoding.UTF8.GetBytes(@this);
        }
        /// <summary>
        /// Converts an encoded byte array to a string
        /// </summary>
        /// <param name="this">The encoded byte array to conver to a string</param>
        /// <returns>The string representation of the encoded byte array</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsString(this byte[] @this)
        {
            return System.Text.Encoding.UTF8.GetString(@this, 0, @this.Length);
        }
        /// <summary>
        /// Converts an encoded byte array to a string
        /// </summary>
        /// <param name="this">The encoded byte array to conver to a string</param>
        /// <param name="index">The starting index</param>
        /// <param name="count">The number of bytes to convert</param>
        /// <returns>The string representation of the encoded byte array</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsString(this byte[] @this, int index, int count)
        {
            return System.Text.Encoding.UTF8.GetString(@this, index, count);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified byte array
        /// </summary>
        /// <param name="this">The byte array used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initialized with the specified byte array</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static MemoryStream AsStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
        /// <summary>
        /// Creates a MemoryStream object and initializes it with the specified string
        /// </summary>
        /// <param name="this">The string used in creating the MemoryStream</param>
        /// <returns>A new MemoryStream initilized with the specified string</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static MemoryStream AsStream(this string @this)
        {
            return new MemoryStream(@this.AsBytes());
        }
        /// <summary>
        /// Serializes an object to a Json string
        /// </summary>
        /// <typeparam name="TType">The type of object to serialize</typeparam>
        /// <param name="this">The object to serialize to Json</param>
        /// <returns>A Json formatted string representation of the specified object</returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsJson<TType>(this object @this)
        {
            var s = new DataContractJsonSerializer(typeof(TType));
            //var s = new DataContractJsonSerializer(typeof(TType), new DataContractJsonSerializerSettings() { EmitTypeInformation = EmitTypeInformation.Always, UseSimpleDictionaryFormat = false});
            using (var stream = new System.IO.MemoryStream())
            {
                s.WriteObject(stream, @this);
                return stream.ToArray().AsString();
            }
        }
        /// <summary>
        /// Serializes an object to an xml string
        /// </summary>
        /// <typeparam name="TType">The type of object to serialize</typeparam>
        /// <param name="this">The object to serialize</param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string AsXml<TType>(this object @this)
        {
            var s = new DataContractSerializer(typeof(TType));
            using (var stream = new System.IO.MemoryStream())
            {
                s.WriteObject(stream, @this);
                return stream.ToArray().AsString();
            }
        }

        /// <summary>
        /// Compresses the byte array using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="bytes">The byte array to compress</param>
        /// <returns>A byte array of compressed data</returns>
        public static byte[] AsCompressed(this byte[] bytes)
        {
            var output = new MemoryStream();
            using (var deflater = new System.IO.Compression.DeflateStream(output, System.IO.Compression.CompressionMode.Compress))
            {
                deflater.Write(bytes, 0, bytes.Length);
            }
#if WINDOWS_PORTABLE || WINDOWS_UWP || NETCOREAPP1_0 || NETCOREAPP1_1
            return output.ToArray();
#else
            return output.GetBuffer();
#endif
        }
        /// <summary>
        /// Compresses the string using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="item">The string to compress</param>
        /// <returns>A compressed string</returns>
        public static string AsCompressed(this string item)
        {
#if WINDOWS_PORTABLE || WINDOWS_UWP || NETCOREAPP1_0 || NETCOREAPP1_1
            var bytes = AsCompressed(System.Text.Encoding.UTF8.GetBytes(item));
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
#else
            return System.Text.Encoding.Default.GetString(AsCompressed(System.Text.Encoding.Default.GetBytes(item)));
#endif
        }
        /// <summary>
        /// Decompresses the byte array using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="bytes">The byte array containing compressed data</param>
        /// <returns>A byte array of the decompressed data</returns>
        public static byte[] AsDecompressed(this byte[] bytes)
        {
            var input = new MemoryStream(bytes);
            var output = new MemoryStream();
            using (var deflater = new System.IO.Compression.DeflateStream(input, System.IO.Compression.CompressionMode.Decompress))
            {
                deflater.CopyTo(output);
            }
#if WINDOWS_PORTABLE || WINDOWS_UWP || NETCOREAPP1_0 || NETCOREAPP1_1
            return output.ToArray();
#else
            return output.GetBuffer();
#endif
        }
        /// <summary>
        /// Decompresses the string using System.IO.Compression.DeflateStream
        /// </summary>
        /// <param name="item">The string containing compressed data</param>
        /// <returns>A string of decompressed data</returns>
        public static string AsDecompressed(this string item)
        {
#if WINDOWS_PORTABLE || WINDOWS_UWP || NETCOREAPP1_0 || NETCOREAPP1_1
            var bytes = AsDecompressed(System.Text.Encoding.UTF8.GetBytes(item));
            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length).TrimEnd('\0');
#else
            return System.Text.Encoding.Default.GetString(AsDecompressed(System.Text.Encoding.Default.GetBytes(item))).TrimEnd('\0');
#endif
        }
    }
}
