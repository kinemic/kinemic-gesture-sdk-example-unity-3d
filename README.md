# Kinemic SDK Unity Sample!

This repository contains a sample application showcasing the usage of the Kinemic SDK to integrate gesture control into a Unity application.

The Kinemic SDK for Unity is available for:

* iOS
* macOS
* Windows
* Windows UWP
* Editor (macOS + Windows)

## SDK Notes
There are some steps you need to configure for some platforms.

### Editor

Independent of the targeted platform, pressing the play button will load the SDK on your host machine and use your **host machines Bluetooth** to search for and connect to a Kinemic Band.

Make sure your computer supports Bluetooth LE and has Bluetooth enabled.

### iOS

For iOS deployment you need to adjust some settings of your generated project. After building the project in Unity, open the generated **Unity-iPhone.xcodeproj** in Xcode.

#### Signing & provisioning profile

In your iOS project settings, select **Signing & Capabilities**, enable **Automatically manage signing** and select your **Team**.

#### Swift support

Anywhere in your project (i.e. Libraries/de.kinemic.gesture/Core/Plugins/iOS/) **right-click > New File > Swift File**, name it *Dummy.swift*. Click **Create Bridging Header**.

#### Bluetooth Capabilities

In your iOS project settings, select **Info**, click the **+** on any key and add:

Deployment target **of iOS 13 and later**
* **Key**: NSBluetoothPeripheralUsageDescription
* **Value**: Bluetooth is used to search for and connect to your Kinemic Band.

Deployment target **earlier than iOS 13**
* **Key**: NSBluetoothAlwaysUsageDescription
* **Value**: Bluetooth is used to search for and connect to your Kinemic Band.

### Windows UWP

For Windows UWP deployment you need to adjust some settings of your generated project. After building the project in Unity (select D3D Project), open the generated Solution in Visual Studio.

#### Bluetooth Capabilities

You need to add Bluetooth Capabilities to you application. Open **Package.appxmanifest**, select **Capabilities** and make sure **Bluetooth** is checked.

## Support

If you have problem running the sample or need help, please contact us at support@kinemic.de.
