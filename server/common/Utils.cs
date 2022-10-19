﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace common
{
    public static class Utils
    {
        public static byte[] ToUtf8Bytes(this string val) => Encoding.UTF8.GetBytes(val);

        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException(nameof(input));
                case "":
                    throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static XElement AddAttribute(this XElement elem, XName name, object value)
        {
            elem.SetAttributeValue(name, value);
            return elem;
        }

        public static T PickRandom<T>(this IEnumerable<T> source) => source.PickRandom(1).Single();

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(x => Guid.NewGuid());

        public static bool IsInt(this string str) => int.TryParse(str, out int dummy);

        public static int ToInt32(this string str) => FromString(str);

        public static T[] ResizeArray<T>(T[] array, int newSize)
        {
            var inventory = new T[newSize];
            for (int i = 0; i < array.Length; i++)
                inventory[i] = array[i];

            return inventory;
        }

        public static string GetBasePath(string folder) => Path.Combine(GetAssemblyDirectory(), folder);

        public static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(path);
        }

        public static string GetCommaSepString<T>(T[] arr)
        {
            var ret = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.Append(arr[i]);
            }
            return ret.ToString();
        }

        public static string[] FromCommaSepString(string x) => x.Split(',').Select(_ => _.Trim()).ToArray();

        public static byte[] Deflate(string src)
        {
            var bytes = Encoding.UTF8.GetBytes(src);
            return Deflate(bytes);
        }

        public static byte[] Deflate(byte[] src)
        {
            byte[] zipBytes;

            using (var dst = new MemoryStream())
            {
                using (var df = new DeflateStream(dst, CompressionMode.Compress))
                    df.Write(src, 0, src.Length);

                zipBytes = dst.ToArray();
            }

            return zipBytes;
        }

        public static int FromString(string x)
        {
            int val = 0;
            try
            {
                val = x.StartsWith("0x") ? int.Parse(x.Substring(2), NumberStyles.HexNumber) : int.Parse(x);
            }
            catch { }

            return val;
        }

        public static string To4Hex(this ushort x) => "0x" + x.ToString("x4");

        public static string ToCommaSepString<T>(this T[] arr)
        {
            StringBuilder ret = new StringBuilder();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.Append(arr[i]);
            }
            return ret.ToString();
        }

        public static T[] CommaToArray<T>(this string x)
        {
            if (typeof(T) == typeof(ushort))
                return x.Split(',').Select(_ => (T)(object)(ushort)FromString(_.Trim())).ToArray();

            if (typeof(T) == typeof(string))
                return x.Split(',').Select(_ => (T)(object)_.Trim()).ToArray();

            return x.Split(',').Select(_ => (T)(object)FromString(_.Trim())).ToArray();
        }

        public static byte[] SHA1(string val)
        {
            SHA1Managed sha1 = new SHA1Managed();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(val));
        }

        public static int ToUnixTimestamp(this DateTime dateTime) => (int)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;

        public static T Exec<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box); while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;

            MatchEvaluator domainMapper = match =>
            {
                // IdnMapping class with default property values.
                var idn = new IdnMapping();

                var domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            };

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", domainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    return attr?.Description;
                }
            }
            return null;
        }

        public static T FromJson<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json)) 
                return null;

            var jsonSerializer = new JsonSerializer();

            using (var strRdr = new StringReader(json))
            using (var jsRdr = new JsonTextReader(strRdr))
                return jsonSerializer.Deserialize<T>(jsRdr);
        }

        public static DateTime FromUnixTimestamp(int time)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(time).ToLocalTime();
            return dateTime;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        // https://www.codeproject.com/Articles/770323/How-to-Convert-a-Date-Time-to-X-minutes-ago-in-Csh
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return $"{years} {(years == 1 ? "year" : "years")} ago";
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return $"{months} {(months == 1 ? "month" : "months")} ago";
            }
            if (span.Days > 0)
                return $"{span.Days} {(span.Days == 1 ? "day" : "days")} ago";
            if (span.Hours > 0)
                return $"{span.Hours} {(span.Hours == 1 ? "hour" : "hours")} ago";
            if (span.Minutes > 0)
                return $"{span.Minutes} {(span.Minutes == 1 ? "minute" : "minutes")} ago";
            if (span.Seconds > 5)
                return $"{span.Seconds} seconds ago";
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
    }

    public static class StringUtils
    {
        public static bool ContainsIgnoreCase(this string self, string val) => self.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) != -1;

        public static bool EqualsIgnoreCase(this string self, string val) => self.Equals(val, StringComparison.InvariantCultureIgnoreCase);

        public static byte[] StringToByteArray(string hex) => Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
    }
}
