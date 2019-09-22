using System.Numerics;

namespace WhisperDragonWPF
{
    public static class EntropyCalcs
    {
        /// <summary>
        /// Calculate password entropy
        /// </summary>
        /// <remarks>
        /// Logic is from https://www.pleacher.com/mp/mlessons/algebra/entropy.html
        /// </remarks>
        /// <param name="password">Password for calculations</param>
        /// <returns>Returns password entropy in bits</returns>
        public static int CalcutePasswordEntropy(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return 0;
            }

            int charsInPassword = password.Length;
            int totalPool = SumUpTotalPool(password);

            BigInteger beforeLog = BigInteger.Pow(totalPool, charsInPassword);
            return (int) BigInteger.Log(beforeLog);
        }

        private static int SumUpTotalPool(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return 0;
            }

            bool includesLowercase = false;
            bool includesUppercase = false;
            bool includesDigits = false;
            bool includesSymbols = false;

            foreach (char c in password)
            {
                if (char.IsLower(c))
                {
                    includesLowercase = true;
                }
                else if (char.IsUpper(c))
                {
                    includesUppercase = true;
                }
                else if (char.IsDigit(c))
                {
                    includesDigits = true;
                }
                else if (char.IsSymbol(c))
                {
                    includesSymbols = true;
                }
            }

            int returnValue = 0;

            if (includesLowercase)
            {
                returnValue += 26;
            }

            if (includesUppercase)
            {
                returnValue += 26;
            }

            if (includesDigits)
            {
                returnValue += 10;
            }

            if (includesSymbols)
            {
                returnValue += 30;
            }

            return returnValue;
        }
    }
}