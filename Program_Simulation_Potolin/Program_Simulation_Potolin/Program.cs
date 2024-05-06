using System.Text;
using Program_Simulation_Potolin;

class Program
{
    static void Main(string[] args)
    {
        // Hardcoded p and g values
        int p = 199, g = 127;
        var (parameters, userAPublicKey, userAPrivateKey) = DiffieHelman.GenerateParametersAndKeys(p, g);
        var (_, userBPublicKey, userBPrivateKey) = DiffieHelman.GenerateParametersAndKeys(p, g);

        // Generate shared secret keys
        var userASharedKey = DiffieHelman.GenerateSharedKey(userAPrivateKey, userBPublicKey);
        var userBSharedKey = DiffieHelman.GenerateSharedKey(userBPrivateKey, userAPublicKey);

        // Simulate sending a message
        string message = "The Mandalorian Must Always Recite, This is The Way!";
        var messages = ChunkAndPadMessage(message);

        Console.WriteLine("Original Messages:");
        foreach (var m in messages)
            Console.WriteLine(BitConverter.ToString(Encoding.UTF8.GetBytes(m)));

        // Encrypt each chunk
        byte[][] encryptedMessages = messages.Select(m => AES.Encrypt(Encoding.UTF8.GetBytes(m), userASharedKey)).ToArray();

        // Simulate receiving and decrypting the message
        string[] decryptedMessages = encryptedMessages.Select(m => Encoding.UTF8.GetString(AES.Decrypt(m, userBSharedKey))).ToArray();

        Console.WriteLine("\nDecrypted Messages:");
        foreach (var m in decryptedMessages)
            Console.WriteLine(m);

        Console.WriteLine("\nFull Decrypted Message:");
        Console.WriteLine(string.Concat(decryptedMessages).Replace("@", string.Empty));
    }

    static string[] ChunkAndPadMessage(string message)
    {
        int chunkSize = 16;
        int fullChunks = (int)Math.Ceiling(message.Length / (double)chunkSize);
        string[] chunks = new string[fullChunks];
        for (int i = 0; i < fullChunks; i++)
        {
            int startIndex = i * chunkSize;
            chunks[i] = message.Substring(startIndex, Math.Min(chunkSize, message.Length - startIndex));
            if (chunks[i].Length < chunkSize)
                chunks[i] = chunks[i].PadRight(chunkSize, '@');
        }
        return chunks;
    }
}