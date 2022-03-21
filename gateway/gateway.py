import serial.tools.list_ports
import sys
import time
from Adafruit_IO import MQTTClient

ADA_FEED_ID = ["farm1.moisture", "farm1.temperature", "farm1.pump-status"]
AIO_USERNAME = "groupaiL09"
AIO_KEY = "aio_vPIj09Q2cV2yVj8uoqnby2UBKCcG"


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
            time.sleep(2)


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
    print(data)
    # Format for processing data:
    # For temperature: "!1:TEMP:value#"
    # For moisture: "!1:MOIST:value#"
    # For pump: "!1:PUMP:value#"
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    if splitData[1] == "TEMP":
        client.publish("farm1.temperature", splitData[2])
    elif splitData[1] == "MOIST":
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
    time.sleep(5)


