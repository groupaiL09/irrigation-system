import random
import time
import  sys
from  Adafruit_IO import  MQTTClient

AIO_FEED_ID = "temperature"
AIO_USERNAME = "groupaiL09"
AIO_KEY = "aio_njzm76fKeROefSeNRAUMsW4ilPix"

def  connected(client):
    print("Ket noi thanh cong...")
    client.subscribe(AIO_FEED_ID)

def  subscribe(client , userdata , mid , granted_qos):
    print("Subscribe thanh cong...")

def  disconnected(client):
    print("Ngat ket noi...")
    sys.exit (1)

def  message(client , feed_id , payload):
    print("Nhan du lieu: " + payload)

client = MQTTClient(AIO_USERNAME , AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()

while True:
    value = random.randint(0, 100)
    print("Cap nhat:", value)
    client.publish("farm1.temperature", value)
    client.publish("farm1.moisture", value+2)
    # client.publish("farm1.pump-status", 1)
    # client.publish("farm1.mode-auto", 0)
    time.sleep(20)