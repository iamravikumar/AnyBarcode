﻿using System.Runtime.Serialization;

namespace AnyBarcode
{
    [Serializable]
    public class BarcodeException : Exception
    {
        public BarcodeException()
        {
        }

        public BarcodeException(string message) : base(message)
        {
        }

        public BarcodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BarcodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
