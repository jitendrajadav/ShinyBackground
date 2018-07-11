namespace KegID.Services
{
    public class CalcCheckDigitMngr : ICalcCheckDigitMngr
    {
        public int CalculateCheckDigit(string barCodeWithoutCheckDigit)
        {
            int chkDigitSum = 0; // this will be a running total
            int weight = 0;
            int nextMultipleOf10 = 0;
            int checkDigit = 0;

            // loop through digits from right to left
            for (int i = 0; i < barCodeWithoutCheckDigit.Length; i++)
            {
                //set ch to "current" character to be processed
                char ch = barCodeWithoutCheckDigit[barCodeWithoutCheckDigit.Length - i - 1];

                // our "digit" is calculated using ASCII value - 48
                int digit = (int)ch - 48;

                // weight will be the current digit's contribution to the running total
                if (i % 2 == 0)
                {
                    weight = (3 * digit);
                }
                else
                {
                    // even-positioned digits just contribute their ascii value minus 48
                    weight = digit;
                }

                // keep a running total of weights
                chkDigitSum += weight;
            }

            if (chkDigitSum % 10 == 0)
                nextMultipleOf10 = chkDigitSum;
            else
                nextMultipleOf10 = chkDigitSum - (chkDigitSum % 10) + 10;

            checkDigit = nextMultipleOf10 - chkDigitSum;

            return (checkDigit);

        }

    }
}
