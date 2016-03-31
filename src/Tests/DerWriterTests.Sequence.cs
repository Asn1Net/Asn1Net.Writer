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

using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Net.Asn1.Writer.Tests
{
    public partial class DerWriterTests
    {
        [TestCase("30 0D 06 09 2A 86 48 86 F7 0D 01 01 0B 05 00 ", "1.2.840.113549.1.1.11")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteSequence(string correctResultHex, string oid)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, 
                    new System.Collections.Generic.List<Asn1ObjectBase>() { new Asn1ObjectIdentifier(oid), new Asn1Null() } );
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("30 00")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteEmptySequence(string correctResultHex)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);

            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, new System.Collections.Generic.List<Asn1ObjectBase>());
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
