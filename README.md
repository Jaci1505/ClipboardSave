# ClipboardSave

Ein schlankes Windows-Tool, das einen Screenshot aus der Zwischenablage direkt als Bilddatei speichert.

## Funktionsweise

Das Programm startet, liest automatisch das aktuelle Bild aus der Windows-Zwischenablage und öffnet einen Speicherdialog. Nach dem Speichern beendet es sich selbst.

## Voraussetzungen

- Windows 10 oder höher
- .NET Framework / .NET (je nach Build-Konfiguration)
- [Windows API Code Pack](https://www.nuget.org/packages/WindowsAPICodePack-Core/) (für den erweiterten Fehlerdialog)

## Verwendung

1. Screenshot in die Zwischenablage kopieren (z. B. mit `Print` oder `Win + Shift + S`)
2. `ClipboardSave.exe` starten
3. Im Speicherdialog Dateinamen und Speicherort wählen
4. Fertig – die Datei wird gespeichert und das Programm beendet sich

## Unterstützte Dateiformate

| Format | Erweiterung |
|--------|-------------|
| PNG    | `.png`      |
| JPEG   | `.jpg`      |

## Fehlerbehandlung

| Situation | Verhalten |
|-----------|-----------|
| Kein Bild in der Zwischenablage | Fehlermeldung + automatisches Beenden |
| Speicherdialog abgebrochen | Programm beendet sich ohne Fehlermeldung |
| Datei bereits geöffnet / nicht speicherbar | Fehlerdialog mit Option zum Wiederholen oder Beenden |

## Ausführung
Das Programm lässt sich über die Datei ".\bin\Debug\net7.0-windows\ClipboardSave.exe" starten. 