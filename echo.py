#!/usr/bin/env python 

""" 
simple TCP server to echo back messages from Unity TCP client

note: dans la phase 'accept', le CTR-C ne fonctionne pas, faire un CTRL-BREAK!

""" 

import socket

client = None
host = '' 
port = 50000 
backlog = 5 
size = 1024 
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
s.bind((host,port)) 
s.listen(backlog) 

while True:
    try:
        client, address = s.accept() 
        print("Client connected.")
        client.send("Hello!\n") 
    except KeyboardInterrupt:
        print("exiting")
        s.close()
        exit(0)
    except:
        print("error/exception!")
        s.close()
        exit(0)


    try:
        while True:
            data = client.recv(size).rstrip('\r\n')
            if not data:
                print("client=>NONE")
                client = None
                break                
            else:
                if data=="quit":
                    client.send("Bye!\n")
                    client.close()
                    break
                else:
                    client.send("You just said: " + data + "\n") 
    except KeyboardInterrupt:
        print("exiting")
        s.close()
        exit(0)
    except:
        print("error/exception!")
    finally:
        # Clean up the connection
        client = None
