
## ☁  Weather Forecast Application  ☁

･ﾟ･ Real-time Weather Data & Smart Alerts ･ﾟ･ 

            
## ✦ Overview

A comprehensive C# console application that provides real-time weather data, forecasts, and intelligent weather alerts. Built with 7 design patterns to demonstrate clean architecture and professional software engineering practices.

```
    ⋆｡°✩     Features at a Glance     ✩°｡⋆
    
    ☀  Current Weather Conditions
    📅  7-Day Weather Forecasts  
    ⏰  24-Hour Hourly Predictions
    📊  Historical Weather Analysis
    ⚠️  Predictive Weather Alerts
    📍  Multi-Location Monitoring
    💾  Smart Data Caching
    📄  Professional Report Generation
```

---

## ☆ Key Features

### ⛅ Weather Data Services
```
┌─────────────────────────────────────────────────────────────┐
│  • Current weather conditions with detailed metrics         │
│  • 7-day daily forecasts with temperature ranges            │
│  • 24-hour hourly predictions                               │
│  • Historical weather data for past 7 days                  │
│  • Automatic geocoding for any city worldwide               │
└─────────────────────────────────────────────────────────────┘
```

### 🔔 Smart Weather Alerts
Predictive alerts that notify you **before** weather events occur:

```
    ⚡ Thunderstorm Alert    →  "Storm in 3h 25min"
    🌊 Flood Risk Alert      →  "Heavy rain in 45min"
    ❄️  Snow Alert           →  "Snowfall in 2h 10min"
    🌫️  Fog Alert            →  "Reduced visibility in 1h"
    🌡️  Temperature Alert    →  "Above 35°C in 4h"
```


### 📍 Location Monitoring
```
┌──────────────────────────────────────────────────────────────┐
│  Watch multiple locations simultaneously                     │
│  Attach custom alerts to each location                       │
│  Get notified about upcoming weather changes                 │
│  Manage alerts with easy add/remove functionality            │
└──────────────────────────────────────────────────────────────┘
```

### 💾 Intelligent Caching
```
┌─────────────────────────────────────────────────────────────┐
│ Memento Pattern for state preservation                      │
├─ 30-minute cache validity                                   │      
├─ 60% reduction in API calls                                 │
├─ 1000x faster cache hits                                    │         
├─ Automatic cleanup of old data                              │       
└─────────────────────────────────────────────────────────────┘
```

### 📊 Data Analysis
```
    ⋆ Precipitation Analysis
      └─ Analyze rainfall patterns over 7 days
    
    ⋆ Temperature Trend Analysis  
      └─ Detect warming, cooling, or stable trends
```

### 📄 Report Generation
Professional weather reports with **Builder Pattern**:
- Location comparison reports
- Weekly alerts summary
- Export to JSON format

---

## ⚙️ Design Patterns

This application showcases **7 professional design patterns**:

```
┌────────────────────────┬─────────────────────────────────────┐
│  Pattern               │  Implementation                     │
├──────────────────────────────────────────────────────────────┤
  ⚡ Singleton          │  APIClient - single HTTP instance                
  📦 Strategy           │  IWeatherAnalysis  
  🎭 Decorator          │  CachedDataProvider                 
  👁️  Observer          │  WatchedLocation + Alerts           
  🏗️  Builder           │  WeatherReportBuilder               
  💾 Memento            │  WeatherHistoryMemento              
  🎯 Facade             │  WeatherForecastApp                 
└──────────────────────────────────────────────────────────────┘
```

---

## 🛠️ Technical Stack

```
    ╭───────────────────────────────────────────╮
    │  Language:     C# / .NET 8.0              │
    │  API:          Open-Meteo Weather API     │
    │  Patterns:     7 Design Patterns          │
    │  Data Format:  JSON                       │
    ╰───────────────────────────────────────────╯
```


---

## 📊 Application Menu

```
╔════════════════════════════════════════════════╗
║     WEATHER FORECAST APPLICATION               ║
╚════════════════════════════════════════════════╝

 ☀️  Weather Data
 ─────────────────────────────────────────────
  1.  Get current weather for city
  2.  Get 7-day forecast
  3.  Get 24-hour forecast  
  4.  Get historical weather

 📍 Location Monitoring
 ─────────────────────────────────────────────
  5.  Add watched location
  6.  Add alert to watched location
  7.  Update all watched locations
  8.  Show watched locations
  9.  Remove watched location

 📊 Analysis & Reports
 ─────────────────────────────────────────────
  10. Perform precipitation analysis
  11. Perform temperature trend analysis
  12. Generate location comparison report
  13. Generate weekly alerts report

 💾 Import/Export
 ─────────────────────────────────────────────
  14. Export last weather data
  15. Export watched locations
  16. Export last report
  17. Import weather data
  18. Import watched locations

  0.  Exit
```
---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

```
    Fork the repository
         ↓
    Create your feature branch
         ↓
    Commit your changes
         ↓
    Push to the branch
         ↓
    Open a Pull Request
```

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 🙏 Acknowledgments

```
    ☁  Open-Meteo - Free Weather API
    ⚡ .NET Team - Excellent framework
    📚 Design Patterns - Gang of Four
    ✨ ASCII Art - Aesthetic borders
```

---

## 📬 Contact

```
    ╭─────────────────────────────────────╮
    │  Questions? Suggestions?            │
    │  Feel free to open an issue!        │
    ╰─────────────────────────────────────╯
```

---

<div align="center">

```
⋆｡°✩ ════════════════════════════════════════════ ✩°｡⋆

            Built with ❤️ and 7 Design Patterns

              Stay informed. Stay prepared.

⋆｡°✩ ════════════════════════════════════════════ ✩°｡⋆
```

**Star ⭐ this repo if you find it helpful!**

</div>
