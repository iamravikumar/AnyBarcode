namespace AnyBarcode.Symbologies
{
    /// <summary>
    /// ITF-14 encoding
    /// </summary>
    public class ITF14 : BarcodeSymbology
    {
        private readonly string[] _itf14Code = { "NNWWN", "WNNNW", "NWNNW", "WWNNN", "NNWNW", "WNWNN", "NWWNN", "NNNWW", "WNNWN", "NWNWN" };

        public ITF14(string input)
        {
            RawData = input;

            CheckDigit();
        }

        /// <summary>
        /// Encode the raw data using the ITF-14 algorithm.
        /// </summary>
        private string EncodeITF14()
        {
            // check length of input
            if (RawData.Length > 14 || RawData.Length < 13)
                Error("EITF14-1: Data length invalid. (Length must be 13 or 14)");

            if (!CheckNumericOnly(RawData))
                Error("EITF14-2: Numeric data only.");

            var result = "1010";

            for (var i = 0; i < RawData.Length; i += 2)
            {
                var bars = true;
                var patternbars = _itf14Code[int.Parse(RawData[i].ToString())];
                var patternspaces = _itf14Code[int.Parse(RawData[i + 1].ToString())];
                var patternmixed = "";

                // interleave
                while (patternbars.Length > 0)
                {
                    patternmixed += patternbars[0].ToString() + patternspaces[0].ToString();
                    patternbars = patternbars.Substring(1);
                    patternspaces = patternspaces.Substring(1);
                }

                foreach (var c1 in patternmixed)
                {
                    if (bars)
                    {
                        if (c1 == 'N')
                            result += "1";
                        else
                            result += "11";
                    }
                    else
                    {
                        if (c1 == 'N')
                            result += "0";
                        else
                            result += "00";
                    }
                    bars = !bars;
                }
            }

            // add ending bars
            result += "1101";
            return result;
        }

        private void CheckDigit()
        {
            //calculate and include checksum if it is necessary
            if (RawData.Length == 13)
            {
                var total = 0;

                for (var i = 0; i <= RawData.Length - 1; i++)
                {
                    var temp = int.Parse(RawData.Substring(i, 1));
                    total += temp * ((i == 0 || i % 2 == 0) ? 3 : 1);
                }

                var cs = total % 10;
                cs = 10 - cs;
                if (cs == 10)
                    cs = 0;

                RawData += cs.ToString();
            }
        }

        #region IBarcode Members

        public override string EncodedValue => EncodeITF14();

        #endregion

    }
}
