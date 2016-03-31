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
    using System.Linq;

    /// <summary>
    /// Implementation of ASN.1 BIT STRING
    /// </summary>
    public class Asn1BitString : Asn1Object<byte[]>
    {
        private static readonly Dictionary<int, byte> CleaningMatrix = new Dictionary<int, byte>
        {
            [1] = 0xFF, // 1111 1110
            [2] = 0xFC, // 1111 1100
            [3] = 0xF8, // 1111 1000
            [4] = 0xF0, // 1111 0000
            [5] = 0xE0, // 1110 0000
            [6] = 0xC0, // 1100 0000
            [7] = 0x80, // 1000 0000
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1BitString"/> class.
        /// </summary>
        /// <param name="content">Content to be encoded.</param>
        /// <param name="unusedBits">Unused bits. Last X bits of Content will be cleared.</param>
        public Asn1BitString(byte[] content, int unusedBits)
            : base(Asn1Class.Universal, false, (int)Asn1Type.BitString, content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (unusedBits < 0 || unusedBits > 7)
            {
                throw new ArgumentException(FormattableString.Invariant($"{unusedBits} shall be in the range zero to seven."));
            }

            if (unusedBits == 0 && content.Length > 1 && (content.Last() & 1) != 1)
            {
                throw new ArgumentException(FormattableString.Invariant($"{content} should not have any trailing zeros"));
            }

            this.UnusedBits = unusedBits;

            if (content.Length > 1 && unusedBits > 0)
            {
                // clean last byte
                this.Content[content.Length - 1] &= CleaningMatrix[unusedBits];
            }
        }

        /// <summary>
        /// Gets unused bits from 0 to 7. Last X bits Of Content will be cleared.
        /// </summary>
        public int UnusedBits { get; private set; }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var resBytes = new List<byte>();
            resBytes.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            resBytes.AddRange(DerWriter.WriteLength(this.Content, (Asn1Type)this.Asn1Tag));
            resBytes.Add((byte)this.UnusedBits);
            resBytes.AddRange(this.Content);

            return resBytes.ToArray();
        }
    }
}
