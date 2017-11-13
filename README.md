Story Builder is an app for children who are just learning to read to support their engagement with words. This application is built in Unity using RTVoice for enabling Text-To-Speech and featuring a robust drag and drop system.

To open the project in a Windows environment, begin by cloning this repository on your local machine. Once cloned, the file MultiPlatformStoryBuilder/Assets/StoryBuilder.unity scene file may be double-clicked to open with your installed distribution of Unity. Once opened, modify the build settings for Android and click the Play button triangle at the top of the screen. To change your build settings, follow the below tutorial for building to Android devices. If you experience trouble with the scene opening you may wish to try the same version of Unity (Unity 5.5.3 https://unity3d.com/get-unity/download/archive) that the project was built on. 

Building a Unity project for Android devices.
https://unity3d.com/learn/tutorials/topics/mobile-touch/building-your-unity-game-android-device-testing

Building a Unity project for iOS devices.
https://unity3d.com/learn/tutorials/topics/mobile-touch/building-your-unity-game-ios-device-testing


Any questions, comments or concerns can be addressed to either of the following emails...
antin006@morris.umn.edu
lamberty@morris.umn.edu



<b>Adding more context packs</b> <br>
The ContextPackFactory manages building context packs stored as text files in the Resources directory. For example, there is currently a MyLittlePony.txt context pack file that contains all of the words of the MyLittlePony context pack. To add a new context pack, simply add a new text file with words separated by new lines to the Resources directory (see MyLittlePony.txt contents as an example). Once a context pack file has been created and words added to it, a string for the context pack text file name must be added to the LOCAL_CONTEXT_PACK_NAMES string array in the ContextPackFactory class. Once the file has been created and its name added to the LOCAL_CONTEXT_PACK_NAMES array, set the flag REBUILD_MASTER_CONTEXT_PACK in the WordBank class to true to enable the master context pack file to be rebuilt. Run the application to have the context pack files re-read and built into the master context pack json file. After the master context pack json file has been created, feel free to set the REBUILD_MASTER_CONTEXT_PACK flag to false to disable future re-builds of the master context pack json file.
