using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using SKCell;

public class PointManager : MonoBehaviour
{

    public int myPoints; // Current points
    public TextMeshProUGUI PointsText; // Points UI text
    public TextMeshProUGUI ComboText1; // Combo state text
    public TextMeshProUGUI ComboText2; // Combo rank text
    public SKSlider ComboSlider; // UI Slider to display combo time

    private float comboTimer; // Timer for the combo duration
    private float comboDuration; // Current combo duration based on rank
    private string[] ranks = { "F", "C", "B", "A", "S" }; // Combo ranks
    private string[] rankIndicate = { "x1", "x2", "x4", "x8", "x16" }; // Multipliers displayed
    private int currentRankIndex; // Current combo rank index
    private bool isComboActive; // Is combo mode active
    private int comboTriggerCount; // Number of times ComboMeter is called during a combo

    private readonly int[] triggerThresholds = { 0, 2, 4, 8, 16 }; // Thresholds for each rank

    void Start()
    {
        ResetCombo();
    }

    void Update()
    {
        // Update the combo slider based on remaining time
        if (isComboActive)
        {
            float comboref = comboTimer / comboDuration;
            ComboSlider.SetValue(comboref);

            // Decrease combo timer
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                DecreaseComboRank();
            }
        }

        // Update UI text
        PointsText.text = myPoints.ToString();
        ComboText1.text = ranks[currentRankIndex];
        ComboText2.text = rankIndicate[currentRankIndex];
    }

    public void AddPoints(int points)
    {
        if (isComboActive)
        {
            int multiplier = (int)Mathf.Pow(2, currentRankIndex); // Multiplier increases with rank
            myPoints += points * multiplier;
        }
        else
        {
            myPoints += points; // Regular point addition
        }
    }

    public void ComboMeter()
    {
        if (!isComboActive)
        {
            StartCombo();
        }
        else
        {
            // Reset the timer if combo is already active
            comboTimer = comboDuration;

            // Increment the combo trigger count
            comboTriggerCount++;
            UpdateComboRank();
        }
    }

    private void StartCombo()
    {
        isComboActive = true;
        comboTriggerCount = 1; // Start with the first trigger
        currentRankIndex = 0; // Start with the lowest rank
        UpdateComboDuration(); // Set the initial combo duration
        comboTimer = comboDuration; // Start the timer
        Debug.Log("Combo Started!");
    }

    private void DecreaseComboRank()
    {
        // Decrease the rank by one if above F rank
        if (currentRankIndex > 0)
        {
            currentRankIndex--;
            comboTriggerCount = triggerThresholds[currentRankIndex]; // Reset combo count to match new rank
            UpdateComboDuration(); // Adjust duration for the new rank
            comboTimer = comboDuration; // Restart the timer for the lower rank
            Debug.Log("Combo Rank Decreased to " + ranks[currentRankIndex]);
        }
        else
        {
            EndCombo(); // End combo if it drops to F rank
        }
    }

    private void EndCombo()
    {
        isComboActive = false;
        ResetCombo();
        Debug.Log("Combo Ended!");
    }

    private void ResetCombo()
    {
        comboTriggerCount = 0;
        currentRankIndex = 0;
        comboDuration = 5f; // Reset to default duration
    }

    private void UpdateComboRank()
    {
        // Determine the rank based on the number of times ComboMeter is called
        for (int i = triggerThresholds.Length - 1; i >= 0; i--)
        {
            if (comboTriggerCount >= triggerThresholds[i])
            {
                currentRankIndex = i;
                break;
            }
        }

        UpdateComboDuration(); // Adjust combo duration based on new rank
    }

    private void UpdateComboDuration()
    {
        // Adjust the combo duration based on the current rank
        switch (currentRankIndex)
        {
            case 4: // S rank
                comboDuration = 2f;
                break;
            case 3: // A rank
                comboDuration = 3f;
                break;
            case 2: // B rank
                comboDuration = 4f;
                break;
            case 1: // C rank
                comboDuration = 5f;
                break;
            default: // F rank
                comboDuration = 5f;
                break;
        }
    }

}
