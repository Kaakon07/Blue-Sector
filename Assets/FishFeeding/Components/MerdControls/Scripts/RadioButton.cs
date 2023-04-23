using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A button that stays down while it is selected.
/// Buttons in the same selection set must be grouped
/// with the same tag.
/// </summary>
public sealed class RadioButton : CustomizableButton
{
    /// <summary>
    /// An event that is fired when the button is selected;
    /// </summary>
    public UnityEvent OnSelected;

    private bool isSelected;

    /// <summary>
    /// Gets a value indicating whether the radio button is selected.
    /// </summary>
    public bool IsSelected
    {
        get => isSelected;
        private set
        {
            if (isSelected == value)
            {
                return;
            }

            isSelected = value;
            OnSelected.Invoke();
        }
    }

    protected override bool CanReleaseButton()
         => base.CanReleaseButton() && !IsSelected;

    /// <summary>
    /// Toggles the radio button.
    /// </summary>
    /// <returns><see langword="true" /> if the button was selected.</returns>
    public void Select()
    {
        if (IsSelected)
        {
            return;
        }

        IsSelected = true;

        // Deselect the other buttons.
        foreach (var button in FindOtherButtons())
        {
            button.IsSelected = false;
        }
    }

    private static IEnumerable<RadioButton> FindButtonsWithTag(string tag)
        => GameObject.FindGameObjectsWithTag(tag)
                    .Select(x => x.GetComponent<RadioButton>())
                    .Where(x => x != null);

    private IEnumerable<RadioButton> FindOtherButtons() => FindButtonsWithTag(tag).Where(x => x != this);

    protected override void Start()
    {
        base.Start();
        // The first button in the set is selected by default.
        if (!FindOtherButtons().Any(x => x.IsSelected))
        {
            IsSelected = true;
        }
    }
}
