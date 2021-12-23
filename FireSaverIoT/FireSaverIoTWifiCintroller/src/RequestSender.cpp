#include "RequestSender.h"
#include <ArduinoJson.h>

RequestSender::RequestSender(WiFiClient client, String iotIdentifier)
{
    this->client = client;
    this->iotIdentifier = iotIdentifier;
}

RequestSender::~RequestSender()
{
}

bool RequestSender::RegisterIoT(int &iotId)
{
    HTTPClient http;
    StaticJsonDocument<100> loginDataJs;
    StaticJsonDocument<100> responseJs;
    loginDataJs["Identifier"] = iotIdentifier;
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
    this->bearerToken = responseJs["token"].as<String>();

    Serial.println("iotId: " + String(iotId));
    Serial.println(responseJs["token"].as<String>());
    return true;
}

void RequestSender::SendSensorDataToServer(float value)
{
    HTTPClient http;
    StaticJsonDocument<100> sensorValueJs;  
    
    http.begin(client, String("http://192.168.0.109:5000/IoT/iotDataSent/") + String(this->iotId));
    http.addHeader("Content-Type", "application/json");


    http.addHeader("Authorization", String("Bearer ") + this->bearerToken);

    sensorValueJs["SensorValue"] = value;
    char sensorValue[100];
    serializeJson(sensorValueJs, sensorValue);
    String dataToSend = String(sensorValue);

    Serial.println("Sending sensor data: " + dataToSend);

    int httpCode = http.POST(dataToSend);
    String payload = http.getString();
    Serial.println("Response after sebding: " + payload);
}