namespace AnyBarcode.Symbologies
{
    /// <summary>
    /// EAN-8 encoding
    /// </summary>
    public class Ean8 : BarcodeSymbology, IBarcode
    {
        private readonly string[] _eanCodeA = { "0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011" };
        private readonly string[] _eanCodeC = { "1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100" };

        public Ean8(string input)
        {
            RawData = input;

            // check numeric only
            if (!CheckNumericOnly(RawData)) Error("EEAN8-2: Numeric only.");

            CheckDigit();
        }

        /// <summary>
        /// Encode the raw data using the EAN-8 algorithm.
        /// </summary>
        private string EncodeEAN8()
        {
            // check length
            if (RawData.Length != 8 && RawData.Length != 7) Error("EEAN8-1: Invalid data length. (7 or 8 numbers only)");

            // encode the data
            var result = "101";

            // first half (Encoded using left hand / odd parity)
            for (var i = 0; i < RawData.Length / 2; i++)
            {
                result += _eanCodeA[int.Parse(RawData[i].ToString())];
            }

            // center guard bars
            result += "01010";

            // second half (Encoded using right hand / even parity)
            for (var i = RawData.Length / 2; i < RawData.Length; i++)
            {
                result += _eanCodeC[int.Parse(RawData[i].ToString())];
            }

            result += "101";

            return result;
        }

        private void CheckDigit()
        {
            // calculate the checksum digit if necessary
            if (RawData.Length == 7)
            {
                // calculate the checksum digit
                var even = 0;
                var odd = 0;

                // odd
                for (var i = 0; i <= 6; i += 2)
                {
                    odd += int.Parse(RawData.Substring(i, 1)) * 3;
                }

                // even
                for (var i = 1; i <= 5; i += 2)
                {
                    even += int.Parse(RawData.Substring(i, 1));
                }

                var total = even + odd;
                var checksum = total % 10;
                checksum = 10 - checksum;
                if (checksum == 10)
                    checksum = 0;

                // add the checksum to the end of the 
                RawData += checksum.ToString();
            }
        }

        #region IBarcode Members

        public override string EncodedValue => EncodeEAN8();

        #endregion

    }
}
