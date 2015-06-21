# RecordedMock
.NET mocking framework that records behaviour from live environment, then replays it while running tests

# Usage for Integration Testing

![Usage](https://github.com/akos-sereg/RecordedMock/blob/master/docs/usage.gif?raw=true "Usage")

## 1. Recording Phase
###  1.1 Recording HTTP Requests and Responses

Simply annotate your Web API controller like this:

```c#
[RecordRequest(@"C:\Users\Akos\dump.json", 10)]
public class SampleController : ApiController
{
   ...
}
```

###  1.2 Recording invocations

You can record the behaviour of your original implementation by injecting a decorated instance of that, using RecordingMock.Create method. The generated proxy instance will capture all input arguments and return values on your  implementation, as well as thrown exceptions.

Example from RecordedMock.SampleWebApi (global.asax.cs)
```c#
IKernel kernel = new StandardKernel();

IDataAccess recordingDataAccess = RecordingMock.Create<IDataAccess>(
  kernel.Get<DataAccess>(), // Original service
  @"C:\path\to\mock\mock-DataAccess.json", // Recording invocations here
  10,     // Maximum allowed dump size in MBs
  true);  // Service's behaviour is deterministic: DataAccess calls provide the same result for the same arguments
  
kernel.Bind<IDataAccess>().ToMethod(context => recordingDataAccess);
```

After that, you can deploy your code to live environment to collect data that can be used for mocking later on.

## 2. Replaying (Test) Phase

### 2.1 Preparing dependencies (Replaying Mocks) of System-under-test

Provided that you have recorded enough data in previous step, you can inject a replaying mock object that is using the captured data.

Example from RecordedMock.SampleWebApi project:
```c#
IKernel kernel = new StandardKernel();
IDataAccess replayingDataAccess = ReplayingMock.Create<IDataAccess>(@"C:\path\to\mock\mock-DataAccess.json");
kernel.Bind<IDataAccess>().ToMethod(context => replayingDataAccess);
```

### 2.2 Run test cases

Run RecordedMock.ObjectBrowser, open "C:\Users\Akos\dump.json" file that you captured in step 1.1 (recorded http requests and responses).
Right click on a request, and click "Resend" or "Resend All" context menu item. Captured request will be resent to your server, and your server's response will be compared to the original (recorded) one to validate that your system's behaviour has not changed.
