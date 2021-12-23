#include <string>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>
#include <PubSubClient.h>
#include <functional>
#include <limits>
#include "TimerController.h"
#include "RequestSender.h"
#include "MQTTClient.h"
#include <optional>
using namespace std;

const char *ssid = "TP-Link_40C8";
const char *password = "lebedyn42200";

string bearerToken = "";

String identifier = "1234";

int iotId = -1;

WiFiClient client;
PubSubClient mqttClient(client);

const char *mqtt_broker = "192.168.0.109";
const char *mqtt_username = "iot";
const char *mqtt_password = "password";
const int mqtt_port = 1883;

const int doorClosedPin = 15;
const int doorOpenedPin = 13;
const int connectingSignalPin = 12;
const int sensorPin = 14;
const int alarmPin = 16;
bool isSetAlarm = false;

TimerController<function<void()>, function<void()>> openCloseDoorTimer(String("Door timer"));
TimerController<function<void()>, function<void()>> alarmTimer(String("Alarm timer"));

RequestSender requestSender(client, identifier);

void OpenDoor()
{
  Serial.println("Door opened");
  digitalWrite(doorOpenedPin, HIGH);
  digitalWrite(doorClosedPin, LOW);
}

void CloseDoor()
{
  Serial.println("Door closed");
  digitalWrite(doorOpenedPin, LOW);
  digitalWrite(doorClosedPin, HIGH);
}

void SetAlarm()
{
  Serial.println("Alarm on");
  OpenDoor();
  digitalWrite(alarmPin, HIGH);
}

void OffAlarm()
{
  Serial.println("Alarm off");
  CloseDoor();
  digitalWrite(alarmPin, LOW);
}
MQTTClient* MQTTClient::current = nullptr;
MQTTClient* mqttConnection;



void setup()
{
  Serial.begin(115200);

  WiFi.begin(ssid, password);
  pinMode(2, OUTPUT);
  pinMode(doorClosedPin, OUTPUT);
  pinMode(doorOpenedPin, OUTPUT);
  pinMode(connectingSignalPin, OUTPUT);
  pinMode(sensorPin, OUTPUT);
  pinMode(alarmPin, OUTPUT);

  digitalWrite(2, LOW);

  digitalWrite(connectingSignalPin, HIGH);
  while (WiFi.status() != WL_CONNECTED)
  {
    digitalWrite(2, LOW);
    delay(1000);
    Serial.println("Connecting..");
    digitalWrite(2, HIGH);
  }

  digitalWrite(2, HIGH);

  int tryCount = 0;

  bool isAuthed = requestSender.RegisterIoT(iotId);

  while (!isAuthed && tryCount < 5)
  {
    tryCount++;
    delay(1000);
    Serial.println("Authorizationg failed");
    digitalWrite(2, LOW);
    isAuthed = requestSender.RegisterIoT(iotId);
  }

  digitalWrite(2, LOW);

  if (isAuthed)
  {
    mqttConnection = new MQTTClient(mqttClient, OpenDoor, CloseDoor, SetAlarm, OffAlarm,
                                    openCloseDoorTimer, alarmTimer);

    mqttConnection->SubscribeToTopic(iotId);
   
  }
  digitalWrite(connectingSignalPin, LOW);
  CloseDoor();
}

int sendingSensorValueThreshold = 5;
int currentThreshold = 0;

void loop()
{
  mqttClient.loop();
  float sensorValue;
  sensorValue = analogRead(A0);

  if (sensorValue > 50)
  {
    currentThreshold++;
    Serial.printf("Sensor value: %f \n", sensorValue);
    digitalWrite(sensorPin, HIGH);

    if (currentThreshold >= sendingSensorValueThreshold)
    {
      requestSender.SendSensorDataToServer(sensorValue);
      currentThreshold = 0;
    }
  }
  else
  {
    digitalWrite(sensorPin, LOW);
    currentThreshold = 0;
  }

  delay(1000);
  openCloseDoorTimer.UpdateTime();
  alarmTimer.UpdateTime();
}
