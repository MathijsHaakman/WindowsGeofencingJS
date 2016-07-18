(function () {
    try {
        var Globalization = Windows.Globalization;
        var Geolocation = Windows.Devices.Geolocation;
        var Background = Windows.ApplicationModel.Background;
        var geofenceMonitor = Geolocation.Geofencing.GeofenceMonitor.current;
        var geofenceTask;
        Windows.Devices.Geolocation.Geolocator.requestAccessAsync().done(
            function () {
                console.log("requestaccesasync complete");
            },
            function (err) {
                console.log("error");
            }
        );
    } catch (e) {
        console.log(e);
    }

    try {
        var builder = new Background.BackgroundTaskBuilder();
        builder.name = "GeolocationBackgroundTask";
        builder.taskEntryPoint = "BackgroundTask.GeofenceBackgroundTask";
        builder.setTrigger(new Windows.ApplicationModel.Background.LocationTrigger(Windows.ApplicationModel.Background.LocationTriggerType.geofence));
        geofenceTask = builder.register();
    } catch (error) {
        console.log("error", error);
    }
    try {
        var fenceId = "geofence";
        var position = {
            altitude: 0.0,
            latitude: 52.492187,
            longitude: 4.797308
        };
        var radius = 500;
        var geocircle = new Geolocation.Geocircle(position, radius);
        var mask = Geolocation.Geofencing.MonitoredGeofenceStates.entered | Geolocation.Geofencing.MonitoredGeofenceStates.exited | Geolocation.Geofencing.MonitoredGeofenceStates.removed;
        var singleUse = false;
        var dwellTime = 10000;
        var startTime = new Date(Date.now());
        var duration = 1000000;
        var geofence = new Geolocation.Geofencing.Geofence(fenceId, geocircle, mask, singleUse, dwellTime, startTime, duration);
        geofenceMonitor.geofences.push(geofence);
    } catch (error) {
        console.error(error);
    }
    document.getElementById("geofencesDiv").innerText = Geolocation.Geofencing.GeofenceMonitor.current.geofences.length;
})();