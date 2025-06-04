# ⚙️ Currency System – Installation Guide

## 🧩 Requirements

- Unity 2021.3+ (recommended)
- Save System package by ergulburak
- Compatible with URP, HDRP, or Built-in

---

## 📦 Installation via Package Manager

1. Open `manifest.json` under `Packages/`
2. Add the following Git URL (if hosted):

```json
"com.ergulburak.currency-system": "https://github.com/ergulburak/unity-currency-system.git"
```

OR use local path:

```json
"com.ergulburak.currency-system": "file:../Packages/Currency System"
```

3. Ensure `Save System` is installed and referenced in your assembly definitions.

---

## 🧾 First-Time Setup

- Use the editor menu:  
  `Tools > Currency System`  
  to create and configure currencies.

- Make sure at least one currency is defined before runtime.
- Click "Update Currencies" button to create Currencies class

---

## 🧪 Validation

- Enter Play Mode
- Test your currencies using `CurrencyHelper`
- Confirm that values persist between sessions

---
# 👤 Currency System – User Guide

## 🎯 Goal

Manage multiple in-game currencies with:
- Exchange capabilities
- Decimal formatting
- UI-friendly metadata
- Persistent storage via Save System

---

## 📘 Creating a Currency

1. Open: `Tools > Currency System`
2. Click “Add New Currency”
3. Fill out:
   - Internal `name`
   - Display `shownName`, `description`, `symbol`, `color`, etc.
4. Set `defaultAmount`, `decimalPlaces`
5. Optionally define `maximumAmount` and enable the toggle

---

## 🔁 Defining Exchange Rates

Inside the Currency Editor:
- Scroll to “Exchange Rates”
- Add a new rate
- Choose a **different currency** from the dropdown
- Define a conversion rate (e.g., 1 Gold → 2 Gems)

Duplicate target keys are automatically prevented.

---

## 💾 Using in Code

Access the system via `CurrencyHelper`:

```csharp
CurrencyHelper.Add("Gold", 100);
CurrencyHelper.Subtract("Gold", 30);
CurrencyHelper.SetAmount("Gem", 500);
CurrencyHelper.TryExchange("Gold", "Gem", 25);
```

All operations auto-save via `SaveHelper.SaveData()` internally.

---

## 🧼 Resetting Data

To reset or delete data manually:
- Navigate to:  
  `%APPDATA%/../LocalLow/{Company}/{Product}/Saves/`
- Delete the relevant `.lugreb` files

---

## 🔒 Notes

- SaveSystem must be initialized at runtime
- Callbacks or event triggers can be added to react to currency changes
- Add custom UI bindings if needed

---
