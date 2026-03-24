using System;
using System.Collections;
using UnityEngine;

namespace ElRaccoone.Timers {
  public static class Timers {
    private static TimerTicker _Enumerator;
    private static TimerTicker Enumerator {
      get {
        try {
          if (_Enumerator == null) {
            _Enumerator = new GameObject ("~timers").AddComponent<TimerTicker> ();
            GameObject.DontDestroyOnLoad (_Enumerator);
          }
          return _Enumerator;
        } catch (Exception e) {
          Debug.LogError($"Error in Timers.Enumerator: {e.Message}");
          return null;
        }
      }
    }

    public static void SetTimeout (int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetTimeout: Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetTimeout (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetTimeout: {e.Message}");
      }
    }

    public static void SetInterval (int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetInterval: Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetInterval (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetInterval: {e.Message}");
      }
    }

    public static void SetTimeout (this Transform target, int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetTimeout (Transform): Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetTimeout (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetTimeout (Transform): {e.Message}");
      }
    }

    public static void SetInterval (this Transform target, int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetInterval (Transform): Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetInterval (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetInterval (Transform): {e.Message}");
      }
    }

    public static void SetTimeout (this GameObject target, int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetTimeout (GameObject): Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetTimeout (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetTimeout (GameObject): {e.Message}");
      }
    }

    public static void SetInterval (this GameObject target, int miliseconds, Action callback) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in SetInterval (GameObject): Enumerator is null");
          return;
        }
        enumerator.StartCoroutine (enumerator.EnumerateSetInterval (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError($"Error in SetInterval (GameObject): {e.Message}");
      }
    }

    public static void StartCoroutine(IEnumerator coroutine) {
      try {
        var enumerator = Enumerator;
        if (enumerator == null) {
          Debug.LogError("Error in StartCoroutine: Enumerator is null");
          return;
        }
        enumerator.StartCoroutine(coroutine);
      } catch (Exception e) {
        Debug.LogError($"Error in StartCoroutine: {e.Message}");
      }
    }
  }
}
