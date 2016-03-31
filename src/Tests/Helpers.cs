/*
 *  Copyright 2012-2016 The Asn1Net Project
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
/*
 *  Written for the Asn1Net project by:
 *  Peter Polacko <peter.polacko+asn1net@gmail.com>
 */

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Net.Asn1.Writer.Tests
{
    /// <summary>
    /// Helper class for test
    /// </summary>
    internal class Helpers
    {
        /// <summary>
        /// Transform examples used in tests from string to bytes to process by BerReader.
        /// </summary>
        /// <param name="example">String representation (mostly hex string and some comments) of ASN.1 structure to be parser by BerReader.</param>
        /// <returns>Byte array of ASN.1 structure.</returns>
        internal static byte[] GetExampleBytes(string example)
        {
            example = string.Join(" ",
                example.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Split(';').First()));

            var codes = example.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);

            return codes.Select(c => Convert.ToByte(c, 16)).ToArray();
        }

        /// <summary>
        /// Regular expression for parsing UTCTime according to ITU-T X.690 recommendation
        /// </summary>
        private static Regex utcTimeRegex = new Regex(@"^(?<date>\d{6})(?<hour>([01][0-9])|(2[0-3]))(?<minute>[0-5][0-9])?(?<second>[0-5][0-9])Z$", RegexOptions.ECMAScript);

        /// <summary>
        /// Regular expression for parsing GeenralizedTime according to ITU-T X.690 recommendation
        /// </summary>
        private static Regex generalizedTimeRegex = new Regex(@"^(?<date>\d{8})(?<hour>([01][0-9])|(2[0-3]))(?<minute>[0-5][0-9])?(?<second>[0-5][0-9])(?<fraction>\.\d+)?Z$", RegexOptions.ECMAScript);

        /// <summary>
        /// Converts string value to DateTimeOffset. String value is representing UTCTime according to ITU-T X.680 recommendation
        /// </summary>
        /// <param name="time">UTCTime as string value.</param>
        /// <returns>Value converted to DateTimeOffset.</returns>
        public static DateTimeOffset ConvertFromUniversalTime(string time)
        {
            var match = utcTimeRegex.Match(time);
            if (match.Success == false)
                throw new FormatException("Time was not in correct format according to UTCTime ITU-T X.690 spec.");

            var newTime = time.Replace("Z", "+0000");

            var res = DateTimeOffset.ParseExact(newTime, "yyMMddHHmmsszzz", CultureInfo.InvariantCulture);
            return res;
        }

        /// <summary>
        /// Converts string value to DateTimeOffset. String value is representing GeneralizedTime according to ITU-T X.680 recommendation
        /// </summary>
        /// <param name="time">GeneralizedTime as string value.</param>
        /// <returns>Value converted to DateTimeOffset.</returns>
        public static DateTimeOffset ConvertFromGeneralizedTime(string time)
        {
            var match = generalizedTimeRegex.Match(time);
            if (match.Success == false)
                throw new FormatException("Time was not in correct format according to GeneralizedTime spec.");

            var hasFraction = match.Groups["fraction"].Success;
            var fractionPart = match.Groups["fraction"].Value;

            if (fractionPart.EndsWith("0"))
                throw new FormatException("Generalized time should not have fractions part with trailing zeros.");

            if (fractionPart.Length - 1 > 7) // platform limit for .NET DateTime.ParseExact
                throw new NotSupportedException("Can only process up to 7 digits in fraction part of Generalized time.");

            var newTime = time.Replace("Z", "+0000");
            var format = (hasFraction == false)
                ? "yyyyMMddHHmmsszzz"
                : String.Format("yyyyMMddHHmmss.{0}zzz", new String('f', fractionPart.Length - 1));

            var res = DateTimeOffset.ParseExact(newTime, format, CultureInfo.InvariantCulture);
            return res;
        }
    }
}
