# EventsExpress

Do you want to hang out with company but don't know who whith? Then this application for you! 
You can join any existing event or create your own one. Find the people, chatting them, following most interesting. 
And loads of other enjoyed things to do.


## Project setup

### step 0: Software requirements
- MS Visual Studio (2017 or later) https://visualstudio.microsoft.com/ru/downloads
- .NET Core SDK (v 3.1.403) https://dotnet.microsoft.com/download/dotnet-core/3.1
- NodeJS (v 12.13.1 LTS or later) https://nodejs.org
- Azure Storage Emulator (v 5 or later) https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409

### step 1: Clone repository
- git clone https://github.com/NastiaGerasymchuk/EventsExpress.git
- then checkout to develop branch

### step 2: Prepare project
- choose EventsExpress as startup project
![image](https://user-images.githubusercontent.com/70840510/114342542-aa3b9b00-9b64-11eb-8df9-6be7cd94162b.png)

- choose IIS Express profile for Windows and EventsExpress otherwise
![image](https://user-images.githubusercontent.com/70840510/114342582-bb84a780-9b64-11eb-85c0-c09a0876ce27.png)

- start Azure Storage Emulator with Start menu -> Azure Storage Emulator
- press Ctrl+Shift+B or Build -> Build Solution
- wait until process completed

### step 3: Start project
- click Ctrl+F5 to start project without debugging or F5 otherwise (you can use command Debug -> Start Debugging / Start Without Debugging)

### step 4: Enjoy testing and developing
### Git Flow
We are using simpliest github flow to organize our work:
![Git Flow Ilustration](https://camo.githubusercontent.com/249bd600310c01188d4daf366519c24044e9883e/68747470733a2f2f7363696c6966656c61622e6769746875622e696f2f736f6674776172652d646576656c6f706d656e742f696d672f6769746875622d666c6f772e706e67)
