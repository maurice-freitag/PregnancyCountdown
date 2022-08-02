import 'package:android_alarm_manager_plus/android_alarm_manager_plus.dart';
import 'package:pregnancy_countdown/widgets/babyNameWidget.dart';
import 'package:pregnancy_countdown/widgets/dueDateWidget.dart';
import 'package:pregnancy_countdown/widgets/enableNotificationsWidget.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class LocalNotificationService {
  static final LocalNotificationService _singleton =
      LocalNotificationService._internal();

  factory LocalNotificationService() {
    return _singleton;
  }

  LocalNotificationService._internal();

  static final _localNotificationService = FlutterLocalNotificationsPlugin();

  static Future<void> initialize() async {
    const AndroidInitializationSettings androidSettings =
        AndroidInitializationSettings('@drawable/heart');
    const InitializationSettings settings =
        InitializationSettings(android: androidSettings);
    await _localNotificationService.initialize(settings);
  }

  static Future<NotificationDetails> _notificationDetails() async {
    const AndroidNotificationDetails androidDetails =
        AndroidNotificationDetails(
            "pregnancy_countdown_notification", "Pregnancy Countdown",
            channelDescription: "Required by PregnancyCountdown",
            importance: Importance.max,
            priority: Priority.max,
            icon: '@drawable/heart',
            largeIcon:
                DrawableResourceAndroidBitmap('@drawable/baby_notification'),
            playSound: true);
    return const NotificationDetails(android: androidDetails);
  }

  Future<void> settingsUpdated() async {
    var prefs = await SharedPreferences.getInstance();
    var babyName = prefs.getString(BabyNameWidget.keyBabyName);
    var dueDate =
        DateTime.tryParse(prefs.getString(DueDateWidget.keyDueDate) ?? "");
    var enableNotifications =
        prefs.getBool(EnableNotificationsWidget.keyEnableNotifications) ??
            false;

    if (babyName != null || dueDate != null || enableNotifications) {
      await _scheduleNotifications();
    } else {
      await _unscheduleNotifications();
    }
  }

  Future<void> _unscheduleNotifications() async {
    await AndroidAlarmManager.initialize();
    await AndroidAlarmManager.cancel(1787);
  }

  Future<void> _scheduleNotifications() async {
    await _unscheduleNotifications();
    await AndroidAlarmManager.initialize();

    await AndroidAlarmManager.periodic(
        const Duration(days: 1), 1787, sendNotification,
        rescheduleOnReboot: true,
        allowWhileIdle: true,
        exact: true,
        startAt: DateTime.now());
  }

  static Future<void> sendNotification() async {
    final notificationDetails = await _notificationDetails();
    final prefs = await SharedPreferences.getInstance();
    await prefs.reload();

    var babyName = prefs.getString(BabyNameWidget.keyBabyName);
    var dueDate =
        DateTime.tryParse(prefs.getString(DueDateWidget.keyDueDate) ?? "");

    if (babyName == null || dueDate == null) {
      return;
    }

    var daysLeft = dueDate.difference(DateTime.now()).inDays;
    if (daysLeft < 0) {
      return;
    }

    var message = "$babyName will arrive in just $daysLeft days!";

    await initialize();
    await _localNotificationService.show(
        32208, "Pregnancy Countdown", message, notificationDetails);
  }
}
