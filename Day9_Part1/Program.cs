using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day9.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string diskMap = File.ReadAllText(filePath).Trim();

        // Convert the disk map to a list of blocks
        var blocks = ConvertDiskMapToBlocks(diskMap);

        // Compact the disk
        CompactDisk(blocks);

        // Calculate the checksum
        long checksum = CalculateChecksum(blocks);

        Console.WriteLine("Final Disk State: " + new string(blocks));
        Console.WriteLine("Filesystem Checksum: " + checksum);
    }

    static char[] ConvertDiskMapToBlocks(string diskMap)
    {
        // Calculate the total length of the blocks array
        int totalLength = 0;
        for (int i = 0; i < diskMap.Length; i += 2)
        {
            totalLength += diskMap[i] - '0';
        }

        var blocks = new char[totalLength];
        int index = 0;
        int fileId = 0;

        for (int i = 0; i < diskMap.Length - 1; i += 2)
        {
            int fileLength = diskMap[i] - '0';
            int freeSpaceLength = diskMap[i + 1] - '0';

            for (int j = 0; j < fileLength; j++)
            {
                if (index < blocks.Length)
                {
                    blocks[index++] = (char)('0' + fileId);
                }
            }
            index += freeSpaceLength;
            fileId++;
        }
        return blocks;
    }

    static void CompactDisk(char[] blocks)
    {
        int writeIndex = 0;
        for (int readIndex = 0; readIndex < blocks.Length; readIndex++)
        {
            if (blocks[readIndex] != '\0')
            {
                blocks[writeIndex++] = blocks[readIndex];
            }
        }
        for (int i = writeIndex; i < blocks.Length; i++)
        {
            blocks[i] = '.';
        }
    }

    static long CalculateChecksum(char[] blocks)
    {
        long checksum = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != '.')
            {
                checksum += i * (blocks[i] - '0');
            }
        }
        return checksum;
    }
}
