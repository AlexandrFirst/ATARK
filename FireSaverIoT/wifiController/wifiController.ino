#include <string>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>
#include <PubSubClient.h>
#include <functional>
#include <limits>
#include "TimerController.h"
#include "RequestSender.h"
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
  digitalWrite(doorOpenedPin, HIGH);
  digitalWrite(doorClosedPin, LOW);
}

void CloseDoor()
{
  if(isSetAlarm)
    return;
  digitalWrite(doorOpenedPin, LOW);
  digitalWrite(doorClosedPin, HIGH);
}

void SetAlarm()
{
  OpenDoor();
  digitalWrite(alarmPin, HIGH);
}

void OffAlarm()
{
  CloseDoor();
  digitalWrite(alarmPin, LOW);
  Serial.println("Switching off alarm");
}

bool RegisterIoT()
{
  digitalWrite(2, HIGH);
  Serial.println("Authorizationg..");

  HTTPClient http;

  StaticJsonDocument<100> loginDataJs;
  StaticJsonDocument<100> responseJs;

  loginDataJs["Identifier"] = identifier;
  char loginData[100];
  serializeJson(loginDataJs, loginData);

  String dataToSend = String(loginData);
  Serial.println("Sending data: " + dataToSend);
  http.begin(client, "http://192.168.0.109:5000/IoT/loginIot");
  http.addHeader("Content-Type", "application/json");

  int httpCode = http.POST(dataToSend);
  String payload = http.getString();
  Serial.println(payload);

  if (payload == "" || httpCode <= 0)
  {

    Serial.println(httpCode);
    Serial.println("Response payload: " + payload);

    return false;
  }

  deserializeJson(responseJs, payload);

  iotId = responseJs["userId"].as<int>();
  Serial.println("iotId: " + String(iotId));

  return true;
}

void callback(char *topic, uint8_t *payload, unsigned int length)
{
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
  Serial.print("Message:");

  string message = std::string((char *)payload);
  if (message == "open1" && !isSetAlarm)
  {
    openCloseDoorTimer.SetTimer(5, CloseDoor, OpenDoor);
  }
  else if (message == "alarm" && !isSetAlarm)
  {
    openCloseDoorTimer.FinishTimer();
    isSetAlarm = true;
    Serial.println("Alarm is on");
    alarmTimer.SetTimer(
        std::numeric_limits<int>::max()/2, OffAlarm, SetAlarm);
  }
  else if (message == "close" && isSetAlarm)
  {
    isSetAlarm = false;
    alarmTimer.FinishTimer();
    Serial.println("Alarm is off");
  }
  Serial.println(message.c_str());
  Serial.println("-----------------------");
}

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
    mqttClient.setServer(mqtt_broker, mqtt_port);
    mqttClient.setCallback(callback);
    while (!mqttClient.connected())
    {
      String client_id = "esp8266-client-";
      client_id += String(WiFi.macAddress());
      Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
      if (mqttClient.connect(client_id.c_str(), mqtt_username, mqtt_password))
      {
        Serial.println("Public emqx mqtt broker connected");
      }
      else
      {
        Serial.print("failed with state ");
        Serial.print(mqttClient.state());
        delay(2000);
      }
    }
    Serial.println("connected to mqtt server");
    string topicName = "door/" + to_string(iotId);
    Serial.printf("Topic name: %s \n", topicName.c_str());
    mqttClient.subscribe(topicName.c_str());
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
