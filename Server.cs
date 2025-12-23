using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

class Server
{
  private readonly ConcurrentDictionary<TcpClient, StreamWriter> _clients = new();
  
  void Broadcast(string msg) //for broadcasting messages between all parallel client handling processes
  {
    foreach(var kvp in _clients)
    {
      var client = kvp.Key;
      var writer = kvp.Value;

      writer.WriteLine(msg);
    } 
  }
  

  void HandleClient(TcpClient client)
  {
    try
    {
      using (client)
      {
        using (var stream = client.GetStream())
        {
          using (var reader = new StreamReader(stream, Encoding.UTF8))
          using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
          {
            _clients[client] = writer;

             while (true)
             {
              string? msg = reader.ReadLine();
              if (msg == null) {break;}
              Broadcast(msg); 
             }
          }
        }
      }
    }
    catch (Exception ex) //In case of error
    {
      Console.WriteLine($"[HOST] Client error: {ex.Message}");    
    }
  }
  
  public void Run()
  {
    //listen for client
    Console.WriteLine("[HOST] Listening on port 5555...");
    var listener = new TcpListener(IPAddress.Any, 5555);
    listener.Start();
    
    
    //client handling loop
    while (true)
    {
      TcpClient client = listener.AcceptTcpClient();
      Console.WriteLine("[HOST] Client connected.");

      Task.Run(() => HandleClient(client));     
    }
  }    
}
