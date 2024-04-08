using System.IO.Compression;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace DesignPatterns.Decorator;

public class NetworkExample
{
    //https://www.linkedin.com/pulse/decorator-pattern-streams-net-andrea-angella/

    public static void Main()
    {
        //First start the server, see code below (RunServer)
        SendMessage("Hello World");
    }
    
    public static void SendMessage(string message)
    {
        using var tcpClient = new TcpClient("localhost", 8888);
        using var stream = tcpClient.GetStream();
        // using var cryptoStream = new CryptoStream(stream, new HMACSHA3_256(), CryptoStreamMode.Write);
        // using var gzipStream = new GZipStream(cryptoStream, CompressionMode.Compress);
        // using var writer = new StreamWriter(gzipStream);

        using var writer =
            new StreamWriter(
                new BufferedStream(
                    new GZipStream(
                        new CryptoStream(tcpClient.GetStream(), new ToBase64Transform(), CryptoStreamMode.Write),
                        CompressionMode.Compress)
                ));


        writer.Write(message);
    }
    
    public static void RunServer()
    {
        using var tcpListener = new TcpListener(8888);
        tcpListener.Start();
        using var tcpClient = tcpListener.AcceptTcpClient();
        using var stream = tcpClient.GetStream();

        using var reader =
            new StreamReader(
                new BufferedStream(
                    new GZipStream(
                        new CryptoStream(tcpClient.GetStream(), new FromBase64Transform(), CryptoStreamMode.Read),
                        CompressionMode.Decompress)
                ));

        var message = reader.ReadToEnd();
        Console.WriteLine($"Received from client: {message}");
    }
}