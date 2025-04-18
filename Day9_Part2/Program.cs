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

        // Compact the disk by moving whole files
        CompactDisk(blocks);

        // Calculate the checksum
        long checksum = CalculateChecksum(blocks);

        Console.WriteLine("Final Disk State: " + new string(blocks));
        Console.WriteLine("Filesystem Checksum: " + checksum);
    }

    static char[] ConvertDiskMapToBlocks(string diskMap)
    {
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
        // Iterate files in reverse order of ID
        for (int fileId = 9; fileId >= 0; fileId--)
        {
            var fileIndices = blocks.Select((block, index) => new { Block = block, Index = index })
                                    .Where(x => x.Block == (char)('0' + fileId))
                                    .Select(x => x.Index)
                                    .ToList();

            if (fileIndices.Count == 0) continue; // Skip if file does not exist

            int fileLength = fileIndices.Count;

            // Search for a span of free space that fits the file
            int spanStart = -1;
            for (int i = 0; i <= blocks.Length - fileLength; i++)
            {
                if (blocks.Skip(i).Take(fileLength).All(block => block == '.'))
                {
                    spanStart = i;
                    break;
                }
            }

            if (spanStart == -1) continue; // No valid span found.

            // Move file to free space span
            foreach (var index in fileIndices)
            {
                blocks[index] = '.';
            }
            for (int i = spanStart; i < spanStart + fileLength; i++)
            {
                blocks[i] = (char)('0' + fileId);
            }
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
