using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;

public class TCPSocketClient : MonoBehaviour {

    public String host = "localhost";
    public Int32 port = 50000;

    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;

    StreamWriter socket_writer;
    StreamReader socket_reader;

    void Update()
    {
        if (!socket_ready)
            return;

        string received_data = readSocket();
        if (received_data != "")
        {
            // Do something with the received data,
            // print it in the log for now
            Debug.Log(received_data);
            string[] words = received_data.ToString().TrimEnd('\r','\n').Split(' ');
            /*foreach (string w in words)
            {
                Debug.Log(w);
            }*/
            if (words[0] == "earth" && words[1] == "reset")
            {
                Debug.Log("RESET RECEIVED!");
                GetComponent<MainController>().SendMessage("reset_scene");
            }
            else if (words[0] == "earth")
            {
                // get the other params
                int bases = Convert.ToInt32(words[1]);
                int munitions = Convert.ToInt32(words[2]);
                int cadence = Convert.ToInt32(words[3]);
                GetComponent<MainController>().SendMessage("update_earth_bases", bases);
                GetComponent<MainController>().SendMessage("update_earth_munitions", munitions);
                GetComponent<MainController>().SendMessage("update_earth_cadence", cadence);
            }
            else if (words[0] == "alien1")
            {
                // get the other param
                int cadence = Convert.ToInt32(words[1]);
                GetComponent<MainController>().SendMessage("update_alien1", cadence);
            }
            else if (words[0] == "alien2")
            {
                // get the other param
                int cadence = Convert.ToInt32(words[1]);
                GetComponent<MainController>().SendMessage("update_alien2", cadence);
            }
            else if (words[0] == "alien3")
            {
                // get the other param
                int cadence = Convert.ToInt32(words[1]);
                GetComponent<MainController>().SendMessage("update_alien3", cadence);
            }
            // then answer
            writeSocket("999\r");
        }

        /*
        string key_stroke = Input.inputString;

        // Collects keystrokes into a buffer
        if (key_stroke != "")
        {
            input_buffer += key_stroke;
            Debug.Log("detected: " + key_stroke);

            if (key_stroke == "\r")
            {
                // Send the buffer, clean it
                Debug.Log("Sending: " + input_buffer);
                writeSocket(input_buffer);
                input_buffer = "";
            }

        }
        */
    }

    void Awake()
    {
        setupSocket();
    }

    void OnApplicationQuit()
    {
        closeSocket();
    }

    public void setupSocket()
    {
        try
        {
            tcp_socket = new TcpClient(host, port);

            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream);
            socket_reader = new StreamReader(net_stream);

            socket_ready = true;
            Debug.Log("Client TCP connecté! :)");
        }
        catch (Exception e)
        {
            // Something went wrong
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket(string line)
    {
        if (!socket_ready)
            return;

        line = line + "\r\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }

    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
    }
}
