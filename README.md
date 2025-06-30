# HZG-Werkstattlager
Meine betriebliche Projektarbeit und IHK-Abschlussprojekt. Aufgrund betrieblicher Umstrukturierungen wird das Projekt nicht weiterentwickelt oder produktiv eingesetzt, daher habe ich beschlossen den Quellcode öffentlich zu archivieren.

Das Projekt bildet ein kleines Fullstack-Projekt ab, dass innerhalb 80 Stunden umgesetzt wurde.

## Starten der Anwendung
Die Anwendung kann über das Projekt WerkstattlagerUI gestartet werden. Wenn kein ConnectionString gefunden wird, wird eine Datenbank im internen Speicher erstellt.

## Auszüge aus der Projektdokumentation:
### Projekdefinition
#### Projektumfeld
Die Diakonie Herzogsägmühle ist als Teil der Diakonie München und Oberbayern eine soziale Einrichtung mit Firmensitz in Herzogsägmühle, ein Ortsteil der Marktgemeinde Peiting im Landkreis Weilheim-Schongau. In diversen sozialen Einrichtungen arbeiten 2.200 Mitarbeitende an der Begleitung und Betreuung von 4.500 hilfebedürftiger Menschen an rund 200 Standorten innerhalb von acht Landkreisen in Oberbayern.

Meine Ausbildung fand im Geschäftsbereich Informationsmanagement, in der Abteilung IT-Werkstatt, und im Geschäftsbereich Arbeit & Integration in der Abteilung Telezentrum - Digitale Medien statt.
Bei dem Projekt handelt es sich um ein internes Projekt für den Geschäftsbereich Informationsmanagement. In diesem ist das Referat für IT-Service tätig, das für die Bestellung, die Betreuung und die Installation von Hardware sowie den technischen Support aller Mitarbeitenden verantwortlich ist. Das Referat setzt sich aus den Abteilungen Service Desk und IT-Werkstatt zusammen.

#### Problembeschreibung
Das Referat für IT-Service verfügt über einen Lagerraum, in dem rückläufige Hardware aufbewahrt wird. Hierbei geht es um Geräte von ausgeschiedenen Mitarbeitern oder Hardware, die durch modernere Systeme ersetzt wurde, soweit aber noch funktionsfähig ist. Die Geräte bleiben im Lager bis sie gegebenenfalls neue Verwendung finden, beispielsweise als Ersatz- oder Leihgeräte. Das Problem liegt darin, dass der Warenfluss im Lager nicht immer transparent verläuft und nirgendwo dokumentiert wird. Informationen zu den Geräten sind oft, sofern vorhanden, über das Ticket-System, das Client-Management-System oder physische Notizen verstreut. Die jährliche Inventur wird mühsam mit statischen Excel-Sheets festgehalten, die zu keinem Zeitpunkt aktuell sind. Wer Übersicht über den Bestand des Inventars haben möchte, dem bleibt nichts anderes übrig, als selbst im Lager nachzuschauen.

#### Soll-Konzept
Um dem Abhilfe zu verschaffen, soll das Inventar in einer relationalen Datenbank festgehalten werden. Dazu wird von dem Referat für IT-Infrastruktur eine Instanz auf einem Microsoft SQL Server bereitgestellt. Die Entscheidung für SQL-Server ist naheliegend, da es bereits im Betrieb produktiv eingesetzt wird. Zudem wird .NET   als Entwicklungsplattform gewählt, da hier die meisten Kenntnisse vorhanden sind. Beide Systeme stammen von Microsoft und harmonieren dementsprechend miteinander.

Im Datenmodell werden alle relevanten Informationen der Geräte gespeichert, darunter die intern vergebene Inventarnummer und die Seriennummer. Wenn ein Gerät ein- oder ausgeht, wird der Zeitstempel festgehalten, und in einem Kommentarfeld kann optional Kontext zum Warenfluss eingetragen werden.

Für die Schnittstelle wird das Framework Entity Framework Core verwendet, ein ORM -Tool, ebenfalls von Microsoft, mit dem Datenbank-Entitäten in Objekte und umgekehrt umgewandelt werden. Da das Datenbank-Modell von überschaubarem Umfang ist, erfolgt die Modellierung des Datenbankschemas Code-First , parallel zu den Modelklassen in der Service-Schicht.

Bei der Art der Applikation fiel die Wahl auf eine Desktop-Anwendung, da vorerst eine Applikation für die Windows-Umgebung ausreichend ist und meine vorhandenen Kenntnisse im Bereich Web-Oberflächen noch nicht ausgereift sind.

Zur Bedienung soll eine grafische Oberfläche für das Betriebssystem Windows 11 erstellt werden, die es ermöglicht, das Inventar einzusehen, zu durchsuchen, Datensätze zu erstellen, zu bearbeiten und den Warenfluss zu steuern.

Hier wird auf das Framework Windows Presentation Foundation (WPF) gesetzt, da hier bereits Erfahrungen vorhanden sind. Bei WPF bietet es sich an, das Projekt nach dem Model-View-ViewModel-Pattern zu kapseln. Microsoft bietet mit dem Community Toolkit eine umfassende Bibliothek mit diversen Helferklassen und Schnittstellen zur UI-Entwicklung. In diesem sind auch Werkzeuge für das MVVM-Pattern enthalten.

Um die Kommunikation mit der Datenbank zu validieren und zukünftige Fehlerbehebung zu erleichtern, werden Integrationstests verfasst, die automatisiert den korrekten Datenaustausch zwischen Datenbank, Service und Logik sicherstellen. Zum Testen wird das Framework xUnit eingesetzt, eine Open-Source Bibliothek, die von Microsoft als Teil der .NET Foundation befürwortet wird.
