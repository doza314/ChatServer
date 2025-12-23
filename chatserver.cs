string? username = " ";
bool quit = false; 

Console.Clear();
Console.Write("Create username: ");
username = Console.ReadLine();
Console.WriteLine();

if (username == "" || username == " " || username == null)
{
  Console.WriteLine("INVALID USERNAME!");
  quit = true; 
}
else if (username == "/q")
{
  quit = true;
}


if (username == "server" && !quit)
{
  Server server = new Server();
  server.Run();
}
else if (username != null && username != "server" && !quit)
{
  Chatter chatter = new Chatter(username);
  chatter.Run();
}
