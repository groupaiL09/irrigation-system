from dataclasses import dataclass
import serial.tools.list_ports
import pickle
import sys
import time
from datetime import datetime
from Adafruit_IO import MQTTClient

farm_data = {'temp':0, 'moisture': 0 ,'pump_status':"0", 'mode_auto':"0"}
nextDay = 0

# ADA_FEED_ID = ["farm1.moisture", "farm1.temperature", "farm1.pump-status","farm1.mobile","farm1.mode-auto"]
ADA_FEED_ID = ["farm1.moisture", "farm1.temperature", "farm1.pump-status","farm1.mobile","farm1.mode-auto"]
AIO_USERNAME = "groupaiL09"
AIO_KEY = "aio_bPNS10ry1euFzL4mPL60LfjAnqVE"


with open ('./classification_model_pkl.txt','rb') as f:
    classificationModel = pickle.load(f)
    
with open('./regression_model_pkl.txt','rb') as g:
    regressionModel = pickle.load(g)

min_temp, max_temp, min_moist, max_moist = (30.8, 35.2, 12.79, 68.55)


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
        




client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()


def getPort():
    ports = serial.tools.list_ports.comports()
    N = len(ports)
    commPort = "None"
    for i in range(0, N):
        port = ports[i]
        strPort = str(port)
        print(strPort)
        if "USB Serial Device" in strPort:
            splitPort = strPort.split(" ")
            commPort = splitPort[0]
    return commPort


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

while True:
    readSerial()
    if farm_data["mode_auto"] == "1":
        print("line 128", type(farm_data["mode_auto"]))
        new_data = [farm_data['temp'],farm_data['moisture']]
        decision = classificationModel.predict([new_data])[0]
        if(str(decision) != farm_data["pump_status"]):
            farm_data["pump_status"] = str(decision)
            client.publish("farm1.pump-status",str(decision))
            if(decision == 0):
                now = datetime.now()
                hour = now.hour
                new_data.append(hour)
                nextDay = regressionModel.predict([new_data])[0]
                client.publish("farm1.mobile", str(nextDay))
        print("line 136", decision)
    time.sleep(3)


