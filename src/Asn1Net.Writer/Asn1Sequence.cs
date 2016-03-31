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
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of ASN.1 SEQUENCE
    /// </summary>
    public class Asn1Sequence : Asn1Object<List<Asn1ObjectBase>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1Sequence"/> class.
        /// </summary>
        /// <param name="asn1Class">Class of given Asn.1 object.</param>
        /// <param name="content">Content to be encoded.</param>
        public Asn1Sequence(Asn1Class asn1Class, List<Asn1ObjectBase> content)
            : base(asn1Class, true, (int)Asn1Type.Sequence, content)
        {
        }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var contentBytes = new List<byte>();
            foreach (var item in this.Content)
            {
                contentBytes.AddRange(item.Write());
            }

            var resBytes = new List<byte>();
            resBytes.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            resBytes.AddRange(DerWriter.WriteLength(contentBytes, (Asn1Type)this.Asn1Tag));
            resBytes.AddRange(contentBytes);

            return resBytes.ToArray();
        }
    }
}
