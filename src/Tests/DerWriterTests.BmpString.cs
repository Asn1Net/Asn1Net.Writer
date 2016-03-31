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
        [TestCase("1E 32 00 61 00 62 00 63 00 64 00 2B 01 3E 01 61 01 0D 01 65 01 7E 00 FD 00 E1 00 ED 00 E9 00 FA 00 E4 00 F4 00 A7 01 48 00 F3 01 55 00 61 00 62 00 63 00 64 ",
            "abcd+ľščťžýáíéúäô§ňóŕabcd")]
        [Category("BMP STRING")]
        [Test]
        public void WriteBmpString(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1BmpString(valueToTest);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
