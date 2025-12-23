using System.Net.Sockets;
using System.Text;

class Chatter
{
  private string? message = "";
  private string? name = "";

  public Chatter(string username)
  {
    name = username;
  }

  void ReceiveLoop(StreamReader reader)
  {
    while(true)
    {
      string? incoming = reader.ReadLine();

      if (incoming != null)
      {
        Console.WriteLine();
        Console.WriteLine(incoming);
        Console.WriteLine();
        Console.Write($"{name}: ");

      } 
    }
  }

  public void Run()
  { 
    //TCP Handshake
    Console.WriteLine("[CLIENT] Connecting...");
    using TcpClient client = new TcpClient();
    
    client.Connect("doza314.tailef3c92.ts.net", 5555);
    Console.WriteLine("[CLIENT] Connected!");
    
    //Stream
    using NetworkStream stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };  
    
    Task.Run(() => ReceiveLoop(reader)); 

    while(true)
    {
      //message prompt
      Console.Write($"{name}: ");
      message = Console.ReadLine();
      Console.WriteLine();

      //check for null or quit command
      if (message == null)
      {
        return; 
      }
      else if (message == "/q") //quit command
      {
        return;
      }
      
      //build and send message
      string msg = $"{name}: {message}";
      writer.WriteLine(msg);

    }
   }
}
