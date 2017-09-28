#!/usr/bin/env python3
# coding: utf-8

"""
script pour tester la démo en simulant les controleurs hardwares
"""


import requests
import schedule
import random
import threading


host = "http://localhost:80"
alien1_rate = 20;
alien2_rate = 10;
alien3_rate = 30;
earth_rate = 60;
earth_bases = 1;
earth_munitions = 5;

# helper

def run_threaded(job_func):
    job_thread = threading.Thread(target=job_func)
    job_thread.start()

# timers/hooks

def alien1_send():
    print("alien1: /update rate==%i" % alien1_rate)
    r = requests.get(host+"/update", params={'uid': "alien1", 'param1': alien1_rate})
    print(r.text)

def alien1_update():
    global alien1_rate
    alien1_rate = alien1_rate + 10
    if (alien1_rate > 200):
        alien1_rate = 10
    print("alien1: modification, nouveau rate==%i" % alien1_rate)

def alien2_send():
    print("alien2: /update rate==%i" % alien2_rate)
    r = requests.get(host+"/update", params={'uid': "alien2", 'param1': alien2_rate})
    print(r.text)

def alien2_update():
    global alien2_rate
    alien2_rate = alien2_rate + 10
    if (alien2_rate > 150):
        alien2_rate = 10
    print("alien2: modification, nouveau rate==%i" % alien2_rate)

def alien3_send():
    print("alien3: /update rate==%i" % alien3_rate)
    r = requests.get(host+"/update", params={'uid': "alien3", 'param1': alien3_rate})
    print(r.text)

def alien3_update():
    global alien3_rate
    alien3_rate = alien3_rate + 10
    if (alien3_rate > 250):
        alien3_rate = 10
    print("alien3: modification, nouveau rate==%i" % alien3_rate)

def earth_reset():
    print("earth: RESET")
    r = requests.get(host+"/update", params={'uid': "earth", 'reset': "true"})
    print(r.text)

def earth_send():
    print("earth: /update bases==%i munitions==%i cadence==%i " % (earth_bases, earth_munitions, earth_rate))
    r = requests.get(host+"/update", params={'uid': "earth", 'param1': earth_bases, 'param2': earth_munitions, 'param3': earth_rate})
    print(r.text)

def earth_update():
    global earth_rate
    global earth_bases
    global earth_munitions
    choice = random.randint(1,3)
    if choice == 1:
        earth_rate = earth_rate + 10
        if (earth_rate > 400):
            earth_rate = 10
        print("earth: modification, nouveau rate==%i" % earth_rate)
    elif choice == 2:
        earth_bases = earth_bases + 1
        if (earth_bases > 4):
            earth_bases = 1
        print("earth: modification, bases==%i" % earth_bases)
    elif choice == 3:
        earth_munitions = earth_munitions - 1
        if (earth_munitions == 0):
            earth_munitions = 5
        print("earth: modification, munitions==%i" % earth_munitions)

# main
earth_reset();
schedule.every(1).seconds.do(run_threaded, alien1_send);
schedule.every(10).seconds.do(run_threaded, alien1_update);
schedule.every(1).seconds.do(run_threaded, alien2_send);
schedule.every(10).seconds.do(run_threaded, alien2_update);
schedule.every(1).seconds.do(run_threaded, alien3_send);
schedule.every(10).seconds.do(run_threaded, alien3_update);
schedule.every(1).seconds.do(run_threaded, earth_send);
schedule.every(10).seconds.do(run_threaded, earth_update);
schedule.every(20).seconds.do(run_threaded, earth_reset);
while True:
    schedule.run_pending()
