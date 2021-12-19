#ifndef RequestSender_h
#define RequestSender_h

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <Arduino.h>
#include <string>

class RequestSender
{
private:
    WiFiClient client;
    String bearerToken;
    int iotId = -1;
    String iotIdentifier = "";
public:
    RequestSender(WiFiClient client, String iotIdentifier);
    bool RegisterIoT(int& iotId);
    void SendSensorDataToServer(float value);
    ~RequestSender();
};

#endif
