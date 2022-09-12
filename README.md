# DeveloperTest

Please write an extended ping-type program as follows:

### Requirements

 - Language : C#
 - .NET Framework : Any .NET4 or .NET Core or .NET6
 - Type : WPF Desktop Application
 - IDE : VS2019 or VS2022

**Please apply best coding practices, methodologies, patterns, principles and conventions _as if this was a commercial application_**
---

### GUI

 - Dialog with a Grid, two buttons (Start and Stop) and a Status bar
 - Pressing Start should start the echo process as described further below
 - Pressing Stop should stop the echo process
 - The results of the echo test should be displayed in the Grid in the following columns (Started , Processed , Elapsed)
 - Status bar should be used for displaying info to the user

### Echo Process

 - Repeat the below exactly every 500 ms
 - Create object that contains an initialized GUID
 - Serialize the object using a Json serializer
 - Send the json string using a TCP socket to the echotool
 <br>(The echotool will echo back all received data)
 - Parse and assemble the echo'ed json data packet(s) and deserialize it back into an object
 - Add this echo result to the GUI Grid into the relevant columns 
 <br>(Started = timestamp when this echo process started , Processed = timestamp after deserialization into an object , Elapsed = milliseconds elapsed from Started to Processed)

### Notes

 - Do not spend too much time however on making a pretty interface, so long as it does not look like something a 10 year old slapped together.
 Also do not spend too much time on handling every exception/error that could possibly occur.
 - Your program **must** still be robust though in that it should not crash, but rather display appropriate messages in the GUI Status bar.
 - There is no time limit on completing the test
 - Create a branch for your code from this Github repository. Once completed publish your code back to GitHub and send a pull request
 <br>Use the pull request to comment on your implementation, possible improvements, or anything else that you would like to mention.

#### Using echotool.exe

This is the TCP server program that echoes your sent data back to you.

Start echotool as follows:<br>
echotool /p tcp /s 12345

#### Using puttytel.exe

This is just used to make a few quick echo tests to the echotool.
 - Run puttytel.exe
 - Open a Telnet type connection to IP address 127.0.0.1 on Port 12345
 - Anything you now type or paste (using right click) into the terminal window should be echoed back to you
