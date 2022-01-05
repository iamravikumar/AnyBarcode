namespace AnyBarcode.Symbologies
{
    /// <summary>
    /// ISBN encoding
    /// </summary>
    public class Isbn : BarcodeSymbology
    {
        public Isbn(string input)
        {
            RawData = input;
        }

        /// <summary>
        /// Encode the raw data using the Bookland/ISBN algorithm.
        /// </summary>
        private string EncodeISBNBookland()
        {
            if (!CheckNumericOnly(RawData))
                Error("EBOOKLANDISBN-1: Numeric Data Only");

            var type = "UNKNOWN";
            switch (RawData.Length)
            {
                case 10:
                case 9:
                    {
                        if (RawData.Length == 10) RawData = RawData.Remove(9, 1);
                        RawData = "978" + RawData;
                        type = "ISBN";
                        break;
                    }
                case 12 when RawData.StartsWith("978"):
                    type = "BOOKLAND-NOCHECKDIGIT";
                    break;
                case 13 when RawData.StartsWith("978"):
                    type = "BOOKLAND-CHECKDIGIT";
                    RawData = RawData.Remove(12, 1);
                    break;
            }

            // check to see if its an unknown type
            if (type == "UNKNOWN") Error("EBOOKLANDISBN-2: Invalid input.  Must start with 978 and be length must be 9, 10, 12, 13 characters.");

            var ean13 = new Ean13(RawData);
            return ean13.EncodedValue;
        }

        #region IBarcode Members

        public override string EncodedValue => EncodeISBNBookland();

        #endregion

    }
}
