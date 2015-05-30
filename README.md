# RecordedMock
.NET mocking framework that records behaviour from live environment, then replays it while running tests

# Usage

**1. Recording**

You can record the behaviour of your original implementation by injecting a decorated instance of your original implementation, using RecordingMock.Create method. The generated proxy instance will capture all input arguments and return values on your  implementation, as well as thrown exceptions.

Example from RecordedMock.SampleWebApi (global.asax.cs)
```c#
IKernel kernel = new StandardKernel();
IDataAccess recordingDataAccess = RecordingMock.Create<IDataAccess>(new DataAccess(), @"C:\path\to\mock\mock-DataAccess.json");
kernel.Bind<IDataAccess>().ToMethod(context => recordingDataAccess);
```

After that, you can deploy your code to live environment to collect data that can be used for mocking later on.

**2. Replaying**

Provided that you have recorded data in previous step, you can inject a replaying mock object that is using the captured data.

Example from RecordedMock.SampleWebApi project:
```c#
IKernel kernel = new StandardKernel();
IDataAccess replayingDataAccess = ReplayingMock.Create<IDataAccess>(@"C:\path\to\mock\mock-DataAccess.json");
kernel.Bind<IDataAccess>().ToMethod(context => replayingDataAccess);
```
