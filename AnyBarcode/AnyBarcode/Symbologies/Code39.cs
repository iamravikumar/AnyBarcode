namespace AnyBarcode.Symbologies
{
    /// <summary>
    /// Code 39 encoding
    /// </summary>
    public class Code39 : BarcodeSymbology
    {
        private readonly System.Collections.Hashtable _c39Code = new();
        private readonly System.Collections.Hashtable _extC39Translation = new();
        private readonly bool _allowExtended;
        private readonly bool _enableChecksum;

        /// <summary>
        /// Encodes with Code39.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        public Code39(string input)
        {
            RawData = input;
        }

        /// <summary>
        /// Encodes with Code39.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        /// <param name="allowExtended">Allow Extended Code 39 (Full Ascii mode).</param>
        public Code39(string input, bool allowExtended)
        {
            RawData = input;
            _allowExtended = allowExtended;
        }

        /// <summary>
        /// Encodes with Code39.
        /// </summary>
        /// <param name="input">Data to encode.</param>
        /// <param name="allowExtended">Allow Extended Code 39 (Full Ascii mode).</param>
        /// <param name="enableChecksum">Whether to calculate the Mod 43 checksum and encode it into the barcode</param>
        public Code39(string input, bool allowExtended, bool enableChecksum)
        {
            RawData = input;
            _allowExtended = allowExtended;
            _enableChecksum = enableChecksum;
        }

        /// <summary>
        /// Encode the raw data using the Code 39 algorithm.
        /// </summary>
        private string EncodeCode39()
        {
            InitCode39();
            InitExtendedCode39();

            var strNoAstr = RawData.Replace("*", "");
            var strFormattedData = "*" + strNoAstr + (_enableChecksum ? GetChecksumChar(strNoAstr).ToString() : String.Empty) + "*";

            if (_allowExtended)
                InsertExtendedCharsIfNeeded(ref strFormattedData);

            var result = "";
            foreach (var c in strFormattedData)
            {
                try
                {
                    result += _c39Code[c]?.ToString();
                    result += "0"; // whitespace
                }
                catch
                {
                    if (_allowExtended)
                        Error("EC39-1: Invalid data.");
                    else
                        Error("EC39-1: Invalid data. (Try using Extended Code39)");
                }
            }

            result = result.Substring(0, result.Length - 1);

            // clear the hashtable so it no longer takes up memory
            _c39Code.Clear();

            return result;
        }
        private void InitCode39()
        {
            _c39Code.Clear();
            _c39Code.Add('0', "101001101101");
            _c39Code.Add('1', "110100101011");
            _c39Code.Add('2', "101100101011");
            _c39Code.Add('3', "110110010101");
            _c39Code.Add('4', "101001101011");
            _c39Code.Add('5', "110100110101");
            _c39Code.Add('6', "101100110101");
            _c39Code.Add('7', "101001011011");
            _c39Code.Add('8', "110100101101");
            _c39Code.Add('9', "101100101101");
            _c39Code.Add('A', "110101001011");
            _c39Code.Add('B', "101101001011");
            _c39Code.Add('C', "110110100101");
            _c39Code.Add('D', "101011001011");
            _c39Code.Add('E', "110101100101");
            _c39Code.Add('F', "101101100101");
            _c39Code.Add('G', "101010011011");
            _c39Code.Add('H', "110101001101");
            _c39Code.Add('I', "101101001101");
            _c39Code.Add('J', "101011001101");
            _c39Code.Add('K', "110101010011");
            _c39Code.Add('L', "101101010011");
            _c39Code.Add('M', "110110101001");
            _c39Code.Add('N', "101011010011");
            _c39Code.Add('O', "110101101001");
            _c39Code.Add('P', "101101101001");
            _c39Code.Add('Q', "101010110011");
            _c39Code.Add('R', "110101011001");
            _c39Code.Add('S', "101101011001");
            _c39Code.Add('T', "101011011001");
            _c39Code.Add('U', "110010101011");
            _c39Code.Add('V', "100110101011");
            _c39Code.Add('W', "110011010101");
            _c39Code.Add('X', "100101101011");
            _c39Code.Add('Y', "110010110101");
            _c39Code.Add('Z', "100110110101");
            _c39Code.Add('-', "100101011011");
            _c39Code.Add('.', "110010101101");
            _c39Code.Add(' ', "100110101101");
            _c39Code.Add('$', "100100100101");
            _c39Code.Add('/', "100100101001");
            _c39Code.Add('+', "100101001001");
            _c39Code.Add('%', "101001001001");
            _c39Code.Add('*', "100101101101");
        }

        private void InitExtendedCode39()
        {
            _extC39Translation.Clear();
            _extC39Translation.Add(Convert.ToChar(0).ToString(), "%U");
            _extC39Translation.Add(Convert.ToChar(1).ToString(), "$A");
            _extC39Translation.Add(Convert.ToChar(2).ToString(), "$B");
            _extC39Translation.Add(Convert.ToChar(3).ToString(), "$C");
            _extC39Translation.Add(Convert.ToChar(4).ToString(), "$D");
            _extC39Translation.Add(Convert.ToChar(5).ToString(), "$E");
            _extC39Translation.Add(Convert.ToChar(6).ToString(), "$F");
            _extC39Translation.Add(Convert.ToChar(7).ToString(), "$G");
            _extC39Translation.Add(Convert.ToChar(8).ToString(), "$H");
            _extC39Translation.Add(Convert.ToChar(9).ToString(), "$I");
            _extC39Translation.Add(Convert.ToChar(10).ToString(), "$J");
            _extC39Translation.Add(Convert.ToChar(11).ToString(), "$K");
            _extC39Translation.Add(Convert.ToChar(12).ToString(), "$L");
            _extC39Translation.Add(Convert.ToChar(13).ToString(), "$M");
            _extC39Translation.Add(Convert.ToChar(14).ToString(), "$N");
            _extC39Translation.Add(Convert.ToChar(15).ToString(), "$O");
            _extC39Translation.Add(Convert.ToChar(16).ToString(), "$P");
            _extC39Translation.Add(Convert.ToChar(17).ToString(), "$Q");
            _extC39Translation.Add(Convert.ToChar(18).ToString(), "$R");
            _extC39Translation.Add(Convert.ToChar(19).ToString(), "$S");
            _extC39Translation.Add(Convert.ToChar(20).ToString(), "$T");
            _extC39Translation.Add(Convert.ToChar(21).ToString(), "$U");
            _extC39Translation.Add(Convert.ToChar(22).ToString(), "$V");
            _extC39Translation.Add(Convert.ToChar(23).ToString(), "$W");
            _extC39Translation.Add(Convert.ToChar(24).ToString(), "$X");
            _extC39Translation.Add(Convert.ToChar(25).ToString(), "$Y");
            _extC39Translation.Add(Convert.ToChar(26).ToString(), "$Z");
            _extC39Translation.Add(Convert.ToChar(27).ToString(), "%A");
            _extC39Translation.Add(Convert.ToChar(28).ToString(), "%B");
            _extC39Translation.Add(Convert.ToChar(29).ToString(), "%C");
            _extC39Translation.Add(Convert.ToChar(30).ToString(), "%D");
            _extC39Translation.Add(Convert.ToChar(31).ToString(), "%E");
            _extC39Translation.Add("!", "/A");
            _extC39Translation.Add("\"", "/B");
            _extC39Translation.Add("#", "/C");
            _extC39Translation.Add("$", "/D");
            _extC39Translation.Add("%", "/E");
            _extC39Translation.Add("&", "/F");
            _extC39Translation.Add("'", "/G");
            _extC39Translation.Add("(", "/H");
            _extC39Translation.Add(")", "/I");
            _extC39Translation.Add("*", "/J");
            _extC39Translation.Add("+", "/K");
            _extC39Translation.Add(",", "/L");
            _extC39Translation.Add("/", "/O");
            _extC39Translation.Add(":", "/Z");
            _extC39Translation.Add(";", "%F");
            _extC39Translation.Add("<", "%G");
            _extC39Translation.Add("=", "%H");
            _extC39Translation.Add(">", "%I");
            _extC39Translation.Add("?", "%J");
            _extC39Translation.Add("[", "%K");
            _extC39Translation.Add("\\", "%L");
            _extC39Translation.Add("]", "%M");
            _extC39Translation.Add("^", "%N");
            _extC39Translation.Add("_", "%O");
            _extC39Translation.Add("{", "%P");
            _extC39Translation.Add("|", "%Q");
            _extC39Translation.Add("}", "%R");
            _extC39Translation.Add("~", "%S");
            _extC39Translation.Add("`", "%W");
            _extC39Translation.Add("@", "%V");
            _extC39Translation.Add("a", "+A");
            _extC39Translation.Add("b", "+B");
            _extC39Translation.Add("c", "+C");
            _extC39Translation.Add("d", "+D");
            _extC39Translation.Add("e", "+E");
            _extC39Translation.Add("f", "+F");
            _extC39Translation.Add("g", "+G");
            _extC39Translation.Add("h", "+H");
            _extC39Translation.Add("i", "+I");
            _extC39Translation.Add("j", "+J");
            _extC39Translation.Add("k", "+K");
            _extC39Translation.Add("l", "+L");
            _extC39Translation.Add("m", "+M");
            _extC39Translation.Add("n", "+N");
            _extC39Translation.Add("o", "+O");
            _extC39Translation.Add("p", "+P");
            _extC39Translation.Add("q", "+Q");
            _extC39Translation.Add("r", "+R");
            _extC39Translation.Add("s", "+S");
            _extC39Translation.Add("t", "+T");
            _extC39Translation.Add("u", "+U");
            _extC39Translation.Add("v", "+V");
            _extC39Translation.Add("w", "+W");
            _extC39Translation.Add("x", "+X");
            _extC39Translation.Add("y", "+Y");
            _extC39Translation.Add("z", "+Z");
            _extC39Translation.Add(Convert.ToChar(127).ToString(), "%T"); // also %X, %Y, %Z 
        }
        private void InsertExtendedCharsIfNeeded(ref string formattedData)
        {
            var output = "";
            foreach (var c in formattedData)
            {
                try
                {
                    output += _c39Code[c]?.ToString();
                }
                catch
                {
                    // insert extended substitution
                    var oTrans = _extC39Translation[c.ToString()];
                    output += oTrans?.ToString();
                }
            }

            formattedData = output;
        }
        private char GetChecksumChar(string strNoAstr)
        {
            // checksum
            var code39Charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
            var sum = 0;
            InsertExtendedCharsIfNeeded(ref strNoAstr);

            // Calculate the checksum
            foreach (var t in strNoAstr)
            {
                sum += code39Charset.IndexOf(t.ToString(), StringComparison.Ordinal);
            }

            // return the checksum char
            return code39Charset[sum % 43];
        }

        #region IBarcode Members

        public override string EncodedValue => EncodeCode39();

        #endregion

    }
}
