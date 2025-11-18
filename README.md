# RocketMoonApp

## Übersicht
RocketMoonApp ist eine moderne Webanwendung, die aus einem Client- und einem Server-Projekt besteht. 

## Projektstruktur

```
RocketMoonApp/
├── rocketmoonapp.client/   # Frontend-Projekt (React + TypeScript)
│   ├── src/               # Quellcode des Frontends
│   ├── public/            # Statische Dateien
│   ├── package.json       # Abhängigkeiten und Skripte
│   └── vite.config.ts     # Vite-Konfiguration
├── RocketMoonApp.Server/  # Backend-Projekt (ASP.NET Core)
│   ├── Program.cs         # Einstiegspunkt des Servers
│   ├── appsettings.json   # Konfigurationsdateien
│   └── RocketMoonApp.Server.csproj  # Projektdatei
└── RocketMoonApp.sln      # Lösung für das gesamte Projekt
```

## Voraussetzungen

### Allgemein
- Node.js (empfohlen: v20.19 oder höher)
- .NET SDK (empfohlen: v9.0 oder höher)

### Installation

1. Repository klonen:
   ```bash
   git clone https://github.com/Terryx420/RocketMoonApp.git
   ```

2. In das Projektverzeichnis wechseln:
   ```bash
   cd RocketMoonApp
   ```

3. Abhängigkeiten für den Client installieren:
   ```bash
   cd rocketmoonapp.client
   npm install
   ```

4. Abhängigkeiten für den Server installieren:
   ```bash
   cd ../RocketMoonApp.Server
   dotnet restore
   ```

## Entwicklung

### Client starten

1. In das Client-Verzeichnis wechseln:
   ```bash
   cd rocketmoonapp.client
   ```

2. Entwicklungsserver starten:
   ```bash
   npm run dev
   ```

3. Öffne die Anwendung im Browser unter [http://localhost:5173](http://localhost:5173).

### Server starten

1. In das Server-Verzeichnis wechseln:
    ```bash
    cd RocketMoonApp.Server
    ```

2. Server starten:
    ```bash
    dotnet run
    ```

3. Der Server läuft standardmäßig unter [http://localhost:5000](http://localhost:5000).


### Server und Client starten

    ```bash
    npm start
    ```

## Deployment

Für das Deployment können die Projekte separat gebaut und auf den entsprechenden Plattformen bereitgestellt werden. Weitere Details folgen in der Dokumentation.

## Lizenz

Dieses Projekt steht unter der MIT-Lizenz. Weitere Informationen findest du in der Datei `LICENSE`.

---

**Autor:** Terryx420