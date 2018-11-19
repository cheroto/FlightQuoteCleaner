using System;
using NUnit.Framework;

namespace FlightQuoteCleaner.Tests
{
    [TestFixture]
    public class QuoteCleanerTests
    {
        IQuoteCleaner _quoteCleaner;

        [SetUp]
        public void SetUp()
        {
            _quoteCleaner = new QuoteCleaner();
        }

        [Test]
        public void RemovePreviousPrices_Success()
        {
            //Arrange
            #region input and expected output strings
            var inputString = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
$1,077
1 passenger
PRICE HISTORY
São Paulo to Toronto
Tracking all flightsWed, May 15–Tue, May 21
São Paulo to Toronto
Round tripEconomyAny Airline
$911
$970
1 passenger
PRICE HISTORY
New York City to Toronto
Tracking all flightsWed, May 15–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
$247
1 passenger
PRICE HISTORY
Tracking all flightsThu, May 16–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
$247
1 passenger";
            var expectedResult = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
1 passenger
PRICE HISTORY
São Paulo to Toronto
Tracking all flightsWed, May 15–Tue, May 21
São Paulo to Toronto
Round tripEconomyAny Airline
$911
1 passenger
PRICE HISTORY
New York City to Toronto
Tracking all flightsWed, May 15–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
1 passenger
PRICE HISTORY
Tracking all flightsThu, May 16–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
1 passenger";
            #endregion

            //Act
            var actualResult = _quoteCleaner.RemovePreviousPrices(inputString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void RemoveTextInBetween_Success()
        {
            //Arrange
            #region input and expected output strings
            var inputString = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
1 passenger
PRICE HISTORY
São Paulo to Toronto
Tracking all flightsWed, May 15–Tue, May 21
São Paulo to Toronto
Round tripEconomyAny Airline
$911
1 passenger
PRICE HISTORY
New York City to Toronto
Tracking all flightsWed, May 15–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
1 passenger
PRICE HISTORY
Tracking all flightsThu, May 16–Tue, May 21
Newark, New York City to Toronto
Round tripEconomyAny Airline
$246
1 passenger";

            var expectedResult = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
$911
$246
$246
1 passenger";
            #endregion

            //Act
            var actualResult = _quoteCleaner.RemoveTextInBetween(inputString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void RemoveTrailingTextBack_Success()
        {
            //Arrange
            #region Input & Expected Result Strings
            var inputString = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
$911
$246
$246
1 passenger";

            var expectedResult = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
$911
$246
$246";
            #endregion
            //Act
            var actualResult = _quoteCleaner.RemoveTrailingTextBack(inputString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void RemoveTrailingTextFront_Success()
        {
            //Arrange
            #region Input & Expected Output Strings
            var inputString = @"Air Canada
1 stop
GRU–LAX
Thu, May 16
7:45 AM–3:30 PM

Air Canada
Nonstop
LAX–YYZ
Tue, May 21
11:15 PM–10:05 AM+1

Air Canada
Nonstop
YYZ–GRU
$1,086
$911
$246
$246";
            var expectedResult = @"$1,086
$911
$246
$246";
            #endregion
            //Act
            var actualResult = _quoteCleaner.RemoveTrailingTextFront(inputString);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
