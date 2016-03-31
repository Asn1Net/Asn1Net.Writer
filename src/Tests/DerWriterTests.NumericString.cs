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
        [TestCase("12 09 31 32 33 34 35 36 37 38 39", "123456789")]
        [Category("NUMERIC STRING")]
        [Test]
        public void WriteNumericString(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1NumericString(valueToTest);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
