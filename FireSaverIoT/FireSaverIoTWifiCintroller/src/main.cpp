#include <string>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>
#include <PubSubClient.h>
using namespace std;

const char *ssid = "TP-Link_40C8";
const char *password = "lebedyn42200";

string bearerToken = "";

string identifier = "1234";

int iotId = -1;

WiFiClient client;
PubSubClient mqttClient(client);

const char *mqtt_broker = "192.168.0.109";
const char *mqtt_username = "iot";
const char *mqtt_password = "password";
const int mqtt_port = 1883;

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
  for (int i = 0; i < length; i++)
  {
    Serial.print((char)payload[i]);
  }
  Serial.println();
  Serial.println("-----------------------");
}

void setup()
{
  Serial.begin(115200);

  WiFi.begin(ssid, password);
  pinMode(2, OUTPUT);
  digitalWrite(2, LOW);

  while (WiFi.status() != WL_CONNECTED)
  {
    digitalWrite(2, LOW);
    delay(1000);
    Serial.println("Connecting..");
    digitalWrite(2, HIGH);
  }

  digitalWrite(2, HIGH);

  int tryCount = 0;

  bool isAuthed = RegisterIoT();

  while (!isAuthed && tryCount < 5)
  {
    tryCount++;
    delay(1000);
    Serial.println("Authorizationg failed");
    digitalWrite(2, LOW);
    isAuthed = RegisterIoT();
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
}

void loop()
{
  mqttClient.loop();
  // put your main code here, to run repeatedly:
}

void OpenDoor()
{
  //http.addHeader("Authorization", bearerToken);
}
