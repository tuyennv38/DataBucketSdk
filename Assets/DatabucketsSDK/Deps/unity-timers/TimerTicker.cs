using System;
using System.Collections;
using UnityEngine;

namespace ElRaccoone.Timers {
  public class TimerTicker : MonoBehaviour {
    public IEnumerator EnumerateSetTimeout (int miliseconds, Action callback) {
      yield return new WaitForSecondsRealtime (miliseconds / 1000f);
      try {
        callback ();
      } catch (Exception e) {
        Debug.LogError ($"Error in SetTimeout callback: {e.Message}\n{e.StackTrace}");
      }
    }

    public IEnumerator EnumerateSetInterval (int miliseconds, Action callback) {
      yield return new WaitForSecondsRealtime (miliseconds / 1000f);
      try {
        callback ();
      } catch (Exception e) {
        Debug.LogError ($"Error in SetInterval callback: {e.Message}\n{e.StackTrace}");
      }
      try {
        this.StartCoroutine (this.EnumerateSetInterval (miliseconds, callback));
      } catch (Exception e) {
        Debug.LogError ($"Error in StartCoroutine (EnumerateSetInterval): {e.Message}\n{e.StackTrace}");
      }
    }
  }
}
