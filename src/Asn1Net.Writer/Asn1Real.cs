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

namespace Net.Asn1.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Implementation of ASN.1 REAL
    /// </summary>
    public class Asn1Real : Asn1Object<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1Real"/> class.
        /// </summary>
        /// <param name="content">Content to be encoded.</param>
        public Asn1Real(double content)
            : base(Asn1Class.Universal, false, (int)Asn1Type.Real, content)
        {
        }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var res = new List<byte>();
            byte[] valBytes = null;

            // SpecialValues
            if (this.Content == 0)
            {
                // If the real value is the value plus zero, there shall be no contents octets in the encoding.
                return new byte[] { 0x09, 0x00 };
            }
            else if (double.IsPositiveInfinity(this.Content))
            {
                valBytes = new byte[] { 0x40 };
            }
            else if (double.IsNegativeInfinity(this.Content))
            {
                valBytes = new byte[] { 0x41 };
            }
            else if (double.IsNaN(this.Content))
            {
                valBytes = new byte[] { 0x42 };
            }
            else if (this.Content == -0.0d)
            {
                valBytes = new byte[] { 0x43 };
            }
            else
            {
                // ToString to find out how many decimal digits value has
                // to achieve a format that looks like this ####.E-0
                // TrimStart will take care of doubles line 0.12314
                // Then remove full stop and we will have number of # to add to format string
                var tmpVal = this.Content.ToString(CultureInfo.InvariantCulture.NumberFormat).TrimStart('0').Replace(".", string.Empty);
                var format = FormattableString.Invariant($"{new string('#', tmpVal.Length)}\\..E+0");

                // Everything was taken care of with format string to meet ITU X690 specification for REAL type.
                var val = this.Content.ToString(format, CultureInfo.InvariantCulture.NumberFormat);

                var valEncoded = Encoding.ASCII.GetBytes(val);
                valBytes = new byte[valEncoded.Length + 1];
                valBytes[0] = 0x03; // base 10 encoding
                valEncoded.CopyTo(valBytes, 1);
            }

            res.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            res.AddRange(DerWriter.WriteLength(valBytes, (Asn1Type)this.Asn1Tag));
            res.AddRange(valBytes);

            return res.ToArray();
        }
    }
}
