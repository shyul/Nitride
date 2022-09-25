/// ***************************************************************************
/// Shared Libraries and Quick Utilities
/// GPL 2001-2007, 2014-2021 Xu Li - me@xuli.us
/// 
/// Data Serializations and Reflection
/// 
/// Code Author: Xu Li
/// 
/// ***************************************************************************

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Runtime.InteropServices;

namespace Nitride
{
    public static partial class Serialization
    {
        public static FileInfo[] GetFileList(string path, string suffix = "*")
        {
            DirectoryInfo d = new(path);
            return d.GetFiles(suffix);
        }

        #region XML Data

        /// <summary>
        /// XML Serialization
        /// </summary>
        public static byte[] SerializeXML<T>(this T source)
        {
            if (source != null && typeof(T).IsSerializable)
            {
                using MemoryStream stream = new();
                XmlSerializer xmlSer = new(typeof(T));
                StreamWriter sw = new(stream, Encoding.Unicode);
                xmlSer.Serialize(sw, source);
                stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;
                return stream.ToArray();
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fileName"></param>
        public static void SerializeXMLFile<T>(this T source, string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            File.WriteAllBytes(fileName, source.SerializeXML());
        }

        /// <summary>
        /// XML Deserialization
        /// </summary>
        public static T DeserializeXML<T>(string source) => DeserializeXML<T>(Encoding.ASCII.GetBytes(source));

        /// <summary>
        /// XML Deserialization
        /// </summary>
        public static T DeserializeXML<T>(this byte[] source)
        {
            using MemoryStream stream = new(source);
            XmlSerializer xmlSer = new(typeof(T));
            stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;

            using XmlReader rd = XmlReader.Create(stream);
            return (T)xmlSer.Deserialize(rd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializeXMLFile<T>(string fileName)
        {
            if (File.Exists(fileName))
                return (T)DeserializeXML<T>(File.ReadAllBytes(fileName));
            else
                return default;
        }

        /// <summary>
        /// XML Path Utilities
        /// </summary>
        public static string GetElementValue(this XDocument doc, string path)
        {
            XElement xe = doc.XPathSelectElement(path);
            if (xe != null)
                return xe.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetElementValue(this XElement xe, string name)
        {
            XElement xe2 = xe.Element(name);
            if (xe2 != null)
                return xe2.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XDocument doc, string path, string attrib)
        {
            XElement xe = doc.XPathSelectElement(path);
            if (xe != null)
            {
                XAttribute xa = xe.Attribute(attrib);
                if (xa != null)
                    return xa.Value;
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XElement xe, string name)
        {
            XAttribute xa = xe.Attribute(name);
            if (xa != null)
                return xa.Value;
            else
                return string.Empty;
        }

        #endregion XML Data

        #region Json Data

        /// <summary>
        /// Json Serialization
        /// </summary>
        public static byte[] SerializeJson<T>(this T source)
        {
            if (source is not null && typeof(T).IsSerializable)
            {
                using MemoryStream stream = new();
                DataContractJsonSerializer JsonSer = new(typeof(T));
                JsonSer.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin); // stream.Position = 0;
                return stream.ToArray();
            }
            else
                throw new ArgumentException("The type must be not null and serializable.", nameof(source));
        }

        public static void SerializeJsonFile<T>(this T source, string fileName)
        {
            FileInfo fi = new(fileName);
            if (!fi.Directory.Exists) fi.Directory.Create();

            string backup_fileName = fileName + "_backup";

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);

            if (File.Exists(fileName))
                File.Move(fileName, backup_fileName);

            File.WriteAllBytes(fileName, source.SerializeJson());

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);
        }

        //public static void SerializeJsonFile(this IDataFile idf) => idf.SerializeJsonFile(idf.DataFileName);

        /// <summary>
        /// Json Deserialization
        /// </summary>
        public static T DeserializeJson<T>(this byte[] source)
        {
            using MemoryStream stream = new(source);
            
            DataContractJsonSerializer JsonSer = new(typeof(T));

            Console.WriteLine("Deserializing Json ##");
            return (T)JsonSer.ReadObject(stream);
        }

        public static T DeserializeJsonFile<T>(string fileName)
        {
            string backup_fileName = fileName + "_backup";

            if (!File.Exists(fileName) && File.Exists(backup_fileName))
                File.Move(backup_fileName, fileName);

            if (File.Exists(backup_fileName))
                File.Delete(backup_fileName);

            if (File.Exists(fileName))
                using (FileStream stream = File.OpenRead(fileName))
                    return DeserializeJson<T>(File.ReadAllBytes(fileName));
            else 
            {
                Console.WriteLine("DeserializeJsonFile() Unable to find file: " + fileName);
                return default;
            }
         
        }

        #endregion Json Data

        public static byte[] SerializeBytes<T>(this T source) where T : notnull
        {
            int size = Marshal.SizeOf(source);
            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(source, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }

        public static int SerializeBytes<T>(this T source, byte[] bytes) where T : notnull
        {
            int size = Marshal.SizeOf(source);
            if (size > bytes.Length) return 0;
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(source, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size); // public static void Copy (IntPtr source, byte[] destination, int startIndex, int length);
            Marshal.FreeHGlobal(ptr);
            return size;
        }

        public static T DeserializeBytes<T>(this byte[] bytes) where T : notnull 
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size); // Copy (byte[] source, int startIndex, IntPtr destination, int length);
            T res = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return res;
        }

        public static int DeserializeBytes<T>(this T dest, byte[] bytes) where T : notnull
        {
            int size = Marshal.SizeOf(typeof(T));
            GCHandle handle = GCHandle.Alloc(dest);
            IntPtr ptr = (IntPtr)handle;
            Marshal.Copy(bytes, 0, ptr, size);
            handle.Free();
            return size;
        }
    }
}
