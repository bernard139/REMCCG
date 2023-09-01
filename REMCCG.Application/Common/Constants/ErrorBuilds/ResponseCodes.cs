using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.Constants.ErrorBuilds
{
    public class ResponseCodes
    {
        public const string LANGUAGE = "language";
        public const string DEFAULT_LANGUAGE = "en";
        public const string SUCCESS = "VAT0000";
        public const string FAIL = "VAT0001";
        public const string NOT_FOUND = "VAT0002";
        public const string FORBIDEN = "VAT0003";
        public const string BAD_REQUEST = "VAT0004";
        public const string INVALID_OBJECT = "VAT0005";
        public const string UNAUTHORIZED = "VAT0006";
        public const string INVALID_TOKEN = "VAT0007";
        public const string INVALID_PARAMETER = "VAT0008";
        public const string DATA_IS_REQUIRED = "VAT0009";
        public const string INVALID_OBJECT_MAPPING = "VAT0010";
        public const string NULL_REFERENCE = "VAT0011";//Object can no tbe null
        public const string RECORD_EXISTS = "VAT0012";
        public const string MERCHANT_CATEGORY_EXISTS = "VAT0013";
        public const string REQUEST_NOT_SUCCESSFUL = "VAT0014";
        public const string REQUIRE_COUNTRY_FLAG = "VAT0015";
        public const string USER_ALREADY_EXISTS = "VAT0016";
        public const string RECORD_DOES_NOT_EXISTS = "VAT0017";
        public const string INVALID_PASSWORD = "VAT0018";
        public const string DATA_IS_INVALID = "VAT0019";
        public const string INVALID_PROJECT = "VAT0020";




        public const string EXCEPTION = "ZE9999";


    }
}
