#!/usr/bin/env python
# coding: utf-8

"""
serveur python pour la démo métro des JdS
(python2 compatible)
<insert licence here>

à tester avec le module HTTPIE:
http localhost/update uid==earth reset==true
ou
http localhost/update uid==alien1 reset==true
ou
http localhost/update uid==earth param1==2 param2==4 param3==56

break console avec CTRL-Y ou CTRL-BREAK

C'est ce serveur qui envoie des valeurs de "0" pour earth ou les alien* si pas d'update depuis plus de 5 secondes...
"""


from threading import Timer
from bottle import run, request, get
import socket
import datetime as dt


# functions
def checkTCPConnection():
    global client, address, connected, s
    if connected is not True:
        try:
            client, address = s.accept()
            print("*** TCP Client connected! ***")
            client.send("Hello!\n")
            connected = True
        except KeyboardInterrupt:
            print("exiting")
            s.close()
            exit(0)
        except:
            print("*** error/exception dans checkTCPConnection ***")
            connected = False
            client = None
    t = Timer(1.0, checkTCPConnection)
    t.start()

"""
def alive_test():
    global client, address, connected, s
    if connected is True:
        try:
            client.send("still alive?\n")
        except:
            print("*** error/exception dans alive_test ***")
            connected = False
            client = None
    t2 = Timer(2.0, alive_test)
    t2.start()
"""

def check_timeouts():
    global connected, last_earth_ts, timeout_delay, client
    if connected is True:
        if (dt.datetime.now()-last_earth_ts).total_seconds() > timeout_delay:
            try:
                print("*** TIMEOUT EARTH ***")
                client.send("earth %s %s %s\n" % (int(0), int(5), int(0)))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        if (dt.datetime.now()-last_alien1_ts).total_seconds() > timeout_delay:
            try:
                print("*** TIMEOUT ALIEN1 ***")
                client.send("alien1 %s\n" % int(0))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        if (dt.datetime.now()-last_alien2_ts).total_seconds() > timeout_delay:
            try:
                print("*** TIMEOUT ALIEN2 ***")
                client.send("alien2 %s\n" % int(0))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        if (dt.datetime.now()-last_alien3_ts).total_seconds() > timeout_delay:
            try:
                print("*** TIMEOUT ALIEN3 ***")
                client.send("alien3 %s\n" % int(0))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
    t2 = Timer(1.0, check_timeouts)
    t2.start()

# webhooks

@get('/alive')
def do_alive():
    return "OK"

@get('/update')
def do_update():
    global client, connected, last_earth_ts, timeout_delay
    global last_alien1_ts, last_alien2_ts, last_alien3_ts
    uid = request.query.uid
    reset = request.query.reset
    param1 = request.query.param1
    param2 = request.query.param2
    param3 = request.query.param3
    # pas de connection avec le client TCP Unity
    if connected is not True:
        return "ERROR"
    else:
        if uid=="earth":
            if reset=="true":
                print("** RESET DEMANDE **")
                try:
                    last_earth_ts = dt.datetime.now()
                    client.send("earth reset\n")
                except:
                    print("*** error/exception sur send ***")
                    connected = False
                    client = None
            elif (param1 is not None and param2 is not None and param3 is not None):
                print("** MAJ EARTH: param1==%i param2==%i param3==%i **" % (int(param1), int(param2), int(param3)))
                try:
                    last_earth_ts = dt.datetime.now()
                    client.send("earth %s %s %s\n" % (int(param1), int(param2), int(param3)))
                except:
                    print("*** error/exception sur send ***")
                    connected = False
                    client = None
        elif (uid=="alien1" and param1 is not None):
            print("** MAJ ALIEN1: param1==%i **" % int(param1))
            try:
                last_alien1_ts = dt.datetime.now()
                client.send("alien1 %s\n" % (int(param1)))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        elif (uid=="alien2" and param1 is not None):
            print("** MAJ ALIEN2: param1==%i **" % int(param1)) 
            try:
                last_alien2_ts = dt.datetime.now()
                client.send("alien2 %s\n" % (int(param1)))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        elif (uid=="alien3" and param1 is not None):
            print("** MAJ ALIEN3: param1==%i **" % int(param1)) 
            try:
                last_alien3_ts = dt.datetime.now()
                client.send("alien3 %s\n" % (int(param1)))
            except:
                print("*** error/exception sur send ***")
                connected = False
                client = None
        else:
            print("*** BAD HTTP REQUEST! ***")
            return "ERROR"
        try:
            data = client.recv(size).rstrip('\r\n')
            if not data:
                client = None
                return "ERROR"
            else:
                return data
        except:
            print("*** error/exception sur receive ***")
            connected = False
            client = None
            return "ERROR"

# main
last_earth_ts = dt.datetime.now()
last_alien1_ts = dt.datetime.now()
last_alien2_ts = dt.datetime.now()
last_alien3_ts = dt.datetime.now()
timeout_delay = 5

print
print("!!! A lancer avec Python2 UNIQUEMENT !!!")
print

# TCP server init
client = None
address = None
connected = False;
client = None
host = '' 
port = 50000 
backlog = 5 
size = 1024 
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM) 
s.bind((host,port)) 
s.listen(backlog)

t = Timer(1.0, checkTCPConnection)
t.start()
t2 = Timer(1.0, check_timeouts)
t2.start()

# HTTP server init
hostname = "localhost"
# ou "192.168.1.10"
port = 80
run(host=hostname, port=port)
