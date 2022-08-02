import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:pregnancy_countdown/services/local_notification_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class DueDateWidget extends StatefulWidget {
  static const keyDueDate = "pregnancy-countdown-due_date";

  const DueDateWidget({super.key});

  @override
  State<DueDateWidget> createState() => _DueDateWidgetState();
}

class _DueDateWidgetState extends State<DueDateWidget> {
  DateTime? dueDate;

  Future<DateTime?> getCurrentDueDate() async {
    final prefs = await SharedPreferences.getInstance();
    return DateTime.tryParse(prefs.getString(DueDateWidget.keyDueDate) ?? "");
  }

  setDueDate(DateTime? date) {
    SharedPreferences.getInstance().then((prefs) async {
      prefs.setString(DueDateWidget.keyDueDate, date?.toString() ?? "");
      await LocalNotificationService().settingsUpdated();
      updateDueDateDisplay();
    });
  }

  updateDueDateDisplay() {
    SharedPreferences.getInstance().then((prefs) {
      var dueDateStr = prefs.getString(DueDateWidget.keyDueDate);
      setState(() {
        dueDate = DateTime.tryParse(dueDateStr ?? "");
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
            children: [
              const Text(
                "When is your baby due?",
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 18,
                    color: Colors.black,
                    fontWeight: FontWeight.bold),
              ),
              Text(
                (dueDate == null
                    ? ""
                    : DateFormat('dd.MM.yyyy').format(dueDate!)),
                textAlign: TextAlign.left,
                style: const TextStyle(fontSize: 16, color: Colors.grey),
              )
            ],
          )),
      const Spacer(),
      Container(
          width: 100,
          padding: const EdgeInsets.fromLTRB(5, 0, 5, 0),
          child: TextButton(
              onPressed: onButtonPressed,
              child: const Text(
                "Change",
                style: TextStyle(fontSize: 16),
              )))
    ]);
  }

  void onButtonPressed() async {
    var currentDueDate = await getCurrentDueDate() ?? DateTime.now();
    var dateTime = await showDatePicker(
        context: context,
        initialDate: currentDueDate,
        firstDate: DateTime.now(),
        lastDate: DateTime(
            DateTime.now().year + 2, DateTime.now().month, DateTime.now().day));
    if (dateTime != null) {
      setDueDate(dateTime);
    }
  }
}
