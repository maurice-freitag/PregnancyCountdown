import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:pregnancy_countdown/services/local_notification_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class EnableNotificationsWidget extends StatefulWidget {
  static const keyEnableNotifications =
      "pregnancy-countdown-enable_notifications";

  const EnableNotificationsWidget({super.key});

  @override
  State<EnableNotificationsWidget> createState() =>
      _EnableNotificationsWidgetState();
}

class _EnableNotificationsWidgetState extends State<EnableNotificationsWidget> {
  bool enableNotifications = false;

  Future<bool> getCurrentEnableNotifications() async {
    final prefs = await SharedPreferences.getInstance();
    var val = prefs.getBool(EnableNotificationsWidget.keyEnableNotifications);
    if (val == null) {
      prefs.setBool(EnableNotificationsWidget.keyEnableNotifications, false);
    }
    return val ?? false;
  }

  setEnableNotifications(bool enableNotifications) {
    SharedPreferences.getInstance().then((prefs) async {
      prefs.setBool(EnableNotificationsWidget.keyEnableNotifications,
          enableNotifications);
      await LocalNotificationService().settingsUpdated();
      updateDueDateDisplay();
    });
  }

  updateDueDateDisplay() {
    SharedPreferences.getInstance().then((prefs) {
      setState(() {
        enableNotifications =
            prefs.getBool(EnableNotificationsWidget.keyEnableNotifications) ??
                false;
      });
    });
  }

  @override
  Widget build(BuildContext context) {
    updateDueDateDisplay();
    return Row(children: [
      Container(
          alignment: Alignment.centerLeft,
          padding: const EdgeInsets.fromLTRB(25, 5, 0, 0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: const [
              Text(
                "Enable daily notifications?",
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 18,
                    color: Colors.black,
                    fontWeight: FontWeight.bold),
              )
            ],
          )),
      const Spacer(),
      Container(
          width: 100,
          padding: const EdgeInsets.fromLTRB(5, 0, 5, 0),
          child: Switch(
            onChanged: onSwitchToggled,
            value: enableNotifications,
          ))
    ]);
  }

  void onSwitchToggled(bool? val) async {
    setEnableNotifications(val ?? false);
  }
}
