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
        [TestCase("18 0F 32 30 31 36 30 33 30 32 32 32  30 31 33 36 5A", "20160302220136")]
        [Category("GENERALIZED TIME")]
        [Test]
        public void WriteGeneralizedTime(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            var datetime = DateTimeOffset.ParseExact(valueToTest, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1GeneralizedTime(datetime);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("18 0F 31 39 39 32 30 35 32 31 30 30 30 30 30 30 5A ", "19920521000000Z")]
        [TestCase("18 0F 31 39 39 32 30 36 32 32 31 32 33 34 32 31 5A", "19920622123421Z")]
        [TestCase("18 11 31 39 39 32 30 37 32 32 31 33 32 31 30 30 2E 33 5A", "19920722132100.3Z")]
        [Category("GENERALIZED TIME")]
        [Test]
        public void WriteGeneralizedTimeFromString(string correctResultHex, string valueToTest)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            var datetime = Helpers.ConvertFromGeneralizedTime(valueToTest);

            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1GeneralizedTime(datetime);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
