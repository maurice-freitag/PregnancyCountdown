import 'package:flutter/material.dart';
import 'package:pregnancy_countdown/services/local_notification_service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class BabyNameWidget extends StatefulWidget {
  static const keyBabyName = "pregnancy-countdown-baby_name";

  const BabyNameWidget({super.key});

  @override
  State<BabyNameWidget> createState() => _BabyNameWidgetState();
}

class _BabyNameWidgetState extends State<BabyNameWidget> {
  String babyName = "";

  Future<String?> getCurrentBabyName() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString(BabyNameWidget.keyBabyName);
  }

  setBabyName(String name) {
    SharedPreferences.getInstance().then((prefs) async {
      prefs.setString(BabyNameWidget.keyBabyName, name);
      await LocalNotificationService().settingsUpdated();
      updateBabyNameDisplay();
    });
  }

  updateBabyNameDisplay() {
    SharedPreferences.getInstance().then((prefs) {
      var name = prefs.getString(BabyNameWidget.keyBabyName);
      setState(() {
        babyName = name ?? "";
      });
    });
  }

  final _textFieldController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    updateBabyNameDisplay();
    return Row(children: [
      Container(
          alignment: Alignment.centerLeft,
          padding: const EdgeInsets.fromLTRB(25, 5, 0, 0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Text(
                "What should I call your baby?",
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 18,
                    color: Colors.black,
                    fontWeight: FontWeight.bold),
              ),
              Text(
                babyName,
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
    var currentName = await getCurrentBabyName();
    var name = await _showTextInputDialog(currentName);
    if (name != null) {
      setBabyName(name);
    }
  }

  Future<String?> _showTextInputDialog(String? name) async {
    return showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            content: TextField(
                autofocus: true,
                controller: _textFieldController,
                decoration: InputDecoration(hintText: name ?? "Name")),
            actions: <Widget>[
              TextButton(
                  child: const Text("Cancel"),
                  onPressed: () {
                    _textFieldController.text = "";
                    Navigator.of(context).pop(null);
                  }),
              ElevatedButton(
                  child: const Text('OK'),
                  onPressed: () {
                    var text = _textFieldController.text;
                    _textFieldController.text = "";
                    Navigator.of(context).pop(text);
                  }),
            ],
          );
        });
  }
}
