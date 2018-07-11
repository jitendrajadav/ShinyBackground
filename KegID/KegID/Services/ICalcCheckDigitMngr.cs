namespace KegID.Services
{
    public interface ICalcCheckDigitMngr
    {
        int CalculateCheckDigit(string barCodeWithoutCheckDigit);
    }
}
