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
        [TestCase("03 03 07 6e 80", "6e 80", 7)] // 7 unused bits
        [TestCase("03 03 06 6e c0", "6e c0", 6)] // 6 unused bits
        [TestCase("03 03 05 6e e0", "6e e0", 5)] // 5 unused bits
        [TestCase("03 03 04 6e f0", "6e f0", 4)] // 4 unused bits
        [TestCase("03 03 03 6e f8", "6e f8", 3)] // 3 unused bits
        [TestCase("03 03 02 6e fc", "6e fc", 2)] // 2 unused bits
        [TestCase("03 03 01 6e fe", "6e fe", 1)] // 1 unused bits
        [TestCase("03 03 00 6e ff", "6e ff", 0)] // zero unused bits
        [TestCase("03 03 00 6e 5d", "6e 5d", 0)] // zero unused bits
        [TestCase("03 01 00", "", 0)] // empty bit string]
        [Category("BIT STRING")]
        [Test]
        public void WriteBitString(string correctResultHex, string valueToTest, int unusedBits)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            var encodedValueToTest = Helpers.GetExampleBytes(valueToTest);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1BitString(encodedValueToTest, unusedBits);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
