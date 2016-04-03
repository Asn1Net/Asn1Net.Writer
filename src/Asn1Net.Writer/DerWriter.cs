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
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Implementation of DER writer
    /// </summary>
    public class DerWriter
    {
        private Stream outputStream = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DerWriter"/> class.
        /// </summary>
        /// <param name="outputStream">Stream where should encoded ASN.1 object be written.</param>
        public DerWriter(Stream outputStream)
        {
            this.outputStream = outputStream;
        }

        /// <summary>
        /// Writes given ASN.1 object to output stream.
        /// </summary>
        /// <param name="objectToWrite">Object to encode and write.</param>
        /// <returns>Instance of self.</returns>
        public DerWriter Write(Asn1ObjectBase objectToWrite)
        {
            if (objectToWrite == null)
            {
                throw new ArgumentNullException(nameof(objectToWrite));
            }

            var res = objectToWrite.Write();
            this.outputStream.Write(res, 0, res.Length);
            return this;
        }

        /// <summary>
        /// Calculates length of ASN.1 object
        /// </summary>
        /// <param name="value">Content of ASN.1 object.</param>
        /// <param name="asn1Type">Type of ASN.1 object. In some types there has to be an octet before content bytes.</param>
        /// <returns>Encoded length of ASN.1 object.</returns>
        internal static byte[] WriteLength(IEnumerable<byte> value, Asn1Type asn1Type)
        {
            var length = value.Count();
            if (asn1Type == Asn1Type.BitString)
            {
                length++; // count unused bits in
            }

            byte[] lengthOctets = null;
            if (length <= 127)
            {
                lengthOctets = new byte[1] { (byte)length };
            }
            else
            {
                var lengthValueOctets = BitConverter.GetBytes(length); // TODO: fix incorrect length computation

                lengthValueOctets = lengthValueOctets.Reverse().SkipWhile(p => p == 0x00).ToArray();

                byte firstLengthByte = (byte)(0x80 | lengthValueOctets.Length);
                lengthOctets = new byte[lengthValueOctets.Length + 1];
                lengthOctets[0] = firstLengthByte;
                Array.Copy(lengthValueOctets, 0, lengthOctets, 1, lengthValueOctets.Length);
            }

            return lengthOctets;
        }

        /// <summary>
        /// Writes Identifier octet of ASN.1 object.
        /// </summary>
        /// <param name="asn1Class">Class of given Asn.1 object.</param>
        /// <param name="tagNumber">Tag number or type of Asn.1 object.</param>
        /// <param name="constructed">Constructed/Primitive bit of identifier octet.</param>
        /// <returns>Encoded identifier octet of ASN.1 object.</returns>
        internal static byte WriteTag(Asn1Class asn1Class, int tagNumber, bool constructed = false)
        {
            // TODO: support tag number > 30
            if (asn1Class == Asn1Class.ContextSpecific && tagNumber > 30)
            {
                throw new NotSupportedException("Currently only tag numbers 0-30 are supported.");
            }

            var classPart = (int)asn1Class;
            var typePart = tagNumber & 0x1F; // clear bits 8 to 6

            int tag = classPart + typePart;

            if (constructed)
            {
                tag = tag | 0x20;
            }

            return (byte)tag;
        }
    }
}
