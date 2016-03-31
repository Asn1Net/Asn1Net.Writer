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

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Net.Asn1.Writer.Tests
{
    public partial class DerWriterTests
    {
        [TestCase("17 0D 31 34 30 34 30 38 30 30 30 30 30 30 5A ", "140408000000")]
        [TestCase("17 0D 31 36 30 34 31 32 31 32 30 30 30 30 5A", "160412120000")]
        [Category("UTC TIME")]
        [Test]
        public void WriteUtcTime(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            var datetime = DateTimeOffset.ParseExact(valueToTest, "yyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1UtcTime(datetime);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("17 0D 31 34 30 34 30 38 30 30 30 30 30 30 5A ", "140408000000Z")]
        [TestCase("17 0D 31 36 30 34 31 32 31 32 30 30 30 30 5A", "160412120000Z")]
        [Category("UTC TIME")]
        [Test]
        public void WriteUtcTimeFromString(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            var datetime = Helpers.ConvertFromUniversalTime(valueToTest);

            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1UtcTime(datetime);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
