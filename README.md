# 💰 Currency System

## 📌 Description

Currency System is a modular runtime tool designed for managing game currencies with support for exchange mechanics and persistent saving. Each currency is defined with customizable metadata and exchange rates. The system is integrated with Save System to ensure data persistence between sessions.

> ⚠️ **Important**: To initialize the system, open it once via Unity Editor:  
> **Tools > Currency System**

---

## 📁 Structure

```
Currency System/
│
├── Runtime/
│   ├── CurrencyInformation.cs       # Currency definitions
│   ├── CurrencyExchangeRate.cs      # Exchange rate structure
│   ├── CurrencySaveData.cs          # Saveable data container
│   ├── CurrencyHelper.cs            # Public API
│   ├── CurrencySystemExample.cs     # Test MonoBehaviour
│
├── Editor/
│   └── CurrencyConfigWindow.cs      # Editor interface for setup
```

---

## 🧠 CurrencyInformation

Each currency is defined via a `CurrencyInformation` object.

```csharp
public class CurrencyInformation
{
    public string name;
    public string shownName;
    public string description;
    public string symbol;
    public Texture icon;
    public Color color;
    public float defaultAmount;
    public int decimalPlaces;
    public bool useMaximumAmount;
    public float maximumAmount;
    public List<CurrencyExchangeRate> exchangeRates;
}
```

---

## 🔄 Exchange System

You can define exchange rates between currencies directly in the editor via `CurrencyConfigWindow`.

```csharp
public class CurrencyExchangeRate
{
    public string targetCurrencyKey;
    public float rate;
}
```

---

## 💾 Save Integration

Currency values are persisted through `CurrencySaveData`, which is managed internally by `SaveHelper`.

```csharp
public class CurrencySaveData
{
    public Dictionary<string, float> Currencies = new();
}
```

Every save operation is queued and handled safely via:

```csharp
SaveHelper.SaveData(saveableInstance);
```

> Only the last queued save of the same type is written to disk.

---

## 🔧 CurrencyHelper API

```csharp
float CurrencyHelper.GetAmount(string key);
void CurrencyHelper.SetAmount(string key, int value);
void CurrencyHelper.Add(string key, int delta);
bool CurrencyHelper.Subtract(string key, int delta);
bool CurrencyHelper.TryExchange(string fromKey, string toKey, int fromAmount);
```

All methods will automatically trigger a save through `SaveHelper.SaveData(...)`.

---

## 🧪 Usage Example

```csharp
CurrencyHelper.Add("Gold", 100);
CurrencyHelper.Subtract("Gold", 30);

if (CurrencyHelper.TryExchange("Gold", "Gem", 20))
{
    Debug.Log("Exchange successful!");
}

Debug.Log("Current Gem: " + CurrencyHelper.GetAmount("Gem"));
```

---

## ⚙️ Editor Integration

Open the currency editor via:

```
Tools > Currency System
```

You’ll be able to:
- Edit currency metadata
- Define exchange rates via dropdown
- Prevent duplicate target keys
- Set visuals like icon, color, decimal places, etc.

---

## ⚠️ Notes

- The system **must be initialized** via `Tools > Currency System` before runtime use.
- `exchangeRates` must not include self-to-self entries.
- Save calls are automatically queued and optimized to avoid redundant disk writes.
- All currencies must be defined in `CurrencyData.asset`.

---
