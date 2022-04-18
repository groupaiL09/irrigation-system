from dataclasses import dataclass
import serial.tools.list_ports
import pickle
import sys
import time
import numpy as np
import skfuzzy as fuzz
from skfuzzy import control as ctrl
from datetime import datetime
from Adafruit_IO import MQTTClient

# Manual checking by switching mode_auto to 0
farm_data = {'temp':28, 'moisture': 50 ,'pump_status':"0", 'mode_auto':"1"}
nextDay = 0

# ADA_FEED_ID = ["farm1.moisture", "farm1.temperature", "farm1.pump-status","farm1.mobile","farm1.mode-auto"]
ADA_FEED_ID = ["farm1.moisture", "farm1.temperature", "farm1.pump-status","farm1.mobile","farm1.mode-auto"]
AIO_USERNAME = "groupaiL09"
AIO_KEY = "aio_GhjT84rbMhQj4y0yhKxDI9DAhkOB"

# Dealing with fuzzy logic starts here
temperature = ctrl.Antecedent(np.arange(15, 40, 0.1), 'temperature')
moisture = ctrl.Antecedent(np.arange(0, 101, 1), 'moisture')
pump = ctrl.Consequent(np.arange(0, 5, 0.1), 'pump')

temperature['low'] = fuzz.trapmf(temperature.universe, [15, 15, 20, 25])
temperature['medium'] = fuzz.trimf(temperature.universe, [20, 26, 32])
temperature['high'] = fuzz.trapmf(temperature.universe, [26, 32, 40, 40])

moisture['low'] = fuzz.trimf(moisture.universe, [0, 0, 40])
moisture['medium'] = fuzz.trimf(moisture.universe, [30, 45, 60])
moisture['high'] = fuzz.trapmf(moisture.universe, [50, 60, 100, 100])

pump['low'] = fuzz.trimf(pump.universe, [0, 0, 3])
pump['high'] = fuzz.trimf(pump.universe, [2, 5, 5])

rule1 = ctrl.Rule(temperature['high'] | moisture['low'], pump['high'])
rule2 = ctrl.Rule(temperature['medium'] & moisture['medium'], pump['high'])
rule3 = ctrl.Rule(moisture['high'] & temperature['medium'], pump['low'])
rule4 = ctrl.Rule(moisture['medium'] & temperature['low'], pump['low'])

pump_ctrl = ctrl.ControlSystem([rule1, rule2, rule3, rule4])
pump_vol = ctrl.ControlSystemSimulation(pump_ctrl)

# Dealing with fuzzy logic finishes here


with open ('./classification_model_pkl.txt','rb') as f:
    classificationModel = pickle.load(f)
    
with open('./regression_model_pkl.txt','rb') as g:
    regressionModel = pickle.load(g)

# min_temp, max_temp, min_moist, max_moist = (30.8, 35.2, 12.79, 68.55)


def connected(client):
    print('Ket noi thanh cong...')
    for feed in ADA_FEED_ID:
        client.subscribe(feed)


def subscribe(client, userdata, mid, granted_qos):
    print("Subscribe thanh cong...")


def disconnected(client):
    print("Ngat Ket noi...")
    sys.exit(1)


def message(client, feed_id, payload):
    print("Nhan du lieu: " + feed_id + payload)
    if isMicrobitConnected:
        if feed_id == "farm1.pump-status":
            ser.write((str(payload) + "#").encode())
            farm_data["pump_status"] = str(payload)
            time.sleep(2)
        if feed_id == "farm1.mobile":
            if payload == "1":
                client.publish("farm1.pump-status",farm_data["pump_status"])
                client.publish("farm1.mode-auto",farm_data["mode_auto"])
                client.publish("farm1.mobile", nextDay)
        if feed_id == "farm1.mode-auto":
            farm_data["mode_auto"] = payload

# Check this
def getNextDay(new_data):
    global nextDay
    nextDay = regressionModel.predict([new_data])[0]
    return nextDay


client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()


def getPort():
    ports = serial.tools.list_ports.comports()
    for i in range(len(ports)):
        print(str(ports[i]))
    # N = len(ports)
    # commPort = "None"
    # for i in range(0, N):
    #     port = ports[i]
    #     strPort = str(port)
    #     print(strPort)
    #     if "USB Serial Device" in strPort:
    #         splitPort = strPort.split(" ")
    #         commPort = splitPort[0]
    # return commPort
    return "COM5"


ser = serial.Serial(port=getPort(), baudrate=115200)
isMicrobitConnected = True

def processData(data):
    print('line 82: ', data)
    
    # Format for processing data:
    # For temperature: "!1:TEMP:value#"
    # For moisture: "!1:MOIST:value#"
    # For pump: "!1:PUMP:value#"

    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    if len(splitData) != 3:
        print('line 92', data)
        return

    if splitData[1] == "TEMP":
        farm_data['temp'] = splitData[2]
        client.publish("farm1.temperature", splitData[2])
    elif splitData[1] == "MOIST" and len(splitData[2]) != 0:
        splitData[2] = round(float(int(splitData[2]) / 1024 *100),2)
        farm_data['moisture'] = splitData[2]
        client.publish("farm1.moisture", splitData[2])
    elif splitData[1] == "PUMP":
        client.publish("farm1.pump-status", splitData[2])

mess = ""

def readSerial():
    bytesToRead = ser.inWaiting()
    if bytesToRead > 0:
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF -8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if end == len(mess):
                mess = ""
            else:
                mess = mess[end + 1:]
        print(mess)

# 0 is initial state, 1 is on and -1 is off.
state = 0
startTime = 0
UPPERLIMIT1 = 20
UPPERLIMIT2 = 3
while True:
    readSerial()
    if farm_data["mode_auto"] == "1":
        print("line 128", type(farm_data["mode_auto"]))
        new_data = [farm_data['temp'],farm_data['moisture']]
        decision = classificationModel.predict([new_data])[0]
        if state != 0:
            t = (datetime.now()-startTime).total_seconds()
            if state == 1:
                if t >= UPPERLIMIT1:
                    print("line 180 at state ",state)
                    farm_data["pump_status"] = str(state)
                    client.publish("farm1.pump-status",str(state)) 
                    state = 0
            if state == -1: # From pumping 2 not pumping
                if t >= UPPERLIMIT2:
                    farm_data["pump_status"] = "0"
                    client.publish("farm1.pump-status","0") 
                    state = 0
                    now = datetime.now()
                    hour = now.hour
                    new_data.append(hour)
                    #nextDay = regressionModel.predict([new_data])[0]
                    nextDay = getNextDay(new_data)
                    client.publish("farm1.mobile", str(nextDay))
        elif(str(decision) != farm_data["pump_status"]):
            # farm_data["pump_status"] = str(decision)
            # client.publish("farm1.pump-status",str(decision))
            if(decision == 0):

                state = -1 
                
            else:
                state = 1
            
            startTime = datetime.now()
        print("line 136", decision)
    time.sleep(3)


