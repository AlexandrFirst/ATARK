#ifndef TimerController_h
#define TimerController_h

#include "Arduino.h"
#include <string>
using namespace std;

template <typename T, typename T1>
class TimerController
{
public:
    TimerController(String timerName)
    {
        this->timerName = timerName;
    }
    ~TimerController() {}

    void SetTimer(int timeToCheck, T funcToRun)
    {
        Serial.printf("timer %s started \n", timerName.c_str());
        if (this->timeToCheck != 0)
            return;
        this->timeToCheck = timeToCheck;
        this->funcToRun = funcToRun;
    }

    void SetTimer(int timeToCheck, T funcToRun, T1 funcOnTimerStart)
    {
        Serial.printf("extedned timer %s started \n", timerName.c_str());
        Serial.printf("%d \n", timeToCheck);
        if (this->timeToCheck != 0)
            return;
        this->timeToCheck = timeToCheck;
        this->funcToRun = funcToRun;
        isTimerOn = true;
        Serial.println("~~~~~");
        funcOnTimerStart();
    }

    void AddTime(int addTime)
    {
        this->timeToCheck += addTime;
    }

    void UpdateTime()
    {
        if (timeToCheck == 0)
            return;
        currentTime++;
        if (currentTime >= timeToCheck)
        {
            funcToRun();
            currentTime = 0;
            timeToCheck = 0;
            Serial.printf("%s finished \n", timerName.c_str());
            isTimerOn = false;
        }
    }
    void FinishTimer()
    {
        currentTime = timeToCheck;
    }

    bool IsTimerOn(){
        return isTimerOn;
    }

private:
    int currentTime = 0;
    int timeToCheck = 0;
    T funcToRun;
    String timerName = "";
    bool isTimerOn = false;
};

#endif
