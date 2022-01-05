namespace AnyBarcode.Symbologies
{
    /// <summary>
    /// UPC-E encoding
    /// </summary>
    public class Upce : BarcodeSymbology
    {
        private readonly string[] _eanCodeA = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        private readonly string[] _eanCodeB = { "0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111" };
        private readonly string[] _eanPattern = { "aaaaaa", "aababb", "aabbab", "aabbba", "abaabb", "abbaab", "abbbaa", "ababab", "ababba", "abbaba" };
        private readonly string[] _upcECode0 = { "bbbaaa", "bbabaa", "bbaaba", "bbaaab", "babbaa", "baabba", "baaabb", "bababa", "babaab", "baabab" };
        private readonly string[] _upcECode1 = { "aaabbb", "aababb", "aabbab", "aabbba", "abaabb", "abbaab", "abbbaa", "ababab", "ababba", "abbaba" };

        /// <summary>
        /// Encodes a UPC-E symbol.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        public Upce(string input)
        {
            RawData = input;
        }

        /// <summary>
        /// Encode the raw data using the UPC-E algorithm.
        /// </summary>
        private string EncodeUPCE()
        {
            if (RawData.Length != 6 && RawData.Length != 8 && RawData.Length != 12) 
                Error("EUPCE-1: Invalid data length. (8 or 12 numbers only)");

            if (!CheckNumericOnly(RawData)) 
                Error("EUPCE-2: Numeric only.");

            // check for a valid number system
            var numberSystem = int.Parse(RawData[0].ToString());
            if (numberSystem != 0 && numberSystem != 1) 
                Error("EUPCE-3: Invalid Number System (only 0 & 1 are valid)");

            var CheckDigit = int.Parse(RawData[RawData.Length - 1].ToString());
            
            // Convert to UPC-E from UPC-A if necessary
            if (RawData.Length == 12)
            {
                var UPCECode = "";

                // break apart into components
                var manufacturer = RawData.Substring(1, 5);
                var productCode = RawData.Substring(6, 5);
                
                if (manufacturer.EndsWith("000") || manufacturer.EndsWith("100") || manufacturer.EndsWith("200") && int.Parse(productCode) <= 999)
                {
                    // rule 1
                    UPCECode += manufacturer.Substring(0, 2); //first two of manufacturer
                    UPCECode += productCode.Substring(2, 3); //last three of product
                    UPCECode += manufacturer[2].ToString(); //third of manufacturer
                }
                else if (manufacturer.EndsWith("00") && int.Parse(productCode) <= 99)
                {
                    // rule 2
                    UPCECode += manufacturer.Substring(0, 3); //first three of manufacturer
                    UPCECode += productCode.Substring(3, 2); //last two of product
                    UPCECode += "3"; //number 3
                }
                else if (manufacturer.EndsWith("0") && int.Parse(productCode) <= 9)
                {
                    // rule 3
                    UPCECode += manufacturer.Substring(0, 4); //first four of manufacturer
                    UPCECode += productCode[4]; //last digit of product
                    UPCECode += "4"; //number 4
                }
                else if (!manufacturer.EndsWith("0") && int.Parse(productCode) <= 9 && int.Parse(productCode) >= 5)
                {
                    // rule 4
                    UPCECode += manufacturer; //manufacturer
                    UPCECode += productCode[4]; //last digit of product
                }
                else
                    Error("EUPCE-4: Illegal UPC-A entered for conversion.  Unable to convert.");

                RawData = UPCECode;
            }

            // get encoding pattern 
            var pattern = "";

            if (numberSystem == 0) pattern = _upcECode0[CheckDigit];
            else pattern = _upcECode1[CheckDigit];

            // encode the data
            var result = "101";

            var pos = 0;
            foreach (var c in pattern)
            {
                var i = int.Parse(RawData[pos++].ToString());
                if (c == 'a')
                {
                    result += _eanCodeA[i];
                }
                else if (c == 'b')
                {
                    result += _eanCodeB[i];
                }
            }

            // guard bars
            result += "01010";

            // end bars
            result += "1";

            return result;
        }

        #region IBarcode Members

        public override string EncodedValue => EncodeUPCE();

        #endregion

    }
}
