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
using System.Numerics;
using NUnit.Framework;

namespace Net.Asn1.Writer.Tests
{
    public partial class DerWriterTests
    {
        [TestCase("02 01 00", 0)]
        [TestCase("02 01 7F", 127)]
        [TestCase("02 02 00 80", 128)]
        [TestCase("02 02 01 00", 256)]
        [TestCase("02 01 80", -128)]
        [TestCase("02 02 FF 7F", -129)]
        [Category("INTEGER")]
        [Test]
        public void WriteInteger(string correctResultHex, int valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);

            var input = new BigInteger(valueToTest);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1Integer(new BigInteger(valueToTest));
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
