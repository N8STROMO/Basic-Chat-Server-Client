﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;

namespace Server
{

  public class ServerUserConnection : UserConnection
  {
    readonly ChatServer server;
   
    public ServerUserConnection(ChatServer server, TcpClient client) : base(client, GetUsername(client))
    {
      
      this.server = server;
    }

    private static string GetUsername(TcpClient client)
    {
      NetworkStream stream = client.GetStream();
      if(stream.CanRead)
      {
        string userName = Utils.ReceiveInformation(stream, client, out MessageType type);
        Console.WriteLine(userName);
        return userName;
      }

      return null;
    }
    protected override void OnRead(MessageType type, string message)
    {
      if(type != MessageType.ChatMessage)
      {
        return;
      }

      server.SendMsgToAll(MessageType.ChatMessage, this, $"{userName} {message}");
    }
  }
}