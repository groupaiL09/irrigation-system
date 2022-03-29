import sys
from Adafruit_IO import MQTTClient

AIO_FEED_ID = "mobile"
AIO_USERNAME = "groupaiL09"
AIO_KEY = "aio_ZToV229NvXpLfP8uk198o2kONuH1"

def connected(client):
    print('Subscribing to Feed {0}'.format(AIO_FEED_ID))
    client.subscribe(AIO_FEED_ID)
    print('Waiting for feed data...')

def disconnected(client):
    sys.exit(1)

def message(client, feed_id, payload):
    print('Feed {0} received new value: {1}'.format(feed_id, payload))

client = MQTTClient(AIO_USERNAME , AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.connect()
client.loop_blocking()