#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HexIO;

#endregion

namespace Harp.Olfactometer.Design.Models
{
    public class EepromManager
    {
        public List<ushort[]> Data { get; }

        private readonly string filePath;
        private MemoryStream stream;

        public EepromManager(string filePath)
        {
            this.filePath = filePath;
            Data = new List<ushort[]>();
        }

        public void ProcessFile()
        {
            // open CSV file using filePath and save data to container
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var container = new ushort[16];

                    // each entry in "values" is in decimal format representing 2 bytes. convert it to hex
                    for (var index = 0; index < values.Length; index++)
                    {
                        var value = values[index];
                        container[index] = Convert.ToUInt16(value);
                    }

                    Data.Add(container);
                }
            }
        }

        public void GenerateEeprom(int serialNumber, int temperature)
        {
            // using the HexIO nuget package, create the EEPROM file by adding Data starting in address 0x10064000
            const int baseOffset = 100;
            const int dataSize = 6;
            var testData = new List<byte>
            {
                0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
            };

            // create memory stream
            stream = new MemoryStream();
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            var writer = new IntelHexStreamWriter(stream, utf8WithoutBom, 1024, true);

            // write test data to stream every 0x10 offset until 0x10064000
            for (int i = 0; i < 128; i++)
            {
                if (i == baseOffset)
                {
                    // get corresponding data from Data and write to stream
                    // each data line has 16 values but we can only write 8 at a time
                    // so we need to write 2 lines of data to the stream
                    // note: we need to increment 'i' differently to account for the 2 lines of data

                    for (int k = 0; k < dataSize; k++)
                    {
                        var data = Data[k];

                        // update data to add the temperature value
                        // we need to write the temperature value to the first byte of the second line on the last 'k' iteration
                        // each entry in data is in decimal format representing 2 bytes. convert it to hex
                        if (k == dataSize - 1)
                        {
                            // find the first empty byte in data
                            var index = Array.IndexOf(data, (ushort)0);
                            if (index == -1)
                                throw new Exception("Data is full");
                            // convert temperature to ushort and add to data
                            byte temperatureByte = Convert.ToByte(temperature);
                            // save it in the upper byte of the ushort on index
                            // (0x00FF is a mask to keep the upper byte unchanged) since it will be reversed
                            data[index] = (ushort)(data[index] & 0x00FF | (temperatureByte << 8));
                        }

                        var firstLine = new List<byte>();
                        var secondLine = new List<byte>();

                        for (int j = 0; j < 16; j++)
                        {
                            ushort[] shortArray = new[] { data[j] };
                            byte[] byteArray = new byte[shortArray.Length * 2];
                            Buffer.BlockCopy(shortArray, 0, byteArray, 0, shortArray.Length * 2);

                            // TODO: we check if the running processor is big endian vs little endian and reverse the byte array accordingly?
                            if (j < 8)
                                firstLine.AddRange(byteArray.Reverse());
                            else
                                secondLine.AddRange(byteArray.Reverse());
                        }

                        writer.WriteDataRecord((ushort)((i + k) * 0x10), firstLine.ToArray());
                        writer.WriteDataRecord((ushort)((i + k + 1) * 0x10), secondLine.ToArray());

                        i++;
                    }

                    // FIXME: this is a hack to get the correct offset
                    i += dataSize - 1;
                }
                else
                {
                    // special case for the first Data record since we will need to add the serial number
                    // we can use a copy of testData as a base and update the ushort on position 4 with the serial number
                    if (i == 0)
                    {
                        // copy testData to a new array
                        var firstLine = new List<byte>(testData);
                        // convert serial number to ushort
                        ushort serialShort = Convert.ToUInt16(serialNumber);
                        // write the high byte of the serial number to the 4th byte of the first line
                        firstLine[4] = (byte)(serialShort >> 8);
                        // write the low byte of the serial number to the 5th byte of the first line
                        firstLine[5] = (byte)(serialShort & 0xFF);

                        writer.WriteDataRecord((ushort)(i * 0x10), firstLine);
                    }
                    else
                    {
                        writer.WriteDataRecord((ushort)(i * 0x10), testData);
                    }
                }
            }

            writer.Close();

            stream.Flush();
            stream.Position = 0;
        }

        public void Save(string filename)
        {
            // save stream field to file
            using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(fileStream);
            }
        }
    }
}
