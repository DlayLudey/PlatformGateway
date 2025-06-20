# Advertisement

## Отвечает за рекламу

У каждой платформы есть 2 типа рекламы - **Interstitial** и **Rewarded**
- **Interstitial** реклама - это реклама которая как правило появляется между страницами в игре, основной тип рекламы, чаще всего на платформах ее можно вызывать не чаще чем раз в минуту, но конкретный интервал зависит от платформы
- **Rewarded** реклама - это реклама за которую игрок получает какую либо награду, в виде монет, жизней, скинов и подобных вещей

### Поля

```csharp
float interstitialCooldown; // Задержка между показами Interstitial рекламы
bool isInterstitialAvailable; // Доступна ли сейчас реклама в зависимости от interstitialCooldown
```

```csharp
bool NeedToPreloadRewarded; // Необходимо ли предзагружать Rewarded рекламу
// На подавляющем большинстве платформ можно игнорировать
```

### Основные методы

---

**ShowInterstitial(Action onOpen = null, Action onClose = null, Action<string> onError = null)**

Показывает **Interstitial** рекламу

Пример использования:
```csharp
Advertisement.ShowInterstitial(() => 
{
    Debug.Log("Interstitial ad opened");
}, () => 
{
    Debug.Log("Interstitial ad closed");
}, error => 
{
    Debug.LogError($"Failed to show interstitial ad, error: {error}");
})
```

---

**ShowRewarded(Action onRewarded, Action onOpen = null, Action onClosed = null, Action<string> onError = null)**

Показывает **Rewarded** рекламу

Важно, что награду игрок получает только в момент onRewarded колбека 

Пример использования:
```csharp
Advertisement.ShowInterstitial(() => 
{
    Debug.Log("Reward was given to player");
},
() => 
{
    Debug.Log("Rewarded ad opened");
}, () => 
{
    Debug.Log("Rewarded ad closed");
}, error => 
{
    Debug.LogError($"Failed to show rewarded ad, error: {error}");
})
```

### Вторичные методы

---

**PreloadRewarded(Action onSuccess, Action<string> onError = null)**

Предзагружает **Rewarded** рекламу, необходимо использовать в случае если `NeedToPreloadRewarded` = `true`

Пример использования:
```csharp
Advertisement.PreloadRewarded(() => 
{
    Debug.Log("Rewarded ad is preloaded");
}, error => 
{
    Debug.LogError($"Failed to preload rewarded ad, error: {error}");
})
```

---

**CheckAdBlock(Action<bool> callback)**

Проверяет включен ли у игрока AdBlock

Пример использования:
```csharp
Advertisement.CheckAdBlock(adBlockEnabled => 
{
    Debug.Log($"Adblock is {adBlockEnabled ? "enabled" : "disabled"}");
})
```

---

**ShowBanner(Dictionary<string, object> options = null)**

Показывает баннер, не актуально

---

**HideBanner()**

Прячет баннер, не актуально