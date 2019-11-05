# Welcome to the Kinemic Gesture SDK!

This document will walk you through the setup to integrate the Kinemic gesture control into your unity project. After you added the package to your project though the Package Manager the SDK is ready to use in the Editor and on all Standalone builds. There are some steps you need to configure for specific platforms.

## Editor

Independent of the targeted platform, pressing the play button will load the SDK on your host machine and use your **host machines Bluetooth** to search for and connect to a Kinemic Band.

Make sure your computer supports Bluetooth LE and has Bluetooth enabled.

## iOS

For iOS deployment you need to adjust some settings of your generated project. After building the project in Unity, open the generated **Unity-iPhone.xcodeproj** in Xcode.

### Signing & provisioning profile

In your iOS project settings, select **Signing & Capabilities**, enable **Automatically manage signing** and select your **Team**.

### Swift support

Anywhere in your project (i.e. Libraries/de.kinemic.gesture/Core/Plugins/iOS/) **right-click > New File > Swift File**, name it *Dummy.swift*. Click **Create Bridging Header**.

### Bluetooth Capabilities

In your iOS project settings, select **Info**, click the **+** on any key and add:

Deployment target **of iOS 13 and later**
* **Key**: NSBluetoothPeripheralUsageDescription
* **Value**: Bluetooth is used to search for and connect to your Kinemic Band.

Deployment target **earlier than iOS 13**
* **Key**: NSBluetoothAlwaysUsageDescription
* **Value**: Bluetooth is used to search for and connect to your Kinemic Band.

## Android

No need to adjust default settings. If you find a problem, please contact us at support@kinemic.de.

## Windows UWP

TODO: Bluetooth Capabilities

## macOS Standalone

No need to adjust default settings. If you find a problem, please contact us at support@kinemic.de.

## Windows Standalone

No need to adjust default settings. If you find a problem, please contact us at support@kinemic.de.
