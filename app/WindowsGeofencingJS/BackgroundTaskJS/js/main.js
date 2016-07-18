// Your code here!
Debug.writeln("test");
console.log("Backgroundservice");
document.getElementById("geofencesDiv").innerText = "HALLOOOEEEIIII";
var toastNotifier = Windows.UI.Notifications.ToastNotificationManager.createToastNotifier();
var toastXML = Windows.UI.Notifications.ToastNotificationManager.getTemplateContent(Windows.UI.Notifications.ToastTemplateType.toastImageAndText02);
var nodeList = toastXML.getElementsByTagName("text");
nodeList.item(0).appendChild(toastXML.createTextNode("WindowsGeolocation JS"));
nodeList.item(1).appendChild(toastXML.createTextNode("TEST"));
var toastNode = toastXML.selectSingleNode("/toast");
//var audio = toastXML.createElement("audio");
//audio.setAttribute("src", "ms-winsoundevent:notification.sms");
var toast = new Windows.UI.Notifications.ToastNotification(toastXML);
toastNotifier.show(toast);
console.log("backgroundTask done");
close();
