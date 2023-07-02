# Evacuation System for Building Safety

## Project Description

The goal of this project is to develop an evacuation system for buildings in case of fire or air strike alerts (specifically for the Ukrainian context). The system consists of separate components, including a server (ASP.NET, EF Core, SignalR), a client application (Angular 15, SignalR), a mobile application (Xamarin Forms), and an IoT component (ESP8266, MQTT).

## Project Structure

The project is structured into the following components:
- **Server**: Developed using ASP.NET with EF Core and SignalR. The server handles requests from clients and the mobile application, stores building data, and provides real-time communication using SignalR. Link to [server project](https://github.com/AlexandrFirst/FireSaver/tree/main/FireSaverApi)
- **Client Application**: Built with Angular 15 and integrated with SignalR for real-time updates. The client application allows users to interact with the system, view evacuation instructions, and receive alerts. Link to [client project](https://github.com/AlexandrFirst/FireSaver/tree/main/FireSaverSPA)
- **Mobile Application**: Developed using Xamarin Forms, the mobile application provides a user-friendly interface for users to access the evacuation system on their mobile devices. It communicates with the server via APIs and receives real-time updates using SignalR. Link to [mobile project](https://github.com/AlexandrFirst/FireSaver/tree/main/FireSaverMobile/FireSaverMobile)
- **IoT Componen**: Utilizes ESP8266 microcontrollers and MQTT protocol for connecting and integrating IoT devices into the evacuation system. These devices can include sensors for detecting smoke, heat, or other emergency conditions. Link to [IOT project](https://github.com/AlexandrFirst/FireSaver/tree/main/FireSaverIoT)

## License
This project is licensed under the MIT License. Feel free to use, modify, and distribute the code as per the terms of the license.
