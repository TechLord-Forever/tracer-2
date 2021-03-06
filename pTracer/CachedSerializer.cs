﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace pTracer
{
    internal static class CachedSerializer
    {
        // borrowed from CInject

        private static readonly Dictionary<Type, XmlSerializer> Serializer;

        static CachedSerializer()
        {
            Serializer = new Dictionary<Type, XmlSerializer>();
        }

        private static XmlSerializer GetSerializer<T>()
        {
            Type type = typeof(T);
            if (Serializer.ContainsKey(type))
                return Serializer[type];
            else
            {
                var xs = new XmlSerializer(type);
                Serializer[type] = xs;
                return xs;
            }
        }

        private static XmlSerializer GetSerializer(Type type)
        {
            if (Serializer.ContainsKey(type))
                return Serializer[type];
            else
            {
                var xs = new XmlSerializer(type);
                Serializer[type] = xs;
                return xs;
            }
        }

        public static string Serialize(Type type, object obj, Encoding encoding)
        {
            try
            {
                string xmlString = null;
                var memoryStream = new MemoryStream();
                XmlSerializer xs = GetSerializer(type);
                var xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = Converter.ToString(memoryStream.ToArray());
                return xmlString;
            }
            catch
            {
                throw;
            }
        }

        public static string Serialize<T>(T obj, Encoding encoding)
        {
            try
            {

                string xmlString = null;
                var memoryStream = new MemoryStream();
                XmlSerializer xs = GetSerializer<T>();
                var xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = Converter.ToString(memoryStream.ToArray());
                return xmlString;
            }
            catch
            {
                throw;
            }
        }

        public static T Deserialize<T>(string xml, Encoding encoding)
        {
            XmlSerializer xs = GetSerializer<T>();
            var memoryStream = new MemoryStream(Converter.ToByte(xml));
            var xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
            return (T)xs.Deserialize(memoryStream);
        }

        public static object Deserialize(Type type, string xml, Encoding encoding)
        {
            XmlSerializer xs = GetSerializer(type);
            var memoryStream = new MemoryStream(Converter.ToByte(xml));
            var xmlTextWriter = new XmlTextWriter(memoryStream, encoding);
            return xs.Deserialize(memoryStream);
        }


    }


    internal class Converter
    {
        internal static string ToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        internal static Byte[] ToByte(string input)
        {
            var encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(input);
            return byteArray;
        }

        internal static string ToString(object value, string defaultValue)
        {
            return value == null ? defaultValue : Convert.ToString(value);
        }

        internal static object Enum<T>(string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        internal static object Enum<T>(long value)
        {
            return (T)System.Enum.Parse(typeof(T), value.ToString(), true);
        }
    }

}
