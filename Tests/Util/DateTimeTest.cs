/*
 * Copyright (C) 2008, Google Inc.
 * Copyright (C) 2009, Gil Ran <gilrun@gmail.com>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the Git Development Community nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using GitSharp.Util;
using NUnit.Framework;

namespace GitSharp.Tests
{
    [TestFixture]
    public class DateTimeTest
    {
        [Test]
        public void EpochStartShouldConvertToJanuaryFirst1970() 
        {
            DateTime epochStart = new DateTime(1970, 1, 1);
            long gitTime = epochStart.ToGitInternalTime();

            Assert.AreEqual(0, gitTime);
        }

        [Test]
        public void JanuaryFirst1970ShouldConvertToEpochStart()
        {
            var epochStartDateTime = (0L).GitTimeToDateTime();

            Assert.AreEqual(new DateTime(1970, 1, 1), epochStartDateTime);
        }

        [Test]
        public void OneDayAfterTheEpochInGitTime()
        {
            DateTime oneDayLater = new DateTime(1970, 1, 2);
            long gitTime = oneDayLater.ToGitInternalTime();

            Assert.AreEqual(86400, gitTime);
        }

        [Test]
        public void EightySixThousandFourHundredSecondsAfterTheEpoch()
        {
            var theNextDay = (86400L).GitTimeToDateTime();

            Assert.AreEqual(new DateTime(1970, 1, 2), theNextDay);
        }

        [Test]
        public void EpochIsTheSameMomentRegardlessOfTimeZone()
        {
            var epochInMelbourne = new DateTimeOffset(1970, 1, 1, 10, 0, 0, TimeSpan.FromHours(10));
            Assert.AreEqual(0L, epochInMelbourne.ToGitInternalTime());

            var epochInFozDoIguacu = new DateTimeOffset(1969, 12, 31, 21, 0, 0, TimeSpan.FromHours(-3));
            Assert.AreEqual(0L, epochInFozDoIguacu.ToGitInternalTime());
        }

        [Test]
        public void SameClockTimeInDifferentTimeZonesIsDifferentNumberOfSecondsFromEpoch()
        {
            var jan2InLondon = new DateTimeOffset(1970, 1, 2, 0, 0, 0, TimeSpan.FromHours(0));
            Assert.AreEqual(86400L, jan2InLondon.ToGitInternalTime());

            var jan2InDubai = new DateTimeOffset(1970, 1, 2, 0, 0, 0, TimeSpan.FromHours(+3));
            Assert.AreEqual(86400 - 3600 * 3, jan2InDubai.ToGitInternalTime());            
        }

        [Test]
        public void OffsetIsPassedInMinutes()
        {
            var epochInMountainTime = (0L).GitTimeToDateTimeOffset(-7 * 60);
            Assert.AreEqual(-7, epochInMountainTime.Offset.Hours);
            Assert.AreEqual(0, epochInMountainTime.Offset.Minutes);

            var epochInChathamIslands = (0L).GitTimeToDateTimeOffset(12 * 60 + 45);
            Assert.AreEqual(12, epochInChathamIslands.Offset.Hours);
            Assert.AreEqual(45, epochInChathamIslands.Offset.Minutes);
        }
    }
}