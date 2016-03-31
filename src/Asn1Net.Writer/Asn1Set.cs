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
    /// Implementation of ASN.1 SET
    /// </summary>
    public class Asn1Set : Asn1Object<List<Asn1ObjectBase>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1Set"/> class.
        /// </summary>
        /// <param name="asn1Class">Class of given Asn.1 object.</param>
        /// <param name="content">Content to be encoded.</param>
        public Asn1Set(Asn1Class asn1Class, List<Asn1ObjectBase> content)
            : base(asn1Class, true, (int)Asn1Type.Set, content)
        {
        }

        /// <inheritdoc/>
        public override byte[] Write()
        {
            var contentBytes = new List<byte>();
            var comparer = new SetComparer();

            var orderedContent = this.Content.OrderBy(p => p, comparer);

            foreach (var item in orderedContent)
            {
                contentBytes.AddRange(item.Write());
            }

            var resBytes = new List<byte>();
            resBytes.Add(DerWriter.WriteTag(this.Asn1Class, this.Asn1Tag, this.Constructed));
            resBytes.AddRange(DerWriter.WriteLength(contentBytes, (Asn1Type)this.Asn1Tag));
            resBytes.AddRange(contentBytes);

            return resBytes.ToArray();
        }

        private class SetComparer : IComparer<Asn1ObjectBase>
        {
            public int Compare(Asn1ObjectBase x, Asn1ObjectBase y)
            {
                if (x == null)
                {
                    throw new ArgumentNullException(nameof(x));
                }

                if (y == null)
                {
                    throw new ArgumentNullException(nameof(y));
                }

                // The encodings of the component values of a set value shall appear in an order determined by their tags as specified
                // in 8.6 of Rec. ITU - T X.680 | ISO / IEC 8824 - 1.

                // those elements or alternatives with universal class tags shall appear first, followed by those with
                // application class tags, followed by those with context-specific tags, followed by those with private class
                // tags;
                var xVal = x.Asn1Class == Asn1Class.Universal ? x.Asn1Tag : x.Asn1Tag * (int)x.Asn1Class;
                var yVal = y.Asn1Class == Asn1Class.Universal ? y.Asn1Tag : y.Asn1Tag * (int)y.Asn1Class;

                return xVal.CompareTo(yVal);
            }
        }
    }
}
