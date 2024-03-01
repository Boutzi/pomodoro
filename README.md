# Pomodoro

## Overview
Pomodoro is a simple timer application built using C# and WPF (Windows Presentation Foundation). It follows the Pomodoro Technique, a time management method that uses a timer to break down work into intervals, traditionally 25 minutes in length, separated by short breaks.

## Features
- **Pomodoro Timer**: Set a timer for your work session, typically 25 minutes long.
- **Short Break Timer**: Take short breaks after each work session, typically 5 minutes long.
- **Long Break Timer**: After completing a set number of work sessions, take a longer break, typically 15 minutes long.
- **Customizable Settings**: Adjust the duration of the pomodoro, short break, and long break sessions according to your preferences.
- **Cycle Counter**: Keep track of how many pomodoro cycles you've completed.

## How to Use
1. **Download**: Clone or download the source code from this repository.
2. **Build**: Open the project in Visual Studio and build the solution.
3. **Run**: Start the application by running the generated executable file (`app.exe`) located in the `release` folder.
4. **Configure Settings**: Adjust the duration of pomodoro, short break, and long break sessions by entering the desired values in the corresponding text boxes.
5. **Start Timer**: Click on the "Pomodoro" button to start the timer for your work session.
6. **Take Breaks**: After each work session, the application will automatically switch to short breaks and long breaks according to your predefined settings.
7. **Repeat**: Continue working in pomodoro cycles until you've reached your productivity goals.

## Screenshots
![Capture d'écran de l'application Pomodoro](https://image.noelshack.com/fichiers/2024/09/5/1709264473-whatsapp-image-2024-03-01-at-04-40-56-2a6933fe.jpg)


## Dependencies
- NAudio.Wave: A .NET audio library that provides APIs to play, record, and process audio.
- System.Media: Provides access to system sounds for playing audio alerts.

## License
This project is licensed under the [MIT License](LICENSE).
