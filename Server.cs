using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
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
             while (true)
             {
              string? msg = reader.ReadLine();
              if (msg == null) {break;}
              writer.WriteLine(msg);  
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
