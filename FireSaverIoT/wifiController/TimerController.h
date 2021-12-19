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
      Serial.printf("%s started \n", timerName.c_str());
      if (timeToCheck != 0)
        return;
      this->timeToCheck = timeToCheck;
      this->funcToRun = funcToRun;
    }

    void SetTimer(int timeToCheck, T funcToRun, T1 funcOnTimerStart)
    {
      Serial.printf("%s started \n", timerName.c_str());
      if (this->timeToCheck != 0)
        return;
      this->timeToCheck = timeToCheck;
      this->funcToRun = funcToRun;

      funcOnTimerStart();
    }

    void AddTime(int addTime)
    {
      this->timeToCheck += addTime;
    }

    void UpdateTime()
    {
//      Serial.printf("%d current time \n", currentTime);
//       Serial.printf("%d time to check \n", timeToCheck);
      if (timeToCheck == 0)
        return;
      currentTime++;
      
      if (currentTime >= timeToCheck)
      {
        funcToRun();
        currentTime = 0;
        timeToCheck = 0;
        Serial.printf("%s finished \n", timerName.c_str());
      }
    }
    void FinishTimer()
    {
      currentTime = timeToCheck;
    }

  private:
    int currentTime = 0;
    int timeToCheck = 0;
    T funcToRun;
    String timerName = "";
};

#endif
