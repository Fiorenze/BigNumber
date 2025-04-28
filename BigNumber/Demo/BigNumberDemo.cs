using UnityEngine;
using TMPro;
public class BigNumberDemo : MonoBehaviour
{
    public BigNumber bigNumber;
    public TextMeshProUGUI bigNumberText;

    public TMP_InputField bigNumberInput;
    public TMP_Dropdown numberTypeDropdown;


    void Start()
    {
        bigNumber = new BigNumber(1, 0);

        numberTypeDropdown.ClearOptions();
        TMP_Dropdown.OptionDataList optionDataList = new();
        foreach (string type in NumberTypes.types)
        {
            optionDataList.options.Add(new TMP_Dropdown.OptionData(type));
        }
        numberTypeDropdown.options = optionDataList.options;
        numberTypeDropdown.value = 0;

        //Test(); // Uncomment to see how it handles various inputs
    }

    private void Test()
    {
        if (BigNumber.TryParse("100M", out BigNumber testNumber))
        {
            Debug.Log(testNumber.ToString());
        }
        if (BigNumber.TryParse("15550K", out BigNumber testNumber2)) // Normalizes the number and prints 15.5M
        {
            Debug.Log(testNumber2.ToString());
        }
        if (BigNumber.TryParse("199a00aa", out BigNumber testNumber3)) // Prints 199. Ignores the rest of the string
        {
            Debug.Log(testNumber3.ToString());
        }
        if (BigNumber.TryParse("030ah", out BigNumber testNumber4)) // Correctly parses 030 to 30
        {
            Debug.Log(testNumber4.ToString());
        }
        if (BigNumber.TryParse("a30cd", out BigNumber testNumber5)) // This is invalid since starts with a letter
        {
            Debug.Log(testNumber5.ToString());
        }
    }


    void Update()
    {
        bigNumberText.text = bigNumber.ToString();
    }


    public void MultiplyBigNumber(int number)
    {
        bigNumber *= number;
    }
    public void DivideBigNumber(int number)
    {
        bigNumber /= number;
    }
    public void AddBigNumber(int number)
    {
        bigNumber += number;
    }
    public void SubstractBigNumber(int number)
    {
        bigNumber -= number;
    }

    public void AddBigNumber()
    {
        if (TryParseInput(out BigNumber number))
        {
            bigNumber += number;
            bigNumberInput.text = "";
        }
        else
        {
            Debug.Log("Invalid input");
        }
    }
    public void SubstractBigNumber()
    {
        if (TryParseInput(out BigNumber number))
        {
            bigNumber -= number;
            bigNumberInput.text = "";
        }
        else
        {
            Debug.Log("Invalid input");
        }
    }
    public void MultiplyBigNumber()
    {
        if (TryParseInput(out BigNumber number))
        {
            bigNumber *= number;
            bigNumberInput.text = "";
        }
        else
        {
            Debug.Log("Invalid input");
        }
    }
    public void DivideBigNumber()
    {
        if (TryParseInput(out BigNumber number))
        {
            bigNumber /= number;
            bigNumberInput.text = "";
        }
        else
        {
            Debug.Log("Invalid input");
        }
    }

    /* private BigNumber ParseInput()
    {
        if (string.IsNullOrEmpty(bigNumberInput.text)) return BigNumber.zero();
        if (!double.TryParse(bigNumberInput.text, out double amount))
        {
            Debug.Log("Invalid input");
            return BigNumber.zero();
        }
        int exp = numberTypeDropdown.value;

        BigNumber number = new BigNumber(amount, exp);
        return number;
    } */
    private bool TryParseInput(out BigNumber result)
    {
        if (string.IsNullOrEmpty(bigNumberInput.text))
        {
            result = default;
            return false;
        }
        if (!double.TryParse(bigNumberInput.text, out double amount))
        {
            Debug.Log("Invalid input");
            result = default;
            return false;
        }
        int exp = numberTypeDropdown.value;

        result = new BigNumber(amount, exp);
        return true;
    }

}
