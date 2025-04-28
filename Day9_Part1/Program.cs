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

        // Convert the disk map string into a list of blocks (representing files and free space).
        var blocks = ConvertDiskMapToBlocks(diskMap);

        CompactDisk(blocks);

        long checksum = CalculateChecksum(blocks);

        Console.WriteLine("Final Disk State: " + new string(blocks));
        Console.WriteLine("Filesystem Checksum: " + checksum);
    }

    static char[] ConvertDiskMapToBlocks(string diskMap)
    {
        // blocks array length
        int totalLength = 0;
        for (int i = 0; i < diskMap.Length; i += 2)
        {
            totalLength += diskMap[i] - '0';
        }

        // Initialize the blocks array with the calculated total length.
        var blocks = new char[totalLength];
        int index = 0; 
        int fileId = 0; 

    
        for (int i = 0; i < diskMap.Length - 1; i += 2)
        {
            int fileLength = diskMap[i] - '0'; 
            int freeSpaceLength = diskMap[i + 1] - '0';

            // Fill the blocks array with the file's ID for the file length.
            for (int j = 0; j < fileLength; j++)
            {
                if (index < blocks.Length)
                {
                    blocks[index++] = (char)('0' + fileId);
                }
            }

            // Skip the free space by incrementing the index.
            index += freeSpaceLength;

            // Increment the file ID for the next file.
            fileId++;
        }
        return blocks;
    }

    static void CompactDisk(char[] blocks)
    {
        int writeIndex = 0; // Index where the next non-empty block should be written.

        // step through the blocks array and move non-empty blocks to the front.
        for (int readIndex = 0; readIndex < blocks.Length; readIndex++)
        {
            if (blocks[readIndex] != '\0')
            {
                blocks[writeIndex++] = blocks[readIndex];
            }
        }

        // Filling the remaining space in the array with '.' to represent free space.
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
            // Only consider blocks that are not free space ('.').
            if (blocks[i] != '.')
            {
                checksum += i * (blocks[i] - '0'); 
            }
        }
        return checksum;
    }
}
