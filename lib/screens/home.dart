import 'package:flutter/material.dart';
import 'package:pregnancy_countdown/widgets/babyNameWidget.dart';
import 'package:pregnancy_countdown/widgets/dueDateWidget.dart';
import 'package:pregnancy_countdown/widgets/enableNotificationsWidget.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  Widget build(BuildContext context) {
    return Column(mainAxisAlignment: MainAxisAlignment.center, children: const [
      BabyNameWidget(),
      Divider(
        endIndent: 10,
        indent: 10,
      ),
      DueDateWidget(),
      Divider(
        endIndent: 10,
        indent: 10,
      ),
      EnableNotificationsWidget()
    ]);
  }
}
