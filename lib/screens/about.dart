import 'package:flutter/material.dart';
import 'package:url_launcher/url_launcher.dart';

class AboutPage extends StatelessWidget {
  const AboutPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(mainAxisAlignment: MainAxisAlignment.spaceEvenly, children: [
      Expanded(
          child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
            padding: const EdgeInsets.all(5.0),
            alignment: Alignment.center,
            child: const Text(
                "If you like the app consider buying the developer a coffee :) ",
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 20, color: Colors.black)),
          ),
          ElevatedButton(
              onPressed: () async {
                await launchUrl(Uri.parse("https://paypal.me/dekyon"));
              },
              child: const Text("paypal.me"))
        ],
      )),
      const Divider(),
      Container(
          alignment: Alignment.center,
          margin: const EdgeInsets.all(0.0),
          padding: const EdgeInsets.all(0.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Text("Created with",
                  style: TextStyle(fontSize: 12, color: Colors.black)),
              TextButton(
                  onPressed: () async {
                    await launchUrl(Uri.parse("https://flutter.dev"));
                  },
                  child: const Text("Flutter"))
            ],
          )),
      Container(
          alignment: Alignment.center,
          margin: const EdgeInsets.all(0.0),
          padding: const EdgeInsets.all(0.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Text("Contains icons by",
                  style: TextStyle(fontSize: 12, color: Colors.black)),
              TextButton(
                  onPressed: () async {
                    await launchUrl(Uri.parse("https://flaticon.com"));
                  },
                  child: const Text("Flaticon"))
            ],
          ))
    ]);
  }
}
