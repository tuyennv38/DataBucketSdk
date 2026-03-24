using UnityEngine;
using System;

public static class StaticEventFieldsUtil
{
    public static string GetUserPseudoId()
    {
        try {
            return PlayerPrefs.GetString("user_pseudo_id", ""); // Returns empty if not found
        } catch (Exception e) {
            Debug.LogError($"Error in GetUserPseudoId: {e.Message}");
            return "";
        }
    }

    public static void SetUserPseudoId(string pseudoId)
    {
        try {
            PlayerPrefs.SetString("user_pseudo_id", pseudoId);
        } catch (Exception e) {
            Debug.LogError($"Error in SetUserPseudoId: {e.Message}");
        }
    }
}
