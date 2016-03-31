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
    using System.Linq;

    /// <summary>
    /// Implementation of ASN.1 RELATIVE OID
    /// </summary>
    public class Asn1RelativeOid : Asn1Object<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1RelativeOid"/> class.
        /// </summary>
        /// <param name="content">Content to be encoded.</param>
        public Asn1RelativeOid(string content)
            : base(Asn1Class.Universal, false, (int)Asn1Type.RelativeOid, content)
        {
        }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var resBytes = new List<byte>();
            var oidParts = this.Content.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(p => Convert.ToInt32(p, CultureInfo.InvariantCulture)).ToArray();

            var res = new List<byte>(oidParts.Length);
            DerWriterUtils.ParseSubIdentifiers(oidParts, res);

            resBytes.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            resBytes.AddRange(DerWriter.WriteLength(res, (Asn1Type)this.Asn1Tag));
            resBytes.AddRange(res);

            return resBytes.ToArray();
        }
    }
}
