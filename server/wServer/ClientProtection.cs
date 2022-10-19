using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wServer
{
	public class ClientProtection
	{

		public bool IsEnabled = false;
        public bool IsAirEnabled = false;

		public string[] Tokens { get; set; } = { "0" };
		public string[] Hashs { get; set; } = { "0" };
        public string[] HashsAir { get; set; } = { "0" };

        public ClientProtection(bool client, bool air, string[] tokens)
		{
			IsEnabled = client;
            IsAirEnabled = air;
			Tokens = tokens;

            if (!File.Exists("UTReborn.swf")) IsEnabled = false;
            if (!File.Exists("UTRebornAir.swf")) IsAirEnabled = false;

            Console.WriteLine($"(Client Check) Normal Client: {IsEnabled}");
            Console.WriteLine($"(Client Check) Air Client: {IsAirEnabled}");

            Hashs = new string[Tokens.Length];
            HashsAir = new string[Tokens.Length];
            if (IsEnabled) GenerateHashs(false, "UTReborn.swf");
            if (IsAirEnabled) GenerateHashs(true, "UTRebornAir.swf");
        }

		private void GenerateHashs(bool air, string file)
		{

			byte[] swf = File.ReadAllBytes(file);
            byte[] client = SwfDecompress(swf);

            int offset = (int)(client.Length * 0.8);
            int length = client.Length - 1;

            byte[] shorted = client.Skip(offset).Take(length - offset).ToArray();

            for(var i = 0; i < Tokens.Length; i++)
            {
                byte[] tk = Encoding.UTF8.GetBytes(Tokens[i]);
                byte[] final = Combine(shorted, tk);

                byte[] hashValue = GetMD5(final);

                string hashString = string.Empty;
                foreach (byte x in hashValue)
                {
                    hashString += string.Format("{0:x2}", x);
                }
                if(air) HashsAir[i] = hashString;
                else Hashs[i] = hashString;
            }
        }

        public static byte[] SwfDecompress(byte[] data)
        {
            var stream = new MemoryStream(data);

            var first = stream.ReadByte();
            if (first != 0x43) // C
            {
                throw new ArgumentException("Not compressed", "data");
            }

            if (stream.ReadByte() != 0x57) // W
            {
                throw new ArgumentException("Not SWF", "data");
            }

            if (stream.ReadByte() != 0x53) // S
            {
                throw new ArgumentException("Not SWF", "data");
            }

            var version = (byte)stream.ReadByte();
            var integer = new byte[4];
            integer[0] = (byte)stream.ReadByte();
            integer[1] = (byte)stream.ReadByte();
            integer[2] = (byte)stream.ReadByte();
            integer[3] = (byte)stream.ReadByte();

            var outStream = new MemoryStream();

            outStream.WriteByte(0x46);
            outStream.WriteByte(0x57);
            outStream.WriteByte(0x53);
            outStream.WriteByte(version);
            outStream.Write(integer, 0, 4);

            var uncompressedInput = new ZlibStream(stream, CompressionMode.Decompress);
            byte[] buffer = new byte[4096];
            int read;
            while ((read = uncompressedInput.Read(buffer, 0, buffer.Length)) > 0)
            {
                outStream.Write(buffer, 0, read);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            return outStream.ToArray();
        }

        private static byte[] GetMD5(byte[] message)
        {
            MD5 hashString = new MD5CryptoServiceProvider();
            return hashString.ComputeHash(message);
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
    }
}
