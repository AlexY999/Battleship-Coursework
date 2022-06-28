using System;
using UnityEngine;
using UnityEngine.UI;

public class FieldCell : MonoBehaviour
{
    public Action<int> OnShipHit; 
    
    public bool opponentPanel = false;
    public FieldStatus fieldStatus = FieldStatus.Empty;
    private Image fieldImage;
    private Button fieldButton;
    private Ship ship;

    private void Awake()
    {
        fieldImage = gameObject.GetComponent<Image>();
        fieldButton = gameObject.GetComponent<Button>();
        
        fieldButton.onClick.AddListener(() => Hit());
    }
    
    public void SetButton(bool activeSelf)
    {
        fieldButton.interactable = activeSelf;
    }
    
    public int Hit()
    {
        int num = -1;
        if (fieldStatus == FieldStatus.Ship)
        {
            ChangeFieldsStatus(FieldStatus.Killed);
            ship.DecreaseLife();
            num = ship.Life == 0 ? ship.Size : 0;
        }
        else if(fieldStatus == FieldStatus.Empty)
        {
            ChangeFieldsStatus(FieldStatus.Miss);
            num = -1;
        }

        OnShipHit(num);

        return num;
    }

    public bool Add(Ship ship)
    {
        if (fieldStatus != FieldStatus.Ship)
        {
            ChangeFieldsStatus(FieldStatus.Ship);
            this.ship = ship;
            return true;
        }
        return false;
    }
    
    private void ChangeFieldsStatus(FieldStatus status)
    {
        fieldStatus = status;

        switch (fieldStatus)
        {
            case FieldStatus.Empty:
                fieldImage.color = Color.white;
                break;
            case FieldStatus.Ship:
                if(!opponentPanel)
                    fieldImage.color = Color.green;
                break;
            case FieldStatus.Killed:
                fieldImage.color = Color.red;
                fieldButton.enabled = false;
                break;
            case FieldStatus.Miss:
                fieldImage.color = Color.black;
                fieldButton.enabled = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum FieldStatus
    {
        Empty,
        Ship,
        Killed,
        Miss
    }
}