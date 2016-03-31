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
    /// Implementation of ASN.1 Context Specific
    /// </summary>
    public class Asn1ContextSpecific : Asn1Object<byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1ContextSpecific"/> class.
        /// </summary>
        /// <param name="constructed">Constructed/Primitive bit of identifier octet.</param>
        /// <param name="tagNumber">Tag number of Asn.1 object.</param>
        /// <param name="content">Content to be encoded.</param>
        public Asn1ContextSpecific(bool constructed, int tagNumber, byte[] content)
            : base(Asn1Class.ContextSpecific, constructed, tagNumber, content)
        {
        }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var resBytes = new List<byte>();
            resBytes.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            resBytes.AddRange(DerWriter.WriteLength(this.Content, (Asn1Type)this.Asn1Tag));
            resBytes.AddRange(this.Content);

            return resBytes.ToArray();
        }
    }
}
