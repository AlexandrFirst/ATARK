#ifndef MQTTClient_h
#define MQTTClient_h
#include <PubSubClient.h>
#include <functional>
#include "TimerController.h"
#include <string>

//template <typename T>
class MQTTClient
{
private:
using T = function<void()>;
    PubSubClient mqttClient;
    T OpenDoor;
    T CloseDoor;
    T SetAlarm;
    T OffAlarm;
    TimerController<function<void()>, function<void()>> &openCloseDoorTimer;
    TimerController<function<void()>, function<void()>> &alarmTimer;

    const char *mqtt_broker = "192.168.0.109";
    const char *mqtt_username = "iot";
    const char *mqtt_password = "password";
    const int mqtt_port = 1883;

    bool isSetAlarm = false;

    void callback(char *topic, uint8_t *payload, unsigned int length)
    {
        Serial.print("Message arrived in topic: ");
        Serial.println(topic);
        Serial.print("Message:");

        string message = std::string((char *)payload);
        if (message == "open" && !isSetAlarm)
        {
            openCloseDoorTimer.SetTimer(5, CloseDoor, OpenDoor);
        }
        else if (message == "alarm" && !isSetAlarm)
        {
            if (openCloseDoorTimer.IsTimerOn())
            {
                openCloseDoorTimer.FinishTimer();
                delay(1000);
            }
            isSetAlarm = true;
            Serial.println("Alarm is on");
            alarmTimer.SetTimer(
                std::numeric_limits<int>::max() / 2, OffAlarm, SetAlarm);
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

    static void callback_function(char *topic, uint8_t *payload, unsigned int length)
    {
        MQTTClient::current->callback(topic, payload, length);
    }

    void initMqttClient()
    {
        mqttClient.setServer(mqtt_broker, mqtt_port);
        MQTTClient::current = this;
        mqttClient.setCallback(callback_function);

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
    }

public:
    MQTTClient(PubSubClient mqttClient, T openDoor,
               T closeDoor, T setAlarm, T offAlarm,
               TimerController<function<void()>, function<void()>> &openCloseDoorTimer,
               TimerController<function<void()>, function<void()>> &alarmTimer)
        : openCloseDoorTimer(openCloseDoorTimer), alarmTimer(alarmTimer)
    {
        this->mqttClient = mqttClient;
        this->CloseDoor = closeDoor;
        this->OpenDoor = openDoor;
        this->SetAlarm = setAlarm;
        this->OffAlarm = offAlarm;

        initMqttClient();
        Serial.println("connected to mqtt server");
    }

    void SubscribeToTopic(int iotId)
    {
        std::string topicName = "door/" + to_string(iotId);
        Serial.printf("Topic name: %s \n", topicName.c_str());
        mqttClient.subscribe(topicName.c_str());
    }

    static MQTTClient *current;
};

#endif